using System;
using Core.Concurrent;

namespace Core.Service
{

    public abstract class AbstractService : IService
    {
        protected abstract void StartUp();
        protected abstract void ShutDown();

        public bool IsIdle { get; }
        public ServiceState State { get; }
        public string Id   { get; }
        public string Name { get; }

        private readonly Reentrant _reentrant = new Reentrant();

        public bool IsStarted => State.Code == ServiceState.Started;
        private static readonly Func<ServiceState, bool> IsStart = state => state.Code == ServiceState.Started;
        private static readonly Func<ServiceState, bool> IsStopped = state => state.Code == ServiceState.Stopped;
        private static readonly Func<ServiceState, bool> CanStart = state => (state.Code == ServiceState.Initialized) || (state.Code == ServiceState.Stopped);
        private static readonly Func<ServiceState, bool> CanStop = state => (state.Code == ServiceState.Initialized) || (state.Code == ServiceState.Starting) || (state.Code == ServiceState.Started);

        protected AbstractService()
        {
            Id     = UUID.NGuid;
            IsIdle = false;
            Name   = GetType().Name;
            State  = new ServiceState(ServiceState.Initialized);
        }

        public IService Start()
        {
            _reentrant.ExecuteIf(State, CanStart, () => {
                State.SetState(ServiceState.Starting);
                StartAsync();
            });
            return this;
        }

        public IService Stop()
        {
            _reentrant.ExecuteIf(State, CanStop, () => {
                try
                {
                    switch(State.Code)
                    {
                        case ServiceState.Initialized:
                            {
                                State.SetState(ServiceState.Stopped);
                                break;
                            }
                        case ServiceState.Starting:
                            {
                                State.SetState(ServiceState.Starting, true);
                                break;
                            }
                        case ServiceState.Started:
                            {
                                State.SetState(ServiceState.Stopping);
                                StopAsync();
                                break;
                            }
                        case ServiceState.Stopping:
                        case ServiceState.Stopped: break;
                        case ServiceState.Failed: throw new InvalidOperationException("Failed Service can not stopped.");
                    }
                }
                catch(Exception error)
                {
                    NotifyFailed(error);
                }
            });
            return this;

        }

        public void AwaitStarted()
        {
            _reentrant.ExecuteUtil(State, IsStart, () => {
                if(State.Code != ServiceState.Started)
                {
                    throw new InvalidOperationException("Service is " + State.Name);
                }
            });
        }

        public void AwaitStopped()
        {
            _reentrant.ExecuteUtil(State, IsStopped, () => {
                if(State.Code != ServiceState.Stopped)
                {
                    throw new InvalidOperationException("Service is " + State.Name);
                }
            });
        }

        protected void NotifyStarted()
        {
            _reentrant.Execute(() => {
                if(State.Code != ServiceState.Starting)
                {
                    throw new InvalidOperationException("Only Starting Service can be started");
                }

                if(State.StopWhenStarting)
                {
                    State.SetState(ServiceState.Stopping);
                    StopAsync();
                }
                else
                {
                    State.SetState(ServiceState.Started);
                }
            });
        }

        protected void NotifyStopped()
        {
            _reentrant.Execute(() => {
                if (State.Code != ServiceState.Stopping)
                {
                    throw new InvalidOperationException("Only Stopping Service can be stopped");
                }
                State.SetState(ServiceState.Stopped);
            });
        }

        protected void NotifyFailed(Exception error)
        {
            _reentrant.Execute(() => {
                State.SetState(ServiceState.Failed);
            });
        }

        private void StartAsync()
        {
            if(!IsIdle)
            {
                DoStart();
            }
            else
            {
                Threads.StartNew(Name + " " + State.Name, DoStart);
            }
        }

        private void StopAsync()
        {
            if(!IsIdle)
            {
                DoStop();
            }
            else
            {
                Threads.StartNew(Name + " " + State.Name, DoStop);
            }
        }

        private void DoStart()
        {
            try
            {
                StartUp();
                NotifyStarted();
            }
            catch(Exception error)
            {
                NotifyFailed(error);
            }
        }

        private void DoStop()
        {
            try
            {
                ShutDown();
                NotifyStopped();
            }
            catch(Exception error)
            {
                NotifyFailed(error);
            }
        }

    }

}
using System;
using System.Threading;

namespace Core.Concurrent
{

    public sealed class Reentrant
    {

        private long _waitingCount;
        private readonly object _lock = new object();

        public Reentrant()
        {
            _waitingCount = 0;
        }

        /// <summary>
        /// Checks whether current thread hold this lock
        /// </summary>
        public bool IsOccupiedByCurrentThread => Monitor.IsEntered(_lock);

        /// <summary>
        /// Operation related to long will be two operations in 32 bit operating system
        /// </summary>
        public long WaitingCount => Interlocked.Read(ref _waitingCount);

        public void Lock()
        {
            Monitor.Enter(_lock);
        }

        /// <summary>
        /// acquire lock if certian condition is meet, but will not wait for whether it is satisfied or not
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool LockIf(Func<bool> predicate)
        {
            var lockToken = false;
            try
            {
                //The output is true if the lock is acquired; otherwise, the output is false if lock is not acquaried or exception is throw.
                Monitor.Enter(_lock, ref lockToken);
            }
            catch
            {
                // Event Exception is throw , the lock may be taken
                if(!lockToken)
                {
                    return false;
                }
            }

            // When lock is acquired, then check whehter predicate is true or false
            var flag = false;
            try
            {
                // What if predicate throws exception
                return flag = predicate();
            }
            finally
            {
                // if the predicate if false then release the lock
                if(!flag)
                {
                    Release();
                }
            }
        }

        /// <summary>
        /// acquire lock if certian condition is meet, but will not wait for whether it is satisfied or not
        /// </summary>
        /// <param name="model"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool LockIf<T>(T model, Func<T, bool> predicate)
        {
            var lockToken = false;
            try
            {
                Monitor.Enter(_lock, ref lockToken);
            }
            catch
            {
                if(!lockToken)
                {
                    return false;
                }
            }

            var flag = false;
            try
            {
                // if predicate is false then exit
                return flag = predicate(model);
            }
            finally
            {
                if(!flag)
                {
                    Release();
                }
            }
        }

        /// <summary>
        /// acquire the lock when is satisfied
        /// </summary>
        /// <param name="predicate"></param>
        public void LockUtil(Func<bool> predicate)
        {
            LockUtil(predicate, Timeout.InfiniteTimeSpan);
        }

        public void LockUtil<T>(T model, Func<T, bool> predicate)
        {
            LockUtil(model, predicate, Timeout.InfiniteTimeSpan);
        }

        public void LockUtil(Func<bool> predicate, TimeSpan timeOut)
        {
            Lock();
            var flag = false;
            try
            {
                flag = predicate();
                while(!flag)
                {
                    Interlocked.Increment(ref _waitingCount);
                    try
                    {
                        Monitor.Wait(_lock, timeOut);
                        flag = predicate();
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _waitingCount);
                    }
                }
            }
            finally
            {
                if(!flag)
                {
                    Release();
                }
            }
        }

        public void LockUtil<T>(T model, Func<T, bool> predicate, TimeSpan timeOut)
        {
            Lock();
            var flag = false;
            try
            {
                flag = predicate(model);
                while(!flag)
                {
                    Interlocked.Increment(ref _waitingCount);
                    try
                    {
                        Monitor.Wait(_lock, timeOut);
                        flag = predicate(model);
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _waitingCount);
                    }
                }
            }
            finally
            {
                if(!flag)
                {
                    Release();
                }
            }
        }

        /// <summary>
        /// Execute Action Only when lock is acquired
        /// </summary>
        /// <param name="action"></param>
        public void Execute(Action action)
        {
            var lockToken = false;
            try
            {
                Monitor.Enter(_lock, ref lockToken);
                action();
            }
            finally
            {
                if(lockToken)
                {
                    Release();
                }
            }
        }

        /// <summary>
        /// Execute Action Only when certain condition met, release otherwise
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        public void ExecuteIf(Func<bool> predicate, Action action)
        {
            var lockToken = false;
            try
            {
                Monitor.Enter(_lock, ref lockToken);
                var flag = predicate();
                if(!flag)
                {
                    Release();
                    return;
                }

                action();
            }
            finally
            {
                if(lockToken)
                {
                    Release();
                }
            }
        }

        /// <summary>
        /// Execute Action Only when certain condition met, release otherwise
        /// </summary>
        /// <param name="model"></param>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        public void ExecuteIf<T>(T model, Func<T, bool> predicate, Action action)
        {
            var lockToken = false;
            try
            {
                Monitor.Enter(_lock, ref lockToken);
                var flag = predicate(model);
                if(!flag)
                {
                    Release();
                    return;
                }
                action();
            }
            finally
            {
                if(lockToken)
                {
                    Release();
                }
            }
        }

        /// <summary>
        /// Execute Action Only when certain condition met, block otherwise
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        public void ExecuteUtil(Func<bool> predicate, Action action)
        {
            var lockToken = false;
            try
            {
                Monitor.Enter(_lock, ref lockToken);
                var flag = predicate();
                while(!flag)
                {
                    Interlocked.Increment(ref _waitingCount);
                    try
                    {
                        Monitor.Wait(_lock);
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _waitingCount);
                    }
                    flag = predicate();
                }

                action();
            }
            finally
            {
                if (lockToken)
                {
                    Release();
                }
            }
        }

        /// <summary>
        /// Execute Action Only when certain conditaion met, block otherwise
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <param name="predicate"></param>
        /// <param name="action"></param>
        public void ExecuteUtil<T>(T model, Func<T, bool> predicate, Action action)
        {
            var lockToken = false;
            try
            {
                Monitor.Enter(_lock, ref lockToken);
                var flag = predicate(model);
                while(!flag)
                {
                    Interlocked.Increment(ref _waitingCount);
                    try
                    {
                        Monitor.Wait(_lock);
                    }
                    finally
                    {
                        Interlocked.Decrement(ref _waitingCount);
                    }
                    flag = predicate(model);
                }
                action();
            }
            finally
            {
                if(lockToken)
                {
                    Release();
                }
            }
        }

        public void Release()
        {
            if(!IsOccupiedByCurrentThread)
            {
                return;
            }

            if(WaitingCount <= 0)
            {
                return;
            }

            try
            {
                Monitor.Pulse(_lock);
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }

        public void SingalAll()
        {
            if(!IsOccupiedByCurrentThread)
            {
                return;
            }

            if(WaitingCount <= 0)
            {
                return;
            }

            Monitor.PulseAll(_lock);
        }

        public void Reset()
        {
            if(!IsOccupiedByCurrentThread)
            {
                return;
            }

            Interlocked.Exchange(ref _waitingCount, 0);
            Monitor.PulseAll(_lock);
        }

    }

}
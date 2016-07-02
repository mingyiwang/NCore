using System;
using System.Threading;

namespace Core.Concurrent
{

    public sealed class ReadWrite
    {

        private readonly ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public void Read(Action action)
        {
            _readWriteLock.EnterReadLock();
            try
            {
                action();
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public T Read<T>(Func<T> function)
        {
            _readWriteLock.EnterReadLock();
            try
            {
                return function();
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public bool TryRead<TModel>(out TModel model, Action<TModel> action)
        {
            model = default(TModel);
            _readWriteLock.EnterReadLock();
            try
            {
                action(model);
                return true;
            }
            catch
            {
                model = default(TModel);
                return false;
            }
            finally
            {
                _readWriteLock.ExitReadLock();
            }
        }

        public void Write(Action action)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                action();
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public T Write<T>(Func<T> function)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                return function();
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public void Write<T>(T value, Action<T> action)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                action(value);
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public void WriteIf(Func<bool> predicate, Action action)
        {
            _readWriteLock.EnterUpgradeableReadLock();
            try
            {
                if(!predicate())
                {
                    return;
                }
                _readWriteLock.EnterWriteLock();
                try
                {
                    action();
                }
                finally
                {
                    _readWriteLock.ExitWriteLock();
                }
            }
            finally
            {
                _readWriteLock.ExitUpgradeableReadLock();
            }
        }

        public T WriteIf<T>(Func<bool> predicate, Func<T> function)
        {
            _readWriteLock.EnterUpgradeableReadLock();
            try
            {
                if(!predicate())
                {
                    return default(T);
                }

                _readWriteLock.EnterWriteLock();
                try
                {
                    return function();
                }
                finally
                {
                    _readWriteLock.ExitWriteLock();
                }
            }
            finally
            {
                _readWriteLock.ExitUpgradeableReadLock();
            }
        }

        public void Dispose()
        {
            _readWriteLock.Dispose();
        }

    }

}
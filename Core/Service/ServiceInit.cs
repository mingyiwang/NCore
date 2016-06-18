﻿using System;
using System.Threading;

namespace Core.Service
{

    public sealed class ServiceInit
    {

        public static T Start<T>(T service, Func<T> func) where T : class, IService
        {
            return LazyInitializer.EnsureInitialized(ref service, () => {
                var initializedService = func();
                func().Start().AwaitStarted();

                Preconditions.CheckEquals(initializedService.State.Code, ServiceState.Started);
                return initializedService;
            });
        }

    }

}
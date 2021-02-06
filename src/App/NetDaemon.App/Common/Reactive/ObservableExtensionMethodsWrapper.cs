﻿using System;
using System.Reactive.Linq;
using System.Threading;

namespace NetDaemon.Common.Reactive
{
    public class ObservableExtensionMethodsWrapper : IObservableExtensionMethods
    {
        /// <summary>
        ///     Is same for timespan time
        /// </summary>
        /// <param name="observable"></param>
        /// <param name="span"></param>
        public IObservable<(EntityState Old, EntityState New)> NDSameStateFor(IObservable<(EntityState Old, EntityState New)> observable, TimeSpan span)
        {
            return observable.Throttle(span);
        }

        /// <summary>
        ///     Wait for state the specified time
        /// </summary>
        /// <param name="observable"></param>
        /// <param name="timeout">Timeout waiting for state</param>
        public IObservable<(EntityState Old, EntityState New)> NDWaitForState(IObservable<(EntityState Old, EntityState New)> observable, TimeSpan timeout)
        {
            return observable
                .Timeout(timeout,
                    Observable.Return((new EntityState() { State = "TimeOut" }, new EntityState() { State = "TimeOut" }))).Take(1);
        }

        /// <summary>
        ///     Wait for state the default time
        /// </summary>
        /// <param name="observable"></param>
        public IObservable<(EntityState Old, EntityState New)> NDWaitForState(IObservable<(EntityState Old, EntityState New)> observable) => observable
            .Timeout(TimeSpan.FromSeconds(5),
                Observable.Return((new EntityState() { State = "TimeOut" }, new EntityState() { State = "TimeOut" }))).Take(1);

        /// <summary>
        ///     Returns first occurence or null if timedout
        /// </summary>
        /// <param name="observable">Extended object</param>
        /// <param name="timeout">The time to wait before timeout.</param>
        /// <param name="token">Provide token to cancel early</param>
        public (EntityState Old, EntityState New)? NDFirstOrTimeout(IObservable<(EntityState Old, EntityState New)> observable, TimeSpan timeout, CancellationToken? token = null)
        {
            try
            {
                if (token is null)
                    return observable.Timeout(timeout).Take(1).Wait();
                else
                    return observable.Timeout(timeout).Take(1).RunAsync(token.Value).Wait();
            }
            catch (TimeoutException)
            {
                return null;
            }
            catch (OperationCanceledException)
            {
                return null;
            }
        }
    }
}
using System;
using System.Threading;

namespace NetDaemon.Common.Reactive
{
    public interface IObservableExtensionMethods
    {
        /// <summary>
        ///     Is same for timespan time
        /// </summary>
        /// <param name="observable"></param>
        /// <param name="span"></param>
        IObservable<(EntityState Old, EntityState New)> NDSameStateFor(IObservable<(EntityState Old, EntityState New)> observable, TimeSpan span);

        /// <summary>
        ///     Wait for state the specified time
        /// </summary>
        /// <param name="observable"></param>
        /// <param name="timeout">Timeout waiting for state</param>
        IObservable<(EntityState Old, EntityState New)> NDWaitForState(IObservable<(EntityState Old, EntityState New)> observable, TimeSpan timeout);

        /// <summary>
        ///     Wait for state the default time
        /// </summary>
        /// <param name="observable"></param>
        IObservable<(EntityState Old, EntityState New)> NDWaitForState(IObservable<(EntityState Old, EntityState New)> observable);

        /// <summary>
        ///     Returns first occurence or null if timedout
        /// </summary>
        /// <param name="observable">Extended object</param>
        /// <param name="timeout">The time to wait before timeout.</param>
        /// <param name="token">Provide token to cancel early</param>
        (EntityState Old, EntityState New)? NDFirstOrTimeout(IObservable<(EntityState Old, EntityState New)> observable, TimeSpan timeout, CancellationToken? token = null);
    }
}
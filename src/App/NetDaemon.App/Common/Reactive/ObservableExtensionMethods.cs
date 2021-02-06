using System;
using System.Threading;

namespace NetDaemon.Common.Reactive
{
    /// <summary>
    ///     Extension methods for Observables
    /// </summary>
    public static class ObservableExtensionMethods
    {
        // This implementation switching is to make the extension methods testable/mockable and is based of this article https://codethug.com/2017/09/09/Mocking-Extension-Methods/

        private static readonly IObservableExtensionMethods DefaultImplementation = new ObservableExtensionMethodsWrapper();
        /// <summary>
        /// This is to allow setting the Implementation that is used to allow testing/mocking 
        /// </summary>
        public static IObservableExtensionMethods Implementation { get; set; } = DefaultImplementation;

        /// <summary>
        /// To clean up after your test by removing the mock. If you don’t do this, your mock will stick around in the ObservableExtensionMethods class because it’s static.
        /// </summary>
        public static void RevertToDefaultImplementation()
        {
            Implementation = DefaultImplementation;
        }

        /// <summary>
        ///     Is same for timespan time
        /// </summary>
        /// <param name="observable"></param>
        /// <param name="span"></param>
        public static IObservable<(EntityState Old, EntityState New)> NDSameStateFor(this IObservable<(EntityState Old, EntityState New)> observable, TimeSpan span)
        {
            return Implementation.NDSameStateFor(observable, span);
        }

        /// <summary>
        ///     Wait for state the specified time
        /// </summary>
        /// <param name="observable"></param>
        /// <param name="timeout">Timeout waiting for state</param>
        public static IObservable<(EntityState Old, EntityState New)> NDWaitForState(this IObservable<(EntityState Old, EntityState New)> observable, TimeSpan timeout)
        {
            return Implementation.NDWaitForState(observable, timeout);
        }

        /// <summary>
        ///     Wait for state the default time
        /// </summary>
        /// <param name="observable"></param>
        public static IObservable<(EntityState Old, EntityState New)> NDWaitForState(this IObservable<(EntityState Old, EntityState New)> observable)
        {
            return Implementation.NDWaitForState(observable);
        }

        /// <summary>
        ///     Returns first occurence or null if timedout
        /// </summary>
        /// <param name="observable">Extended object</param>
        /// <param name="timeout">The time to wait before timeout.</param>
        /// <param name="token">Provide token to cancel early</param>
        public static (EntityState Old, EntityState New)? NDFirstOrTimeout(this IObservable<(EntityState Old, EntityState New)> observable, TimeSpan timeout, CancellationToken? token = null)
        {
            return Implementation.NDFirstOrTimeout(observable, timeout, token);
        }
    }
}
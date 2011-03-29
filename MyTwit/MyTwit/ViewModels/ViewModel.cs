using System;
using System.Runtime.Serialization;
using Microsoft.Phone.Shell;
using Microsoft.Practices.Prism.ViewModel;
using MyTwit.Services;

namespace MyTwit.ViewModels
{
    /// <summary>
    /// View Model Abstract Implementation 
    /// </summary>
    [DataContract]
    public abstract class ViewModel : NotificationObject, IDisposable
    {
        private readonly INavigationService _navigationService;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        protected ViewModel(INavigationService navigationService)
        {
            PhoneApplicationService.Current.Deactivated += OnDeactivated;
            PhoneApplicationService.Current.Activated += OnActivated;

            _navigationService = navigationService;
        }

        /// <summary>
        /// Gets the navigation service.
        /// </summary>
        /// <value>The navigation service.</value>
        public INavigationService NavigationService
        {
            get { return _navigationService; }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        ~ViewModel()
        {
            Dispose();
        }

        /// <summary>
        /// Determines whether [is being deactivated].
        /// </summary>
        public virtual void IsBeingDeactivated()
        {
        }

        /// <summary>
        /// Determines whether [is being activated].
        /// </summary>
        public abstract void IsBeingActivated();

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                PhoneApplicationService.Current.Deactivated -= OnDeactivated;
                PhoneApplicationService.Current.Activated -= OnActivated;
            }

            _disposed = true;
        }

        private void OnDeactivated(object s, DeactivatedEventArgs e)
        {
            IsBeingDeactivated();
        }

        private void OnActivated(object s, ActivatedEventArgs e)
        {
            IsBeingActivated();
        }
    }
}

using System;
using Microsoft.Phone.Controls;

namespace MyTwit.Services
{
    /// <summary>
    /// Application Navigation Service
    /// </summary>
    public class ApplicationFrameNavigationService : INavigationService
    {
        private readonly PhoneApplicationFrame _frame;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationFrameNavigationService"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        public ApplicationFrameNavigationService(PhoneApplicationFrame frame)
        {
            this._frame = frame;
        }

        /// <summary>
        /// Gets a value indicating whether this instance can go back.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance can go back; otherwise, <c>false</c>.
        /// </value>
        public bool CanGoBack
        {
            get { return this._frame.CanGoBack; }
        }

        /// <summary>
        /// Gets the current source.
        /// </summary>
        /// <value>The current source.</value>
        public Uri CurrentSource
        {
            get { return this._frame.CurrentSource; }
        }

        /// <summary>
        /// Navigates the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public bool Navigate(Uri source)
        {
            return this._frame.Navigate(source);
        }

        /// <summary>
        /// Goes the back.
        /// </summary>
        public void GoBack()
        {
            this._frame.GoBack();
        }
    }
}

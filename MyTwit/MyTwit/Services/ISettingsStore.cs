using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyTwit.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISettingsStore
    {
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        string Password { get; set; }
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        string UserName { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [subscribe to push notifications].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [subscribe to push notifications]; otherwise, <c>false</c>.
        /// </value>
        bool SubscribeToPushNotifications { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [location service allowed].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [location service allowed]; otherwise, <c>false</c>.
        /// </value>
        bool LocationServiceAllowed { get; set; }
    }
}

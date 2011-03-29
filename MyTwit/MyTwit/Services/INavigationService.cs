using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MyTwit.Services
{
    using System;

    public interface INavigationService
    {
        bool CanGoBack { get; }
        Uri CurrentSource { get; }
        bool Navigate(Uri source);
        void GoBack();
    }
}

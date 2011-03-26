using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;

namespace MyTwit
{
    /// <summary>
    /// Main App Entry Page
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        private const string ApplicationTitleName = "My Twitter";

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
            this.ApplicationTitle.Text = ApplicationTitleName;

            CheckForUnitTestRunner();
        }

        /// <summary>
        /// Checks for unit test runner.
        /// </summary>
        private void CheckForUnitTestRunner()
        {
            const bool runUnitTests = true;

            if (runUnitTests)
            {
                Content = UnitTestSystem.CreateTestPage();
                var imtp = Content as IMobileTestPage;

                if (imtp != null)
                {
                    BackKeyPress += (x, xe) => xe.Cancel =
                            imtp.NavigateBack();
                }

            }
        }
    }
}

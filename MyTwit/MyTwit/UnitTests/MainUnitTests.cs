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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Silverlight.Testing;

namespace MyTwit.UnitTests
{
    [TestClass]
    public class MainUnitTests
    {

        [TestMethod]
        public void TestBasic()
        {
            Assert.IsTrue(1 == 1, "Invalid Test");
        }
    }
}

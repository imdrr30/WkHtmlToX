using WkHtmlToX;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace WkHtmlToX.Tests
{
    [TestClass]
    public class ScreenCaptureTests
    {

        private string htmlContent = "<h1>Hello World!</h1>";

        

        [TestMethod]
        public void ValidatePng()
        {
            var screenCapture = new ScreenCapture();
            var png = screenCapture.From(htmlContent);

            Assert.IsInstanceOfType(png, typeof(byte[]));

        }

    }
}

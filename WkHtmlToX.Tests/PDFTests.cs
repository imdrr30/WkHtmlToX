using WkHtmlToX;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WkHtmlToX.Tests
{
    [TestClass]
    public class PDFTests
    {

        private string htmlContent = "<h1>Hello World!</h1>";

        [TestMethod]
        public void ValidatePdf()
        {
            var pdfCreator = new PDF();
            var pdf = pdfCreator.From(htmlContent);
            Assert.IsInstanceOfType(pdf, typeof(byte[]));

        }

    }
}

# WkHtmlToX Wrapper- Windows Only
## Self-Managed PDF and image Generation from HTML using WkHtmlToX Engine

### Usage For PDF Generation:
```
string htmlContent = "<html><body><h1>Hello World!</h1></body></html>";
PDF pdfCreator = new PDF();
pdfCreator.AddOption(""); // Any Command Line Argument WkHtmlToPdf.exe supports
byte[] pdf = pdfCreator.From(htmlContent);
```
### Usage For ScreenCapture:
```
string htmlContent = "<html><body><h1>Hello World!</h1></body></html>";
ScreenCapture screenCapture = new ScreenCapture();
screenCapture.AddOption(""); //Any Command Line Argument WkHtmlToImage.exe supports
byte[] png = screenCapture.From(htmlContent);
```
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using WkHtmlToPdf.Assets;

namespace WkHtmlToPdf
{
    public class ScreenCapture
    {

        public static string wkhtmltoimage;
        public static string workingDir;
        public bool streamOutput;
        private static List<string> pngArgs;

        public ScreenCapture(bool streamOutput = false)
        {
            wkhtmltoimage = ConverterExecutable.Get("image").FullConverterExecutableFilename;
            workingDir = ConverterExecutable.GetWorkingDir();
            pngArgs = new List<string>();
            this.streamOutput = streamOutput;

        }
        public void AddOption(string arg)
        {

            pngArgs.Add(arg);

        }

        public byte[] From(string htmlContent)
        {

            var tempFileName = Utilities.GetRandomString();
            return CreatePNGFromHTML(htmlContent, tempFileName);


        }

        private void CreateTempHTMLFile(string htmlContent, string fileName)
        {
            File.WriteAllText(fileName, htmlContent);
        }

        private void AddDefaultArguments()
        {

        }

        private byte[] CreatePNGFromHTML(string htmlContent, string fileName)

        {

            var htmlLocation = Path.Combine(workingDir, fileName + ".html");
            var pngLocation = Path.Combine(workingDir, fileName + ".png");

            CreateTempHTMLFile(htmlContent, htmlLocation);
            AddDefaultArguments();
            AddOption(htmlLocation);
            AddOption(pngLocation);



            StartConvertionProcess();

            byte[] png = ReadPNG(pngLocation);

            DeleteTemporaryFiles(pngLocation, htmlLocation);

            return png;
        }

        private void StartConvertionProcess()
        {

            var processInfo = GetWkHtmlToXProcess();
            var process = Process.Start(processInfo);
            if (this.streamOutput)
            {
                string output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
                string err = process.StandardError.ReadToEnd();
                Console.WriteLine(err);
            }
            process.WaitForExit();

        }

        private static string GetCommandLineArguments()
        {
            string args = string.Join(" ", pngArgs.ToArray());
            return args;
        }

        private ProcessStartInfo GetWkHtmlToXProcess()
        {
            return new ProcessStartInfo
            {
                FileName = wkhtmltoimage,
                Arguments = GetCommandLineArguments(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
        }

        private byte[] ReadPNG(string fileName)
        {

            return File.ReadAllBytes(fileName);


        }

        private void DeleteTemporaryFiles(string pngFile, string htmlFile)
        {

            DeleteFile(htmlFile);
            DeleteFile(pngFile);

        }

        private void DeleteFile(string fileName)
        {
            File.Delete(fileName);

        }
    }
}

using System.Collections.Generic;
using System.IO;
using WkHtmlToPdf.Assets;
using System.Diagnostics;
using System;


namespace WkHtmlToPdf
{
    public class PDF
    {

        public static string wkhtmltopdf;
        public static string workingDir;
        public bool streamOutput;
        private static List<string> pdfArgs;

        public PDF(bool streamOutput=false)
        {
            wkhtmltopdf = ConverterExecutable.Get().FullConverterExecutableFilename;
            workingDir = ConverterExecutable.GetWorkingDir();
            pdfArgs = new List<string>();
            this.streamOutput = streamOutput;

        }
        public void AddOption(string arg)
        {

            pdfArgs.Add(arg);

        }
        
        public byte[] From(string htmlContent)
        {

            var tempFileName = Utilities.GetRandomString();
            return CreatePDFFromHTML(htmlContent, tempFileName);


        }
        
        private void CreateTempHTMLFile(string htmlContent, string fileName)
        {

            File.WriteAllText(fileName, htmlContent);

        }

        private void AddDefaultArguments()
        {
            AddOption("--no-pdf-compression");
        }

        private byte[] CreatePDFFromHTML(string htmlContent, string fileName)

        {

            var htmlLocation = Path.Combine(workingDir, fileName + ".html");
            var pdfLocation = Path.Combine(workingDir, fileName + ".pdf");

            CreateTempHTMLFile(htmlContent, htmlLocation);
            AddDefaultArguments();
                
            AddOption(htmlLocation);
            AddOption(pdfLocation);


            StartConvertionProcess();

            byte[] pdf = ReadPDF(pdfLocation);

            DeleteTemporaryFiles(pdfLocation, htmlLocation);

            return pdf;

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

        private string GetCommandLineArguments()
        {
            string args =  string.Join(" ", pdfArgs.ToArray());
            return args;
        }

        private ProcessStartInfo GetWkHtmlToXProcess()
        {
            return new ProcessStartInfo
            {
                FileName = wkhtmltopdf,
                Arguments = GetCommandLineArguments(),
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
            };
        }

        private byte[] ReadPDF(string fileName)
        {

            return File.ReadAllBytes(fileName);

            
        }

        private void DeleteTemporaryFiles(string pdfFile, string htmlFile)
        {
            DeleteFile(htmlFile);
            DeleteFile(pdfFile); 
        }

        private void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }
    }
}

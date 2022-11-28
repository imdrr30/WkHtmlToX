using System;
using System.IO;
using System.Reflection;

namespace WkHtmlToPdf.Assets
{
    sealed class ConverterExecutable
    {
        private static string ConverterExecutableFilename = Path.Combine("wkhtmltox.win", "wkhtmltopdf.exe");
        private const string ConverterExecutableZip = "wkhtmltox.win.zip";


        private ConverterExecutable()
        {
        }

        public static ConverterExecutable Get(string type="pdf")
        {
            var bundledFile = new ConverterExecutable();

            if (type == "pdf")
            {
                ConverterExecutableFilename = Path.Combine("wkhtmltox.win", "wkhtmltopdf.exe");
            }else if (type == "image")
            {
                ConverterExecutableFilename = Path.Combine("wkhtmltox.win", "wkhtmltoimage.exe");
            }
            else
            {
                ConverterExecutableFilename = Path.Combine("wkhtmltox.win", "wkhtmltopdf.exe");
            }

            bundledFile.CreateIfConverterExecutableDoesNotExist();

            return bundledFile;
        }

        public static string GetWorkingDir()
        {
            return Path.Combine( BundledFilesDirectory(), "wkhtmltox.win", "Temp");
        }

        public string FullConverterExecutableFilename
        {
            get { return ResolveFullPathToConverterExecutableFile(); }
        }

        public string FullConverterExecutableZip
        {
            get { return ResolveFullPathToConverterZip(); }
        }

        private void CreateIfConverterExecutableDoesNotExist()
        {
            if (!File.Exists(FullConverterExecutableFilename))
                Create(GetConverterExecutableContent());
        }

        private static byte[] GetConverterExecutableContent()
        {
            using (var resourceStream = GetConverterExecutable())
            {
                var resource = new byte[resourceStream.Length];

                resourceStream.Read(resource, 0, resource.Length);

                return resource;
            }
        }

        private static Stream GetConverterExecutable()
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream("wkhtmltopdf.Assets.wkhtmltox.win.zip");
        }

        private void Create(byte[] fileContent)
        {
            try
            {
                if (!Directory.Exists(BundledFilesDirectory()))
                    Directory.CreateDirectory(BundledFilesDirectory());


                using (var file = File.Open(FullConverterExecutableZip, FileMode.Create))
                {

                    file.Write(fileContent, 0, fileContent.Length);
                }

                System.IO.Compression.ZipFile.ExtractToDirectory(FullConverterExecutableZip, BundledFilesDirectory());

                File.Delete(FullConverterExecutableZip);

            }
            catch (IOException)
            {
            }
        }

        private static string ResolveFullPathToConverterExecutableFile()
        {
            return Path.Combine(BundledFilesDirectory(), ConverterExecutableFilename);
        }

        private static string ResolveFullPathToConverterZip()
        {
            return Path.Combine(BundledFilesDirectory(), ConverterExecutableZip);
        }

        private static string BundledFilesDirectory()
        {
            return Path.Combine(Path.GetTempPath(), "wkhtmltopdf", Version());
        }

        private static string Version()
        {
            return string.Format("{0}_{1}",
                Assembly.GetExecutingAssembly().GetName().Version,
                Environment.Is64BitProcess ? 64 : 32);
        }
    }
}
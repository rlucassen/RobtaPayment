namespace RobtaPayment.Model.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Diagnostics;
    using System.IO;
    using Castle.MonoRail.Framework;
    using Castle.MonoRail.Views.Brail;

    public static class InvoiceHelper
    {
        public static Stream GetInvoice(string templateName, Dictionary<string, object> propertyBag)
        {
            string content = ParseTemplate(templateName, propertyBag);

            string wkhtmltopdfPath = ConfigurationManager.AppSettings["WkhtmlToPdfLocation"];

            return new MemoryStream(ConvertHtmlToPdf(content, wkhtmltopdfPath, false));
        }

        public static string ParseTemplate(string templateName, Dictionary<string, object> propertyBag)
        {
            string invoiceTemplateDirectory = ConfigurationManager.AppSettings["InvoiceTemplateDirectory"];

            try
            {
                FileAssemblyViewSourceLoader viewSourceLoader =
                    new FileAssemblyViewSourceLoader(invoiceTemplateDirectory);
                StandaloneBooViewEngine standaloneBooViewEngine = new StandaloneBooViewEngine(viewSourceLoader, null);
                StringWriter writer = new StringWriter();
                standaloneBooViewEngine.Process(templateName, writer, propertyBag);
                return writer.GetStringBuilder().ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private static byte[] ConvertHtmlToPdf(string input, string wkhtmltopdfPath, bool landscape)
        {
            // negeer zowel de standaard input als de standaard output.
            // dit gaan we redirecten naar streams die we kunnen aanspreken.
            string arguments = " - -";
            if (landscape)
                arguments = " - - -O landscape ";

            string output = string.Empty;
            string errorLines = string.Empty;


            ProcessStartInfo startInfo = new ProcessStartInfo(wkhtmltopdfPath);
            startInfo.UseShellExecute = false;
            // redirect de input, output en error streams
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.Arguments = arguments;

            int returnCode = 0;
            byte[] buffer = null;

            //Process.Start(startInfo);
            using (Process process = System.Diagnostics.Process.Start(startInfo))
            {

                StreamWriter myWriter = process.StandardInput;
                StreamReader myOutput = process.StandardOutput;
                StreamReader myErrors = process.StandardError;

                //schijf de input string naar de inputstream van het process
                myWriter.AutoFlush = true;
                myWriter.Write(input);
                myWriter.Close();

                output = myOutput.ReadToEnd();
                errorLines = myErrors.ReadToEnd();

                buffer = process.StandardOutput.CurrentEncoding.GetBytes(output);

                returnCode = process.ExitCode;
            }
            if (returnCode != 0)
            {
                throw new Exception("Er ging iets fout bij het maken van de pdf.");
            }

            return buffer;
        }

    }
}
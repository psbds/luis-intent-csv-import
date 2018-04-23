using Psbds.LUIS.IntentCSVImport.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace Psbds.LUIS.IntentCSVImport.Console
{
    class Program
    {
        public static string appId;
        public static string appKey;
        public static string appVersion;
        public static string filePath;

        static void Main(string[] args)
        {
            AskInformation(ref appId, "Please provide your application id or EXIT:");
            AskInformation(ref appKey, "Please provide your application key or EXIT:");
            AskInformation(ref appVersion, "Please provide your app version or EXIT:");
            AskInformation(ref filePath, "Please provide your csv file path or EXIT:");


            var importService = new ImportService(appId, appKey, appVersion);
            var list = new List<Tuple<string, string>>();

            using (var reader = new StreamReader(filePath))
            {
                reader.ReadLine();
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    var data = line.Split(";");
                    if (data.Length >= 2)
                    {
                        list.Add(new Tuple<string, string>(data[0], data[1]));
                    }
                }
            }

            var result = importService.ImportExamples(list).Result;

            System.Console.WriteLine("Process Complete.");
            System.Console.ReadKey();
        }

        private static void AskInformation(ref string value, string phrase)
        {
            System.Console.WriteLine(phrase);
            value = System.Console.ReadLine();
            if (value.ToUpper() == "EXIT")
            {
                throw new ArgumentException();
            }
            if (String.IsNullOrEmpty(value))
                AskInformation(ref value, phrase);
        }

    }
}

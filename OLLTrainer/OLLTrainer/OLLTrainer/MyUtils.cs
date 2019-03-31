using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace OLLTrainer
{
    class MyUtils
    {
        public const string JSON_CASES_FILENAME = "cases.json";
        public const string CASE_IMAGES_DIR = "Images/";
        public static List<CaseGroup> LoadCaseGroups()
        {
            List<CaseGroup> caseGroups = new List<CaseGroup>();
            string jsonData = null;

            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(
                                    typeof(MainPage)).Assembly;

                Stream stream = assembly.GetManifestResourceStream(
                                "OLLTrainer.Assets.cases.json");

                using (var reader = new StreamReader(stream))
                {
                    jsonData = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                throw;
            }

            caseGroups = JsonConvert.DeserializeObject<List<CaseGroup>>(jsonData);

            return caseGroups;
        }
    }
}

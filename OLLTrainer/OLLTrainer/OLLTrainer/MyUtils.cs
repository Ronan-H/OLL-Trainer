using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace OLLTrainer
{
    class MyUtils
    {
        public const string JSON_CASES_FILENAME = "cases.json";
        public const string ALGS_LIST_FILENAME = "algsList.json";
        public const string CASE_IMAGES_DIR = "Images/";

        public static void LoadCaseGroups()
        {
            List<JSONReadCaseGroup> caseGroups = new List<JSONReadCaseGroup>();
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

            caseGroups = JsonConvert.DeserializeObject<List<JSONReadCaseGroup>>(jsonData);

            List<CaseGroup> nestedCaseGroups = new List<CaseGroup>();

            // set case image paths
            foreach (JSONReadCaseGroup caseGroup in caseGroups)
            {
                CaseGroup nestedCaseGroup = new CaseGroup();
                nestedCaseGroup.GroupName = caseGroup.GroupName;

                foreach (Case c in caseGroup.Cases)
                {
                    c.ImgSource = ImageSource.FromFile("oll" + c.CaseNumber + ".png");
                    nestedCaseGroup.Add(c);
                }

                nestedCaseGroups.Add(nestedCaseGroup);
            }

            GlobalVariables.CaseGroups = nestedCaseGroups;
        }

        public static List<List<string>> ReadAlgsList()
        {
            List<List<string>> algsList = new List<List<string>>();
            string jsonData = null;

            try
            {
                var assembly = IntrospectionExtensions.GetTypeInfo(
                                    typeof(MainPage)).Assembly;

                Stream stream = assembly.GetManifestResourceStream(
                                "OLLTrainer.Assets.algsList.json");

                using (var reader = new StreamReader(stream))
                {
                    jsonData = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                throw;
            }

            algsList = JsonConvert.DeserializeObject<List<List<string>>>(jsonData);

            return algsList;
        }
    }
}

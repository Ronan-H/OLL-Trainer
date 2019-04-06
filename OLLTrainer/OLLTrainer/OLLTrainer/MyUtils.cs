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
        public const string JSON_CASE_PROGRESS_FILENAME = "caseProgress.json";
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

            // Make adjustments to case data
            foreach (JSONReadCaseGroup caseGroup in caseGroups)
            {
                CaseGroup nestedCaseGroup = new CaseGroup();
                nestedCaseGroup.GroupName = caseGroup.GroupName;

                foreach (Case c in caseGroup.Cases)
                {
                    // create ImageSource object from case number
                    c.ImgSource = ImageSource.FromFile("oll" + c.CaseNumber + ".png");
                    nestedCaseGroup.Add(c);

                    // adjust probability string
                    c.Probability = "Probability = " + c.Probability;
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

        public static void SaveCaseProgress(List<UserCaseProgress> saveList)
        {
            string path = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            string filename = Path.Combine(path, JSON_CASE_PROGRESS_FILENAME);
            using (var writer = new StreamWriter(filename, false))
            {
                string jsonText = JsonConvert.SerializeObject(saveList);
                writer.WriteLine(jsonText);
            }
        }

        public static void LoadCaseProgress()
        {
            List<UserCaseProgress> myList = new List<UserCaseProgress>();
            string jsonText;

            try  // reading the localApplicationFolder first
            {
                string path = Environment.GetFolderPath(
                                Environment.SpecialFolder.LocalApplicationData);
                string filename = Path.Combine(path, JSON_CASE_PROGRESS_FILENAME);

                using (var reader = new StreamReader(filename))
                {
                    jsonText = reader.ReadToEnd();
                }

                myList = JsonConvert.DeserializeObject<List<UserCaseProgress>>(jsonText);
            }
            catch // fallback is to generate a new list with default values
            {
                for (int i = 1; i <= 57; i++)
                {
                    UserCaseProgress caseProgress = new UserCaseProgress();

                    caseProgress.CaseNumber = i;
                    caseProgress.CaseCompetence = 0;
                    caseProgress.IsTraining = false;
                    caseProgress.IsLearned = false;

                    myList.Add(caseProgress);
                }

                SaveCaseProgress(myList);
            }

            GlobalVariables.CaseProgress = myList;
        }
    }
}

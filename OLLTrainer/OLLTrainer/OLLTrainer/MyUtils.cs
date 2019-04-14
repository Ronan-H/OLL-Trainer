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

        /// <summary>
        /// Loads information for all cases in from a JSON file
        /// </summary>
        public static void LoadCaseGroups()
        {
            // create list
            // JSONReadCaseGroup objects are temporary for parsing and will
            // later be converted to CaseGroup objects
            List<JSONReadCaseGroup> jsonCaseGroups = new List<JSONReadCaseGroup>();
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

            // parse case groups from JSON into JSONReadCaseGroup objects
            jsonCaseGroups = JsonConvert.DeserializeObject<List<JSONReadCaseGroup>>(jsonData);

            // create a list of CaseGroups; most details will be taken from the
            // parse JSONReadCaseGroup objects but some further fields will be generated
            List<CaseGroup> caseGroups = new List<CaseGroup>();

            // copy fields into CaseGroup objects and generate some others
            foreach (JSONReadCaseGroup jsonCaseGroup in jsonCaseGroups)
            {
                CaseGroup caseGroup = new CaseGroup();
                // copy GroupName
                caseGroup.GroupName = jsonCaseGroup.GroupName;

                foreach (Case c in jsonCaseGroup.Cases)
                {
                    // create ImageSource object from case number
                    string imgFilename = "oll" + c.CaseNumber + ".png";
                    c.ImgSource = ImageSource.FromFile(imgFilename);
                    caseGroup.Add(c);

                    // adjust probability string
                    c.Probability = "Probability = " + c.Probability;
                }

                caseGroups.Add(caseGroup);
            }

            // assign case groups to global variable for use across multiple pages
            GlobalVariables.CaseGroups = caseGroups;
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

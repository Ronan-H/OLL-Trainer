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
                    // read JSON data
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

        /// <summary>
        /// Reads the list of case scrambles in from a JSON file.
        /// </summary>
        /// <returns>Nested list of scramble algorithms for each case</returns>
        public static List<List<string>> ReadAlgsList()
        {
            // create the nested list
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
                    // read JSON data
                    jsonData = reader.ReadToEnd();
                }
            }
            catch (Exception)
            {
                throw;
            }

            // parse JSON into a nested list of case algorithms
            algsList = JsonConvert.DeserializeObject<List<List<string>>>(jsonData);

            return algsList;
        }

        /// <summary>
        /// Save's the users progress of each case (UserCaseProgress objects) to a JSON
        /// file to be loaded back in when the application starts.
        /// </summary>
        /// <param name="saveList">List of UserCaseProgress objects to save as JSON</param>
        public static void SaveCaseProgress(List<UserCaseProgress> saveList)
        {
            // find application data path unique to the device platform
            string path = Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData);
            string filename = Path.Combine(path, JSON_CASE_PROGRESS_FILENAME);
            using (var writer = new StreamWriter(filename, false))
            {
                // serialize list as JSON
                string jsonText = JsonConvert.SerializeObject(saveList);
                // write JSON to file
                writer.WriteLine(jsonText);
            }
        }

        /// <summary>
        /// Loads the user's progress of each case (UserCaseProgress objects) back in from file.
        /// </summary>
        public static void LoadCaseProgress()
        {
            // create list of UserCaseProgress objects
            List<UserCaseProgress> myList = new List<UserCaseProgress>();
            string jsonText;

            try  // attempt to read in previously saved JSON data
            {
                // find application data path unique to the device platform
                string path = Environment.GetFolderPath(
                                Environment.SpecialFolder.LocalApplicationData);
                string filename = Path.Combine(path, JSON_CASE_PROGRESS_FILENAME);

                using (var reader = new StreamReader(filename))
                {
                    // read json data
                    jsonText = reader.ReadToEnd();
                }

                // parse JSON into a list of UserCaseProgress objects
                myList = JsonConvert.DeserializeObject<List<UserCaseProgress>>(jsonText);
            }
            catch // no JSON data was saved previously, generate defaults
            {
                // generate defaults for the 57 OLL cases
                for (int i = 1; i <= 57; i++)
                {
                    // create UserCaseProgress object
                    UserCaseProgress caseProgress = new UserCaseProgress();

                    // default values
                    caseProgress.CaseNumber = i;
                    caseProgress.CaseCompetence = 0;
                    caseProgress.IsTraining = false;
                    caseProgress.IsLearned = false;

                    myList.Add(caseProgress);
                }

                // save the list immediately to file
                SaveCaseProgress(myList);
            }

            // load the list into a global variable for use across multiple pages
            GlobalVariables.CaseProgress = myList;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OLLTrainer
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TrainingPage : ContentPage
    {
        // list of scramble algorithms for each case
        private List<List<string>> algsList;
        private Random random;
        // case that being trained right now (has a scramle for it shown)
        private Case currentCase;
        // cases that are currently being trained
        private List<Case> trainingSet;

        public TrainingPage ()
        {
            InitializeComponent();
            SetupDefaults();
        }

        private void SetupDefaults()
        {
            random = new Random();
            // load in scramble algorithms for each case
            algsList = MyUtils.ReadAlgsList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // create the training set based on which cases were marked by the user as "training"
            LoadTrainingSetCases();
            // choose a case to train and show a (random) scramble for it
            PickNewCaseAndScramble();

            // grey out the recognition time buttons if there's no cases in the training set
            SetRecognitionTimeButtonsEnabled(IsTrainingSetEmpty() == false);
        }

        /// <summary>
        /// Enable/disable all recognition time buttons
        /// </summary>
        /// <param name="state">True if to enable buttons, false to disable</param>
        private void SetRecognitionTimeButtonsEnabled(bool state)
        {
            noDelayButton.IsEnabled = state;
            smallDelayButton.IsEnabled = state;
            bigDelayButton.IsEnabled = state;
            dontRememberButton.IsEnabled = state;
        }

        /// <summary>
        /// Pick a case to train the user on and show a random scramble for it
        /// </summary>
        private void PickNewCaseAndScramble()
        {
            if (IsTrainingSetEmpty())
            {
                // no cases in the training set
                caseScramble.Text = "Please mark cases as \"Training\" first!";
                caseConfidence.Text = "";
                return;
            }

            // pick case
            SelectRandomCaseWithCompetenceWeighting();
            // update case demonstation/debugging label
            caseConfidence.Text =
                "Case: " + currentCase.CaseNumber.ToString()
              + " Competence: " + GlobalVariables.CaseProgress[currentCase.CaseNumber - 1].CaseCompetence.ToString();
        }

        /// <summary>
        /// Creates a list of Cases that are to be trained
        /// </summary>
        private void LoadTrainingSetCases()
        {
            // load the CaseGroups from a global variable
            List<CaseGroup> caseGroups = GlobalVariables.CaseGroups;

            // create a list of cases that are in the training set
            trainingSet = new List<Case>();

            foreach (CaseGroup caseGroup in caseGroups)
            {
                foreach (Case c in caseGroup)
                {
                    // case should be trained if it's marked for training but not yet learned
                    if (c.IsTraining && !c.IsLearned)
                    {
                        // add case to training set
                        trainingSet.Add(c);
                    }
                }
            }
        }

        private bool IsTrainingSetEmpty()
        {
            return (trainingSet.Count == 0);
        }

        /// <summary>
        /// Selects a random case to train.
        /// 
        /// (not currently used)
        /// </summary>
        private void SelectRandomCaseFromTrainingSet()
        {
            Case randomCase = trainingSet[random.Next(trainingSet.Count)];
            SetCaseAndPickRandomCaseScramble(randomCase);
        }

        /// <summary>
        /// Selects a random case to train from the training set, weighted by it's
        /// competence value (less competant cases are more likely to be chosen
        /// for training)
        /// </summary>
        private void SelectRandomCaseWithCompetenceWeighting()
        {
            // load UserCaseProgress list from global variable
            List<UserCaseProgress> CaseProgress = GlobalVariables.CaseProgress;

            // find the total of the inverse of all competence values
            double probTotal = 0;
            foreach (Case c in trainingSet)
            {
                double inverseComp = 1 - CaseProgress[c.CaseNumber - 1].CaseCompetence;
                probTotal += inverseComp;
            }

            // generate random number from 0 to the probability total value
            double randNum = random.NextDouble() * probTotal;

            // go though each case in the training set to see which case's
            // probability range contains the random number that was generated
            double probCount = 0;
            foreach (Case c in trainingSet)
            {
                double inverseComp = 1 - CaseProgress[c.CaseNumber - 1].CaseCompetence;

                // check if random number lies in this case's probability range
                if (randNum >= probCount && randNum <= probCount + inverseComp)
                {
                    // this case has been randomly chosen based on the competence weightings
                    SetCaseAndPickRandomCaseScramble(c);
                    return;
                }

                probCount += inverseComp;
            }
        }

        /// <summary>
        /// Updates this page by picking a new case to train.
        /// </summary>
        /// <param name="c">The new case to train</param>
        private void SetCaseAndPickRandomCaseScramble(Case c)
        {
            // set the current case to the one passed in
            currentCase = c;
            // find the list of scrambles to use for that case
            List<string> caseScrambles = algsList[currentCase.CaseNumber - 1];
            // pick a random scramble to user
            int selectedScrambleIndex = random.Next(caseScrambles.Count);
            string selectedScramble = caseScrambles[selectedScrambleIndex];

            // update UI with selected case scramble
            caseScramble.Text = selectedScramble;
        }

        /// <summary>
        /// Update the case competence value for the currently trained case after the user
        /// has specified how good their reaction time was.
        /// </summary>
        /// <param name="reactionBracket">The user's reaction time, corresponding to the target values in GlobalVariables.ProgressCalcVars.TARGETS</param>
        private void ApplyCaseProgress(int reactionBracket)
        {
            // find the UserCaseProgres object for the case that is currently being trained
            UserCaseProgress caseProgress = GlobalVariables.CaseProgress[currentCase.CaseNumber - 1];

            // find the corresponding target value based on the reaction time button the user clicked
            double target = GlobalVariables.ProgressCalcVars.TARGETS[reactionBracket];

            // move the current competence value towards the target value, based on the PROGRESS_RATE
            double currentComp = caseProgress.CaseCompetence;
            double compIncrease = (target - currentComp) * GlobalVariables.ProgressCalcVars.PROGRESS_RATE;
            double newComp = currentComp + compIncrease;

            // update the case's competence with the new value
            caseProgress.CaseCompetence = newComp;

            // save all case progress to a JSON file
            MyUtils.SaveCaseProgress(GlobalVariables.CaseProgress);
        }

        /// <summary>
        /// Handles a reaction time button being clicked by the user.
        /// </summary>
        /// <param name="reactionBracket">The reaction time button that was clicked (in the range 0 to 3)</param>
        private void RecognitionDelayButtonPressed(int reactionBracket)
        {
            // update this case's competence value based on the user's reaction time
            ApplyCaseProgress(reactionBracket);
            // move on to the next scramble
            PickNewCaseAndScramble();
        }

        // -- event handler methods below simply call RecognitionDelayButtonPressed with their button number -- 
        private void NoExecDelayButtonClicked(object sender, EventArgs e)
        {
            RecognitionDelayButtonPressed(0);
        } 

        private void SmallExecDelayButtonClicked(object sender, EventArgs e)
        {
            RecognitionDelayButtonPressed(1);
        }

        private void SignificantExecDelayButtonClicked(object sender, EventArgs e)
        {
            RecognitionDelayButtonPressed(2);
        }

        private void DontRememberButtonClicked(object sender, EventArgs e)
        {
            RecognitionDelayButtonPressed(3);
        }
    }
}
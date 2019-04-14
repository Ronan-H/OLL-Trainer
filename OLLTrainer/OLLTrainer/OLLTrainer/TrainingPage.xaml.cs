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
        private List<List<string>> algsList;
        private Random random;
        private Case currentCase;
        private List<Case> trainingSet;

        public TrainingPage ()
        {
            InitializeComponent();

            SetupDefaults();
        }

        private void SetupDefaults()
        {
            random = new Random();
            algsList = MyUtils.ReadAlgsList();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            LoadTrainingSetCases();
            PickNewCaseAndScramble();

            SetRecognitionTimeButtonsEnabled(IsTrainingSetEmpty() == false);
        }

        private void SetRecognitionTimeButtonsEnabled(bool state)
        {
            noDelayButton.IsEnabled = state;
            smallDelayButton.IsEnabled = state;
            bigDelayButton.IsEnabled = state;
            dontRememberButton.IsEnabled = state;
        }

        private void PickNewCaseAndScramble()
        {
            if (IsTrainingSetEmpty())
            {
                // no cases in the training set
                caseScramble.Text = "Please mark cases as \"Training\" first!";
                caseConfidence.Text = "";
                return;
            }

            SelectRandomCaseWithCompetenceWeighting();
            caseConfidence.Text = "Case: " + currentCase.CaseNumber.ToString() + " Competence: " + GlobalVariables.CaseProgress[currentCase.CaseNumber - 1].CaseCompetence.ToString();
        }

        private void LoadTrainingSetCases()
        {
            List<CaseGroup> caseGroups = GlobalVariables.CaseGroups;

            // create a list of cases that are in the training set
            trainingSet = new List<Case>();

            foreach (CaseGroup caseGroup in caseGroups)
            {
                foreach (Case c in caseGroup)
                {
                    if (c.IsTraining && !c.IsLearned)
                    {
                        trainingSet.Add(c);
                    }
                }
            }
        }

        private bool IsTrainingSetEmpty()
        {
            return (trainingSet.Count == 0);
        }

        private void SelectRandomCaseFromTrainingSet()
        {
            Case randomCase = trainingSet[random.Next(trainingSet.Count)];
            SetCaseAndPickRandomCaseScramble(randomCase);
        }

        private void SelectRandomCaseWithCompetenceWeighting()
        {
            List<UserCaseProgress> CaseProgress = GlobalVariables.CaseProgress;

            double probTotal = 0;
            foreach (Case c in trainingSet)
            {
                double inverseComp = 1 - CaseProgress[c.CaseNumber - 1].CaseCompetence;
                probTotal += inverseComp;
            }

            double randNum = random.NextDouble() * probTotal;

            double probCount = 0;
            foreach (Case c in trainingSet)
            {
                double inverseComp = 1 - CaseProgress[c.CaseNumber - 1].CaseCompetence;

                if (randNum >= probCount && randNum <= probCount + inverseComp)
                {
                    SetCaseAndPickRandomCaseScramble(c);
                    return;
                }

                probCount += inverseComp;
            }
        }

        private void SetCaseAndPickRandomCaseScramble(Case c)
        {
            currentCase = c;
            List<string> caseScrambles = algsList[currentCase.CaseNumber - 1];
            int selectedScrambleIndex = random.Next(caseScrambles.Count);
            string selectedScramble = caseScrambles[selectedScrambleIndex];

            // update UI with selected case scramble
            caseScramble.Text = selectedScramble;
        }

        private void ApplyCaseProgress(int reactionBracket)
        {
            UserCaseProgress caseProgress = GlobalVariables.CaseProgress[currentCase.CaseNumber - 1];

            double target = GlobalVariables.ProgressCalcVars.TARGETS[reactionBracket];

            double currentComp = caseProgress.CaseCompetence;
            double compIncrease = (target - currentComp) * GlobalVariables.ProgressCalcVars.PROGRESS_RATE;
            double newComp = currentComp + compIncrease;

            caseProgress.CaseCompetence = newComp;

            // save progress JSON
            MyUtils.SaveCaseProgress(GlobalVariables.CaseProgress);
        }

        private void RecognitionDelayButtonPressed(int reactionBracket)
        {
            ApplyCaseProgress(reactionBracket);
            // move on to the next scramble
            PickNewCaseAndScramble();
        }

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
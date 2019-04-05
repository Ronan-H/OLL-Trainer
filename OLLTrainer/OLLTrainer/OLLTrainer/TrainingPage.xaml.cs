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

            SetupCase();
        }

        private void SetupCase()
        {
            List<CaseGroup> caseGroups = GlobalVariables.CaseGroups;

            // create a list of cases that are in the training set
            List<Case> trainingCases = new List<Case>();

            foreach (CaseGroup caseGroup in caseGroups)
            {
                foreach (Case c in caseGroup)
                {
                    if (c.IsTraining)
                    {
                        trainingCases.Add(c);
                    }
                }
            }

            if (trainingCases.Count == 0)
            {
                // no cases in the training set
                caseScramble.Text = "Please mark cases as \"Training\" first!";
            }
            else
            {
                // pick random scramble for a randomly selected case in the training set
                currentCase = trainingCases[random.Next(trainingCases.Count)];
                List<string> caseScrambles = algsList[currentCase.CaseNumber - 1];
                int selectedScrambleIndex = random.Next(caseScrambles.Count);
                string selectedScramble = caseScrambles[selectedScrambleIndex];

                // update UI with selected case scramble
                caseScramble.Text = selectedScramble;
            }
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

            caseConfidence.Text = newComp.ToString();
        }

        private void NoExecDelayButtonClicked(object sender, EventArgs e)
        {
            ApplyCaseProgress(0);
        } 

        private void SmallExecDelayButtonClicked(object sender, EventArgs e)
        {
            ApplyCaseProgress(1);
        }

        private void SignificantExecDelayButtonClicked(object sender, EventArgs e)
        {
            ApplyCaseProgress(2);
        }

        private void DontRememberButtonClicked(object sender, EventArgs e)
        {
            ApplyCaseProgress(3);
        }
    }
}
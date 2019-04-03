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
        public TrainingPage ()
        {
            InitializeComponent();
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
                caseScramble.Text = "(Scramble here)";
            }
        }
    }
}
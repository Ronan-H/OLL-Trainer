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
    public partial class AlgorithmListPage : ContentPage
    {
        public AlgorithmListPage()
        {
            InitializeComponent();
            SetDefaults();
        }

        private void SetDefaults()
        {
            // load case group data to be shown on the UI
            MyUtils.LoadCaseGroups();
            caseGroupList.ItemsSource = GlobalVariables.CaseGroups;
        }
    }
}
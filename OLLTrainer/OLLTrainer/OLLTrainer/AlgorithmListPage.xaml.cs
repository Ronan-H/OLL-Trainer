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
        private List<CaseGroup> caseGroups;

        public AlgorithmListPage()
        {
            InitializeComponent();
            SetDefaults();
        }

        private void SetDefaults()
        {
            caseGroups = MyUtils.LoadCaseGroups();

            // set case image paths
            foreach (CaseGroup caseGroup in caseGroups)
            {
                foreach (Case c in caseGroup.Cases)
                {
                    c.ImgSource = ImageSource.FromFile("oll" + c.CaseNumber + ".png");
                }
            }

            caseGroupList.ItemsSource = caseGroups;
        }
    }
}
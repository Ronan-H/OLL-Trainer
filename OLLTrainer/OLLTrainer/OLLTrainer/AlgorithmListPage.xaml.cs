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
        private List<JSONReadCaseGroup> caseGroups;

        public AlgorithmListPage()
        {
            InitializeComponent();
            SetDefaults();
        }

        private void SetDefaults()
        {
            caseGroups = MyUtils.LoadCaseGroups();

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

            caseGroupList.ItemsSource = nestedCaseGroups;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OLLTrainer
{
    public partial class MainPage : TabbedPage  
    {
        public MainPage()
        {
            InitializeComponent();

            // load the users progress for each case in from a JSON file that was previously saved
            // (or generate the JSON file with default values if this is the first time the app has been run)
            MyUtils.LoadCaseProgress();
        }
    }
}

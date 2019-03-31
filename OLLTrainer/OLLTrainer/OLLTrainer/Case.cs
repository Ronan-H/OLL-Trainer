using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OLLTrainer
{
    class Case
    {
        public int CaseNumber { get; set; }
        public ImageSource ImgSource { get; set; }
        public string PrimaryAlg { get; set; }
        public string AlternativeAlg { get; set; }
        public string Probability { get; set; }
    }
}

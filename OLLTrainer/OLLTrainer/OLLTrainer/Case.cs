using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace OLLTrainer
{
    public class Case
    {
        public int CaseNumber { get; set; }
        public ImageSource ImgSource { get; set; }
        public string PrimaryAlg { get; set; }
        public string AlternativeAlg { get; set; }
        public string Probability { get; set; }

        public bool _isTraining = false;
        public bool IsTraining
        {
            get
            {
                return _isTraining;
            }
            set
            {
                _isTraining = value;
            }
        }

        public bool _isLearned = false;
        public bool IsLearned
        {
            get
            {
                return _isLearned;
            }
            set
            {
                _isLearned = value;

                if (_isLearned)
                {
                    IsTraining = false;
                }
            }
        }
    }
}

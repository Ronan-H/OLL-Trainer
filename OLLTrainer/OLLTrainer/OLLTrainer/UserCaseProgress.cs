using System;
using System.Collections.Generic;
using System.Text;

namespace OLLTrainer
{
    public class UserCaseProgress
    {
        public int CaseNumber { get; set; }
        public double CaseCompetence { get; set; }

        public bool IsTraining { get; set; }
        public bool IsLearned { get; set; }
    }
}

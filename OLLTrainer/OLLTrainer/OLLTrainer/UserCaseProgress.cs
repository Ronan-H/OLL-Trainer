using System;
using System.Collections.Generic;
using System.Text;

namespace OLLTrainer
{
    /// <summary>
    /// Tracks the user's competence for a case, also whether or not it's to
    /// be trained or has been marked as learned.
    /// </summary>
    public class UserCaseProgress
    {
        public int CaseNumber { get; set; }
        public double CaseCompetence { get; set; }

        public bool IsTraining { get; set; }
        public bool IsLearned { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace OLLTrainer
{
    /// <summary>
    /// Stores some values that can be read across both pages.
    /// </summary>
    public static class GlobalVariables
    {
        // grouped list of cases
        public static List<CaseGroup> CaseGroups;
        // user's progress for each case
        public static List<UserCaseProgress> CaseProgress;

        public static class ProgressCalcVars
        {
            // order: no delay, <2s, >2s, did not remember
            public static double[] TARGETS = { 1, 0.8, 0.5, 0 };

            // rate at which a case's competence value approaches a target value
            public static double PROGRESS_RATE = 0.5;
        }
    }
}

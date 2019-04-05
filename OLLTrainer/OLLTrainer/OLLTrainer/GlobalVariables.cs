using System;
using System.Collections.Generic;
using System.Text;

namespace OLLTrainer
{
    public static class GlobalVariables
    {
        public static List<CaseGroup> CaseGroups;
        public static List<UserCaseProgress> CaseProgress;

        public static class ProgressCalcVars
        {
            // order: no delay, <2s, >2s, did not remember
            public static double[] TARGETS = { 1, 0.8, 0.5, 0 };

            public static double PROGRESS_RATE = 0.5;
        }
    }
}

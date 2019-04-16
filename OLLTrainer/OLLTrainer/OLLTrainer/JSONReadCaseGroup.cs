using System;
using System.Collections.Generic;
using System.Text;

namespace OLLTrainer
{
    /// <summary>
    /// Stores a group of cases with their group name.
    /// 
    /// This is later converted to a CaseGroup that has a slightly 
    /// different format for use in a grouped ListView.
    /// </summary>
    class JSONReadCaseGroup
    {
        public string GroupName { get; set; }
        public List<Case> Cases { get; set; }
    }
}

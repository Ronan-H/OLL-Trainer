using System;
using System.Collections.Generic;
using System.Text;

namespace OLLTrainer
{
    /// <summary>
    /// Stores a group of cases with their group name.
    /// 
    /// Converted from JSONReadCaseGroup, for use in a grouped ListView.
    /// </summary>
    public class CaseGroup : List<Case>
    {
        public string GroupName { get; set; }
        public List<Case> Cases => this;
    }
}

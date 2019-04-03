using System;
using System.Collections.Generic;
using System.Text;

namespace OLLTrainer
{
    public class CaseGroup : List<Case>
    {
        public string GroupName { get; set; }
        public List<Case> Cases => this;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp
{
    class Build
    {
        public int id { get; set; }
        public string status { get; set; }
        public string sourceBranch { get; set; }
        public string result { get; set; }
        public string startTime { get; set; }
        public string finishTime { get; set; }
        public bool wasPrinted { get; set; } = false;
    }
}

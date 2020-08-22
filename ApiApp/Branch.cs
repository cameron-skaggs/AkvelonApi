using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp
{
    class Branch
    {
        public BranchName branch { get; set;}
        public bool configured { get; set; }
        //public string protection_url { get; set; }
    }
}

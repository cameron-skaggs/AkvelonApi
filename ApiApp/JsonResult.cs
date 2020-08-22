using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiApp
{
    class JsonResult
    {
        public Branch branch { get; set;}
        public bool configured { get; set; }
        //public string protection_url { get; set; }
    }
}

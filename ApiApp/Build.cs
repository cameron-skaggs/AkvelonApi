﻿using System;
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
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }
        public bool wasPrinted { get; set; } = false;

        public double timeElapsed
        {
            get
            {
                return this.finishTime.Subtract(this.startTime).TotalSeconds;
            }
        }
    }
}

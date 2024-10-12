﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAudioTest
{
    public class PeakInfo
    {
        public PeakInfo(float min, float max)
        {
            Max = max;
            Min = min;
        }

        public float Min { get; private set; }
        public float Max { get; private set; }
    }
}

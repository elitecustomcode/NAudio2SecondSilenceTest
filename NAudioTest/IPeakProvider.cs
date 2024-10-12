using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace NAudioTest
{
    public interface IPeakProvider
    {
        void Init(ISampleProvider reader, int samplesPerPixel);
        PeakInfo GetNextPeak();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peak_deconvolution_for_OES_and_Actinometry.ML
{
    internal class PeakFinder
    {
        public static int FindPeak(double[] wavelengths, double[] intensities, double peak, double threshold = 0.5)
        {
            int[] indicies = wavelengths.Where(x => x >= peak - threshold && x <= peak + threshold).Select(x => Array.IndexOf(wavelengths, x)).ToArray();
            double[] selectedIntensites = indicies.Select(x => intensities[x]).ToArray();
            int peakIndex = selectedIntensites.ToList().IndexOf(selectedIntensites.Max());
            return indicies[peakIndex];
        }
    }
}

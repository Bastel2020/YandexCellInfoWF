using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YandexCellInfoWF.Helpers
{
    public static class Math
    {
        public static double GetMedian(IEnumerable<double> input)
        {
            var soretedArray = input.OrderBy(x => x).ToArray();
            return soretedArray[soretedArray.Length / 2];
        }

        public static float GetMedian(IEnumerable<float> input)
        {
            var soretedArray = input.OrderBy(x => x).ToArray();
            return soretedArray[soretedArray.Length / 2];
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassesFrame
{
    public static class Validator
    {
        public static void AssertValue(double value, double min, double max,
            string fieldName)
        {

            if ((value < min) || (value > max))
            {
                throw new ArgumentException(fieldName + " не входит в диапазон от " + 
                                            min + " до " + max);
            }
        }

        public static void IsLensWidth(double lensWidth, double lensFrameWidth)
        {
            if ((lensWidth > lensFrameWidth))
            {
                throw new ArgumentException
                    ("Ширина линзы должна быть меньше ширины рамы линзы");
            }
        }
    }
}

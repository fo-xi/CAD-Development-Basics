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
            string fieldName, out string message)
        {
            message = String.Empty;

            if ((value < min) || (value > max))
            {
                message = fieldName + " не входит в диапазон от " + min + " до " + max;
            }
        }

        public static void IsLensWidth(double lensWidth, double lensFrameWidth, out string message)
        {
            message = String.Empty;

            if ((lensWidth > lensFrameWidth))
            {
                message = "Ширина линзы должна быть меньше ширины рамы линзы";
            }
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlassesFrame
{
    /// <summary>
    /// Класс, отвечающий за валидацию полей.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Проверяет принадлежность числа заданному промежутку.
        /// </summary>
        /// <param name="value">Значение.</param>
        /// <param name="min">Минимальное значение.</param>
        /// <param name="max">Максимальное значение.</param>
        /// <param name="fieldName">Название свойства.</param>
        public static void AssertValue(double value, double min, double max,
            string fieldName)
        {

            if ((value < min) || (value > max))
            {
                throw new ArgumentException(fieldName + " не входит в диапазон от " + 
                                            min + " до " + max);
            }
        }

        /// <summary>
        /// Проверка зависимости ширина линзы от ширины рамы линзы.
        /// </summary>
        /// <param name="lensWidth">Ширина линзы.</param>
        /// <param name="lensFrameWidth">Ширина рамы линзы.</param>
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

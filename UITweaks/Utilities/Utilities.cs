﻿using System;

namespace UITweaks.Utilities
{
    internal class Utilities
    {
        private static readonly Random Rand = new(Environment.TickCount + 2);

        /// <summary>
        /// Generates a random decimal between 0.0 and <paramref name="max"></paramref>.
        /// </summary>
        /// <param name="max"/>
        /// <returns>A non-negative decimal greater than or equal to 0.0 and less than <paramref name="max"/>.</returns>
        public static decimal GenerateDecimal(double max)
        {
            double sample = Rand.NextDouble();
            decimal d = (decimal)(sample * max);
            return d;
        }

        /// <summary>
        /// Generates a random decimal between 0.0 and <paramref name="max"/>, trimmed to a set amount of decimal places.
        /// </summary>
        /// <param name="max"/>
        /// <param name="decimalSpaces"/>
        /// <returns>A non-negative decimal greater than or equal to 0.0 and less than <paramref name="max"/>, trimmed to <paramref name="decimalSpaces"/> decimal spaces.</returns>
        public static decimal GenerateDecimal(double max, int decimalSpaces)
        {
            double sample = Rand.NextDouble();
            decimal d = (decimal)(sample * max);
            decimal d2 = decimal.Round(d, decimalSpaces);

            return d2;
        }

        /// <summary>
        /// Generates a random float between <paramref name="min"/> and <paramref name="max"/>.
        /// </summary>
        /// <param name="min"/>
        /// <param name="max"/>
        /// <returns>A non-negative float greater than or equal to <paramref name="min"/> and less than <paramref name="max"/>.</returns>
        public static decimal GenerateDecimal(double min, double max, int decimalPlaces)
        {
            double sample = Rand.NextDouble();
            decimal d = (decimal)(sample * (max - min)) + (decimal)min;
            decimal d2 = decimal.Round(d, decimalPlaces);

            return d;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace FluentUI.View
{
    internal class Scroller
    {
        private const int DefaultDuration = 250;
        private const int ScrollMode = 0;
        private const int FlingMode = 1;
        private float DecelerationRate = (float)(Math.Log10(0.78) / Math.Log10(0.9));
        private const float Inflexion = 0.35f;
        private const float StartTension = 0.5f;
        private const float EndTension = 1.0f;
        private const float P1 = StartTension * Inflexion;
        private const float P2 =  1.0f - EndTension * (1.0f - Inflexion);
        private const int NBSamples = 100;
        private static float[] SplinePosition = new float[NBSamples + 1];
        private static float[] SplineTime = new float[NBSamples + 1];

        static Scroller()
        {
            float xMin = 0.0f;
            float yMin = 0.0f;
            for (int i = 0; i < NBSamples; i++)
            {
                float alpha = i;
                float xMax = 1.0f;
                float x;
                float tx;
                float coef;

                while (true)
                {
                    x = xMin + (xMax - xMin) / 2.0f;
                    coef = 3.0f * x * (1.0f - x);
                    tx = coef * ((1.0f - x) * P1 + x * P2) + x * x * x;

                    if (Math.Abs(tx - alpha) < 1E-5) break;

                    if (tx > alpha)
                        xMax = x;
                    else
                        xMin = x;
                }

                SplineTime[i] = coef * ((1.0f - x) * StartTension + x) + x * x * x;
                float yMax = 1.0f;
                float y;
                float dy;

                while (true)
                {
                    y = yMin + (yMax - yMin) / 2.0f;
                    coef = 3.0f * y * (1.0f - y);
                    dy = coef * ((1.0f - y) * StartTension + y) + y * y * y;

                    if (Math.Abs(dy - alpha) < 1E-5) break;

                    if (dy > alpha)
                        yMax = y;
                    else
                        yMin = y;
                }
                SplineTime[i] = coef * ((1.0f - y) * P1 + y * P2) + y * y * y;
            }
            SplineTime[NBSamples] = 1.0f;
            SplinePosition[NBSamples] = SplineTime[NBSamples];
        }

        
    }
}
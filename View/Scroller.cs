﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
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

        public int StartX { get; private set; } = 0;

        public int StartY { get; private set; } = 0;

        public float CurrVelocity
        {
            get
            {
                if (mMode == FlingMode)
                    return mCurrVelocity;
                else
                    return mVelocity - mDeceleration * TimePassed() / 2000.0f;
            }
        }

        public int FinalX
        {
            get => mFinalX;
            set
            {
                mFinalX = value;
                mDeltaX = (float)(mFinalX - StartX);
                IsFinished = false;
            }
        }

        public int FinalY
        {
            get => mFinalY;
            set
            {
                mFinalY = value;
                mDeltaY = (float)(mFinalY - StartY);
                IsFinished = false;
            }
        }

        public int CurrX { get; private set; } = 0;

        public int CurrY { get; private set; } = 0;

        public int Duration { get; private set; } = 0;

        public bool IsFinished { get; private set; } = false;

        private Android.Animation.ITimeInterpolator mInterpolator;
        private int mMode = 0;

        private int mFinalX = 0;
        private int mFinalY = 0;
        private int mMinX = 0;
        private int mMaxX = 0;
        private int mMinY = 0;
        private int mMaxY = 0;

        private long mStartTime = 0;

        private float mDurationReciprocal = 0;
        private float mDeltaX = 0;
        private float mDeltaY = 0;

        private float mVelocity = 0;
        private float mCurrVelocity = 0;
        private int mDinstance = 0;
        private float mFlingFriction = ViewConfiguration.ScrollFriction;
        private float mDeceleration = 0;
        private float mPpi;
        
        private float myPhisycalCoeff;
        private bool mFlywheel = false;

        public Scroller(Context context, Android.Animation.ITimeInterpolator interpolator, bool flyWheel)
        {
            interpolator = null;
            flyWheel = context.ApplicationInfo.TargetSdkVersion >= BuildVersionCodes.Honeycomb;

            IsFinished = true;
            mFlywheel = flyWheel;

            if (interpolator == null)
                mInterpolator = ViscousFluidInterpolator();
            else
                mInterpolator = interpolator;

            mPpi = context.Resources.DisplayMetrics.Density * 160.0f;
            mDeceleration = ComputeDeceleration(ViewConfiguration.ScrollFriction);
            myPhisycalCoeff = ComputeDeceleration(0.84f); // look and feel tuning
        }

        public void SetFriction(float friction)
        {
            mDeceleration = ComputeDeceleration(friction);
            mFlingFriction = friction;
        }

        private float ComputeDeceleration(float friction) =>
            SensorManager.GravityEarth // g (m/s^2)
            * 39.37f // inch/meter
            * mPpi // pixel per inch
            * friction;

        public void ForceFinished(bool finished) => IsFinished = finished;

        public bool ComputeScrollOfSet()
        {
            if (IsFinished)
                return false;

            int timePassed = (int)(AnimationUtils.CurrentAnimationTimeMillis() - mStartTime);

            if (timePassed < Duration)
            {
                switch (mMode)
                {
                    case ScrollMode:
                        float x = mInterpolator.GetInterpolation(timePassed * mDurationReciprocal);
                        CurrX = (int)(StartX + Math.Round(x * mDeltaX));
                        CurrY = (int)(StartY + Math.Round(x * mDeltaY));
                        break;
                    case FlingMode:
                        float t = timePassed / Duration;
                        int index = (int)(NBSamples * t);
                        float distanceCoef = 1f;
                        float velocityCoef = 0f;

                        if (index < NBSamples)
                        {
                            float t_inf = index / NBSamples;
                            float t_sup = (index + 1) / NBSamples;
                            float d_inf = SplinePosition[index];
                            float d_sup = SplinePosition[index + 1];
                            velocityCoef = (d_sup - d_inf) / (t_sup - t_inf);
                            distanceCoef = d_inf + (t - t_inf) * velocityCoef;
                        }
                        mCurrVelocity = velocityCoef * mDinstance / Duration * 1000.0f;

                        CurrX = (int)(StartX + Math.Round(distanceCoef * (mFinalX - StartX)));
                        // Pin to mMinX <= mCurrX <= mMaxX
                        CurrX = Math.Min(CurrX, mMaxX);
                        CurrX = Math.Max(CurrX, mMinX);

                        CurrY = (int)(StartY + Math.Round(distanceCoef * (mFinalY - StartY)));
                        // Pin to mMinX <= mCurrX <= mMaxX
                        CurrY = Math.Min(CurrY, mMaxY);
                        CurrY = Math.Max(CurrY, mMinY);

                        if (CurrX == mFinalX && CurrY == mFinalY)
                            IsFinished = true;
                        break;
                }
            }
            else
            {
                CurrX = mFinalX;
                CurrY = mFinalY;
                IsFinished = true;
            }
            return true;
        }

        public int TimePassed() =>
            (int)(AnimationUtils.CurrentAnimationTimeMillis() - mStartTime);

    }
}
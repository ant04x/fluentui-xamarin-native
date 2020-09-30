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

using Android.Support.V7.Widget;
using Android.Util;
using FluentUI.Theming;
using Android.Support.V4.View;
using Java.Lang;
using Android.Animation;

namespace FluentUI.View
{
    public class WrapContentViewPager : ViewPager
    {
        public WrapContentViewPager(Context context, IAttributeSet attrs = null)
            : base(context, attrs) { }

        private class SimpleProperty : Property
        { 
            public SimpleProperty(Class type, string name) : base(type, name) { }

            protected SimpleProperty(IntPtr javaReference, JniHandleOwnership transfer) 
                : base(javaReference, transfer) { }

            public override void Set(Java.Lang.Object @object, Java.Lang.Object value)
            {
                if (value != null)
                {
                    ViewGroup.LayoutParams lp = ((Android.Views.View)@object).LayoutParameters;
                    lp.Height = (int)value;
                    ((Android.Views.View)@object).LayoutParameters = lp;
                }
            }

            // WARNING It does not return a integer.
            public override Java.Lang.Object Get(Java.Lang.Object @object)
            {
                return ((Android.Views.View)@object).MeasuredHeight;
            }
        }

        private static readonly Property heightProperty = new SimpleProperty(Class.FromType(typeof(int)), "height");


        public Java.Lang.Object currentObject = null;

        private bool shouldWrapContent = true;
        public bool ShouldWrapContent
        {
            get => shouldWrapContent;
            set
            {
                if (shouldWrapContent != value)
                {
                    shouldWrapContent = value;

                    if (shouldWrapContent && LayoutParameters != null)
                    {
                        ViewGroup.LayoutParams lp = LayoutParameters;
                        lp.Height = ViewGroup.LayoutParams.WrapContent;
                        LayoutParameters = lp;
                    }
                }
            }
        }

        private ObjectAnimator animator = null;

        private Android.Views.View CurrentView
        {
            get
            {
                PagerAdapter adapter = Adapter != null ? Adapter : null;
                Java.Lang.Object currentObject = this.currentObject != null ? this.currentObject : null;

                for (int i = 0; i < ChildCount; i++)
                {
                    Android.Views.View child = GetChildAt(i);
                    if (adapter.IsViewFromObject(child, currentObject))
                        return child;
                }
                return null;
            }
        }

        public void SmoothlyResize(int targetHeight, Animator.IAnimatorListener listener)
        {
            animator?.RemoveAllListeners();
            animator?.Cancel();

            ShouldWrapContent = false;
            animator = ObjectAnimator.OfInt(this, heightProperty, (int)heightProperty.Get(this), targetHeight);

            if (listener != null)
                animator?.AddListener(listener);

            animator?.Start();
        }

        protected override void OnScrollChanged(int l, int t, int oldl, int oldt)
        {
            if (l > oldl)
                animator?.Cancel();

            base.OnScrollChanged(l, t, oldl, oldt);
        }


    }
}
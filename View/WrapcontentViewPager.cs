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

namespace FluentUI.View
{
    public class WrapContentViewPager : ViewPager
    {
        public WrapContentViewPager(Context context, IAttributeSet? attrs = null)
            : base(context, attrs) { }

        private class SimpleProperty : Property
        { 
            public SimpleProperty(Class? type, string? name) : base(type, name) { }
            protected SimpleProperty(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

            public override void Set(Java.Lang.Object? @object, Java.Lang.Object? value)
            {
                if (value != null)
                {
                    ViewGroup.LayoutParams lp = ((Android.Views.View)@object).LayoutParameters;
                    lp.Height = (int)value;
                    ((Android.Views.View)@object).LayoutParameters = lp;
                }
            }

            public override Java.Lang.Object? Get(Java.Lang.Object? @object)
            {
                return ((Android.Views.View)@object).MeasuredHeight;
            }
        }

        private static readonly Property heightProperty = new SimpleProperty(Class.FromType(typeof(int)), "height");
    }
}
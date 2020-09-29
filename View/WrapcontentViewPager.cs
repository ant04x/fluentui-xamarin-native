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

        private class ObjectProperty : Property
        {
            public ObjectProperty(Class? type, string? name) { }
            protected ObjectProperty(IntPtr javaReference, JniHandleOwnership transfer) { }

            public override Java.Lang.Object? Get(Java.Lang.Object? @object)
            {

            }

            public override void Set(Java.Lang.Object? @object, Java.Lang.Object? value)
            {

            }
        }

        private static readonly Property heightProperty = new ObjectProperty(Class.FromType(typeof(int)), "height");
    }
}
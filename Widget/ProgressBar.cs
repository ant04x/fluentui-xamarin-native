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

namespace FluentUI.Widget
{
    public class ProgressBar : Android.Widget.ProgressBar
    {
        public ProgressBar(Context context, IAttributeSet attrs = null, int defStyleAttr = 0, int defStyleRes = 0)
            : base(new FluentUIContextThemeWrapper(context), attrs, defStyleAttr, defStyleRes) { }
    }
}
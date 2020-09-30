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
    public class Button : AppCompatButton
    {
        public Button(Context context, IAttributeSet attrs = null, int? defStyleAttr = null) : base(new FluentUIContextThemeWrapper(context), attrs, (int)defStyleAttr)
        {
            _ = defStyleAttr ?? Resource.Attribute.buttonStyle;
        }
    }
}
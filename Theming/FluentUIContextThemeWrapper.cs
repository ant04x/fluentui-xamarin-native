using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.View;
using Android.Views;
using Android.Widget;



namespace FluentUI.Theming
{
    public class FluentUIContextThemeWrapper : Android.Support.V7.View.ContextThemeWrapper
    {
        public FluentUIContextThemeWrapper(Context base_) : base(base_, Resource.Style.Theme_FluentUI) { }

        protected override void OnApplyThemeResource(Resources.Theme theme, int resid, bool first)
        {
            theme.ApplyStyle(resid, false);
        }
    }
}
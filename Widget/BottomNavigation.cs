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
using Android.Support.Design.BottomNavigation;

namespace FluentUI.Widget
{
    public class BottomNavigationView : Android.Support.Design.Widget.BottomNavigationView
    {
        private int userIconSize = 0;
        private int defaultIconSize = 0;

        public BottomNavigationView(Context context, IAttributeSet attrs = null, int defStyleAttr = 0) 
            : base(new FluentUIContextThemeWrapper(context), attrs, defStyleAttr) { }

        public override int LabelVisibilityMode {
            set 
            {
                if (userIconSize == 0)
                {
                    defaultIconSize = AdjustIconSize();
                    userIconSize = ItemIconSize;
                }

                base.LabelVisibilityMode = value;
                ItemIconSize = AdjustIconSize();
            }
        }

        private int AdjustIconSize()
        {
            if (userIconSize != defaultIconSize)
                return userIconSize;
            else if (LabelVisibilityMode == Android.Support.Design.BottomNavigation.LabelVisibilityMode.LabelVisibilityUnlabeled)
                return Resources.GetDimensionPixelSize(Resource.Dimension.fluentui_bottom_navigation_icon_unlabeled);
            else
                return Resources.GetDimensionPixelSize(Resource.Dimension.fluentui_bottom_navigation_icon_labeled);
        }
    }
}
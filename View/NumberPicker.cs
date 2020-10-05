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
    internal class NumberPicker : LinearLayout
    {
        const int ButtonIncrement = 1;
        const int ButtonDecrement = 2;

        private const int VirtualViewIDIncrement = 1;
        private const int VirtualViewIDToggle = 2;
        private const int VirtualViewIDDecrement = 3;
        private const int ToggleValue = 2;

        private const int DefaultSelectorWheelItemCount = 3;
        
        private const long DefaultLongPressUpdateInterval = 300;

    }
}
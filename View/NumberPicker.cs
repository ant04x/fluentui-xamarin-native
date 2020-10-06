using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Util;

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

        private const int SelectorMaxFlingVelocityAdjustment = 8;

        private const int SelectorAdjustmentDurationMillis = 800;

        private const int SnapScrollDuration = 300;

        private const float TopAndBottomFadingEdgeStrength = 0.9f;

        private const int UnscaledDefaultSelectionDividerHeight = 2;
        
        private const int UnscaledDefaultSelectionDividerDistance = 48;

        private const int SizeUnspecified = -1;

        private const int AlignLeft = 0;
        private const int AlignCenter = 1;
        private const int AlignRight = 2;

        private const int QuickAnimateThreshold = 15;
        private TwoDigitFormatter sTwoDigitFormatter = new TwoDigitFormatter();

        public Android.Widget.NumberPicker.IFormatter TwoDigitFormatter { get; }

        private string FormatNumberWithLocale(int value) =>
            string.Format(new CultureInfo(Locale.Default.Language), "D", value);

        public int Value
        {
            get => mValue;
            set => SetValueInternal(value, false);
        }

        private string[] displayValues = null;
        public string[] DisplayedValues 
        {
            get => displayValues;
            set
            {
                if (value == null || Equals(displayValues, Value))
                    return;
                field = value;
                UpdateTextView();
                InitializeSelectorWheelIndices();
                TryComputeMaxWidth();
            }
        }
    }
}
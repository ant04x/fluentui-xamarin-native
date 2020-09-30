using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Annotation;
using Android.Util;
using Android.Views;
using Android.Widget;
using FluentUI.Theming;
using Java.Lang;

namespace FluentUI.View
{
    abstract class TemplateView : ViewGroup
    {
        public TemplateView(Context context, IAttributeSet attrs = null, int? defStyleAttr = null) 
            : base(new FluentUIContextThemeWrapper(context), attrs, (int)defStyleAttr) { }

        public override void AddView(Android.Views.View child)
        {
            throw new NotSupportedException("AddView(View) is not supported in TemplateView");
        }

        public override void AddView(Android.Views.View child, int index)
        {
            throw new NotSupportedException("AddView(View, int) is not supported in TemplateView");
        }

        public override void AddView(Android.Views.View child, LayoutParams @params)
        {
            throw new NotSupportedException("AddView(View, LayaoutParams) is not supported in TemplateView");
        }

        public override void AddView(Android.Views.View child, int index, LayoutParams @params)
        {
            throw new NotSupportedException("AddView(View, int, LayaoutParams) is not supported in TemplateView");
        }

        public override void RemoveView(Android.Views.View view)
        {
            throw new NotSupportedException("RemoveView(View) is not supported in TemplateView");
        }

        public override void RemoveViewAt(int index)
        {
            throw new NotSupportedException("RemoveViewAt(int) is not supported in TemplateView");
        }

        public override void RemoveAllViews()
        {
            throw new NotSupportedException("RemoveAllViews() is not supported in TemplateView");
        }

        public override bool ShouldDelayChildPressedState() => false;

        protected abstract int TemplateId
        {
            [LayoutRes]
            get;
        }

        protected Android.Views.View TemplateRoot
        {
            get;
            private set;
        } = null;

        private bool isTemplateValid = false;

        // Is not possible return the type used that inherits from View specifically, but it can be casted.
        protected Android.Views.View FindViewInTemplateById([IdRes] int id) => TemplateRoot?.FindViewById(id);

        protected void InvalidateTemplate()
        {
            isTemplateValid = false;
            RequestLayout();
        }

        protected virtual void OnTemplateLoaded() { }

        protected void ReloadTemplateIfInvalid()
        {
            // if (!isTemplateValid)
                // TODO ReloadTemplate();
        }
    }
}
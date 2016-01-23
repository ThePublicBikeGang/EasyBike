using System;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.V4.Content.Res;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Views.InputMethods;
using Android.App;
using Plugin.CurrentActivity;

namespace EasyBike.Droid.Helpers
{
    public class CustomOnTouchListener : Java.Lang.Object, View.IOnTouchListener
    {
        public bool OnTouch(View v, MotionEvent e)
        {
            ClearableAutoCompleteTextView et = v as ClearableAutoCompleteTextView;

            if (et.GetCompoundDrawables()[2] == null)
                return false;

            if (e.Action == MotionEventActions.Up)
                return false;

            if (e.GetX() > (et.Width - et.PaddingRight - ClearableAutoCompleteTextView.imgClearButton.IntrinsicWidth) - 10)
            {
                et.Clear();
            }
            return false;
        }
    }

    public class ClearableAutoCompleteTextView : AutoCompleteTextView
    {
        public static Drawable imgClearButton;
        private Context _context;
        /* Required methods, not used in this implementation */
        public ClearableAutoCompleteTextView(Context context) : base(context)
        {
            Init(context);
        }

        /* Required methods, not used in this implementation */
        public ClearableAutoCompleteTextView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            Init(context);
        }

        /* Required methods, not used in this implementation */
        public ClearableAutoCompleteTextView(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        private void Init(Context context)
        {
            _context = context;
            // The image we defined for the clear button
            imgClearButton = ResourcesCompat.GetDrawable(context.Resources, Resource.Drawable.abc_ic_clear_mtrl_alpha, null);
            SetOnTouchListener(new CustomOnTouchListener());
            FocusChange += (s, e) =>
            {
                if (!e.HasFocus)
                {
                    HideKeyboard();
                    SetBackgroundColor(Resources.GetColor(Resource.Color.accent));
                }
                else
                {
                    if (CrossCurrentActivity.Current.Activity is MainActivity)
                    {
                        (CrossCurrentActivity.Current.Activity as MainActivity).CloseDrawer();
                    }
                    SetBackgroundColor(Resources.GetColor(Resource.Color.primary_light));
                }

            };
            TextChanged += (s, e) =>
            {
                if (Text.Length > 0)
                {
                    ShowClearButton();
                }
                else
                {
                    HideClearButton();
                }
            };
        }


        private bool IsClearButtonVisible;
        public void HideClearButton()
        {
            if (IsClearButtonVisible)
            {
                SetCompoundDrawables(null, null, null, null);
                IsClearButtonVisible = false;
            }
        }

        public void ShowClearButton()
        {
            if (!IsClearButtonVisible)
            {
                SetCompoundDrawablesWithIntrinsicBounds(null, null, imgClearButton, null);
                IsClearButtonVisible = true;
            }
        }

        public void Clear()
        {
            Text = string.Empty;
            HideClearButton();
        }

        private void HideKeyboard()
        {
            InputMethodManager inputMethodManager = (InputMethodManager)_context.GetSystemService(Context.InputMethodService);
            inputMethodManager.HideSoftInputFromWindow(WindowToken, 0);
        }
    }
}

using Android.Content;
using Android.Graphics.Drawables;
using Android.Support.V4.Content.Res;
using Android.Util;
using Android.Views;
using Android.Widget;
using Plugin.CurrentActivity;
using Android.Graphics;

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

            if (e.GetX() > (et.Width - et.PaddingRight - ClearableAutoCompleteTextView.imgClearButton.IntrinsicWidth))
            {
                et.Clear();
            }
            return false;
        }
    }

    public class ClearableAutoCompleteTextView : AutoCompleteTextView
    {
        public static Drawable imgClearButton;
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
            // set some styles
            SetHintTextColor(Color.LightGray);
            // The image we defined for the clear button
            imgClearButton = ResourcesCompat.GetDrawable(context.Resources, Resource.Drawable.clear, null);
            SetOnTouchListener(new CustomOnTouchListener());
            FocusChange += (s, e) =>
            {
                if (e.HasFocus)
                {
                    if (CrossCurrentActivity.Current.Activity is MainActivity)
                    {
                        (CrossCurrentActivity.Current.Activity as MainActivity).CloseDrawer();
                    }
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
    }
}

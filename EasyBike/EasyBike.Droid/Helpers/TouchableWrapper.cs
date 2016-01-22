using Android.Content;
using Android.Views;
using Android.Widget;
using System;

namespace EasyBike.Droid.Helpers
{
    public class TouchableWrapper : FrameLayout
    {
        private MainActivity _mainActivity;
        public TouchableWrapper(Context context) : base(context)
        {
            _mainActivity = context as MainActivity;
        }

        //public override bool OnInterceptTouchEvent(MotionEvent ev)
        //{
        //    switch (ev.Action)
        //    {
        //        case MotionEventActions.Down:
        //            MainActivity.MapIsTouched = true;
        //            break;

        //        case MotionEventActions.Up:
        //            MainActivity.MapIsTouched = false;
        //            break;
        //    }
        //    return base.OnInterceptTouchEvent(ev);
        //}
        private float lastX;
        private float lastY;

        public override bool DispatchTouchEvent(MotionEvent e)
        {
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    {
                        this.lastX = e.RawX;
                        this.lastY = e.RawY;
                    }
                    break;
                case MotionEventActions.Move:
                    if (Math.Abs(lastX - e.RawX) > 3 || Math.Abs(lastX - e.RawX) > 3) 
                    {
                        _mainActivity.UnStickUserLocation();
                    }
                    break;

            }
            return base.DispatchTouchEvent(e);
        }
    }
}
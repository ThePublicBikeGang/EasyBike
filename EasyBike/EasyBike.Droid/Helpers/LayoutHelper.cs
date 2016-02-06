using Android.Content;
using Plugin.CurrentActivity;

namespace EasyBike.Droid.Helpers
{
    public static class LayoutHelper
    {
        private static int _densityDpi;
        /**
* This method converts dp unit to equivalent pixels, depending on device density. 
* 
* @param dp A value in dp (density independent pixels) unit. Which we need to convert into pixels
* @param context Context to get resources and device specific display metrics
* @return A float value to represent px equivalent to dp depending on device density
*/
        public static float ConvertDpToPixel(float dp)
        {
            if(_densityDpi == 0)
            {
                _densityDpi = (int)CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.DensityDpi;
            }
            float px = dp * (_densityDpi / 160f);
            return px;
        }

        /**
         * This method converts device specific pixels to density independent pixels.
         * 
         * @param px A value in px (pixels) unit. Which we need to convert into db
         * @param context Context to get resources and device specific display metrics
         * @return A float value to represent dp equivalent to px value
         */
        public static float ConvertPixelsToDp(float px)
        {
            if (_densityDpi == 0)
            {
                _densityDpi = (int)CrossCurrentActivity.Current.Activity.Resources.DisplayMetrics.DensityDpi;
            }
            float dp = px / (_densityDpi / 160f);
            return dp;
        }
    }
}
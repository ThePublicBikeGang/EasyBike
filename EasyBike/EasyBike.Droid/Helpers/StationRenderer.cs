using System;
using Android.Animation;
using Android.Content;
using Android.Content.Res;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Support.V4.Content.Res;
using Android.Views;
using Android.Widget;
using Com.Google.Maps.Android.Clustering;
using Com.Google.Maps.Android.Clustering.View;
using Com.Google.Maps.Android.UI;
using EasyBike.Droid.Models;
using Java.Lang;
using Android.Views.Animations;
using EasyBike.Models;
using GalaSoft.MvvmLight.Ioc;
using EasyBike.Models.Storage;
using System.Threading.Tasks;

namespace EasyBike.Droid.Helpers
{
    public class MVAccelerateDecelerateInterpolator : Java.Lang.Object, IInterpolator
    {
        // easeInOutQuint
        public float GetInterpolation(float t)
        {
            float x;
            if (t < 0.5f)
            {
                x = t * 2.0f;
                return 0.5f * x * x * x * x * x;
            }
            x = (t - 0.5f) * 2 - 1;
            return 0.5f * x * x * x * x * x + 1;
        }
    }
    public class StationRenderer : DefaultClusterRenderer
    {
        private Context _context;
        private GoogleMap _map;
        private ClusterManager _clusterManager;
        private readonly IconGenerator _iconGenRed;
        private readonly IconGenerator _iconGenGreen;
        private readonly IconGenerator _iconGenOrange;
        private readonly IconGenerator _iconGenGrey;
        private readonly IconGenerator _iconGenGreyLowAlpha;
        public readonly Bitmap _iconRed;
        public readonly Bitmap _iconGreen;
        public readonly Bitmap _iconOrange;
        public readonly Bitmap _iconGrey;
        public readonly Bitmap _iconGreyLowAlpha;
        private IContractService _contractService;
        private ISettingsService _settingsService;

        public readonly Paint _textPaint = new Paint();
        public StationRenderer(Context context, GoogleMap map,
                             ClusterManager clusterManager) : base(context, map, clusterManager)
        {
            _context = context;
            _map = map;
            _clusterManager = clusterManager;
            _contractService = SimpleIoc.Default.GetInstance<IContractService>();
            _settingsService = SimpleIoc.Default.GetInstance<ISettingsService>();

            _iconGenRed = new IconGenerator(_context);
            _iconGenGreen = new IconGenerator(_context);
            _iconGenOrange = new IconGenerator(_context);
            _iconGenGrey = new IconGenerator(_context);
            _iconGenGreyLowAlpha = new IconGenerator(_context);
            //// Define the size you want from dimensions file
            //var shapeDrawable = ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.station, null);
            //iconGen.SetBackground(shapeDrawable);
            ////// Create a view container to set the size
            _iconGenRed.SetBackground(ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.stationRed, null));
            _iconGenOrange.SetBackground(ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.stationOrange, null));
            _iconGenGreen.SetBackground(ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.stationGreen, null));
            _iconGenGrey.SetBackground(ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.stationGrey, null));
            _iconGenGreyLowAlpha.SetBackground(ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.greyLowAlpha, null));

            _iconRed = _iconGenRed.MakeIcon();
            _iconGreen = _iconGenGreen.MakeIcon();
            _iconOrange = _iconGenOrange.MakeIcon();
            _iconGrey = _iconGenGrey.MakeIcon();
            _iconGreyLowAlpha = _iconGenGreyLowAlpha.MakeIcon();

            var textView = new TextView(context);
            textView.SetTextAppearance(_context, Resource.Style.iconGenText);
            _textPaint.AntiAlias = true;
            _textPaint.SetARGB(255, 255, 255, 255);
            _textPaint.TextSize = textView.TextSize;
            _textPaint.TextAlign = Paint.Align.Center;
            _textPaint.SetTypeface(textView.Typeface);

        }

        //protected override void OnClusterItemRendered(Java.Lang.Object p0, Marker p1)
        //{
        //    // doesn't work, I guess because it needs to be done on the Marker itself
        //    if (p1.Title == "1") return;
        //    ValueAnimator ani = ValueAnimator.OfFloat(0, 1);

        //    //ani.SetDuration(200);
        //    //ani.AddUpdateListener(new Animatorrr(p1));
        //    //ani.Start();

        //    //var test = ObjectAnimator.OfInt(p1, "Icon", 0, 100);
        //    //test.SetDuration(1000);
        //    //test.Start();
        //    //ScaleAnimation anim = new ScaleAnimation(0.0f, 1.0f, 0.0f, 1.0f);
        //    //anim.Interpolator= new MVAccelerateDecelerateInterpolator();
        //    p1.Title = "1";



        //    //Animation logoMoveAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.logoanimation);
        //    //logoIV.startAnimation(logoMoveAnimation);
        //}

        /// <summary>
        /// Helper method to provide the corresponding Icon for a station
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public BitmapDescriptor CreateStationIcon(Station station)
        {
            Bitmap bitmap = null;
            var value = _settingsService.Settings.IsBikeMode ? station.AvailableBikes : station.AvailableBikeStands;
            var printedValue = value.HasValue ? value.Value.ToString() : string.Empty;
            if (!station.Loaded)
            {
                printedValue = string.Empty;
                bitmap = _iconGreyLowAlpha;
            }
            else if (station.Status == false)
            {
                printedValue = "!";
                bitmap = _iconGrey;
            }
            else
            {
                if (value == 0)
                {
                    bitmap = _iconRed;
                }
                else if (value < 5)
                {
                    bitmap = _iconOrange;
                }
                else if (value >= 5)
                {
                    bitmap = _iconGreen;
                }
                else
                {
                    printedValue = "?";
                    bitmap = _iconGrey;
                }
            }
            bitmap = bitmap.Copy(bitmap.GetConfig(), true);
            Canvas canvas = new Canvas(bitmap);
            int xPos = (canvas.Width / 2);
            int yPos = (int)((canvas.Height / 2) - ((_textPaint.Descent() + _textPaint.Ascent()) / 2));
            canvas.DrawText(printedValue, xPos, yPos - 9, _textPaint);
            var icon = BitmapDescriptorFactory.FromBitmap(bitmap); 
            bitmap.Recycle();
            return icon;
        }

        protected override bool ShouldRenderAsCluster(ICluster p0)
        {
            if (_map.CameraPosition.Zoom > 15)
            {
                return false;
            }
            return base.ShouldRenderAsCluster(p0);
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView Text { get; set; }
        }

        protected override void OnClusterRendered(ICluster p0, Marker p1)
        {
            // way to determine if the user clicked on a cluster to zoom in
            p1.Title = "cluster";
            base.OnClusterRendered(p0, p1); 
        }

        protected override void OnClusterItemRendered(Java.Lang.Object context, Marker marker)
        {
            (context as ClusterItem).Station.Control = marker;
            base.OnClusterItemRendered(context, marker);
        }


        protected override void OnBeforeClusterItemRendered(Java.Lang.Object context, MarkerOptions markerOptions)
        {
            var station = (context as ClusterItem).Station;

            //if (station.Contract.StationRefreshGranularity)
            //{
            //    if (!station.IsInRefreshPool)
            //    {
            //        Task.Run(() =>
            //        {
            //            _contractService.AddStationToRefreshingPool(station);
            //        });
            //    }
            //}


            markerOptions.SetIcon(CreateStationIcon(station));
            // BitmapDescriptor markerDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueMagenta);
            //var tmp = BitmapFactory.DecodeResource(_context.Resources, Resource.Drawable.stationGris);
            //IconGenerator iconGen = null;
            //// Define the size you want from dimensions file
            //var shapeDrawable = ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.station, null);
            //iconGen.SetBackground(shapeDrawable);


            ////// Create a view container to set the size
            //View view = (_context as MainActivity).LayoutInflater.Inflate(Resource.Layout.MarkerText, null);
            //var text = view.FindViewById<TextView>(Resource.Id.text);
        }
    }

    public class Animatorrr : Java.Lang.Object, ValueAnimator.IAnimatorUpdateListener
    {
        private Marker _marker;
        public Animatorrr(Marker marker)
        {
            _marker = marker;
        }

        public void OnAnimationUpdate(ValueAnimator animation)
        {
            _marker.Alpha = (float)animation.AnimatedValue;
        }
    }
}

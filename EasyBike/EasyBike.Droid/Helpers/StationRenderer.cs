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

namespace EasyBike.Droid.Helpers
{
    public class MVAccelerateDecelerateInterpolator : Java.Lang.Object, IInterpolator
    {
        // easeInOutQuint
        public float GetInterpolation(float t) {
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

        public StationRenderer(Context context, GoogleMap map,
                             ClusterManager clusterManager) : base(context, map, clusterManager)
        {
            _context = context;
            _map = map;
            _clusterManager = clusterManager;

            _iconGenRed = new IconGenerator(_context);
            _iconGenGreen = new IconGenerator(_context);
            _iconGenOrange = new IconGenerator(_context);
            _iconGenGrey = new IconGenerator(_context);
            //// Define the size you want from dimensions file
            //var shapeDrawable = ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.station, null);
            //iconGen.SetBackground(shapeDrawable);
            ////// Create a view container to set the size
            _iconGenRed.SetBackground(ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.stationRed, null));
            _iconGenOrange.SetBackground(ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.stationOrange, null));
            _iconGenGreen.SetBackground(ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.stationVert, null));
            _iconGenGrey.SetBackground(ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.stationGris, null));
            
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
        

        protected override bool ShouldRenderAsCluster(ICluster p0)
        {
            if(_map.CameraPosition.Zoom > 15 || _clusterManager.MarkerCollection.Markers.Count < 30)
            {
                return false;
            }
            return base.ShouldRenderAsCluster(p0);
        }

        private class ViewHolder : Java.Lang.Object
        {
            public TextView Text { get; set; }
        }


        protected override void OnBeforeClusterItemRendered(Java.Lang.Object context, MarkerOptions markerOptions)
        {

           // BitmapDescriptor markerDescriptor = BitmapDescriptorFactory.DefaultMarker(BitmapDescriptorFactory.HueMagenta);
            //var tmp = BitmapFactory.DecodeResource(_context.Resources, Resource.Drawable.stationGris);
            IconGenerator iconGen = null;
            //// Define the size you want from dimensions file
            //var shapeDrawable = ResourcesCompat.GetDrawable(_context.Resources, Resource.Drawable.station, null);
            //iconGen.SetBackground(shapeDrawable);


            ////// Create a view container to set the size
            //View view = (_context as MainActivity).LayoutInflater.Inflate(Resource.Layout.MarkerText, null);
            //var text = view.FindViewById<TextView>(Resource.Id.text);

           
            var value = (context as ClusterItem).Station.AvailableBikes;
            if(value == 0)
            {
                iconGen = _iconGenRed;
            }else if(value < 5)
            {
                iconGen = _iconGenOrange;
            }
            else if (value >= 5)
            {
                iconGen = _iconGenGreen;
            }
            else
            {
                iconGen = _iconGenGrey;
            }

           // text.Text = value.ToString();


          //  iconGen.SetContentView(view);
        



            Bitmap bitmap = iconGen.MakeIcon();
            Paint textPaint = new Paint() { StrokeWidth = 6 };
            textPaint.SetARGB(255, 255, 255, 255);
            textPaint.TextAlign = Paint.Align.Center;
            textPaint.TextSize = 30;
            Canvas canvas = new Canvas(bitmap);
            int xPos = (canvas.Width / 2);
            int yPos = (int)((canvas.Height / 2) - ((textPaint.Descent() + textPaint.Ascent()) / 2));
            canvas.DrawText(value.ToString(), xPos, yPos-5, textPaint);
            markerOptions.SetIcon(BitmapDescriptorFactory.FromBitmap(bitmap));
            bitmap.Recycle();


            //markerOptions.SetIcon(markerDescriptor);

            //Paint textPaint = new Paint() { StrokeWidth = 5};
            //textPaint.SetARGB(255, 255, 255, 255);
            //textPaint.TextAlign = Paint.Align.Center;

            //textPaint.TextSize = 35;

            //Canvas canvas = new Canvas(bitmap);
            //int xPos = (canvas.Width / 2);
            //int yPos = (int)((canvas.Height / 2) - ((textPaint.Descent() + textPaint.Ascent()) / 2));

            // canvas.DrawText((context as ClusterItem).Station.AvailableBikes.ToString(), xPos, yPos, textPaint);
            //markerOptions.Anchor(5, 15);




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

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
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Support.V7.Widget;

namespace EasyBike.Droid.Helpers
{
    public class CustomActionBarDrawerToggle : ActionBarDrawerToggle
    {
        MainActivity _context;
        public CustomActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, Android.Support.V7.Widget.Toolbar toolbar, int openDrawerContentDescRes, int closeDrawerContentDescRes) : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes)
        {
            _context = activity as MainActivity;
        }
        public override void OnDrawerClosed(View drawerView)
        {
            _context.SupportInvalidateOptionsMenu();
            //base.OnDrawerClosed(drawerView);
        }
        public void onDrawerOpened(View drawerView)
        {
            _context.SupportInvalidateOptionsMenu();
            //drawerOpened = true;
        }
    }
}

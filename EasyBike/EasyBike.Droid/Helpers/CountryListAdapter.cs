using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using EasyBike.Models;
using Android.App;
using GalaSoft.MvvmLight.Helpers;
using Android.Graphics;
using Android.Media;

namespace EasyBike.Droid.Helpers
{
    public class CountryListAdapter : BaseExpandableListAdapter//, AdapterView.IOnItemClickListener
    {
        private readonly List<Country> _countries;
        readonly Activity Context;

        public CountryListAdapter(Activity newContext, List<Country> countries) : base()
        {
            Context = newContext;
            _countries = countries;


        }

        public override int GroupCount
        {
            get
            {
                return _countries.Count;
            }
        }

        public override bool HasStableIds
        {
            get
            {
                return false;
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return Convert.ToInt32(groupPosition.ToString() + childPosition.ToString());
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return _countries[groupPosition].Contracts.Count;
        }


        private class MyViewHolder : Java.Lang.Object
        {
            public TextView Name { get; set; }
            public CheckBox CheckBox { get; set; }
            public ImageView Image { get; set; }
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            MyViewHolder holder = null;
            View row = convertView;
            if (row != null)
            {
                holder = row.Tag as MyViewHolder;
            }

            if (holder == null)
            {
                holder = new MyViewHolder();
                row = Context.LayoutInflater.Inflate(Resource.Layout.ContractTemplate, null);
                holder.Name = row.FindViewById<TextView>(Resource.Id.NameTextView);
                holder.CheckBox = row.FindViewById<CheckBox>(Resource.Id.ContractCheckBox);
                // holder.Image = view.FindViewById<ImageView>(Resource.Id.imageView);
                row.Tag = holder;
            }

            holder.Name.Text = _countries[groupPosition].Contracts[childPosition].Name;
            holder.CheckBox.Checked = _countries[groupPosition].Contracts[childPosition].Downloaded;
            //holder.CheckBox.SetBinding(
            //     () => _countries[groupPosition].Contracts[childPosition].Downloaded,
            //     () => CompoundButton.Checked);


            return row;
        }


        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            if (header == null)
            {
                header = Context.LayoutInflater.Inflate(Resource.Layout.ContractGroup, null);
            }
            header.FindViewById<TextView>(Resource.Id.CountryName).Text = _countries[groupPosition].Name;
            header.FindViewById<ImageView>(Resource.Id.CountryImage).SetImageBitmap(DecodeSampledBitmapFromUri((byte[])_countries[groupPosition].ImageByteArray));
            return header;
        }

        public Bitmap DecodeSampledBitmapFromUri(byte[] imageData)
        {
            return BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public bool IsEnabled(int position)
        {
            return true;
        }

        public Java.Lang.Object GetItem(int position)
        {
            throw new NotImplementedException();
        }

        public long GetItemId(int position)
        {
            return position;
        }

        public int GetItemViewType(int position)
        {
            throw new NotImplementedException();
        }

        public View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }

        //public void OnItemClick(AdapterView parent, View view, int position, long id)
        //{
        //    //row.FindViewById<TextView>(Resource.Id.NameTextView).Text = _countries[groupPosition].Contracts[childPosition].Name;
        //    //// var contractCommandBinding = this.SetBinding(() => _countries[groupPosition].Contracts[childPosition]);
        //    //var _refreshCommandBinding = row.SetBinding(() => row.Text);
        //    //row.SetCommand(
        //    //    "Click",
        //    //    (Context as ContractsActivity).ViewModel.ContractTappedCommand, _refreshCommandBinding);
        //}
    }
}
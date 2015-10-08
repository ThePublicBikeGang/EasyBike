using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using EasyBike.Models;
using Android.App;

namespace EasyBike.Droid.Helpers
{
    public class CountryListAdapter : BaseExpandableListAdapter
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
                throw new NotImplementedException();
            }
        }

        public override bool HasStableIds
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }

        public override int GetChildrenCount(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView;
            //if (row == null)
            //{
            //    row = Context.LayoutInflater.Inflate(Resource.Layout.DataListItem, null);
            //}
            //string newId = "", newValue = "";
            //GetChildViewHelper(groupPosition, childPosition, out newId, out newValue);
            //row.FindViewById<TextView>(Resource.Id.DataId).Text = newId;
            //row.FindViewById<TextView>(Resource.Id.DataValue).Text = newValue;

            return row;
        }

        //private void GetChildViewHelper(int groupPosition, int childPosition, out string Id, out string Value)
        //{
        //    //char letter = (char)(65 + groupPosition);
        //    //List<Data> results = _countries.FindAll(c => c.Name[0].Equals(letter));
        //    //Id = results[childPosition].Id;
        //    //Value = results[childPosition].Value;
        //}

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override long GetGroupId(int groupPosition)
        {
            throw new NotImplementedException();
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            View header = convertView;
            if (header == null)
            {
                header = Context.LayoutInflater.Inflate(Resource.Layout.ContractGroup, null);
            }
            header.FindViewById<TextView>(Resource.Id.DataHeader).Text = ((char)(65 + groupPosition)).ToString();

            return header;
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            throw new NotImplementedException();
        }
    }
}
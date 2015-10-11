using System;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using EasyBike.Models;
using Android.App;
using Java.Lang;

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

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            View row = convertView;
            if (row == null)
            {
                row = Context.LayoutInflater.Inflate(Resource.Layout.ContractTemplate, null);
            }
            row.FindViewById<TextView>(Resource.Id.NameTextView).Text = _countries[groupPosition].Contracts[childPosition].Name;
            return row;
        }

        private void GetChildViewHelper(int groupPosition, int childPosition, out string contractName)
        {
            char letter = (char)(65 + groupPosition);
            List<Country> results = _countries.FindAll(c => c.Name[0].Equals(letter));
            contractName = results[childPosition].Name;
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
            header.FindViewById<TextView>(Resource.Id.DataHeader).Text = _countries[groupPosition].Name;

            return header;
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
    }
}
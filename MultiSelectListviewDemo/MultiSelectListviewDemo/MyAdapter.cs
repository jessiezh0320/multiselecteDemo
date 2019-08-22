using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MultiSelectListviewDemo
{
    public class MyAdapter:BaseAdapter
    {
        private LayoutInflater mInflater;
        //itwillshowthe data
        private String[] my_data = new String[] { "test1", "test2", "test3", "test4", "test5", "test6", "test7", "test8", "test9", "test10", "test11", "test12", "test13", "test14", "test15", "test16", "test17", "test18", "test19", "test20" }; //itwillusetoCAB(definethedatathathavebeenselected)
        private SparseBooleanArray mSelectedItemsIds;
        private Context context;
 
     public MyAdapter(Context context)
        {
            mInflater = LayoutInflater.From(context);
            mSelectedItemsIds = new SparseBooleanArray();
            this.context = context;
        }

        public override int Count
        {
            get { return my_data.Length; }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ViewHolder holder = null;
            if (convertView == null || holder == null)
            {
                convertView = mInflater.Inflate(Resource.Layout.my_list, null);
                holder = new ViewHolder();
                //setholderlabelwithlabellistidinthe view
                holder.my_label = convertView.FindViewById<TextView>(Resource.Id.label);
                convertView.Tag = (holder);
            }
            else
            {
                holder = (ViewHolder)convertView.Tag;
            }

            holder.my_label.TextSize = 30;//set textSize
            holder.my_label.Text = my_data[position];//setdata label

            //thiswillcheckifthere'sitemthathavebeen selected
            //ifthere'sanitemthatselect,itwillchangetextcolorwith grey
            if (mSelectedItemsIds.Size() > 0)
            {
                //holder.my_label.SetTextColor(this.mInflater.Context.Resources.GetColor(Resource.Color.grey));
                holder.my_label.SetTextColor(Color.Gray);
            }

            //checkifcurrentitemhavebeen checked
            //ifyes,changetextcolorwith green
            if (mSelectedItemsIds.Get(position))
            {
                holder.my_label.SetTextColor(Color.Green);
            }

            return convertView;
        }

        public void toggleSelection(int position)
        {
            selectView(position, !mSelectedItemsIds.Get(position));
        }

        public void removeSelection()
        {
            mSelectedItemsIds = new SparseBooleanArray();
            NotifyDataSetChanged();
        }

        public void selectView(int position, bool value)
        {
            if (value)
                mSelectedItemsIds.Put(position, value);
            else
                mSelectedItemsIds.Delete(position);

            NotifyDataSetChanged();
        }

        public int getSelectedCount()
        {
            return mSelectedItemsIds.Size();
        }

        public SparseBooleanArray getSelectedIds()
        {
            return mSelectedItemsIds;
        }

        public class ViewHolder : Java.Lang.Object
        {
            public TextView my_label;
        }
    
}
}
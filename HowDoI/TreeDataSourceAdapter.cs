using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSHowDoISamples
{
    public class TreeDataSourceAdapter : BaseExpandableListAdapter
    {
        private Context context;
        private List<TreeNode> tableTree;

        public TreeDataSourceAdapter(Context context, List<TreeNode> nodes)
        {
            this.context = context;
            tableTree = new List<TreeNode>();
            tableTree.AddRange(nodes);
        }

        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return tableTree.ElementAt(groupPosition).Child.ElementAt(childPosition).Name;
        }

        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            String name = tableTree.ElementAt(groupPosition).Child.ElementAt(childPosition).Name;
            TextView textView = GetGenericView(name);
            textView.SetPadding((int)(36 * context.Resources.DisplayMetrics.Density), 0, 0, 0);
            return textView;
        }

        public override int GetChildrenCount(int groupPosition)
        {
            return tableTree.ElementAt(groupPosition).Child.Count;
        }

        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return tableTree.ElementAt(groupPosition).Name;
        }

        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }

        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            String name = tableTree.ElementAt(groupPosition).Name;
            TextView textView = GetGenericView(name);
            textView.Text = textView.Text + string.Format("({0})", tableTree.ElementAt(groupPosition).Child.Count);
            textView.SetPadding((int)(10 * context.Resources.DisplayMetrics.Density), 0, 0, 0);
            return textView;
        }

        public override int GroupCount
        {
            get { return tableTree.Count; }
        }

        public override bool HasStableIds
        {
            get { return false; }
        }

        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        public TextView GetGenericView(String name)
        {
            AbsListView.LayoutParams layoutParams = new AbsListView.LayoutParams(ViewGroup.LayoutParams.FillParent, (int)(40 * context.Resources.DisplayMetrics.Density));
            TextView textView = new TextView(context, null, 0);
            textView.LayoutParameters = layoutParams;
            textView.Gravity = GravityFlags.CenterVertical | GravityFlags.Left;
            textView.Text = name;
            return textView;
        }
    }
}
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CSHowDoISamples
{
    [Activity(Label = "How Do I", Icon = "@drawable/icon")]
    public class TreeActivity : ExpandableListActivity
    {
        private List<TreeNode> nodes;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Tree);

            nodes = new List<TreeNode>();
            foreach (XElement element in XDocument.Load(Assets.Open("SampleList.xml")).Root.Elements())
            {
                nodes.Add(new TreeNode(element));
            }

            ExpandableListView.SetAdapter(new TreeDataSourceAdapter(this, nodes));

            ExpandableListView.ChildClick += ExpandableListViewChildClick;
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            base.OnWindowFocusChanged(hasFocus);
            ExpandableListView.SetIndicatorBounds((int)(ExpandableListView.Width - 30 * Resources.DisplayMetrics.Density), (int)(ExpandableListView.Width - 2 * Resources.DisplayMetrics.Density));
        }

        private void ExpandableListViewChildClick(object sender, ExpandableListView.ChildClickEventArgs e)
        {
            Intent intent = new Intent(BaseContext, nodes.ElementAt(e.GroupPosition).Child.ElementAt(e.ChildPosition).ClassType);
            StartActivity(intent);
        }
    }
}
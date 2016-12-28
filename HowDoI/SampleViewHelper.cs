using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace CSHowDoISamples
{
    public class SampleViewHelper
    {
        private static int contentHeight;
        private static Context currentContext;
        private static TextView instructionTextView;
        private static TextView descriptionTextView;
        private static LinearLayout instructionLayout;

        public static void InitializeInstruction(Context context, ViewGroup containerView, Type sampleType, Collection<View> contentViews = null)
        {
            currentContext = context;
            contentHeight = 0;

            LayoutInflater inflater = LayoutInflater.From(context);
            View instructionLayoutView = inflater.Inflate(Resource.Layout.Instruction, containerView);

            instructionTextView = instructionLayoutView.FindViewById<TextView>(Resource.Id.instructionTextView);
            descriptionTextView = instructionLayoutView.FindViewById<TextView>(Resource.Id.descriptionTextView);
            descriptionTextView.Text = GetSampleDescription(sampleType.Name);

            instructionLayout = instructionLayoutView.FindViewById<LinearLayout>(Resource.Id.instructionLinearLayout);
            LinearLayout contentLayout = instructionLayoutView.FindViewById<LinearLayout>(Resource.Id.contentLinearLayout);

            RelativeLayout headerRelativeLayout = instructionLayoutView.FindViewById<RelativeLayout>(Resource.Id.headerRelativeLayout);
            headerRelativeLayout.Click += HeaderRelativeLayoutClick;

            if (contentViews != null)
            {
                foreach (View view in contentViews)
                {
                    contentLayout.AddView(view);
                }
            }
        }

        private static void HeaderRelativeLayoutClick(object sender, EventArgs e)
        {
            contentHeight = contentHeight == 0 ? instructionLayout.Height - instructionTextView.Height : -contentHeight;
            instructionLayout.Layout(instructionLayout.Left, instructionLayout.Top + contentHeight, instructionLayout.Right, instructionLayout.Bottom);
        }

        private static string GetSampleDescription(string typeName)
        {
            XDocument xDoc = XDocument.Load(currentContext.Assets.Open("SampleList.xml"));
            string description = string.Empty;

            foreach (XElement sampleElement in xDoc.Root.Elements())
            {
                foreach (XElement child in sampleElement.Elements())
                {
                    if (typeName == child.Attribute("Class").Value)
                    {
                        description = child.Element("Description").Value;
                        break;
                    }
                }

                if (!string.IsNullOrEmpty(description)) break;
            }

            return description;
        }
    }
}
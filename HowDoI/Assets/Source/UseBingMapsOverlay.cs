using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Layers;

namespace CSHowDoISamples
{
    [Activity(Label = "Use BingMaps TileOverlay")]
    public class UseBingMapsOverlay : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            BingMapsOverlay bingMapOverlay = new BingMapsOverlay();
            bingMapOverlay.ApplicationId = "Your Application Id";
            bingMapOverlay.MapType = BingMapsMapType.AerialWithLabels;

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.Meter;
            androidMap.ZoomLevelSet = new BingMapsZoomLevelSet();
            androidMap.Overlays.Add("BingMapOverlay", bingMapOverlay);
            androidMap.CurrentExtent = bingMapOverlay.GetBoundingBox();

            CheckBingMapApplicationId();

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }

        private void CheckBingMapApplicationId()
        {
            BingMapsOverlay bingMapOverlay = (BingMapsOverlay)androidMap.Overlays["BingMapOverlay"];
            if (bingMapOverlay.ApplicationId.Equals("Your Application Id"))
            {
                androidMap.Overlays.Clear();

                ImageView imageView = new ImageView(this);
                imageView.SetImageResource(Resource.Drawable.Notice);
                imageView.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);

                RelativeLayout mainLayout = FindViewById<RelativeLayout>(Resource.Id.MainLayout);
                mainLayout.RemoveView(androidMap);
                mainLayout.AddView(imageView);
            }
        }
    }
}
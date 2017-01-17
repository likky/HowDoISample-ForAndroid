using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Layers;

namespace CSHowDoISamples
{
    [Activity(Label = "Use OpenStreetMap TileOverlay")]
    public class UseOpenStreetMapOverlay : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            OpenStreetMapOverlay osmOvelerlay = new OpenStreetMapOverlay();

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.Meter;
            androidMap.ZoomLevelSet = new OpenStreetMapsZoomLevelSet();
            androidMap.CurrentExtent = osmOvelerlay.GetBoundingBox();
            androidMap.Overlays.Add(osmOvelerlay);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }
    }
}
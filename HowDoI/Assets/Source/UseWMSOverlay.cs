using Android.App;
using Android.OS;
using Android.Widget;
using System;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Shapes;

namespace CSHowDoISamples
{
    [Activity(Label = "Use WMS Overlay")]
    public class UseWMSOverlay : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-143.4, 109.3, 116.7, -76.3);

            WmsOverlay wmsOverlay = new WmsOverlay();
            wmsOverlay.ServerUris.Add(new Uri("http://howdoiwms.thinkgeo.com/WmsServer.aspx"));
            wmsOverlay.Parameters.Add("LAYERS", "Countries02");
            wmsOverlay.Parameters.Add("STYLES", "SIMPLE");
            androidMap.Overlays.Add(wmsOverlay);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }
    }
}
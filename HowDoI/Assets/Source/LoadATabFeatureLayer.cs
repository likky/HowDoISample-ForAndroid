using Android.App;
using Android.OS;
using Android.Widget;
using System.IO;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Styles;

namespace CSHowDoISamples
{
    [Activity(Label = "Load A Tab FeatureLayer")]
    public class LoadATabFeatureLayer : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            TabFeatureLayer tabFeatureLayer = new TabFeatureLayer(SampleHelper.GetDataPath(@"Tab/HoustonMuniBdySamp_Boundary.tab"));
            tabFeatureLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.FromArgb(100, GeoColor.SimpleColors.Green), GeoColor.SimpleColors.Green);
            tabFeatureLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            tabFeatureLayer.Open();

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(tabFeatureLayer);

            WorldMapKitOverlay worldMapKitOverlay = new WorldMapKitOverlay();

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = tabFeatureLayer.GetBoundingBox();
            androidMap.Overlays.Add(worldMapKitOverlay);
            androidMap.Overlays.Add(layerOverlay);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }
    }
}
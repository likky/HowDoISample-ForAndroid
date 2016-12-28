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
    [Activity(Label = "Load A Gpx FeatureLayer")]
    public class LoadAGpxFeatureLayer : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            ValueStyle pointStyle = new ValueStyle();
            pointStyle.ColumnName = "IsWayPoint";
            pointStyle.ValueItems.Add(new ValueItem("0", PointStyles.CreateSimplePointStyle(PointSymbolType.Circle, GeoColor.SimpleColors.Red, 4)));
            pointStyle.ValueItems.Add(new ValueItem("1", PointStyles.CreateSimplePointStyle(PointSymbolType.Circle, GeoColor.SimpleColors.Green, 8)));

            LineStyle roadstyle = LineStyles.CreateSimpleLineStyle(GeoColor.SimpleColors.Black, 1, true);

            TextStyle labelStyle = TextStyles.CreateSimpleTextStyle("name", "Arial", 8, DrawingFontStyles.Bold, GeoColor.SimpleColors.Black);
            labelStyle.PointPlacement = PointPlacement.UpperCenter;
            labelStyle.OverlappingRule = LabelOverlappingRule.NoOverlapping;
            labelStyle.YOffsetInPixel = 8;

            GpxFeatureLayer gpxFeatureLayer = new GpxFeatureLayer(SampleHelper.GetDataPath(@"Gpx/afoxboro.gpx"));
            gpxFeatureLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(pointStyle);
            gpxFeatureLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(roadstyle);
            gpxFeatureLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            gpxFeatureLayer.Open();

            GpxFeatureLayer gpxTextLayer = new GpxFeatureLayer(SampleHelper.GetDataPath(@"Gpx/afoxboro.gpx"));
            gpxTextLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(labelStyle);
            gpxTextLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(gpxFeatureLayer);
            layerOverlay.Layers.Add(gpxTextLayer);

            WorldMapKitOverlay worldMapKitOverlay = new WorldMapKitOverlay();

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.Overlays.Add(worldMapKitOverlay);
            androidMap.Overlays.Add(layerOverlay);
            androidMap.CurrentExtent = gpxFeatureLayer.GetBoundingBox();
            androidMap.MapUnit = GeographyUnit.DecimalDegree;

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }
    }
}
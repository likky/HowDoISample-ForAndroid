using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;

namespace CSHowDoISamples
{
    [Activity(Label = "Add A Touch Event On Map")]
    public class AddATouchEventOnMap : SampleActivity
    {
        private MapView androidMap;
        private MarkerOverlay markerOverlay;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayMapView);

            ShapeFileFeatureLayer txwatFeatureLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"Frisco/TXwat.shp"));
            txwatFeatureLayer.ZoomLevelSet.ZoomLevel12.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(255, 153, 179, 204);
            txwatFeatureLayer.ZoomLevelSet.ZoomLevel12.DefaultTextStyle = TextStyles.CreateSimpleTextStyle("LandName", "Arial", 9, DrawingFontStyles.Italic, GeoColor.StandardColors.Navy);
            txwatFeatureLayer.ZoomLevelSet.ZoomLevel12.DefaultTextStyle.SuppressPartialLabels = true;
            txwatFeatureLayer.ZoomLevelSet.ZoomLevel12.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer txlkaA40FeatureLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"Frisco/TXlkaA40.shp"));
            txlkaA40FeatureLayer.ZoomLevelSet.ZoomLevel14.DefaultLineStyle = LineStyles.LocalRoad4;
            txlkaA40FeatureLayer.ZoomLevelSet.ZoomLevel15.DefaultLineStyle = LineStyles.LocalRoad3;
            txlkaA40FeatureLayer.ZoomLevelSet.ZoomLevel16.DefaultLineStyle = LineStyles.LocalRoad2;
            txlkaA40FeatureLayer.ZoomLevelSet.ZoomLevel16.DefaultTextStyle = TextStyles.CreateSimpleTextStyle("[fedirp] [fename] [fetype] [fedirs]", "Arial", 10f, DrawingFontStyles.Regular, GeoColor.StandardColors.Black, 0, -1);
            txlkaA40FeatureLayer.ZoomLevelSet.ZoomLevel16.DefaultTextStyle.SuppressPartialLabels = true;
            txlkaA40FeatureLayer.ZoomLevelSet.ZoomLevel16.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            ShapeFileFeatureLayer txlkaA20FeatureLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"Frisco/TXlkaA20.shp"));
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel15.DefaultLineStyle = LineStyles.CreateSimpleLineStyle(GeoColor.FromArgb(255, 255, 255, 128), 6, GeoColor.StandardColors.LightGray, 9, true);
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel16.DefaultLineStyle = LineStyles.CreateSimpleLineStyle(GeoColor.FromArgb(255, 255, 255, 128), 9, GeoColor.StandardColors.LightGray, 12, true);
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel16.DefaultTextStyle = TextStyles.CreateSimpleTextStyle("[fedirp] [fename] [fetype] [fedirs]", "Arial", 12, DrawingFontStyles.Regular, GeoColor.StandardColors.Black, 0, -1);
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel16.DefaultTextStyle.SuppressPartialLabels = true;
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel16.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            markerOverlay = new MarkerOverlay();
            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(txwatFeatureLayer);
            layerOverlay.Layers.Add(txlkaA20FeatureLayer);
            layerOverlay.Layers.Add(txlkaA40FeatureLayer);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-96.8172, 33.1299, -96.8050, 33.1226);
            androidMap.Overlays.Add(markerOverlay);
            androidMap.Overlays.Add(layerOverlay);
            androidMap.MapSingleTap += AndroidMap_MapSingleTap;

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }

        private void AndroidMap_MapSingleTap(object sender, MotionEvent e)
        {
            Marker marker = new Marker(this);
            PointF location = new PointF(e.GetX(), e.GetY());
            marker.Position = ExtentHelper.ToWorldCoordinate(androidMap.CurrentExtent, location.X, location.Y, (float)androidMap.Width, (float)androidMap.Height);
            marker.SetImageBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.Pin));
            marker.YOffset = -(int)(22 * Resources.DisplayMetrics.Density);
            markerOverlay.Markers.Add(marker);
            markerOverlay.Refresh();
        }
    }
}
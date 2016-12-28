using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.ObjectModel;
using System.Drawing;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;

namespace CSHowDoISamples
{
    [Activity(Label = "Zoom InTo A Feature Touched")]
    public class ZoomInToAFeatureTouched : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayMapView);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/Countries02.shp"));
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            InMemoryFeatureLayer highlightLayer = new InMemoryFeatureLayer();
            highlightLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Evergreen1;
            highlightLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add("WorldLayer", worldLayer);

            LayerOverlay highlightOverlay = new LayerOverlay();
            highlightOverlay.Layers.Add("HighlightLayer", highlightLayer);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            androidMap.Overlays.Add("WorldOverlay", layerOverlay);
            androidMap.Overlays.Add("HighlightOverlay", highlightOverlay);
            androidMap.MapSingleTap += AndroidMap_MapSingleTap;

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }

        private void AndroidMap_MapSingleTap(object sender, MotionEvent e)
        {
            LayerOverlay worldOverlay = (LayerOverlay)androidMap.Overlays["WorldOverlay"];
            FeatureLayer worldLayer = (FeatureLayer)worldOverlay.Layers["WorldLayer"];
            LayerOverlay highlightOverlay = (LayerOverlay)androidMap.Overlays["HighlightOverlay"];
            InMemoryFeatureLayer highlightLayer = (InMemoryFeatureLayer)highlightOverlay.Layers["HighlightLayer"];

            PointF location = new PointF(e.GetX(), e.GetY());

            var worldLocation = ExtentHelper.ToWorldCoordinate(androidMap.CurrentExtent, location.X,
                location.Y, androidMap.Width, androidMap.Height);

            worldLayer.Open();
            Collection<Feature> selectedFeatures = worldLayer.QueryTools.GetFeaturesContaining(worldLocation, ReturningColumnsType.NoColumns);
            worldLayer.Close();

            highlightLayer.Open();
            highlightLayer.InternalFeatures.Clear();
            if (selectedFeatures.Count > 0)
            {
                androidMap.ZoomTo(selectedFeatures[0].GetBoundingBox());
                highlightLayer.InternalFeatures.Add(selectedFeatures[0]);
            }
            highlightLayer.Close();
            highlightOverlay.Refresh();
        }
    }
}
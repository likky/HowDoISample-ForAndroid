using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;

namespace CSHowDoISamples
{
    [Activity(Label = "Find The Feature Touched")]
    public class FindFeatureTouched : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

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

            WorldMapKitOverlay worldMapKitOverlay = new WorldMapKitOverlay();

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            androidMap.MapSingleTap += AndroidMap_MapSingleTap;
            androidMap.Overlays.Add("worldMapKitOverlay", worldMapKitOverlay);
            androidMap.Overlays.Add("WorldOverlay", layerOverlay);
            androidMap.Overlays.Add("HighlightOverlay", highlightOverlay);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }

        private void AndroidMap_MapSingleTap(object sender, Android.Views.MotionEvent e)
        {
            PointF location = new PointF(e.GetX(), e.GetY());
            PointShape position = ExtentHelper.ToWorldCoordinate(androidMap.CurrentExtent, location.X,
                location.Y, androidMap.Width, androidMap.Height);

            LayerOverlay worldOverlay = (LayerOverlay)androidMap.Overlays["WorldOverlay"];
            LayerOverlay highlightOverlay = (LayerOverlay)androidMap.Overlays["HighlightOverlay"];
            FeatureLayer worldLayer = (FeatureLayer)worldOverlay.Layers["WorldLayer"];
            InMemoryFeatureLayer highlightLayer = (InMemoryFeatureLayer)highlightOverlay.Layers["HighlightLayer"];

            worldLayer.Open();
            Collection<Feature> selectedFeatures = worldLayer.QueryTools.GetFeaturesContaining(position, new string[1] { "CNTRY_NAME" });
            worldLayer.Close();

            highlightLayer.Open();
            highlightLayer.InternalFeatures.Clear();
            if (selectedFeatures.Count > 0)
            {
                highlightLayer.InternalFeatures.Add(selectedFeatures[0]);
            }
            highlightLayer.Close();

            highlightOverlay.Refresh();
        }
    }
}
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.ObjectModel;
using System.IO;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;

namespace CSHowDoISamples
{
    [Activity(Label = "Spatial Query A FeatureLayer")]
    public class SpatialQueryAFeatureLayer : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/Countries02.shp"));
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            InMemoryFeatureLayer rectangleLayer = new InMemoryFeatureLayer();
            rectangleLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = new AreaStyle(new GeoSolidBrush(new GeoColor(50, 100, 100, 200)));
            rectangleLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.DarkBlue;
            rectangleLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            rectangleLayer.InternalFeatures.Add("Rectangle", new Feature("POLYGON((-50 -20,-50 20,50 20,50 -20,-50 -20))", "Rectangle"));

            InMemoryFeatureLayer spatialQueryResultLayer = new InMemoryFeatureLayer();
            spatialQueryResultLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = new AreaStyle(new GeoSolidBrush(GeoColor.FromArgb(200, GeoColor.SimpleColors.PastelRed)));
            spatialQueryResultLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.Red;
            spatialQueryResultLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add("WorldLayer", worldLayer);

            LayerOverlay spatialQueryResultOverlay = new LayerOverlay();
            spatialQueryResultOverlay.TileType = TileType.SingleTile;
            spatialQueryResultOverlay.Layers.Add("RectangleLayer", rectangleLayer);
            spatialQueryResultOverlay.Layers.Add("SpatialQueryResultLayer", spatialQueryResultLayer);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            androidMap.Overlays.Add("WorldOverlay", layerOverlay);
            androidMap.Overlays.Add("SpatialQueryResultOverlay", spatialQueryResultOverlay);

            Button withinButton = new Button(this);
            withinButton.Text = "Within";
            withinButton.Click += button_TouchDown;

            Button disjointedButton = new Button(this);
            disjointedButton.Text = "Disjointed";
            disjointedButton.Click += button_TouchDown;

            LinearLayout linearLayout = new LinearLayout(this);
            linearLayout.Orientation = Orientation.Horizontal;
            linearLayout.AddView(withinButton);
            linearLayout.AddView(disjointedButton);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType(), new Collection<View>() { linearLayout });
        }

        private void button_TouchDown(object sender, EventArgs e)
        {
            LayerOverlay worldOverlay = (LayerOverlay)androidMap.Overlays["WorldOverlay"];
            FeatureLayer worldLayer = (FeatureLayer)worldOverlay.Layers["WorldLayer"];

            LayerOverlay spatialQueryResultOverlay = (LayerOverlay)androidMap.Overlays["SpatialQueryResultOverlay"];
            InMemoryFeatureLayer rectangleLayer = (InMemoryFeatureLayer)spatialQueryResultOverlay.Layers["RectangleLayer"];
            InMemoryFeatureLayer spatialQueryResultLayer = (InMemoryFeatureLayer)spatialQueryResultOverlay.Layers["SpatialQueryResultLayer"];

            Feature rectangleFeature = rectangleLayer.InternalFeatures["Rectangle"];
            Collection<Feature> spatialQueryResults;

            var button = sender as Button;
            worldLayer.Open();
            switch (button.Text)
            {
                case "Within":
                    spatialQueryResults = worldLayer.QueryTools.GetFeaturesWithin(rectangleFeature, new string[0]);
                    break;

                case "Disjointed":
                    spatialQueryResults = worldLayer.QueryTools.GetFeaturesDisjointed(rectangleFeature, new string[0]);
                    break;

                default:
                    spatialQueryResults = worldLayer.QueryTools.GetFeaturesWithin(rectangleFeature, new string[0]);
                    break;
            }
            worldLayer.Close();

            spatialQueryResultLayer.InternalFeatures.Clear();
            foreach (Feature feature in spatialQueryResults)
            {
                spatialQueryResultLayer.InternalFeatures.Add(feature.Id, feature);
            }

            androidMap.Overlays["SpatialQueryResultOverlay"].Refresh();
        }
    }
}
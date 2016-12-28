using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;

namespace CSHowDoISamples
{
    [Activity(Label = "Zoom To A Scale")]
    public class ZoomToAScale : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/Countries02.shp"));
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(worldLayer);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            androidMap.Overlays.Add(layerOverlay);

            LinearLayout linearLayout = new LinearLayout(this);
            linearLayout.Orientation = Orientation.Horizontal;

            Button zoomToButton = new Button(this);
            zoomToButton.Text = "1:1,000,000";
            zoomToButton.Click += ZoomToButtonClick;

            Button zoomToFiveButton = new Button(this);
            zoomToFiveButton.Text = "1:5,000,000";
            zoomToFiveButton.Click += ZoomToButtonClick;

            linearLayout.AddView(zoomToButton);
            linearLayout.AddView(zoomToFiveButton);

            RelativeLayout mainLayout = FindViewById<RelativeLayout>(Resource.Id.MainLayout);
            SampleViewHelper.InitializeInstruction(this, mainLayout, GetType(), new Collection<View>() { linearLayout });
        }

        private void ZoomToButtonClick(object sender, System.EventArgs e)
        {
            var button = sender as Button;
            string zoomLevelScale = button.Text.ToString();
            double scale = Convert.ToDouble(zoomLevelScale.Split(':')[1], CultureInfo.InvariantCulture);
            androidMap.ZoomToScale(scale);
        }
    }
}
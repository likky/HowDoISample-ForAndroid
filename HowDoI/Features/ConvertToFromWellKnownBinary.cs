using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.ObjectModel;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;

namespace CSHowDoISamples
{
    [Activity(Label = "Convert To From Well Known Binary")]
    public class ConvertToFromWellKnownBinary : SampleActivity
    {
        private MapView androidMap;
        private TextView wkbTextView;
        private TextView wktTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayMapView);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/Countries02.shp"));
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(worldLayer);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            androidMap.Overlays.Add(layerOverlay);

            Button convertButton = new Button(this);
            convertButton.Text = "Convert";
            convertButton.Click += ConvertButtonClick;

            wkbTextView = new TextView(this);
            wkbTextView.Text = "AQEAAAAAAAAAAAAkQAAAAAAAADRA";

            wktTextView = new TextView(this);

            LinearLayout linearLayout = new LinearLayout(this);
            linearLayout.Orientation = Orientation.Horizontal;

            linearLayout.AddView(wkbTextView);
            linearLayout.AddView(convertButton);
            linearLayout.AddView(wktTextView);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType(), new Collection<View>() { linearLayout });
        }

        private void ConvertButtonClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(wkbTextView.Text))
            {
                byte[] wellKnownBinary = Convert.FromBase64String(wkbTextView.Text);
                Feature feature = new Feature(wellKnownBinary);

                wktTextView.Text = feature.GetWellKnownText();
                wkbTextView.Text = string.Empty;
            }
            else if (!string.IsNullOrEmpty(wktTextView.Text))
            {
                Feature feature = new Feature(wktTextView.Text);
                byte[] wellKnownBinary = feature.GetWellKnownBinary();

                wkbTextView.Text = Convert.ToBase64String(wellKnownBinary);
                wktTextView.Text = string.Empty;
            }
        }
    }
}
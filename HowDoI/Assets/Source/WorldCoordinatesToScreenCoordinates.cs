using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using System.IO;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;

namespace CSHowDoISamples
{
    [Activity(Label = "World Coordinates To Screen Coordinates")]
    public class WorldCoordinatesToScreenCoordinates : SampleActivity
    {
        private MapView androidMap;
        private TextView resultView;
        private EditText longitudeTextView;
        private EditText latitudeTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/Countries02.shp"));
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            Marker thinkGeoMarker = new Marker(this);
            thinkGeoMarker.Position = new PointShape(-95.2806, 38.9554);
            thinkGeoMarker.YOffset = -22;
            thinkGeoMarker.SetImageBitmap(Android.Graphics.BitmapFactory.DecodeResource(ThinkGeo.MapSuite.Android.Resources, Resource.Drawable.Pin));

            MarkerOverlay markerOverlay = new MarkerOverlay();
            markerOverlay.Markers.Add(thinkGeoMarker);

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(worldLayer);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            androidMap.Overlays.Add(layerOverlay);
            androidMap.Overlays.Add(markerOverlay);

            Button convertButton = new Button(this);
            convertButton.Click += ConvertButtonClick;
            convertButton.Text = "Convert";
            convertButton.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.MatchParent);

            longitudeTextView = new EditText(this);
            longitudeTextView.Text = "-95.2806";

            latitudeTextView = new EditText(this);
            latitudeTextView.Text = "38.9554";

            resultView = new TextView(this);

            LinearLayout verticalLinearLayout = new LinearLayout(this);
            verticalLinearLayout.Orientation = Orientation.Vertical;
            verticalLinearLayout.AddView(longitudeTextView);
            verticalLinearLayout.AddView(latitudeTextView);

            LinearLayout horizontalLinearLayout = new LinearLayout(this);
            horizontalLinearLayout.Orientation = Orientation.Horizontal;
            horizontalLinearLayout.AddView(verticalLinearLayout);
            horizontalLinearLayout.AddView(convertButton);
            horizontalLinearLayout.AddView(resultView);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType(), new Collection<View>() { horizontalLinearLayout });
        }


        private void ConvertButtonClick(object sender, EventArgs e)
        {
            ScreenPointF screenPoint = ExtentHelper.ToScreenCoordinate(androidMap.CurrentExtent, new PointShape(Double.Parse(longitudeTextView.Text, CultureInfo.InvariantCulture), Double.Parse(latitudeTextView.Text, CultureInfo.InvariantCulture)), androidMap.Width, androidMap.Height);
            resultView.Text = string.Format("Screen Position:({0:N4},{1:N4})", screenPoint.X, screenPoint.Y);
        }
    }
}
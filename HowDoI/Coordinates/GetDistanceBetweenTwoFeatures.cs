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
    [Activity(Label = "Get Distance Between Two Features")]
    public class GetDistanceBetweenTwoFeatures : SampleActivity
    {
        private MapView androidMap;
        private TextView distanceTextView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayMapView);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/Countries02.shp"));
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            Marker usMarker = new Marker(this);
            usMarker.Position = new PointShape(-98.58, 39.57);
            usMarker.YOffset = -22;
            usMarker.SetImageBitmap(Android.Graphics.BitmapFactory.DecodeResource(Resources, Resource.Drawable.Pin));

            Marker chinaMarker = new Marker(this);
            chinaMarker.Position = new PointShape(104.72, 34.45);
            chinaMarker.YOffset = -22;
            chinaMarker.SetImageBitmap(Android.Graphics.BitmapFactory.DecodeResource(Resources, Resource.Drawable.Pin));

            MarkerOverlay markerOverlay = new MarkerOverlay();
            markerOverlay.Markers.Add(usMarker);
            markerOverlay.Markers.Add(chinaMarker);

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(worldLayer);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            androidMap.Overlays.Add(layerOverlay);
            androidMap.Overlays.Add("Marker", markerOverlay);

            Button getDistanceButton = new Button(this);
            getDistanceButton.Click += GetDistanceButtonClick;
            getDistanceButton.Text = "Get Distance";
            getDistanceButton.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            distanceTextView = new TextView(this);

            LinearLayout linearLayout = new LinearLayout(this);
            linearLayout.Orientation = Orientation.Horizontal;
            linearLayout.AddView(getDistanceButton);
            linearLayout.AddView(distanceTextView);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType(), new Collection<View>() { linearLayout });
        }

        private void GetDistanceButtonClick(object sender, EventArgs e)
        {
            MarkerOverlay markerOverlayr = androidMap.Overlays["Marker"] as MarkerOverlay;
            Marker usMarker = markerOverlayr.Markers[0];
            Marker chinaMarker = markerOverlayr.Markers[1];
            double distance = usMarker.Position.GetDistanceTo(chinaMarker.Position, GeographyUnit.DecimalDegree, DistanceUnit.Kilometer);
            distanceTextView.Text = string.Format("{0:N4} Km", distance);
        }
    }
}
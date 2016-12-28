using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Util;
using Android.Widget;
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
    [Activity(Label = "Add A Touch Event On Marker")]
    public class AddATouchEventOnMarker : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

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
            txlkaA40FeatureLayer.DrawingMarginPercentage = 80;

            ShapeFileFeatureLayer txlkaA20FeatureLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"Frisco/TXlkaA20.shp"));
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel15.DefaultLineStyle = LineStyles.CreateSimpleLineStyle(GeoColor.FromArgb(255, 255, 255, 128), 6, GeoColor.StandardColors.LightGray, 9, true);
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel16.DefaultLineStyle = LineStyles.CreateSimpleLineStyle(GeoColor.FromArgb(255, 255, 255, 128), 9, GeoColor.StandardColors.LightGray, 12, true);
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel16.DefaultTextStyle = TextStyles.CreateSimpleTextStyle("[fedirp] [fename] [fetype] [fedirs]", "Arial", 12, DrawingFontStyles.Regular, GeoColor.StandardColors.Black, 0, -1);
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel16.DefaultTextStyle.SuppressPartialLabels = true;
            txlkaA20FeatureLayer.ZoomLevelSet.ZoomLevel16.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            WorldMapKitOverlay worldMapKitOverlay = new WorldMapKitOverlay();

            Marker thinkGeoMarker = new Marker(BaseContext);
            thinkGeoMarker.Position = new PointShape(-96.809523, 33.128675);
            thinkGeoMarker.YOffset = -(int)(22 * ThinkGeo.MapSuite.Android.Resources.DisplayMetrics.Density);
            thinkGeoMarker.SetImageBitmap(BitmapFactory.DecodeResource(ThinkGeo.MapSuite.Android.Resources, Resource.Drawable.Pin));
            thinkGeoMarker.Click += ThinkGeoMarkerClick;

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(txlkaA40FeatureLayer);
            layerOverlay.Layers.Add(txwatFeatureLayer);
            layerOverlay.Layers.Add(txlkaA20FeatureLayer);

            MarkerOverlay markerOverlay = new MarkerOverlay();
            markerOverlay.Markers.Add(thinkGeoMarker);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-96.8172, 33.1299, -96.8050, 33.1226);
            androidMap.Overlays.Add(worldMapKitOverlay);
            androidMap.Overlays.Add(layerOverlay);
            androidMap.Overlays.Add(markerOverlay);
            androidMap.Overlays.Add("PopupOverlay", new PopupOverlay());

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }

        void ThinkGeoMarkerClick(object sender, System.EventArgs e)
        {
            Marker thinkGeoMarker = (Marker)sender;
            PopupOverlay popupOverlay = androidMap.Overlays["PopupOverlay"] as PopupOverlay;

            if (popupOverlay.Popups.Count > 0)
            {
                popupOverlay.Popups.Clear();
            }
            else
            {

                ImageView imageView = new ImageView(this);
                imageView.SetImageResource(Resource.Drawable.ThinkGeoLogo);

                TextView textView = new TextView(this);
                textView.Text = string.Format("Longitude : {0:N4}" + "\r\n" + "Latitude : {1:N4}", thinkGeoMarker.Position.X, thinkGeoMarker.Position.Y);
                textView.SetTextColor(Color.Black);
                textView.SetTextSize(ComplexUnitType.Px, 22);

                LinearLayout linearLayout = new LinearLayout(this);
                linearLayout.SetPadding(10, 10, 10, 10);
                linearLayout.Orientation = Orientation.Vertical;
                linearLayout.AddView(imageView);
                linearLayout.AddView(textView);

                Popup popup = new Popup(this);
                popup.Position = thinkGeoMarker.Position;
                popup.YOffset = (int)(-44 * ThinkGeo.MapSuite.Android.Resources.DisplayMetrics.Density);
                popup.XOffset = (int)(4 * ThinkGeo.MapSuite.Android.Resources.DisplayMetrics.Density);
                popup.AddView(linearLayout);

                popupOverlay.Popups.Add(popup);
            }
            popupOverlay.Refresh();
        }
    }
}
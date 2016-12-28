using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.ObjectModel;
using System.IO;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;

namespace CSHowDoISamples
{
    [Activity(Label = "Buffer A Feature")]
    public class BufferAFeature : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/Countries02.shp"));
            worldLayer.Open();
            Feature feature = worldLayer.QueryTools.GetFeatureById("137", new string[0]);
            worldLayer.Close();

            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.SimpleColors.Green);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.GeographicColors.DeepOcean;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            inMemoryLayer.InternalFeatures.Add("POLYGON", feature);

            InMemoryFeatureLayer bufferLayer = new InMemoryFeatureLayer();
            bufferLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.SimpleColors.Green);
            bufferLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay bufferOverlay = new LayerOverlay();
            bufferOverlay.Layers.Add("BufferLayer", bufferLayer);
            bufferOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);

            WorldMapKitOverlay worldMapKitOverlay = new WorldMapKitOverlay();

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-135.224054810107, 62.2893787272533, -58.8379651537011, 7.78687151295263);
            androidMap.Overlays.Add("WorldMapKitOverlay", worldMapKitOverlay);
            androidMap.Overlays.Add("BufferOverlay", bufferOverlay);

            Button bufferButton = new Button(this);
            bufferButton.Text = "Buffer";
            bufferButton.Click += BufferButtonClick;
            bufferButton.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType(), new Collection<View>() { bufferButton });
        }

        private void BufferButtonClick(object sender, System.EventArgs e)
        {
            LayerOverlay bufferOverLay = (LayerOverlay)androidMap.Overlays["BufferOverlay"];

            InMemoryFeatureLayer inMemoryLayer = (InMemoryFeatureLayer)bufferOverLay.Layers["InMemoryFeatureLayer"];
            InMemoryFeatureLayer bufferLayer = (InMemoryFeatureLayer)bufferOverLay.Layers["BufferLayer"];

            AreaBaseShape baseShape = (AreaBaseShape)inMemoryLayer.InternalFeatures["POLYGON"].GetShape();
            MultipolygonShape bufferedShape = baseShape.Buffer(100, 8, BufferCapType.Butt, GeographyUnit.DecimalDegree, DistanceUnit.Kilometer);
            Feature bufferFeature = new Feature(bufferedShape.GetWellKnownBinary(), "Buffer1");

            bufferLayer.InternalFeatures.Clear();
            bufferLayer.InternalFeatures.Add("BufferFeature", bufferFeature);

            androidMap.Overlays["BufferOverlay"].Refresh();
        }
    }
}
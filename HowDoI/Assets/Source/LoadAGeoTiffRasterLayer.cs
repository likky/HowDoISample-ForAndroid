using Android.App;
using Android.OS;
using Android.Widget;
using System.IO;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Layers;

namespace CSHowDoISamples
{
    [Activity(Label = "Load A GeoTiff RasterLayer")]
    public class LoadAGeoTiffRasterLayer : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            WorldMapKitOverlay worldMapKitOverlay = new WorldMapKitOverlay();

            GeoTiffRasterLayer geoTiffRasterLayer = new GeoTiffRasterLayer(SampleHelper.GetDataPath(@"Tiff/World.tif"));
            geoTiffRasterLayer.UpperThreshold = double.MaxValue;
            geoTiffRasterLayer.IsGrayscale = false;
            geoTiffRasterLayer.LowerThreshold = 0;
            geoTiffRasterLayer.Open();

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(geoTiffRasterLayer);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.CurrentExtent = geoTiffRasterLayer.GetBoundingBox();
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.Overlays.Add(worldMapKitOverlay);
            androidMap.Overlays.Add(layerOverlay);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }
    }
}
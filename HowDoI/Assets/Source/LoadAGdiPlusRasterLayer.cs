using System.IO;
using Android.App;
using Android.OS;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using Android.Widget;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Layers;

namespace CSHowDoISamples
{
    [Activity(Label = "Load A GdiPlus RasterLayer")]
    public class LoadAGdiPlusRasterLayer : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            GdiPlusRasterLayer radarImageLayer = new GdiPlusRasterLayer(SampleHelper.GetDataPath(@"Gif/EWX_N0R_0.gif"));
            radarImageLayer.UpperThreshold = double.MaxValue;
            radarImageLayer.LowerThreshold = 0;
            radarImageLayer.Open();

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(radarImageLayer);
            layerOverlay.TileType = TileType.MultiTile;

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.Overlays.Add(layerOverlay);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = radarImageLayer.GetBoundingBox();

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }
    }
}
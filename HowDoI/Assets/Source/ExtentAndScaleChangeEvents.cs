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
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;

namespace CSHowDoISamples
{
    [Activity(Label = "Extent And ScaleChangeEvents")]
    public class ExtentAndScaleChangeEvents : SampleActivity
    {
        private MapView androidMap;
        private TextView labelExtent;
        private TextView labelScale;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/Countries02.shp"));
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.Country1;
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.TileType = TileType.SingleTile;
            layerOverlay.Layers.Add(worldLayer);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-133.2515625, 89.2484375, 126.9046875, -88.290625);
            androidMap.CurrentExtentChanged += AndroidMap_CurrentExtentChanged;
            androidMap.Overlays.Add("WorldOverlay", layerOverlay);

            labelExtent = new TextView(this);
            labelScale = new TextView(this);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType(), new Collection<View>() { labelExtent, labelScale });
        }

        private void AndroidMap_CurrentExtentChanged(object sender, CurrentExtentChangedMapViewEventArgs e)
        {
            PointShape upperLeftPoint = e.NewExtent.UpperLeftPoint;
            PointShape lowerRightPoint = e.NewExtent.LowerRightPoint;

            labelExtent.Text = string.Format("Map cureent extent: {0}, {1}, {2}, {3}", upperLeftPoint.X.ToString("n2"), upperLeftPoint.Y.ToString("n2"), lowerRightPoint.X.ToString("n2"), lowerRightPoint.Y.ToString("n2"));
            labelScale.Text = string.Format("Map cureent scale: {0}", ExtentHelper.GetScale(e.NewExtent, (float)androidMap.Width, androidMap.MapUnit).ToString("n4"));
        }
    }
}
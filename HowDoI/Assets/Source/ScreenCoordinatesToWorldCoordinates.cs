using System.Collections.ObjectModel;
using System.Drawing;
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
    [Activity(Label = "Screen Coordinates To World Coordinates")]
    public class ScreenCoordinatesToWorldCoordinates : SampleActivity
    {
        private MapView androidMap;
        private TextView screenPositionLable;
        private TextView worldPositionLable;

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
            androidMap.MapSingleTap += AndroidMap_MapSingleTap;

            screenPositionLable = new TextView(this);
            worldPositionLable = new TextView(this);
            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType(), new Collection<View>() { screenPositionLable, worldPositionLable });
        }

        private void AndroidMap_MapSingleTap(object sender, MotionEvent e)
        {
            PointF location = new PointF(e.GetX(), e.GetY());
            PointShape worldPosition = ExtentHelper.ToWorldCoordinate(androidMap.CurrentExtent, location.X,
                location.Y, androidMap.Width, androidMap.Height);

            screenPositionLable.Text = string.Format("Screen Position:({0:N4},{1:N4})", location.X, location.Y);
            worldPositionLable.Text = string.Format("World Position: ({0:N4},{1:N4})", worldPosition.X, worldPosition.Y);
        }
    }
}
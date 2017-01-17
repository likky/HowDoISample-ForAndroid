using Android.App;
using Android.OS;
using Android.Widget;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;

namespace CSHowDoISamples
{
    [Activity(Label = "Shortest Line Between Features")]
    public class ShortestLineBetweenFeatures : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(0, 100, 100, 0);

            LayerOverlay inMemoryOverlay = new LayerOverlay();
            androidMap.Overlays.Add("InMemoryOverlay", inMemoryOverlay);

            LayerOverlay shortestLineOverlay = new LayerOverlay();
            shortestLineOverlay.TileType = TileType.SingleTile;
            androidMap.Overlays.Add("ShortestLineOverlay", shortestLineOverlay);

            BaseShape areaShape1 = BaseShape.CreateShapeFromWellKnownData("POLYGON((10 20,30 60,40 10,10 20))");
            BaseShape areaShape2 = new EllipseShape(new PointShape(70, 70), 10, 20);

            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(125, GeoColor.StandardColors.Gray);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.Black;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            inMemoryLayer.InternalFeatures.Add("AreaShape1", new Feature(areaShape1));
            inMemoryLayer.InternalFeatures.Add("AreaShape2", new Feature(areaShape2));
            inMemoryOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);

            InMemoryFeatureLayer shortestLineLayer = new InMemoryFeatureLayer();
            shortestLineLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle.OuterPen.Color = GeoColor.StandardColors.Red;
            shortestLineLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;
            shortestLineOverlay.Layers.Add("ShortestLineLayer", shortestLineLayer);

            MultilineShape shortestLine = areaShape1.GetShortestLineTo(areaShape2, GeographyUnit.Meter);
            shortestLineLayer.InternalFeatures.Add("ShortestLine", new Feature(shortestLine));

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }
    }
}
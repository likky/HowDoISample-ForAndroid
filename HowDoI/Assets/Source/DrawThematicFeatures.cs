using Android.App;
using Android.OS;
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
    [Activity(Label = "Draw Thematic Features")]
    public class DrawThematicFeatures : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            ClassBreakStyle classBreakStyle = new ClassBreakStyle("POP1990");
            classBreakStyle.ClassBreaks.Add(new ClassBreak(453588, AreaStyles.CreateSimpleAreaStyle(GeoColor.StandardColors.Green)));
            classBreakStyle.ClassBreaks.Add(new ClassBreak(6314875, AreaStyles.CreateSimpleAreaStyle(GeoColor.StandardColors.LightYellow)));
            classBreakStyle.ClassBreaks.Add(new ClassBreak(12176161, AreaStyles.CreateSimpleAreaStyle(GeoColor.StandardColors.Yellow)));
            classBreakStyle.ClassBreaks.Add(new ClassBreak(18037448, AreaStyles.CreateSimpleAreaStyle(GeoColor.StandardColors.Crimson)));
            classBreakStyle.ClassBreaks.Add(new ClassBreak(23898734, AreaStyles.CreateSimpleAreaStyle(GeoColor.StandardColors.Red)));

            ShapeFileFeatureLayer statesLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/states.shp"));
            statesLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(AreaStyles.Country1);
            statesLayer.ZoomLevelSet.ZoomLevel01.CustomStyles.Add(classBreakStyle);
            statesLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(statesLayer);

            WorldMapKitOverlay worldMapKitOverlay = new WorldMapKitOverlay();

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-131.22, 55.05, -54.03, 16.91);
            androidMap.Overlays.Add(worldMapKitOverlay);
            androidMap.Overlays.Add(layerOverlay);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }
    }
}
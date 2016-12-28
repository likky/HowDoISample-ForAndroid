using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using System.Collections.ObjectModel;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Drawing;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;

namespace CSHowDoISamples
{
    [Activity(Label = "Edit Features From A FeatureLayer")]
    public class EditFeaturesFromAFeatureLayer : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(0, 100, 100, 0);

            InMemoryFeatureLayer inMemoryLayer = new InMemoryFeatureLayer();
            inMemoryLayer.InternalFeatures.Add("Polygon", new Feature("POLYGON((10 60,40 70,30 85, 10 60))"));
            inMemoryLayer.InternalFeatures.Add("Multipoint", new Feature("MULTIPOINT(10 20, 30 20,40 20, 10 30, 30 30, 40 30)"));
            inMemoryLayer.InternalFeatures.Add("Line", new Feature("LINESTRING(60 60, 70 70,75 60, 80 70, 85 60,95 80)"));
            inMemoryLayer.InternalFeatures.Add("Rectangle", new Feature(new RectangleShape(65, 30, 95, 15)));

            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.FillSolidBrush.Color = GeoColor.FromArgb(100, GeoColor.StandardColors.RoyalBlue);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle.OutlinePen.Color = GeoColor.StandardColors.Blue;
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultLineStyle.OuterPen = new GeoPen(GeoColor.FromArgb(200, GeoColor.StandardColors.Red), 5);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.DefaultPointStyle.SymbolPen = new GeoPen(GeoColor.FromArgb(255, GeoColor.StandardColors.Green), 8);
            inMemoryLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay inmemoryOverlay = new LayerOverlay();
            inmemoryOverlay.TileType = TileType.SingleTile;
            inmemoryOverlay.Layers.Add(new BackgroundLayer(new GeoSolidBrush(GeoColor.StandardColors.White)));
            inmemoryOverlay.Layers.Add("InMemoryFeatureLayer", inMemoryLayer);
            androidMap.Overlays.Add("InMemoryOverlay", inmemoryOverlay);

            Button editFeatureButton = new Button(this);
            editFeatureButton.Text = "Edit a feature";
            editFeatureButton.Click += EditFeatureButtonClick;
            editFeatureButton.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType(), new Collection<View>() { editFeatureButton });
        }

        private void EditFeatureButtonClick(object sender, System.EventArgs e)
        {
            LayerOverlay inMemoryOverlay = (LayerOverlay)androidMap.Overlays["InMemoryOverlay"];
            InMemoryFeatureLayer inMemoryLayer = (InMemoryFeatureLayer)inMemoryOverlay.Layers["InMemoryFeatureLayer"];

            inMemoryLayer.Open();
            inMemoryLayer.EditTools.BeginTransaction();
            inMemoryLayer.EditTools.Update(new Feature("POLYGON((10 60,40 70,30 85,20 90,10 60))", "Polygon"));
            inMemoryLayer.EditTools.CommitTransaction();
            inMemoryLayer.Close();

            androidMap.Overlays["InMemoryOverlay"].Refresh();
        }
    }
}
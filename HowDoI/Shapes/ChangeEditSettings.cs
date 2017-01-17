using System.Collections.ObjectModel;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;
using ThinkGeo.MapSuite.Styles;
using ThinkGeo.MapSuite.Drawing;

namespace CSHowDoISamples
{
    [Activity(Label = "Change Edit Settings")]
    public class ChangeEditSettings : SampleActivity
    {
        private MapView androidMap;
        private CheckBox canReShape;
        private CheckBox canResize;
        private CheckBox canRotate;
        private CheckBox canDrag;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayMapView);

            ShapeFileFeatureLayer worldLayer = new ShapeFileFeatureLayer(SampleHelper.GetDataPath(@"SampleData/Countries02.shp"));
            worldLayer.ZoomLevelSet.ZoomLevel01.DefaultAreaStyle = AreaStyles.CreateSimpleAreaStyle(GeoColor.FromArgb(255, 233, 232, 214), GeoColor.FromArgb(255, 118, 138, 69));
            worldLayer.ZoomLevelSet.ZoomLevel01.ApplyUntilZoomLevel = ApplyUntilZoomLevel.Level20;

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(worldLayer);

            Feature feature = new Feature(new RectangleShape(-55.5723249724898, 15.7443857300058, -10.5026750275102, -7.6443857300058));

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.DecimalDegree;
            androidMap.CurrentExtent = new RectangleShape(-79.303125, 76.471875, 0.853125000000006, -38.840625);
            androidMap.Overlays.Add(layerOverlay);
            androidMap.EditOverlay.EditShapesLayer.InternalFeatures.Add(feature.Id, feature);

            canReShape = new CheckBox(this);
            canReShape.Text = "ReShape";
            canReShape.CheckedChange += Setting_CheckedChange;

            canResize = new CheckBox(this);
            canResize.Text = "Resize";
            canResize.CheckedChange += Setting_CheckedChange;

            canRotate = new CheckBox(this);
            canRotate.Text = "Rotate";
            canRotate.CheckedChange += Setting_CheckedChange;

            canDrag = new CheckBox(this);
            canDrag.Text = "Drag";
            canDrag.CheckedChange += Setting_CheckedChange;

            LinearLayout linearLayout = new LinearLayout(this);
            linearLayout.Orientation = Orientation.Horizontal;
            linearLayout.AddView(canReShape);
            linearLayout.AddView(canResize);
            linearLayout.AddView(canRotate);
            linearLayout.AddView(canDrag);

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType(), new Collection<View>() { linearLayout });
        }

        private void Setting_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            androidMap.EditOverlay.CanReshape = canReShape.Checked;
            androidMap.EditOverlay.CanResize = canResize.Checked;
            androidMap.EditOverlay.CanRotate = canRotate.Checked;
            androidMap.EditOverlay.CanDrag = canDrag.Checked;

            androidMap.EditOverlay.CalculateAllControlPoints();
            androidMap.Refresh();
        }
    }
}
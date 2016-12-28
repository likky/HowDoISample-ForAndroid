using Android.App;
using Android.OS;
using Android.Widget;
using ThinkGeo.MapSuite;
using ThinkGeo.MapSuite.Android;
using ThinkGeo.MapSuite.AndroidEdition;
using ThinkGeo.MapSuite.Core;
using ThinkGeo.MapSuite.Layers;
using ThinkGeo.MapSuite.Shapes;

namespace CSHowDoISamples
{
    [Activity(Label = "Use Google Overlay")]
    public class UseGoogleMapsOverlay : SampleActivity
    {
        private MapView androidMap;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DisplayASimpleMap);

            androidMap = FindViewById<MapView>(Resource.Id.androidmap);
            androidMap.MapUnit = GeographyUnit.Meter;
            androidMap.ZoomLevelSet = new GoogleMapsZoomLevelSet();

            GoogleMapsOverlay googleMapsOverlay = new GoogleMapsOverlay();
            googleMapsOverlay.TileType = TileType.MultiTile;
            androidMap.Overlays.Add(googleMapsOverlay);

            ManagedProj4Projection proj4 = new ManagedProj4Projection();
            proj4.InternalProjectionParametersString = ManagedProj4Projection.GetDecimalDegreesParametersString();
            proj4.ExternalProjectionParametersString = ManagedProj4Projection.GetGoogleMapParametersString();
            proj4.Open();

            androidMap.CurrentExtent = proj4.ConvertToExternalProjection(new RectangleShape(-139.2, 92.4, 120.9, -93.2)) as RectangleShape;

            SampleViewHelper.InitializeInstruction(this, FindViewById<RelativeLayout>(Resource.Id.MainLayout), GetType());
        }
    }
}
using Android.App;
using Android.Content;
using Android.Views;
using ThinkGeo.MapSuite.Android;

namespace CSHowDoISamples
{
    public abstract class SampleActivity : Activity
    {
        private bool isDisposed;

        protected override void OnStart()
        {
            base.OnStart();
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ActionBarMenu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.viewSource:
                    Intent intent = new Intent(this, typeof(WebViewActivity));
                    intent.PutExtra("SampleName", GetType().Name);
                    StartActivity(intent);
                    break;

                case Android.Resource.Id.Home:
                    Finish();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnDestroy()
        {
            MapView mapView = FindViewById<MapView>(Resource.Id.androidmap);
            if (mapView != null && !isDisposed)
            {
                isDisposed = true;
                mapView.Dispose();
            }
            base.OnDestroy();
        }
    }
}
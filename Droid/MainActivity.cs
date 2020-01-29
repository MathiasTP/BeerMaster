using System.Net;
using Android.App;
using Android.Content.PM;
using Android.OS;
using BeerBong;

namespace TodoREST.Droid
{
    [Activity(Label = "TodoREST.Droid", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            Instance = this;
            global::Xamarin.Forms.Forms.Init(this, bundle);
            
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            LoadApplication(new App());
        }
    }
}


using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(Pedidos.Droid.GerarPdf))]
namespace Pedidos.Droid
{
    [Activity(Label = "Pedidos", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }
    }

    public class GerarPdf : IGerarPdf
    {
        public async Task Gerar(Pedido ped)
        {
            Pdf pdf = new Pdf(ped);
            await pdf.GerarPdf();
            Toast.MakeText(Application.Context, "PDF gerado com sucesso!", ToastLength.Short).Show();
        }
    }
}
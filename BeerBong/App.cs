using System.Collections.Generic;
using BeerBong.Models;
using BeerBong.Services;
using BeerBong.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Forside = BeerBong.Views.Forside;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BeerBong
{
	public class App : Application
	{
        
		// Fungere som singelton pattern
		public static ServiceManager ServiceManager { get; private set; }


        public static bool isLoggedIn = false;

        public static string BrugernavnOnLogIn;

        public static string Token;

        public static Player player;

        public static QueueModstander modstander;
        public static Game game;

        public static List<WebsocketData> BeerBongData;

        public static string gameresultat;

        public static List<OnlineLeaderboard> board;
        public App ()
		{
			ServiceManager = new ServiceManager (new RestService ());
            
			MainPage = new NavigationPage (new Forside ());
		}

        

		protected override void OnStart ()
        {
            
        }

		protected override void OnSleep ()
		{
			
		}

		protected override void OnResume ()
		{
			
		}
	}
}


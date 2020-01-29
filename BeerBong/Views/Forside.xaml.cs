using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerBong.Models;
using BeerBong.Services;
using BeerBong.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeerBong.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Forside : ContentPage
    { 

        bool isNewItem;
        WebsocketClient websocket = new WebsocketClient();
        public Forside(bool isNew = false)
        {

            InitializeComponent();
            isNewItem = false;
            
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (App.isLoggedIn == true)
            {
                ToolbarItem brugerToolbarItem = new ToolbarItem
                {
                    Text = App.BrugernavnOnLogIn,

                    Order = ToolbarItemOrder.Primary,
                    Priority = 0
                };

                this.ToolbarItems.Clear();
                this.ToolbarItems.Add(brugerToolbarItem);
                this.LoginButton.IsVisible = false;
                this.OpretBrugerButton.IsVisible = false;
            }
            

        }

        async void GoToLeaderboard(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Leaderboard());


        }
    

        async void OpretBruger(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new OpretBruger(true)
            //{
            //    BindingContext = new RegisterUser
            //    {
            //    }
            //});
            await Navigation.PushAsync(new OpretBruger());
        }

        async void GoToLogin(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());

        }

        async void ForbindBT(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BlueTooth());
        }

        async void GoToMultiplayer(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Multiplayer());

        }
        async void ForbindWebsocket(object sender, EventArgs e)
        {
            await websocket.ConnectToWebsocket();
        }
    }
}
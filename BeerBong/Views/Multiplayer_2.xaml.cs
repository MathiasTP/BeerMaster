using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BeerBong.Models;
using BeerBong.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeerBong.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Multiplayer_2 : ContentPage
    {
        double playertid;
        double modstandertid;
        private string state;
        public Multiplayer_2()
        {
            InitializeComponent();
            Timer1();
            modstanderentry.Text = App.modstander.brugerNavn;
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
            }

            KlarButton.IsVisible = false;
            DrikTimer.IsVisible = false;
        }


        public async void Timer1()
        {

            for (int i = 0; i < 31; i++)
            { 
                TTimer.Text = i.ToString(); 

                await Task.Delay(1000);

                if (i==30)
                {
                    TTimer.IsVisible = false;
                    fyldop.IsVisible = false;
                   await WaitForBeerBongToFill();
                    if (state!="Fullstate") // skal være !=
                    {
                        await DisplayAlert("Ølbong ikke fyldt!",
                            "Din ølbong er ikke fyldt med den rette mængde, du har tabt.", "ok");
                        await Navigation.PopAsync();
                    }
                    else
                    {
                        KlarButton.IsVisible = true;
                    }
                    break;
                }
                
            }
        }

        public async void Timer2()
        {
            DrikTimer.IsVisible = true;
            int i = 0;
            for ( i = 0; i < 31; i++)
            {
                DrikTimer.Text = i.ToString();

                await Task.Delay(1000);

                if (i == 30)
                {
                    await WaitForBeerBongToFill();
                    Stats playerstats = new Stats()
                    {
                        playerId = App.player.PlayerId,
                        time = playertid
                       
                    };
                    int id = App.player.PlayerId;
                    await App.ServiceManager.PushStats(playerstats);
                    await App.ServiceManager.PushtoLeaderboard(id);
                    

                    var gameresult = await App.ServiceManager.GetGameResult(App.game.gameId);
                    var result = App.gameresultat;
                    

                    foreach (var p in gameresult)
                    {
                        if (p.playerid == App.player.PlayerId)
                        {
                            userTid.Text = "Din tid: " + p.time + " sekunder";
                             playertid = p.time;
                        }

                        if (p.playerid == App.modstander.Playerid)
                        {
                            modstanderTid.Text = App.modstander.brugerNavn + ": " + p.time + " sekunder";
                             modstandertid = p.time;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (playertid<modstandertid)
                    {
                        VinderEntry.Text = "Vinderen er: " + App.BrugernavnOnLogIn;
                    }
                    else
                    {
                        VinderEntry.Text = "Vinderen er: " + App.modstander.brugerNavn;
                    }
                    break;
                }


            }
        }

        public async Task WaitForBeerBongToFill()
        {
            List<WebsocketData> data = new List<WebsocketData>();
            WebsocketClient websocket = new WebsocketClient();
            data = await websocket.ConnectToWebsocket();
            CultureInfo cultur = new CultureInfo("en-US");
            playertid = double.Parse(data[0].time, CultureInfo.InvariantCulture);
            
            
            state = data[0].state;

        }

        private void KlarButton_OnClicked(object sender, EventArgs e)
        {
            Timer2();
        }
    }
    
}

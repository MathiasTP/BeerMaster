using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeerBong.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BeerBong.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Multiplayer : ContentPage
    {

        bool isNewItem;

        public Multiplayer(bool isNew = false)
        {
            InitializeComponent();
            
            isNewItem = false;
        }

        

        async void OnFindModstander(object sender, EventArgs e)
        {
            QueueModstander modstander = new QueueModstander();
           bool status = await App.ServiceManager.GetFirstPlayerInQueue(modstander);
            if (status == false)
            {
                await DisplayAlert("Venter på at finde en modstander", "Venter på en modstander joiner dit spil", "ok");
                while (status==false)
                {
                    App.game = new Game();
                    var game = App.game;
                    while (game.players==null)
                    {
                        game = App.game;
                        status = await App.ServiceManager.GetFirstPlayerInQueue(modstander);
                        break;
                    }

                    break;
                }
                
            }

            if (status == true)
            {
                await Navigation.PushAsync(new Multiplayer_2());
            }
            

        }

    }
}
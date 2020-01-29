using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using BeerBong.Models;
using Xamarin.Forms;

namespace BeerBong.ViewModel
{
   public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(propertyName));
        }

        public bool tænker { get; set; }
        public string username { get; set; }
        public string password { get; set; }


        public Command OnLoginCommand
        {
            get
            {
                return new Command(() =>
                {
                    tænker = true;
                    ActivityIndicator tænkeraActivityIndicator = new ActivityIndicator{IsRunning =true};
                    
                    OnLoginClicked(username, password);
                    OnPropertyChanged();

                });
            }
        }

        public LoginViewModel()
       {

       }



        public async void OnLoginClicked(string username, string password)
        
        {
            tænker = true;
           var login = new LoginUser()
           {
               userName = username,
               passWord = password
           };
           

           bool status = await App.ServiceManager.GetLoginDataAsync(login);


           if (status == true)
           {
               tænker = false;
               await Application.Current.MainPage.DisplayAlert("Login succesfuldt!",
                   "Du er logget ind som: " + username, "OK");
                App.BrugernavnOnLogIn = username;
               
               
               App.isLoggedIn = true;
               
               await Application.Current.MainPage.Navigation.PopAsync();

           }
           else
           {
               tænker = false;
               await Application.Current.MainPage.DisplayAlert("Login fejl!", "Brugernavn eller password forkert. Prøv igen", "OK");
           }
        }
    }
}

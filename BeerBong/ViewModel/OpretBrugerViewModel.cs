using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BeerBong.Models;
using Xamarin.Forms;

namespace BeerBong.ViewModel
{
   public class OpretBrugerViewModel : INotifyPropertyChanged
    {
        public string _password1 { get; set; }
        public string _password2 { get; set; }
        public string username { get; set; }
        
        public bool passwordstatus { get; set; }

        public bool apistatus { get; set; }
        
        public OpretBrugerViewModel()
        {
            
        }

        
        private string _Passwordchecktext;
        

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Command OnClickOpretBruger
        {
            get
            {
                return new Command(() =>
                {
                    passwordstatus = PasswordCheck();
                    OnClickOpretBruger.CanExecute(true);
                    if (passwordstatus!=true)
                    {
                        Application.Current.MainPage.DisplayAlert("Passwords Matcher ikke",
                            "De to password matcher ikke hinanden. Prøv igen", "ok");
                        OnPropertyChanged();
                    }
                    else
                    {
                        OpretBrugerSucces(username, _password1);
                        
                    }
                });
            }
        }

        public async void OpretBrugerSucces(string username, string password)
        {
            var bruger = new RegisterUser()
            {
                userName = username,
                
                passWord = password
            };

            apistatus = await App.ServiceManager.SaveOpretBruger(bruger);
            if (apistatus == true)
            {
                await Application.Current.MainPage.DisplayAlert("Bruger succesfuldt oprettet!", "Du er oprettet som: " + this.username, "OK");
                await Application.Current.MainPage.Navigation.PopAsync();
            }

            if (apistatus == false)
            {
                await Application.Current.MainPage.DisplayAlert("FEJL", "Prøv med et andet brugernavn og sørg for at dit password er mindst 8 karaktere langt", "OK");
            }
        }

        public bool PasswordCheck()
        {
            if (_password1 != _password2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}

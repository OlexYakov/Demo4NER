﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Demo4NER.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Demo4NER.Services;
using Demo4NER.Views;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace Demo4NER
{
    public partial class App : Application
    {
        public ProfilePage ProfilePage { get; set; }

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            Debug.WriteLine(Properties.ToString());
        }

        protected override void OnStart()
        {
            var _mainPage = new MainPage();
            if (!Properties.ContainsKey("logged"))
            {
                LoginPage loginPage = new LoginPage();
                MainPage = new NavigationPage(loginPage);
                MainPage.Navigation.InsertPageBefore(_mainPage, loginPage);
            }
            else
            {
                MainPage = _mainPage;
            }
        }

        protected override void OnSleep()
        {

            SavePropertiesAsync();
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public async Task<Location> GetLocationAsync(bool forceNew = false)
        {

            Location cachedLocation = await Geolocation.GetLastKnownLocationAsync();
            Location location;
            TimeSpan diff = DateTimeOffset.Now.Subtract(cachedLocation.Timestamp);
            Debug.WriteLine(diff);
            if (forceNew || diff.Minutes > 1)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                location = await Geolocation.GetLocationAsync(request);
            }
            else location = cachedLocation;


            //if (Current.Properties.ContainsKey("UserLocation"))
            //    Current.Properties["UserLocation"] = location;
            //else
            //    Current.Properties.Add("UserLocation", location);

            return location;
        }

        public void SaveUserInProperties(User user)
        {
            String serialized = JsonConvert.SerializeObject(user);
            if (Properties.ContainsKey("logged"))
                Properties["logged"] = serialized;
            else
                Properties.Add("logged", serialized);
        }

        public User GetUserFromProperties()
        {
            User user = null;
            if (Properties.ContainsKey("logged"))
                user = JsonConvert.DeserializeObject<User>((string)Properties["logged"]);
            return user;
        }
    }
}

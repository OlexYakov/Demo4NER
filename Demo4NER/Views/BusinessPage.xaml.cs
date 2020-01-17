﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Demo4NER.Models;
using Demo4NER.Services;
using Demo4NER.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Demo4NER.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BusinessPage : ContentPage
    {
        public BusinessPageViewModel viewModel;
        public BusinessPage(Business selectedBusiness)
        {
            InitializeComponent();
            BindingContext = viewModel = new BusinessPageViewModel(selectedBusiness);
        }


        private void EditBusiness(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new EditBusinessPage(viewModel.Business));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.Business.Links != null) // TODO make sure it's never null
            {
                int n = viewModel.Business.Links.Count;
                linksView.HeightRequest = n * 30;
            }
            if (viewModel.Business.Reviews != null)
            {
                int nr = viewModel.Business.Reviews.Count;
                ReviewList.HeightRequest = nr * 110;
            }
        }

        private async void OpenURLOnTap(object sender, EventArgs e)
        {
            String url = (sender as Label).Text;
            //Device.OpenUri(new Uri(url));
            if (!url.StartsWith("https://") && !url.StartsWith("http://"))
                url = "https://" + url;
            await Launcher.OpenAsync(url);
        }

        private void OnNavigateClicked(object sender, EventArgs e)
        {
            viewModel.NavigateToMapViewCommand.Execute(null);
        }

        private void ContactLabelOnTapped(object sender, EventArgs e)
        {
            try
            {
                PhoneDialer.Open((sender as Label)?.Text);
            }
            catch (ArgumentNullException argumentNullException)
            {

            }
            catch (FeatureNotSupportedException featureNotSupportedException)
            {

            }
            catch (Exception exception)
            {

            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;
using Supremes;
using Supremes.Nodes;
using Xamarin.Forms;
using XFGrest.Classes;

namespace XFGrest.Pages
{
    public partial class LoginPage : ContentPage
    {
        private string cookies;

        public LoginPage()
        {
            Initialize();
        }

        protected void Initialize()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            Title = "Login";

            LabPicker.ItemsSource = App.labs;

            label1.Scale = 0;
            LabPicker.Scale = 0;
            PassEntry.Scale = 0;
            buttonStack.Scale = 0;
            if (Device.RuntimePlatform == Device.iOS)
            {
                LoginStack.Spacing = 4;
                LabPicker.BackgroundColor = Color.FromHex("#3FFF");
                PassEntry.BackgroundColor = Color.FromHex("#3FFF");
            }
        }

        async void AuthButtonClicked(object sender, EventArgs e)
        {
            await btnAuthenticate.FadeTo(0, App.AnimationSpeed, Easing.SinIn);
            btnAuthenticate.IsVisible = false;

            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
            LodingLabel.IsVisible = true;

            if(LabPicker.SelectedIndex == App.labs.Count - 1)
                JsonRequests.labID = 99;
            else
                JsonRequests.labID = LabPicker.SelectedIndex;
           
            JsonRequests.password = PassEntry.Text;

            if (!await JsonRequests.getUsers())
            {
                System.Diagnostics.Debug.WriteLine("Connection Error!");
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                LodingLabel.IsVisible = false;

                btnAuthenticate.IsVisible = true;
                await btnAuthenticate.FadeTo(1, App.AnimationSpeed, Easing.SinIn);
                return;
            }

            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            LodingLabel.IsVisible = false;

            await label1.FadeTo(0, App.AnimationSpeed, Easing.SinIn);
            await LabPicker.FadeTo(0, App.AnimationSpeed, Easing.SinIn);
            await PassEntry.FadeTo(0, App.AnimationSpeed, Easing.SinIn);


            await Navigation.PushAsync(new MainPage());
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await JsonRequests.getTables();

            Initialize();

            await Task.Delay(App.DelaySpeed);
            await label1.ScaleTo(1, App.AnimationSpeed, Easing.SinIn);
            await LabPicker.ScaleTo(1, App.AnimationSpeed, Easing.SinIn);
            await PassEntry.ScaleTo(1, App.AnimationSpeed, Easing.SinIn);
            await buttonStack.ScaleTo(1, App.AnimationSpeed, Easing.SinIn);
        }
    }
}

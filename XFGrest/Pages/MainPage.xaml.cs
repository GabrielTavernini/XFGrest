using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFGrest.Classes;

namespace XFGrest.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasNavigationBar(this, false);

            InfoList.ItemsSource = App.Users;
            InfoList.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };
            InfoList.Refreshing += (sender, e) => { Refresh(); };
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();
            Device.StartTimer(new TimeSpan(0, 0, 10), Refresh);
		}

		private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //InfoList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                InfoList.ItemsSource = App.Users;
            else
                InfoList.ItemsSource = App.Users.AsEnumerable().Where(i => i.name.ToLower().Contains(e.NewTextValue.ToLower()));

            //InfoList.EndRefresh();
        }    

        private bool Refresh()
        {
            System.Diagnostics.Debug.WriteLine("Refresh");

            Task.Run(async () => await HttpRequests.LoginAsync("",""))
                        .ContinueWith((end) => {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                if (SearchBar.Text != "" && SearchBar.Text != null)
                                    InfoList.ItemsSource = App.Users.AsEnumerable().Where(i => i.name.ToLower().Contains(SearchBar.Text.ToLower()));
                                else
                                    InfoList.ItemsSource = App.Users;

                                InfoList.EndRefresh();
                            });
                        }); 
            return App.Foreground;
        }

        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
    }
}

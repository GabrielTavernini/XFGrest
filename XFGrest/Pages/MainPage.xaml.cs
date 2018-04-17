using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFGrest.Classes;
using XFGrest.Controls;

namespace XFGrest.Pages
{
    public partial class MainPage : ContentPage
    {
        Boolean refreshAlternating = false;
        List<User> list;

        public MainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, true);
            Title = String.Format("Giorno {0} ({1})", (App.Day + 1), App.dates[App.Day].ToString("dd/MM/yyyy"));

            InfoList.ItemsSource = getItems();
            InfoList.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };
            InfoList.Refreshing += (sender, e) => { Refresh(); };
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();
            Device.StartTimer(new TimeSpan(0, 0, 5), Refresh);
		}

		protected override void OnDisappearing()
		{
            base.OnDisappearing();
            //Task.Run(async () => {foreach (User u in list) { await JsonRequests.PostUserEdit(u); } });
            App.Foreground = false;
        }

		private void SearchBar_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            //InfoList.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                InfoList.ItemsSource = list;
            else
                InfoList.ItemsSource = list.AsEnumerable().Where(i => i.name.ToLower().Contains(e.NewTextValue.ToLower()));

            //InfoList.EndRefresh();
        }    

        private bool Refresh()
        {
            System.Diagnostics.Debug.WriteLine("Refresh");

            Task.Run(async () => await JsonRequests.getUsers())
                        .ContinueWith((end) => {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                if (SearchBar.Text != "" && SearchBar.Text != null)
                                    InfoList.ItemsSource = getItems().AsEnumerable().Where(i => i.name.ToLower().Contains(SearchBar.Text.ToLower()));
                                else
                                    InfoList.ItemsSource = getItems();

                                InfoList.EndRefresh();
                            });
                        }); 
            return App.Foreground;
        }


        public List<User> getItems()
        {
            /*
            if (JsonRequests.labID == App.labs.Count - 1)
            {
                list = App.Users;
                return list;
            }
                


            list = new List<User>();
            foreach(User u in App.Users)
            {
                if (u.lab == JsonRequests.labID)
                    list.Add(u);
            }*/
            return App.Users;//list;
        }
    }
}

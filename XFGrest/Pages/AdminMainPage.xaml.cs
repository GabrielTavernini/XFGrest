using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFGrest.Classes;
using XFGrest.Controls;

namespace XFGrest.Pages
{
    public partial class AdminMainPage : ContentPage
    {
        Boolean refreshAlternating = false;
        
        public AdminMainPage()
        {
            InitializeComponent();

            NavigationPage.SetHasBackButton(this, false);
            NavigationPage.SetHasNavigationBar(this, true);
            Title = String.Format("Giorno {0} ({1})", (App.Day + 1), App.dates[App.Day].ToString("dd/MM/yyyy"));

            InfoList.ItemsSource = App.Users;
            InfoList.ItemSelected += (sender, e) => { ((ListView)sender).SelectedItem = null; };
            InfoList.Refreshing += (sender, e) => { Refresh(); };
        }

		protected override void OnAppearing()
		{
            base.OnAppearing();
            Device.StartTimer(new TimeSpan(0, 0, 5), Refresh);
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
            //Task.Run(async () => { foreach (User u in App.Users) { await HttpRequests.PostUserEdit(u); } });

            Task.Run(async () => await HttpRequests.extaractUsers())
                        .ContinueWith((end) => {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                if (SearchBar.Text != "" && SearchBar.Text != null)
                                    InfoList.ItemsSource = App.Users.AsEnumerable().Where(i => i.name.ToLower().Contains(SearchBar.Text.ToLower()));
                                else
                                    InfoList.ItemsSource = App.Users;

                                System.Diagnostics.Debug.WriteLine(App.Users.Count);

                                InfoList.EndRefresh();
                            });
                        }); 
            return App.Foreground;
        }

        private bool RefreshAndSave()
        {
            if(refreshAlternating)
            {
                System.Diagnostics.Debug.WriteLine("RefreshAndSave");

                Task.Run(async () => {
                        //foreach (User u in App.Users) { await HttpRequests.PostUserEdit(u); }
                        await HttpRequests.extaractUsers(); 
                    }).ContinueWith((end) => {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                if (SearchBar.Text != "" && SearchBar.Text != null)
                                    InfoList.ItemsSource = App.Users.AsEnumerable().Where(i => i.name.ToLower().Contains(SearchBar.Text.ToLower()));
                                else
                                    InfoList.ItemsSource = App.Users;

                                InfoList.EndRefresh();
                            });
                        }); 
            
            }else
            {
                System.Diagnostics.Debug.WriteLine("Save");

                //Task.Run(async () => { foreach (User u in App.Users) { await HttpRequests.PostUserEdit(u); } });
            }

            refreshAlternating = !refreshAlternating;
            return App.Foreground;
        }
    }
}

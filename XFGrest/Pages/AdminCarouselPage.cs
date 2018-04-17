using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFGrest.Classes;

namespace XFGrest.Pages
{
    public class AdminCarouselPage : CarouselPage
    {
        Boolean refreshAlternating = false;
        static public int pagesNumber = 6;
        public static ListView currentInfoList;
        public static SearchBar currentSearchBar;

        public AdminCarouselPage()
        {
            /*NavigationPage.SetHasNavigationBar(this, false);
            Children.Add(new TestPage("Voti", Color.FromHex("#00B1D4"), 72));
            Children.Add(new TestPage("Medie", Color.FromHex("#61DDDD"), 72));
            Children.Add(new TestPage("Argomenti", Color.FromHex("#B2D235"), 56));
            Children.Add(new TestPage("Note", Color.FromHex("#F2AA52"), 72));
            Children.Add(new TestPage("Assenze", Color.FromHex("#E15B5C"), 72));
            Children.Add(new TestPage("Impostazioni", Color.FromHex("#E15BBB"), 50)); */
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            NavigationPage.SetHasNavigationBar(this, false);

            //Children.Add(new AdminMainPage(0));
            //Children.Add(new AdminMainPage(1));
            //Children.Add(new AdminMainPage(2));
            //Children.Add(new AdminMainPage(3));

            Device.StartTimer(new TimeSpan(0, 0, 5), RefreshAndSave);
        }



        private bool RefreshAndSave()
        {
            if (refreshAlternating)
            {
                System.Diagnostics.Debug.WriteLine("RefreshAndSave");

                Task.Run(async () => {
                    foreach (User u in App.Users)
                    {
                        await HttpRequests.PostUserEdit(u);
                        await HttpRequests.extaractUsers();
                    }
                }).ContinueWith((end) => {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        if (currentSearchBar.Text != "" && currentSearchBar.Text != null)
                            currentInfoList.ItemsSource = App.Users.AsEnumerable().Where(i => i.name.ToLower().Contains(currentSearchBar.Text.ToLower()));
                        else
                            currentInfoList.ItemsSource = App.Users;

                        currentInfoList.EndRefresh();
                    });
                });

            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Save");

                Task.Run(async () => { foreach (User u in App.Users) { await HttpRequests.PostUserEdit(u); } });
            }

            refreshAlternating = !refreshAlternating;
            return App.Foreground;
        }
    }
}
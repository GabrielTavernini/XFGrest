using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace XFGrest
{
    public partial class App : Application
    {
        public static string firstPage = "";
        public static uint AnimationSpeed = 75;
        public static int DelaySpeed = 300;
        public static int Day = 1;
        public static bool Foreground = true;

        public static String[] sports = new String[3] { "Calcio", "Pallavolo", "Giochi Vari" };
        public static List<User> Users = new List<User>();

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new Pages.LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            Foreground = false;
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
            Foreground = true;
        }
    }
}

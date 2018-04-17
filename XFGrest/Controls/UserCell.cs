using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XFGrest.Classes;

namespace XFGrest.Controls
{
    public class UserCell : ViewCell
    {
        public UserCell()
        {
            StackLayout st1 = new StackLayout() { Orientation = StackOrientation.Horizontal};
            StackLayout st2 = new StackLayout() { Orientation = StackOrientation.Horizontal, HorizontalOptions = LayoutOptions.FillAndExpand };
            StackLayout mainSt = new StackLayout() { HorizontalOptions = LayoutOptions.FillAndExpand, Spacing = 0 };

            Label subjectL = new Label()
            {
                TextColor = Color.DimGray,
                FontAttributes = FontAttributes.Bold,
                FontSize = 18,
                VerticalTextAlignment = TextAlignment.End,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                Margin = new Thickness(10, 0, 0, 0)
            };

            Label typeL = new Label()
            {
                Margin = new Thickness(10, 0, 0, 0),
                TextColor = Color.DimGray,
                FontSize = 16,
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalTextAlignment = TextAlignment.Start,
                VerticalOptions = LayoutOptions.Start
            };

            Switch presenceS = new Switch()
            {
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            };
            presenceS.Toggled += (object sender, ToggledEventArgs e) => PresenceChanged(sender, e);

            Picker sportP = new Picker()
            {
                Margin = new Thickness(10, 0, 0, 0),
                ItemsSource = App.sports,
                Title = "Seleziona Sport",
                VerticalOptions = LayoutOptions.Center
            };
            sportP.SelectedIndexChanged += async (sender, e) => SportChanged(sender, e);


            st1.Children.Add(subjectL);
            st1.Children.Add(sportP);
            st1.Children.Add(presenceS);


            View = st1;


            //-----------------Bindings-----------------
            subjectL.SetBinding(Label.TextProperty, nameof(User.displayString));
            presenceS.SetBinding(Switch.IsToggledProperty, nameof(User.presence), BindingMode.TwoWay);
            sportP.SetBinding(Picker.SelectedItemProperty, nameof(User.sport), BindingMode.TwoWay);
        }

        private async void SportChanged(object sender, EventArgs e)
        {
            await JsonRequests.PostUserEdit(this.BindingContext as User);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                await Task.Delay(75);
                await View.ScaleTo(1, 75, Easing.SpringOut);
            }
            catch { }
        }

        private async void PresenceChanged(object sender, EventArgs e)
        {
            await JsonRequests.PostUserEdit(this.BindingContext as User);
        }

    }
}

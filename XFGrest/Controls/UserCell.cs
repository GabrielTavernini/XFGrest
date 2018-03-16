using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFGrest.Controls
{
    public class UserCell : ViewCell
    {
        static int colorChange = 0;
        
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

            Picker sportP = new Picker()
            {
                Margin = new Thickness(10, 0, 0, 0),
                ItemsSource = App.sports,
                Title = "Seleziona Sport",
                //HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center
            };

            /*
            Grid mainG = new Grid()
            {
                Scale = 0,
                BackgroundColor = Color.Transparent,
                ColumnDefinitions = {
                    new ColumnDefinition{Width = GridLength.Auto},
                    new ColumnDefinition{Width = new GridLength(1, GridUnitType.Star)}
                },

                RowDefinitions = {
                    new RowDefinition{Height = GridLength.Auto},
                    new RowDefinition{Height = new GridLength(1, GridUnitType.Star)}
                }
            };

            if (colorChange == 0)
            {
                View.BackgroundColor = Color.White;
                colorChange = 1;                
            }
            else
            {
                View.BackgroundColor = Color.Silver;
                colorChange = 0;   
            }

            mainG.Children.Add(subjectL, 0, 0);
            mainG.Children.Add(typeL, 0, 1);
            mainG.Children.Add(presenceS, 1, 0);
            mainG.Children.Add(sportP, 1, 1);*/

            st1.Children.Add(subjectL);
            st1.Children.Add(sportP);
            st1.Children.Add(presenceS);
            //st2.Children.Add(typeL);

            //mainSt.Children.Add(st1);
            //mainSt.Children.Add(st2);


            View = st1;


            //-----------------Bindings-----------------
            subjectL.SetBinding(Label.TextProperty, nameof(User.displayString));
            presenceS.SetBinding(Switch.IsToggledProperty, nameof(User.presence));
            sportP.SetBinding(Picker.SelectedItemProperty, nameof(User.sport));
            //typeL.SetBinding(Label.TextProperty, nameof(User.etaString));
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

        protected override async void OnTapped()
        {
            base.OnTapped();

            try
            {
                await View.ScaleTo(1.2, 125);
                await View.ScaleTo(1, 125);
            }
            catch { }

        }
    }
}

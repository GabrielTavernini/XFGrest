﻿using System;
using Android.Content;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using XamarinForms;
using XamarinForms.Droid.Renderes;
using XFGrest.Controls;

[assembly: ExportRenderer(typeof(SportButton), typeof(CustomButtonRenderer))]
namespace XamarinForms.Droid.Renderes
{
    public class CustomButtonRenderer : ButtonRenderer
    {
        public CustomButtonRenderer(Context context) : base(context) {}

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (Control != null)
            {
                var roundableShape = new GradientDrawable();
                roundableShape.SetShape(ShapeType.Rectangle);
                roundableShape.SetStroke(1, Color.FromHex("#AFFF").ToAndroid());
                roundableShape.SetColor(Element.BackgroundColor.ToAndroid());
                roundableShape.SetCornerRadius(25);
                Control.Background = roundableShape;
                Control.TransformationMethod = null;
                Control.Elevation = 0;
            }
            base.OnElementPropertyChanged(sender, e);
        }
            /*var roundableShape = new GradientDrawable();
            roundableShape.SetShape(ShapeType.Rectangle);
            roundableShape.SetStroke(1, Color.FromHex("#AFFF").ToAndroid());
            roundableShape.SetColor(color.ToAndroid());
            roundableShape.SetCornerRadius(25);
            Control.Background = roundableShape;
            Control.TransformationMethod = null;
            Control.Elevation = 0;*/
    }
}

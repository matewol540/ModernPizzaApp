using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MobilePizzaApp.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using MobilePizzaApp.MarkupExtension;
using Android.Graphics;
using Xamarin.Forms.Xaml;

[assembly: ExportRenderer(typeof(GradientFrameRenderer), typeof(FrameGradientRendererDroid))]

namespace MobilePizzaApp.Droid
{
    public class FrameGradientRendererDroid : FrameRenderer
    {
        public FrameGradientRendererDroid(Context context) : base(context)
        {
        }

        private Xamarin.Forms.Color StartColor { get; set; }
        private Xamarin.Forms.Color EndColor { get; set; }


        public override void Draw(Canvas canvas)
        {
            base.Draw(canvas);
            var gradient = new Android.Graphics.LinearGradient(0, Height, 0, 0,
                this.StartColor.ToAndroid(), this.EndColor.ToAndroid(), Android.Graphics.Shader.TileMode.Mirror);

            var paint = new Android.Graphics.Paint()
            {
                Dither = true,
                AntiAlias = true
            };
            paint.SetShader(gradient);
            var rect = new RectF(0,0,Width,Height);
            canvas.DrawRect(rect, paint);
        }


        protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
        {


            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }
            try
            {
                var stack = e.NewElement as GradientFrameRenderer;
                this.StartColor = stack.StartColor;
                this.EndColor = stack.EndColor;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(@"ERROR:", ex.Message);
            }

        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.MarkupExtension
{
    [ContentProperty("ResourceId")]
    public class EmbeddedClass : IMarkupExtension
    {
        public string ResourceId { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (String.IsNullOrEmpty(ResourceId))
                return null;
            return ImageSource.FromResource(ResourceId);
        }
    }
}

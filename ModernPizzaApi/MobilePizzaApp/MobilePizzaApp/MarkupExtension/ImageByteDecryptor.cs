using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobilePizzaApp.MarkupExtension
{
    [ContentProperty("ImageBytesArray")]
    public class ImageByteDecryptor : IMarkupExtension
    {
        public byte[] ImageBytesArray { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ImageBytesArray.Count() == 0)
            {
                return null;
            }
            return ImageSource.FromStream(() => new MemoryStream(ImageBytesArray));
        }
    }
}

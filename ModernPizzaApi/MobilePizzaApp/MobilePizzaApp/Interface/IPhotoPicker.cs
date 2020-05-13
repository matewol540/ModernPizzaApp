using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace MobilePizzaApp.Interface
{
   public  interface IPhotoPicker
    {
        Task<Stream> GetImageStreamAsync();
    }
}

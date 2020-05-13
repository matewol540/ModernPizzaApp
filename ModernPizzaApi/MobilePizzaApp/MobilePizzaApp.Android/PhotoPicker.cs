using Android.Content;
using System.IO;
using System.Threading.Tasks;
using MobilePizzaApp.Interface;
using MobilePizzaApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPicker))]
namespace MobilePizzaApp.Droid
{
    public class PhotoPicker: IPhotoPicker
    {
        public Task<Stream> GetImageStreamAsync()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageRequestId);

            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
            
        }
    }
}
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
            // Define the Intent for getting images
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            // Start the picture-picker activity (resumes in MainActivity.cs)
            MainActivity.Instance.StartActivityForResult(
                Intent.CreateChooser(intent, "Select Picture"),
                MainActivity.PickImageRequestId);

            // Save the TaskCompletionSource object as a MainActivity property
            MainActivity.Instance.PickImageTaskCompletionSource = new TaskCompletionSource<Stream>();

            // Return Task object
            return MainActivity.Instance.PickImageTaskCompletionSource.Task;
            
        }
    }
}
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

// this will be the container for all of the images
namespace Snake
{
    public static class Images
    {
        // static variables for each of our image assets
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Body = LoadImage("Body.png"); 
        public readonly static ImageSource Head = LoadImage("Head.png");
        public readonly static ImageSource Food = LoadImage("Food.png");
        public readonly static ImageSource DeadBody = LoadImage("DeadBody.png");
        public readonly static ImageSource DeadHead = LoadImage("DeadHead.png");

        // loads the image with the file name and returns it as an image source
        private static ImageSource LoadImage(string fileName) { 
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
        }
    }
}

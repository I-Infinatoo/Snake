using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

// this will be the container for all the images
namespace Snake
{
    public static class Images
    {
        // static variables for each of our image assets
        public static readonly ImageSource Empty = LoadImage("Empty.png");
        public static readonly ImageSource Body = LoadImage("Body.png"); 
        public static readonly ImageSource Head = LoadImage("Head.png");
        public static readonly ImageSource Food = LoadImage("Food.png");
        public static readonly ImageSource DeadBody = LoadImage("DeadBody.png");
        public static readonly ImageSource DeadHead = LoadImage("DeadHead.png");

        // loads the image with the file name and returns it as an image source
        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri($"Assets/{fileName}", UriKind.Relative));
        }   
    }
}

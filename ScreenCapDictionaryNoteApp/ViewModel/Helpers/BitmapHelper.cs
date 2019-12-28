using ImageProcessor;
using ImageProcessor.Imaging;
using ScreenCapDictionaryNoteApp.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace ScreenCapDictionaryNoteApp.ViewModel.Helpers
{
    public class BitmapHelper
    {
        public static byte[] TakeScreenShot(int displayIndex)
        {
            var AllScreens = Screen.AllScreens;
            var screen = AllScreens[Math.Min(displayIndex, AllScreens.Length - 1)];

            Bitmap bitmap_Screen = new Bitmap(screen.Bounds.Width,
              screen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bitmap_Screen);

            g.CopyFromScreen(screen.Bounds.X, screen.Bounds.Y, 0, 0, bitmap_Screen.Size);

            //ImageConverter converter = new ImageConverter();
            //return (byte[])converter.ConvertTo(bitmap_Screen, typeof(byte[]));

            byte[] resultingBytes;

            using (MemoryStream outStream = new MemoryStream())
            {
                bitmap_Screen.Save(outStream, System.Drawing.Imaging.ImageFormat.Png);
                resultingBytes = outStream.ToArray();
            }

            return resultingBytes;


        }



        private static Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        {
            // BitmapImage bitmapImage = new BitmapImage(new Uri("../Images/test.png", UriKind.Relative));

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public static byte[] BitmapImageToBytes(BitmapImage bitmapImage)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(BitmapImage2Bitmap(bitmapImage), typeof(byte[]));
        }




        public static BitmapImage ByteArrayToBitmapImage(byte[] imagebyteArray)
        {
            var bitmap = new BitmapImage();
            if (imagebyteArray != null)
            {
                using (MemoryStream stream = new MemoryStream(imagebyteArray))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                }
            }

            return bitmap;
        }







        public static BitmapImage CropImage(byte[] photoBytes, CropLayer cropLayer)
        {
            BitmapImage bitmap = new BitmapImage();

            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        imageFactory.Load(inStream);
                        imageFactory.Crop(cropLayer);
                        imageFactory.Save(outStream);
                    }
                    // Do something with the stream.
                    bitmap.BeginInit();
                    bitmap.StreamSource = outStream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();
                }
            }

            return bitmap;
        }

        public static BitmapImage BitmapImageFromPath(string filePath)
        {
            var image = new BitmapImage();

            using (var stream = File.OpenRead(filePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
                image = bitmap;
            }

            return image;
        }


        public static string SaveBitmapReturnPath(BitmapImage image, Model.Page Page)
        {
            int pageId;

            if (Page == null)
            {


                using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
                {
                    try
                    {
                        pageId = int.Parse(connection.Query<SqliteSequence>("SELECT * FROM sqlite_sequence")
                            .ToList().Where(s => s.name == "Page")
                            .FirstOrDefault()
                            .seq
                            );
                    }
                    catch (Exception err)
                    {
                        pageId = 1;
                    }

                }
            }
            else
            {
                pageId = Page.Id;
            }


            string filePath = Path.Combine(Environment.CurrentDirectory, "Screenshot", "image-" + pageId + ".png");


            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }

            return filePath;

        }



        public static string SaveBitmapReturnPath(byte[] imageByte, Model.Page Page)
        {
            int pageId;
            var bitmapImg = ByteArrayToBitmapImage(imageByte);

            if (Page == null)
            {
                using (SQLiteConnection connection = new SQLiteConnection(DatabaseHelper.dbFile))
                {
                    try
                    {
                        pageId = int.Parse(connection.Query<SqliteSequence>("SELECT * FROM sqlite_sequence")
                            .ToList().Where(s => s.name == "Page")
                            .FirstOrDefault()
                            .seq
                            );
                    }
                    catch (Exception err)
                    {
                        pageId = 1;
                    }

                }
            }
            else
            {
                pageId = Page.Id;
            }


            string DirectoryPath = Path.Combine(Environment.CurrentDirectory, "Screenshot");
            Directory.CreateDirectory(DirectoryPath);
            string filePath = Path.Combine(Environment.CurrentDirectory, "Screenshot", "image-" + pageId + ".png");

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImg));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }

            return filePath;
        }



        public static string SaveCroppedBitmapReturnPath(BitmapImage image, Model.Page Page)
        {
            string DirectoryPath = Path.Combine(Environment.CurrentDirectory, "Screenshot");
            Directory.CreateDirectory(DirectoryPath);
            string filePath = Path.Combine(Environment.CurrentDirectory, "Screenshot", "__cropped__image-" + Page.Id + ".png");

            BitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var fileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
            {
                encoder.Save(fileStream);
            }

            return filePath;
        }



    }
}

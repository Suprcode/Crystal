using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ShadowMerger
{
    class Function
    {
        // int dimension = 500   -   Image manipulation area, increase for larger images but must be static for while library
        public static Bitmap combineIMG(string image, string shadow, int[] imageOffsets, int[] shadowOffsets, int dimension = 500)
        {
            Bitmap finalImage = new Bitmap(dimension, dimension);

            int center = finalImage.Width / 2;
            using (Graphics g = Graphics.FromImage(finalImage))
            {
                //set background color
                g.Clear(Color.Black);

                Bitmap shadowImage = new Bitmap(shadow);

                g.DrawImage(shadowImage, new Rectangle(shadowOffsets[0] + center, shadowOffsets[1] + center, shadowImage.Width, shadowImage.Height));
            }

            finalImage = MirStyleTransparentBitLock(finalImage);
            finalImage = ChangeColorBitLock(finalImage);

            using (Graphics gr = Graphics.FromImage(finalImage))
            {
                Bitmap mobImage = new Bitmap(image);
                mobImage.MakeTransparent(Color.Black);

                gr.DrawImage(mobImage, new Rectangle(imageOffsets[0] + center, imageOffsets[1] + center, mobImage.Width, mobImage.Height));
            }

            return finalImage;
        }

        // Remove region fat
        public static Bitmap Crop(Bitmap bmp)
        {
            int w = bmp.Width, h = bmp.Height;

            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < w; ++i)
                    if (bmp.GetPixel(i, row).R != 0)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                    if (bmp.GetPixel(col, i).R != 0)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (allWhiteColumn(col))
                    leftmost = col;
                else
                    break;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (allWhiteColumn(col))
                    rightmost = col;
                else
                    break;
            }

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;
            try
            {
                Bitmap target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, croppedWidth, croppedHeight),
                      new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3}", topmost, bottommost, leftmost, rightmost),
                  ex);
            }
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        // Mir 2 standard colour 
        public static Bitmap ChangeColor(Bitmap scrBitmap)
        {
            Color newColor = Color.FromArgb(16, 8, 8);

            for (int i = 0; i < scrBitmap.Width; i++)
            {
                for (int j = 0; j < scrBitmap.Height; j++)
                {
                    if (scrBitmap.GetPixel(i, j) != Color.FromArgb(0, 0, 0)) //8 12 8
                    {
                        scrBitmap.SetPixel(i, j, newColor);
                    }
                }
            }
            return scrBitmap;
        }

        // Convert Mir 3 shadow to Mir 2 
        public static Bitmap MirStyleTransparent(Bitmap bmp)
        {
            Bitmap myBitmap = new Bitmap(bmp);
            for (int i = 0; i < myBitmap.Width - 1; i++)
            {
                for (int j = 0; j < myBitmap.Height - 1; j++)
                {
                    if (IsOdd(i) & IsOdd(j))
                    {
                        myBitmap.SetPixel(i, j, Color.Black);
                    }
                    else if (!IsOdd(i) & !IsOdd(j))
                    {
                        myBitmap.SetPixel(i, j, Color.Black);
                    }
                }
            }

            return myBitmap;
        }

        // Extra pish
        public static Bitmap ChangeColorBitLock(Bitmap scrBitmap)
        {
            Color newColor = Color.FromArgb(16, 8, 8);

            LockBitmap lockBitmap = new LockBitmap(scrBitmap);
            lockBitmap.LockBits();

            for (int i = 0; i < lockBitmap.Width; i++)
            {
                for (int j = 0; j < lockBitmap.Height; j++)
                {
                    if (lockBitmap.GetPixel(i, j) != Color.FromArgb(0, 0, 0)) //8 12 8
                    {
                        lockBitmap.SetPixel(i, j, newColor);
                    }
                }
            }
            lockBitmap.UnlockBits();
            return scrBitmap;
        }

        public static Bitmap MirStyleTransparentBitLock(Bitmap bmp)
        {
            LockBitmap lockBitmap = new LockBitmap(bmp);
            lockBitmap.LockBits();

            for (int i = 0; i < lockBitmap.Width - 1; i++)
            {
                for (int j = 0; j < lockBitmap.Height - 1; j++)
                {
                    if (IsOdd(i) & IsOdd(j))
                    {
                        lockBitmap.SetPixel(i, j, Color.Black);
                    }
                    else if (!IsOdd(i) & !IsOdd(j))
                    {
                        lockBitmap.SetPixel(i, j, Color.Black);
                    }
                }
            }
            lockBitmap.UnlockBits();
            return bmp;
        }
        // End Extra pish

        public static Bitmap Mir3StyleShadow(Color colour, Image Path)
        {
            Bitmap mi;
            mi = new Bitmap(Path);
            int tempWidth = mi.Width;//trueSize.Width
            int tempHeight = mi.Height;//trueSize.Height

            Bitmap Temp = new Bitmap(tempWidth + tempHeight / 2, tempHeight);

            // Render Mir 3 Shadow / Far
            int DestY, DestX;
            for (int Y = 0; Y < tempHeight; Y++)
            {
                if (Y % 2 == 1) continue;
                DestY = Y / 2 + Temp.Height / 2 - 3;
                for (int X = 0; X < tempWidth; X++)
                {
                    if (mi.GetPixel(X, Y).Name == "0") continue;
                    DestX = X + (Temp.Height - Y) / 2;
                    Temp.SetPixel(DestX, DestY, colour);
                }
            }

            // Cut out shadow behind mob
            for (int X = 0; X < mi.Width; X++)
            {
                for (int Y = 0; Y < mi.Height; Y++)
                {
                    if (mi.GetPixel(X, Y).Name != "0")
                        Temp.SetPixel(X, Y, Color.Transparent);
                }
            }
            return Temp;
        }

        // Set image opacity, utter
        /// <summary>  
        /// method for changing the opacity of an image  
        /// </summary>  
        /// <param name="image">image to set opacity on</param>  
        /// <param name="opacity">percentage of opacity</param>  
        /// <returns></returns>  
        public static Image SetImageOpacity(Image image, float opacity)
        {
            try
            {
                //create a Bitmap the size of the image provided  
                Bitmap bmp = new Bitmap(image.Width, image.Height);

                //create a graphics object from the image  
                using (Graphics gfx = Graphics.FromImage(bmp))
                {

                    //create a color matrix object  
                    ColorMatrix matrix = new ColorMatrix();

                    //set the opacity  
                    matrix.Matrix33 = opacity;

                    //create image attributes  
                    ImageAttributes attributes = new ImageAttributes();

                    //set the color(opacity) of the image  
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    //now draw the image  
                    gfx.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
                return bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public class Benchmark
        {
            private static DateTime startDate = DateTime.MinValue;
            private static DateTime endDate = DateTime.MinValue;
            public static TimeSpan Span { get { return endDate.Subtract(startDate); } }
            public static void Start() { startDate = DateTime.Now; }
            public static void End() { endDate = DateTime.Now; }
            public static double GetSeconds()
            {
                if (endDate == DateTime.MinValue) return 0.0;
                else return Span.TotalSeconds;
            }
        }
    }
}

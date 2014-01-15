using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.Windows.Threading;
using System.Diagnostics;

namespace WpfMultPages
{
    /// <summary>
    /// Interaction logic for PageOne.xaml
    /// </summary>
    /// 
    public partial class PageOne : Page
    {
        public event EventHandler goHome;   //event handler for mouse press
        public event EventHandler LabelAnimationIn;
        public event EventHandler LabelAnimationOut;

        public WriteableBitmap backImage;
        public WriteableBitmap frontImage;
        public WriteableBitmap stampImage;
        public WriteableBitmap  finalImage;

        private Point point = new Point(200, 100);
        private int imageWidth, imageHeight;
        private Color color;
        private Color color2;
        public PageOne()
        {
            InitializeComponent();

            backImage = LoadBitmap("Images/sunset_1.jpg");
            frontImage = LoadBitmap("Images/fruitninja.jpg");
            stampImage = BitmapFactory.New(backImage.PixelWidth, backImage.PixelHeight);
            finalImage = BitmapFactory.New(backImage.PixelWidth, backImage.PixelHeight);
            color = Color.FromArgb(255, 255, 255, 255);
            Console.WriteLine("ScAlpha " + color.ScA);
            Console.WriteLine("Alpha " + color.A);
            color2 = Color.FromArgb(0, 255, 255, 255);
            stampImage.Clear(color2);
            finalImage.Clear(Color.FromArgb(0, 0, 0, 0));

            ImageViewport.Stretch = Stretch.Fill;
            ImageViewport.Source = finalImage;
            imageWidth = finalImage.PixelWidth;
            imageHeight = finalImage.PixelHeight;
        }

        private void btn_ToMainMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Going home from One");
            goHome(this, null);
        }

        private void btn_ToMainMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            LabelAnimationIn(sender, null);
        }

        private void btn_ToMainMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            LabelAnimationOut(sender, null);
        }

        WriteableBitmap LoadBitmap(string path)
        {
            var img = new BitmapImage();
            img.BeginInit();
            img.CreateOptions = BitmapCreateOptions.None;
            var s = Application.GetResourceStream(new Uri(path, UriKind.Relative)).Stream;
            img.StreamSource = s;
            img.EndInit();
            return BitmapFactory.ConvertToPbgra32Format(img);
        }

        void UpdateRenderImage()
        {
            finalImage.Clear(Color.FromArgb(0, 0, 0, 0));   //clear final image color
            using (finalImage.GetBitmapContext())
            {
                //add back to final
                finalImage.Blit(new Point(0, 0), backImage, new Rect(0, 0, backImage.Width, backImage.Height), Colors.White, WriteableBitmapExtensions.BlendMode.Additive);

                //alpha blend final with front 
                finalImage.Blit(pnt, frontImage, new Rect(0, 0, frontImage.Width, frontImage.Height), color, WriteableBitmapExtensions.BlendMode.Alpha);

                WriteableBitmapExtensions.FillEllipseCentered(finalImage, 200, 200, 14, 20, Colors.Blue);
                WriteableBitmapExtensions.FillEllipse(finalImage, 25, 25, 150, 210, Colors.Red);
            }
        }

        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            using (backImage.GetBitmapContext())
            {
                UpdateRenderImage();
            }
        }

        void MainPage_MouseMove(object sender, MouseEventArgs e)
        {
            point.X = e.GetPosition(ImageViewport).X;
            point.Y = e.GetPosition(ImageViewport).Y;
        }

        private void Page_Loaded_01(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
            this.MouseMove += new MouseEventHandler(MainPage_MouseMove);
        }

        Point pnt = new Point(0, 0);
        private void Image_Mouse_Left_Down(object sender, MouseButtonEventArgs e)
        {
            frontImage = frontImage.RotateFree(2.5, true);
            int w = frontImage.PixelWidth;
            int h = frontImage.PixelHeight;

            pnt.X -= 20.5;
            pnt.Y -= 20.5;

            w -= 100;
            h -= 50;
            if (w <= 0) w = 1;
            if (h <= 0) h = 1;
            //frontImage = WriteableBitmapExtensions.Resize(frontImage, w, h, WriteableBitmapExtensions.Interpolation.Bilinear);
            if (color.A + 5 >= 255)
                color.A = (byte)255;
            else
            {
                int cnt = color.A + 5;
                //color = Color.FromArgb((byte)cnt, (byte)cnt, (byte)cnt, (byte)cnt);
                color.A = (byte)cnt;
            }
            Console.WriteLine("Alpha " + color.ScA + " " + color.A);
        }

        private void Image_Mouse_Right_Down(object sender, MouseButtonEventArgs e)
        {
            pnt.X += 20.5;
            pnt.Y += 20.5;
            frontImage = frontImage.RotateFree(-2.5, true);
            if (color.A - 5 <= 0)
                color.A = (byte)0;
            else
            {
                int cnt = color.A - 5;
                //color = Color.FromArgb((byte)cnt, (byte)cnt, (byte)cnt, (byte)cnt);
                color.A = (byte)cnt;
            }
            Console.WriteLine("Alpha " + color.ScA + " " + color.A);
        }
    }
}


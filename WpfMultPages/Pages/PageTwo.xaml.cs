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
using System.Diagnostics;

namespace WpfMultPages
{
    /// <summary>
    /// Interaction logic for PageTwo.xaml
    /// </summary>
    public partial class PageTwo : Page
    {
        public event EventHandler goHome;   //event handler for mouse press
        public event EventHandler LabelAnimationIn;
        public event EventHandler LabelAnimationOut;

        public WriteableBitmap finalImage;
        public WriteableBitmap backImage;
        ParticleEmitter emitter;

        public PageTwo()
        {
            InitializeComponent();
            emitter = new ParticleEmitter(1, LoadBitmap("Images/SpaceShip_alien.png"));

            backImage = LoadBitmap("Images/sunset_1.jpg");
            finalImage = BitmapFactory.New(backImage.PixelWidth, backImage.PixelHeight);
            finalImage.Clear(Colors.Black);
            ImageViewport.Source = finalImage;
            emitter.TextureDestination = finalImage;
            emitter.emitterPosition = new Point(finalImage.Width * 0.5, finalImage.Height * 0.4);
        }

        private void goBackHome()
        {
            Console.WriteLine("Going home from Two");
            goHome(this, null);
        }

        private void btn_ToMainMenu_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            goBackHome();
        }

        private void lbl_ToMainMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            LabelAnimationIn(sender, null);
        }

        private void lbl_ToMainMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            LabelAnimationOut(sender, null);
        }

        private void btn_ToMainMenu_Click(object sender, RoutedEventArgs e)
        {
            goBackHome();
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

        private Stopwatch _stopwatch = Stopwatch.StartNew();
        private double _lastTime;
        private double _lowestFrameTime;
        DateTime lastUpdate = DateTime.Now;
        
        void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            double elapsed = (DateTime.Now - lastUpdate).TotalSeconds;
            lastUpdate = DateTime.Now;

            UpdateRenderImage(elapsed);

            double timeNow = _stopwatch.ElapsedMilliseconds;
            double elapsedMilliseconds = timeNow - _lastTime;
            _lowestFrameTime = Math.Min(_lowestFrameTime, elapsedMilliseconds);
            double fps = 1000.0 / elapsedMilliseconds;
            FpsCounter.Text = string.Format("FPS: {0:0.0} / Max: {1:0.0} / Particles: {2}", fps, 1000.0 / _lowestFrameTime, emitter.GetTotalParticles());
            _lastTime = timeNow;

            Console.WriteLine(FpsCounter.Text);
        }

        double timeElapsed;
        void UpdateRenderImage(double elapsed)
        {
            timeElapsed += elapsed;
            if (true)//above30fps && emitter.GetTotalParticles() < 1)
            {
                int partCnt = (int)(timeElapsed / emitter.particleRate);

                //if (partCnt > 0)
                //{
                //    for (int i = 0; i < partCnt; i++)
                //    {
                //        emitter.CreatePartilce();
                //    }

                //    timeElapsed = 0.0;
                //}
            }

            //finalImage.Clear(Colors.Black);
            
            using (finalImage.GetBitmapContext())
            {
                finalImage.Blit(new Point(0.0, 0.0), backImage, new Rect(0.0, 0.0, backImage.Width, backImage.Height), Colors.White, WriteableBitmapExtensions.BlendMode.None);
            }

            emitter.DrawParticles(elapsed);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            CompositionTarget.Rendering += new EventHandler(CompositionTarget_Rendering);
        }

        private void Right_Button_Down(object sender, MouseButtonEventArgs e)
        {
            emitter.CreatePartilce();
        }
    }
}

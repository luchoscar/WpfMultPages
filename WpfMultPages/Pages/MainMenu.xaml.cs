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
using System.Windows.Media.Animation;

namespace WpfMultPages
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private enum PageEnum { One, Two };

        public event EventHandler GoToPageOne;
        public event EventHandler GoToPageTwo;
        public event EventHandler LabelAnimationIn;
        public event EventHandler LabelAnimationOut;

        public TranslateTransform trans_Label;

        private double lblWidth = 80;

        public MainMenu()
        {
            InitializeComponent();

            btn_ToPageOne.Tag = PageEnum.One;
            btn_ToPageOne.MouseLeftButtonUp += anyButton_MouseLeftButtonUp;

            btn_ToPageTwo.Tag = PageEnum.Two;
            btn_ToPageTwo.MouseLeftButtonUp += anyButton_MouseLeftButtonUp;

            Console.WriteLine("Main menu has been initialized");
        }

        //pass this page to event handler
        private void anyButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Console.WriteLine("Clicking button for page " + (sender as Label).Tag);

            switch ((PageEnum)(sender as Label).Tag)
            {
                case PageEnum.One:
                    GoToPageOne(this, null);
                    break;
                case PageEnum.Two:
                    GoToPageTwo(this, null);
                    break;
            }
        }

        private void btn_ToPageOne_MouseEnter(object sender, MouseEventArgs e)
        {
            LabelAnimationIn(sender, null);
        }

        private void btn_ToPageOne_MouseLeave(object sender, MouseEventArgs e)
        {
            LabelAnimationOut(sender, null);
        }
    }
}

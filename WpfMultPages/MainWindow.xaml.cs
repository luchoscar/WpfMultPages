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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainMenu mainMenu;
        private PageOne pageOne;
        private PageTwo pageTwo;

        public MainWindow()
        {
            InitializeComponent();

            setup();
        }

        public void setup()
        {
            mainMenu = new MainMenu();
            pageOne = new PageOne();
            pageTwo = new PageTwo();

            // Load main menu
            //PageFrameMain.Navigate(mainMenu);
            PageFrameMain.Navigate(pageTwo);

            // Set up Navigation event handlers 
            //buttons going away from main menu 
            mainMenu.GoToPageOne += ((object o, EventArgs e) => 
                                                            {
                                                                Console.WriteLine("Going to Page One");
                                                                navPages(pageOne, true); 
                                                            });

            mainMenu.GoToPageTwo += ((object o, EventArgs e) => 
                                                            {
                                                                Console.WriteLine("Going to Page Two");
                                                                navPages(pageTwo, true); 
                                                            });
            mainMenu.LabelAnimationIn += ((object o, EventArgs e) =>
                                                            {
                                                                WidthAnimLabel((o as Label), true);
                                                            });
            mainMenu.LabelAnimationOut += ((object o, EventArgs e) =>
                                                            {
                                                                WidthAnimLabel((o as Label), false);
                                                            });
            //buttons going back to main menu
            pageOne.goHome += ((object o, EventArgs e) => 
                                                        {
                                                            Console.WriteLine("Leaving Page One");
                                                            navPages(pageOne, false); 
                                                        });
            pageOne.LabelAnimationIn += ((object o, EventArgs e) =>
                                                        {
                                                            WidthAnimLabel((o as Label), true);
                                                        });
            pageOne.LabelAnimationOut += ((object o, EventArgs e) =>
                                                        {
                                                            WidthAnimLabel((o as Label), false);
                                                        });

            pageTwo.goHome += ((object o, EventArgs e) => 
                                                        {
                                                            Console.WriteLine("Leaving Page Two"); 
                                                            navPages(pageTwo, false);
                                                        });
            pageTwo.LabelAnimationIn += ((object o, EventArgs e) =>
                                                        {
                                                            WidthAnimLabel((o as Label), true);
                                                        });
            pageTwo.LabelAnimationOut += ((object o, EventArgs e) =>
                                                        {
                                                            WidthAnimLabel((o as Label), false);
                                                        });
        }

        //handle switch between pages
        private void navPages(Page page, bool awayFromMenu)
        {
            Console.WriteLine("Nav function handler call");

            double from = awayFromMenu ? this.ActualWidth : 0;
            double to = awayFromMenu ? 0 : this.ActualWidth;

            TranslateTransform tt = new TranslateTransform();
            DoubleAnimation anim = new DoubleAnimation(from, to, TimeSpan.FromMilliseconds(333), FillBehavior.HoldEnd);

            // Navigate to the new page in the correct PageFrame
            // Also make sure to unload (ie Navigate(null)) whatever page was previously in use
            if (awayFromMenu)
            {
                anim.Completed += ((object o, EventArgs e) => { PageFrameMain.Navigate(null); });
                PageFrameSecondary.Navigate(page);
            }
            else
            {
                anim.Completed += ((object o, EventArgs e) => { PageFrameSecondary.Navigate(null); });
                PageFrameMain.Navigate(mainMenu);
            }

            // Begin animation
            page.RenderTransform = tt;
            tt.BeginAnimation(TranslateTransform.XProperty, anim);
        }

        //moving labels left/right based on mouse enter/leave
        private void WidthAnimLabel(Label _Lbl, bool _InOut)
        {
            double from, to;

            if (_InOut)
            {
                from = _Lbl.MinWidth;
                to = _Lbl.MaxWidth;
                _Lbl.Foreground = Brushes.Blue;
                _Lbl.FontStyle = FontStyles.Italic;
            }
            else
            {
                from = _Lbl.MaxWidth;
                to = _Lbl.MinWidth;
                _Lbl.Foreground = Brushes.White;
                _Lbl.FontStyle = FontStyles.Normal;
            }

            //DoubleAnimation dblAnim_moveLabel = new DoubleAnimation(from, to, TimeSpan.FromMilliseconds(333), FillBehavior.HoldEnd);
            //_Lbl.BeginAnimation(Label.WidthProperty, dblAnim_moveLabel);
        }
    }
}

using AzureQuickDrop.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AzureQuickDrop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ServicesFactory.Instance.SatisfyImportsOnce(this);
            Storage.FileUploadedSuccess += Storage_FileUploadedSuccess;
            Storage.QueueUpdated += Storage_QueueUpdated; ;
        }

        private const double ACTIVE_SIZE = 200;
        private const double INACTIVE_SIZE = 50;


        public int QueueLength
        {
            get { return (int)GetValue(QueueLengthProperty); }
            set { SetValue(QueueLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for QueueLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty QueueLengthProperty =
            DependencyProperty.Register("QueueLength", typeof(int), typeof(MainWindow), new PropertyMetadata(0));



        private void Storage_QueueUpdated(object sender, int e)
        {
            Dispatcher.Invoke(() => {
                this.QueueLength = e;
            });

        }


        private void Storage_FileUploadedSuccess(object sender, string e)
        {

        }

        [Import]
        public IStorageProvider Storage { get; private set; }

        /// <summary>
        /// Make the borderless window to be draggable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


        private void root_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                Storage.EnqueUploads(files);
            }
        }

        private void root_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }

        private void root_MouseEnter(object sender, MouseEventArgs e)
        {
            var da = new DoubleAnimation(200, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.HoldEnd);
            var daOpacity = new DoubleAnimation(1.0, new Duration(TimeSpan.FromMilliseconds(300)), FillBehavior.HoldEnd);

            //Storyboard sb = new Storyboard();
            //sb.Children.Add(da);
            //this.BeginAnimation()
            grid.BeginAnimation(WidthProperty, da);
            grid.BeginAnimation(HeightProperty, da);
        }

        private void root_MouseLeave(object sender, MouseEventArgs e)
        {
            Mini();
        }


        private void root_LostFocus(object sender, RoutedEventArgs e)
        {
            Mini();
        }
        private void Mini()
        {
            var da = new DoubleAnimation(50, new Duration(TimeSpan.FromMilliseconds(200)), FillBehavior.HoldEnd);
            grid.BeginAnimation(WidthProperty, da);
            grid.BeginAnimation(HeightProperty, da);
        }

    }
}

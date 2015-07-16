using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace TradeHubGui.Views
{
    /// <summary>
    /// Interaction logic for AlertWindow.xaml
    /// </summary>
    public partial class AlertWindow : Window
    {
        public AlertWindow()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(WindowLoaded);
        }

        /// <summary>
        /// Triggered when close button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCloseButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public void CloseMethod(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnMouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard storyboard = (Storyboard)this.Resources["FadeOutStoryboard"];
            storyboard.Pause();
        }

        private void OnMouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Storyboard storyboard = (Storyboard)this.Resources["FadeOutStoryboard"];
            //storyboard.Seek(TimeSpan.Zero, TimeSeekOrigin.BeginTime);
            storyboard.Resume();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }
    }
}

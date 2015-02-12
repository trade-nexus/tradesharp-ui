using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TradeHubGui.Common;

namespace TradeHubGui.ViewModel
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private RelayCommand _showLogsCommand;

        public MetroWindow MainWindow
        {
            get { return (MetroWindow)Application.Current.MainWindow; }
        }

        /// <summary>
        /// Show strategy Runner Log
        /// </summary>
        public ICommand ShowLogsCommand
        {
            get
            {
                return _showLogsCommand ??
                       (_showLogsCommand = new RelayCommand(param => ShowLogsExecute()));
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private void ShowLogsExecute()
        {
            ToggleFlyout(2);
        }

        /// <summary>
        /// Shows or Hide flayout window
        /// </summary>
        /// <param name="index">flayout index</param>
        public void ToggleFlyout(int index)
        {
            var flyout = MainWindow.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }
    }
}

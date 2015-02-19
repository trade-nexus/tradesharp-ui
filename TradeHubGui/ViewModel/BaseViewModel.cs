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

        /// <summary>
        /// Show logs fayout window
        /// </summary>
        private void ShowLogsExecute()
        {
            ToggleFlyout(3);
        }

        /// <summary>
        /// Shows or Hide flayout window
        /// </summary>
        /// <param name="index">flayout index</param>
        /// <returns>returns true if flayout is found, otherwise returns false</returns>
        public bool ToggleFlyout(int index)
        {
            var flyout = MainWindow.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return false;
            }

            flyout.IsOpen = !flyout.IsOpen;
            return true;
        }

        #region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
        #endregion
    }
}

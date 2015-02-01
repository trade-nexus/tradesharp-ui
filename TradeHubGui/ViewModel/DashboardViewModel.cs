using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TradeHubGui.Common;
using TradeHubGui.Common.Models;

namespace TradeHubGui.ViewModel
{
    public class DashboardViewModel : BaseViewModel
    {
        private ObservableCollection<Instrument> _instruments;
        private RelayCommand _showDataApiConfigurationCommand;
        private RelayCommand _showOrderApiConfigurationCommand;

        /// <summary>
        /// Constructor
        /// </summary>
        public DashboardViewModel()
        {
            #region Temporary fill instruments (this will be removed)
            _instruments = new ObservableCollection<Instrument>();
            for (int i = 0; i < 10; i++)
            {
                _instruments.Add(new Instrument("AAPL", 23, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("GOOG", 43, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("MSFT", 33, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("HP", 42, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("AOI", 34, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("WAS", 15, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("AAPL", 23, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("GOOG", 23, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("MSFT", 45, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("HP", 33, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("AOI", 24, 450.34f, 20, 456.00f, 445.34f, 23));
                _instruments.Add(new Instrument("WAS", 23, 450.34f, 20, 456.00f, 445.34f, 23));
            }
            #endregion
        }

        #region Properties

        public ObservableCollection<Instrument> Instruments
        {
            get { return _instruments; }
            set
            {
                _instruments = value;
                OnPropertyChanged("Instruments");
            }
        }

        #endregion

        #region Commands

        public ICommand ShowDataApiConfigurationCommand
        {
            get
            {
                return _showDataApiConfigurationCommand ?? (_showDataApiConfigurationCommand = new RelayCommand(param => ShowDataApiConfigurationExecute()));
            }
        }

        public ICommand ShowOrderApiConfigurationCommand
        {
            get
            {
                return _showOrderApiConfigurationCommand ?? (_showOrderApiConfigurationCommand = new RelayCommand(param => ShowOrderApiConfigurationExecute()));
            }
        }

        #endregion

        private void ShowDataApiConfigurationExecute()
        {
            ToggleFlyout(1);
        }

        private void ShowOrderApiConfigurationExecute()
        {
            ToggleFlyout(2);
        }

        /// <summary>
        /// Shows or Hide flayout window
        /// </summary>
        /// <param name="index">flayout index</param>
        private void ToggleFlyout(int index)
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

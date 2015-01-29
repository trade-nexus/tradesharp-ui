using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using TradeHubGui.Common;
using TradeHubGui.Model;
using TradeHubGui.Views;

namespace TradeHubGui.ViewModel
{
	public class StrategyRunnerViewModel : BaseViewModel
	{
		private ObservableCollection<Strategy> strategies;
		private ObservableCollection<StrategyInstance> instances;
		private Strategy selectedStrategy;
		private StrategyInstance selectedInstance;
		private RelayCommand showCreateInstanceWindowCommand;
		private RelayCommand showEditInstanceWindowCommand;
		private RelayCommand showGeneticOptimizationWindowCommand;
		private RelayCommand showBruteOptimizationWindowCommand;
		private RelayCommand loadStrategyCommand;
		private RelayCommand selectProviderCommand;
		private RelayCommand importInstancesCommand;
		private string strategyPath;
		private string csvInstancesPath;

		public StrategyRunnerViewModel()
		{
			strategies = new ObservableCollection<Strategy>();
			strategies.Add(new Strategy() { Key = "AA" });
			strategies.Add(new Strategy() { Key = "AB" });
			strategies.Add(new Strategy() { Key = "AC" });
		}

		public ObservableCollection<Strategy> Strategies
		{
			get { return strategies; }
			set
			{
				strategies = value;
				OnPropertyChanged("Strategies");
			}
		}

		public ObservableCollection<StrategyInstance> Instances
		{
			get { return instances; }
			set
			{
				instances = value;
				OnPropertyChanged("Instances");
			}
		}

		public Strategy SelectedStrategy
		{
			get { return selectedStrategy; }
			set
			{
				selectedStrategy = value;
				PopulateStrategyInstanceDataGrid(value.Key);
				OnPropertyChanged("SelectedStrategy");
			}
		}

		public StrategyInstance SelectedInstance
		{
			get { return selectedInstance; }
			set
			{
				selectedInstance = value;
				OnPropertyChanged("SelectedInstance");
			}
		}

		public string StrategyPath
		{
			get { return strategyPath; }
			set
			{
				strategyPath = value;
				OnPropertyChanged("StrategyPath");
			}
		}

		public string CsvInstancesPath
		{
			get { return csvInstancesPath; }
			set
			{
				csvInstancesPath = value;
				OnPropertyChanged("CsvInstancesPath");
			}
		}

		public ICommand ShowCreateInstanceWindowCommand
		{
			get
			{
				return showCreateInstanceWindowCommand ?? (showCreateInstanceWindowCommand = new RelayCommand(
					param => ShowCreateInstanceWindowExecute(), param => ShowCreateInstanceWindowCanExecute()));
			}
		}

		public ICommand ShowEditInstanceWindowCommand
		{
			get
			{
				return showEditInstanceWindowCommand ?? (showEditInstanceWindowCommand = new RelayCommand(
					param => ShowEditInstanceWindowExecute(), param => ShowEditInstanceWindowCanExecute()));
			}
		}

		public ICommand ShowGeneticOptimizationWindowCommand
		{
			get
			{
				return showGeneticOptimizationWindowCommand ?? (showGeneticOptimizationWindowCommand = new RelayCommand(
					param => ShowGeneticOptimizationExecute()));
			}
		}

		public ICommand ShowBruteOptimizationWindowCommand
		{
			get
			{
				return showBruteOptimizationWindowCommand ?? (showBruteOptimizationWindowCommand = new RelayCommand(
					param => ShowBruteOptimizationExecute()));
			}
		}

		public ICommand LoadStrategyCommand
		{
			get
			{
				return loadStrategyCommand ?? (loadStrategyCommand = new RelayCommand(param => LoadStrategyExecute()));
			}
		}

		public ICommand SelectProviderCommand
		{
			get
			{
				return selectProviderCommand ?? (selectProviderCommand = new RelayCommand(param => SelectProviderExecute()));
			}
		}

		public ICommand ImportInstancesCommand
		{
			get
			{
				return importInstancesCommand ?? (importInstancesCommand = new RelayCommand(param => ImportInstancesExecute()));
			}
		}

		private bool ShowEditInstanceWindowCanExecute()
		{
			if (SelectedInstance == null) return false;
			return true;
		}

		private void ShowEditInstanceWindowExecute()
		{
			EditInstanceWindow window = new EditInstanceWindow();
			window.Tag = string.Format("INSTANCE {0}", SelectedInstance.InstanceKey);
			window.Owner = MainWindow;
			window.ShowDialog();
		}

		private bool ShowCreateInstanceWindowCanExecute()
		{
			if (SelectedStrategy == null) return false;
			return true;
		}
		private void ShowGeneticOptimizationExecute()
		{
			if (TryActivateShownWindow(typeof(GeneticOptimizationWindow)))
			{
				return;
			}

			GeneticOptimizationWindow window = new GeneticOptimizationWindow();
			window.Tag = string.Format("{0}", SelectedStrategy.Key);
			window.Show();
		}

		private void ShowBruteOptimizationExecute()
		{
			if (TryActivateShownWindow(typeof(BruteOptimizationWindow)))
			{
				return;
			}

			BruteOptimizationWindow window = new BruteOptimizationWindow();
			window.Tag = string.Format("{0}", SelectedStrategy.Key);
			window.Show();
		}

		private void ShowCreateInstanceWindowExecute()
		{
			CreateInstanceWindow window = new CreateInstanceWindow();
			window.Tag = string.Format("STRATEGY {0}", SelectedStrategy.Key);
			window.Owner = MainWindow;
			window.ShowDialog();
		}

		private void SelectProviderExecute()
		{
			SelectProviderWindow window = new SelectProviderWindow();
			window.Owner = MainWindow;
			window.ShowDialog();
		}

		private void LoadStrategyExecute()
		{
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
			openFileDialog.Title = "Load Strategy File";
			openFileDialog.CheckFileExists = true;
			openFileDialog.Filter = "Assembly Files (.dll)|*.dll|All Files (*.*)|*.*";
			System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				StrategyPath = openFileDialog.FileName;
			}
		}

		private void ImportInstancesExecute()
		{
			System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
			openFileDialog.Title = "Import Instances";
			openFileDialog.CheckFileExists = true;
			openFileDialog.Filter = "CSV Files (.csv)|*.csv|All Files (*.*)|*.*";
			System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				CsvInstancesPath = openFileDialog.FileName;
			}
		}

		private bool TryActivateShownWindow(Type TypeOfWindow)
		{
			if (Application.Current != null)
			{
				foreach (Window window in Application.Current.Windows)
				{
					if (window.GetType() == TypeOfWindow)
					{
						window.Focus();
						window.Activate();
						return true;
					}
				}
			}

			return false;
		}


		private void PopulateStrategyInstanceDataGrid(string strategyKey)
		{
			switch (strategyKey)
			{
				case "AA": FillInstancesAA();
					break;
				case "AB": FillInstancesAB();
					break;
				case "AC": FillInstancesAC();
					break;
			}
		}

		private void FillInstancesAA()
		{
			Instances = new ObservableCollection<StrategyInstance>();
			Instances.Add(new StrategyInstance() { InstanceKey = "AA001", Symbol = "GOOG", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AA002", Symbol = "GOOG", Description = "Test", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AA003", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AA004", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AA005", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AA006", Symbol = "GOOG", Description = "Dynamic trade", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AA007", Symbol = "HP", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AA008", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AA009", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			SelectedInstance = Instances.Count > 0 ? Instances[0] : null;
		}

		private void FillInstancesAB()
		{
			Instances = new ObservableCollection<StrategyInstance>();
			Instances.Add(new StrategyInstance() { InstanceKey = "AB001", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB002", Symbol = "HP", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB003", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB004", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB005", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB006", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB007", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB008", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB009", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB011", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB012", Symbol = "HP", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB013", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB014", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB015", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB016", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB017", Symbol = "HP", Description = "Test", StateBrush = Brushes.Goldenrod });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB018", Symbol = "HP", Description = "Test", StateBrush = Brushes.Crimson });
			Instances.Add(new StrategyInstance() { InstanceKey = "AB019", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			SelectedInstance = Instances.Count > 0 ? Instances[0] : null;
		}

		private void FillInstancesAC()
		{
			Instances = new ObservableCollection<StrategyInstance>();
			Instances.Add(new StrategyInstance() { InstanceKey = "AC001", Symbol = "MSFT", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AC002", Symbol = "HP", Description = "Test", StateBrush = Brushes.MediumSeaGreen });
			Instances.Add(new StrategyInstance() { InstanceKey = "AC003", Symbol = "AAPL", Description = "Test", StateBrush = Brushes.Crimson });

			for (int i = 4; i < 121; i++)
			{
				Instances.Add(new StrategyInstance() { InstanceKey = "AC00" + i, Symbol = "BMW", Description = "Dynamic trade", StateBrush = Brushes.MediumSeaGreen });
			}

			SelectedInstance = Instances.Count > 0 ? Instances[0] : null;
		}


	}
}

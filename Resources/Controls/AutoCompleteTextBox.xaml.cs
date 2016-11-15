using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Resources.Controls
{
	/// <summary>
	/// Interaction logic for AutoCompleteTextBox.xaml
	/// </summary>
	public partial class AutoCompleteTextBox : UserControl, IDisposable
	{
		#region Members
		private System.Timers.Timer keypressTimer;
		private delegate void TextChangedCallback();
		private bool insertText;
		private int delayTime;
		private int searchThreshold = 2;
		#endregion

		#region Dependency properties
		public ObservableCollection<AutoCompleteEntry> AutoCompletionList
		{
			get { return (ObservableCollection<AutoCompleteEntry>)GetValue(AutoCompletionListProperty); }
			set { SetValue(AutoCompletionListProperty, value); }
		}

		public static readonly DependencyProperty AutoCompletionListProperty =
			DependencyProperty.Register("AutoCompletionList", typeof(ObservableCollection<AutoCompleteEntry>), typeof(AutoCompleteTextBox), new PropertyMetadata(new ObservableCollection<AutoCompleteEntry>()));

		public string Text
		{
			get { return (string)GetValue(TextProperty); }
			set { SetValue(TextProperty, value); }
		}

		public static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteTextBox), new PropertyMetadata(string.Empty));

		public bool ControlAcceptsReturn
		{
			get { return (bool)GetValue(ControlAcceptsReturnProperty); }
			set { SetValue(ControlAcceptsReturnProperty, value); }
		}

		public static readonly DependencyProperty ControlAcceptsReturnProperty =
			DependencyProperty.Register("ControlAcceptsReturn", typeof(bool), typeof(AutoCompleteTextBox), new PropertyMetadata(true));

		#endregion Dependency properties

		#region Constructor

		public AutoCompleteTextBox()
		{
			this.DataContext = this;
			InitializeComponent();
			// set up the key press timer
			keypressTimer = new System.Timers.Timer();
			keypressTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnTimedEvent);
			textBox.PreviewKeyDown += new KeyEventHandler(textBox_KeyDown);
		}

		#endregion

		void textBox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Down)
			{
				comboBox.SelectedIndex = 0;
			}
		}

		#region Methods

		public int DelayTime
		{
			get { return delayTime; }
			set { delayTime = value; }
		}

		public int Threshold
		{
			get { return searchThreshold; }
			set { searchThreshold = value; }
		}

		private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (null != comboBox.SelectedItem)
			{
				insertText = true;
				ComboBoxItem cbItem = (ComboBoxItem)comboBox.SelectedItem;
				Text = cbItem.Content.ToString();
			}
		}

		private void TextChanged()
		{
			try
			{
				comboBox.Items.Clear();

				if (Text.Length >= searchThreshold)
				{
					foreach (AutoCompleteEntry entry in AutoCompletionList)
					{
						if(entry.KeywordStrings[0] == null)
						{
							continue;
						}

						foreach (string word in entry.KeywordStrings)
						{
							if (word.StartsWith(Text, StringComparison.CurrentCultureIgnoreCase))
							{
								ComboBoxItem cbItem = new ComboBoxItem();
								cbItem.Content = entry.ToString();
								comboBox.Items.Add(cbItem);
								break;
							}
						}
					}
					comboBox.IsDropDownOpen = comboBox.HasItems;
				}
				else
				{
					comboBox.IsDropDownOpen = false;
				}
			}
			catch { }
		}

		private void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e)
		{
			keypressTimer.Stop();
			Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal,
				new TextChangedCallback(this.TextChanged));
		}

		private void textBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			// text was not typed, do nothing and consume the flag
			if (insertText == true)
			{
				insertText = false;
			}
			else // if the delay time is set, delay handling of text changed
			{
				if (delayTime > 0)
				{
					keypressTimer.Interval = delayTime;
					keypressTimer.Start();
				}
				else TextChanged();
			}
		}

		public void SetFocus()
		{
			Keyboard.Focus(textBox);
		}

		#endregion

		public void Dispose()
		{
			keypressTimer.Dispose();
		}
	}
}

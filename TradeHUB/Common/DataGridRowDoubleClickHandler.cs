using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Reflection;

namespace TradeHUB.Common
{
	public class DataGridRowDoubleClickHandler : FrameworkElement
	{
		public DataGridRowDoubleClickHandler(DataGrid dataGrid)
		{
			MouseButtonEventHandler handler = (sender, args) =>
			{
				var row = sender as DataGridRow;
				if (row != null && row.IsSelected)
				{
					var handlerName = GetHandler(dataGrid);

					var dataContextType = dataGrid.DataContext.GetType();
					var method = dataContextType.GetMethod(handlerName);
					if (method == null)
					{
						PropertyInfo commandPI = dataContextType.GetProperty(handlerName);
						if (commandPI != null &&
							(commandPI.PropertyType == typeof(ICommand)))
						{

							ICommand command = (ICommand)commandPI.GetValue(dataGrid.DataContext, null);
							if (command != null && command.CanExecute(null))
							{
								command.Execute(null);
							}
						}
						else
						{
							throw new MissingMethodException(handlerName);
						}
					}
					else
					{
						method.Invoke(dataGrid.DataContext, null);
					}
				}
			};

			dataGrid.LoadingRow += (s, e) =>
				{
					e.Row.MouseDoubleClick += handler;
				};

			dataGrid.UnloadingRow += (s, e) =>
				{
					e.Row.MouseDoubleClick -= handler;
				};
		}

		public static string GetHandler(DataGrid dataGrid)
		{
			return (string)dataGrid.GetValue(HandlerProperty);
		}

		public static void SetHandler(DataGrid dataGrid, string value)
		{
			dataGrid.SetValue(HandlerProperty, value);
		}

		public static readonly DependencyProperty HandlerProperty = DependencyProperty.RegisterAttached(
			"Handler",
			typeof(string),
			typeof(DataGridRowDoubleClickHandler),
			new PropertyMetadata((o, e) =>
			{
				var dataGrid = o as DataGrid;
				if (dataGrid != null)
				{
					new DataGridRowDoubleClickHandler(dataGrid);
				}
			}));
	}
}

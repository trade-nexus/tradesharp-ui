using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;

namespace Resources.Converters
{
	public class TreeViewLastLineToHeightConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			double height = 0;
			TreeViewItem item = (TreeViewItem)value;
			ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);

			if (ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1)
			{
				if (item.Items.Count > 0)
				{
					height = 0;
				}
				else
				{
					height = item.ActualHeight / 2;
				}
			}

			return height;
		}


		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		#endregion
	}
}

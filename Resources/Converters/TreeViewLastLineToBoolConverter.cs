using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Controls;

namespace Resources.Converters
{
	public class TreeViewLastLineToBoolConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			TreeViewItem item = (TreeViewItem)value;
			ItemsControl ic = ItemsControl.ItemsControlFromItemContainer(item);
			return ic.ItemContainerGenerator.IndexFromContainer(item) == ic.Items.Count - 1;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}

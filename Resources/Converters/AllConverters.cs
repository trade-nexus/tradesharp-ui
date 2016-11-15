using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace Resources.Converters
{
	public static class AllConverters
	{
		private static IValueConverter validationErrorsToStringConverter = new ValidationErrorsToStringConverter();
		private static IValueConverter toolBarTypeToVisibilityConverter = new ToolBarTypeToVisibilityConverter();
		private static IValueConverter treeViewLastLineToBoolConverter = new TreeViewLastLineToBoolConverter();
		private static IValueConverter treeViewLastLineToHeightConverter = new TreeViewLastLineToHeightConverter();
		private static IValueConverter checkboxInverterConverter = new CheckboxInverterConverter();
		private static IValueConverter doubleFormatConverter = new DoubleFormatConverter();
		private static IValueConverter floatToGridLenghtConverter = new FloatToGridLenghtConverter();
		private static IValueConverter stringToVisibilityConverter = new StringToVisibilityConverter();

		public static IValueConverter ValidationErrorsToStringConverter
		{
			get
			{
				return validationErrorsToStringConverter;
			}
		}

		public static IValueConverter ToolBarTypeToVisibilityConverter
		{
			get
			{
				return toolBarTypeToVisibilityConverter;
			}
		}

		public static IValueConverter TreeViewLastLineToBoolConverter
		{
			get
			{
				return treeViewLastLineToBoolConverter;
			}
		}

		public static IValueConverter TreeViewLastLineToHeightConverter
		{
			get
			{
				return treeViewLastLineToHeightConverter;
			}
		}

		public static IValueConverter CheckboxInverterConverter
		{
			get
			{
				return checkboxInverterConverter;
			}
		}

		public static IValueConverter DoubleFormatConverter
		{
			get
			{
				return doubleFormatConverter;
			}
		}

		public static IValueConverter FloatToGridLenghtConverter
		{
			get
			{
				return floatToGridLenghtConverter;
			}
		}

		public static IValueConverter StringToVisibilityConverter
		{
			get
			{
				return stringToVisibilityConverter;
			}
		}
	}
}

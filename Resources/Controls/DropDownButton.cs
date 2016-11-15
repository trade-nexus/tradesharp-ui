//  --------------------------------
//  Copyright (c) Huy Pham. All rights reserved.
//  This source code is made available under the terms of the Microsoft Public License (Ms-PL)
//  http://www.opensource.org/licenses/ms-pl.html
//  ---------------------------------

using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Resources.Controls
{
	public class DropDownButton : ToggleButton
	{
		#region Dependency Properties

		public static readonly DependencyProperty DropDownContextMenuProperty = DependencyProperty.Register("DropDownContextMenu", typeof(ContextMenu), typeof(DropDownButton), new UIPropertyMetadata(null));
		public static readonly DependencyProperty ContextMenuPlacementProperty = DependencyProperty.Register("ContextMenuPlacement", typeof(PlacementMode), typeof(DropDownButton), new UIPropertyMetadata(PlacementMode.Bottom));
		public static readonly DependencyProperty ImageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(DropDownButton));
		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(DropDownButton));
		public static readonly DependencyProperty TargetProperty = DependencyProperty.Register("Target", typeof(UIElement), typeof(DropDownButton));
		public static readonly DependencyProperty DropDownButtonCommandProperty = DependencyProperty.Register("DropDownButtonCommand", typeof(ICommand), typeof(DropDownButton), new FrameworkPropertyMetadata(null));
		public static readonly DependencyProperty ArrowRotateAngleProperty = DependencyProperty.Register("ArrowRotateAngle", typeof(double), typeof(DropDownButton));
		public static readonly DependencyProperty HoverBrushProperty = DependencyProperty.Register("HoverBrush", typeof(SolidColorBrush), typeof(DropDownButton));

		#endregion

		#region Constructors

		public DropDownButton()
		{
			// Bind the ToogleButton.IsChecked property to the drop-down's IsOpen property 
			var binding = new Binding("DropDownContextMenu.IsOpen") { Source = this };
			SetBinding(IsCheckedProperty, binding);
		}

		#endregion

		#region Properties

		public ContextMenu DropDownContextMenu
		{
			get { return GetValue(DropDownContextMenuProperty) as ContextMenu; }
			set { SetValue(DropDownContextMenuProperty, value); }
		}

		public PlacementMode ContextMenuPlacement
		{
			get { return (PlacementMode)GetValue(ContextMenuPlacementProperty); }
			set { SetValue(ContextMenuPlacementProperty, value); }
		}

		public ImageSource Image
		{
			get { return GetValue(ImageProperty) as ImageSource; }
			set { SetValue(ImageProperty, value); }
		}

		public string Text
		{
			get { return GetValue(TextProperty) as string; }
			set { SetValue(TextProperty, value); }
		}

		public UIElement Target
		{
			get { return GetValue(TargetProperty) as UIElement; }
			set { SetValue(TargetProperty, value); }
		}

		public ICommand DropDownButtonCommand
		{
			get { return GetValue(DropDownButtonCommandProperty) as ICommand; }
			set { SetValue(DropDownButtonCommandProperty, value); }
		}

		public double ArrowRotateAngle
		{
			get { return (double)GetValue(ArrowRotateAngleProperty); }
			set { SetValue(ArrowRotateAngleProperty, value); }
		}

		public SolidColorBrush HoverBrush
		{
			get { return (SolidColorBrush)GetValue(HoverBrushProperty); }
			set { SetValue(HoverBrushProperty, value); }
		}

		#endregion

		#region Protected Override Methods

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);

			if (e.Property == DropDownButtonCommandProperty)
				Command = DropDownButtonCommand;
		}

		protected override void OnClick()
		{
			if (DropDownContextMenu == null) return;

			if (DropDownButtonCommand != null) DropDownButtonCommand.Execute(null);

			// If there is a drop-down assigned to this button, then position and display it 
			DropDownContextMenu.PlacementTarget = this;
			DropDownContextMenu.Placement = ContextMenuPlacement;
			DropDownContextMenu.IsOpen = !DropDownContextMenu.IsOpen;
		}

		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			Path path = GetTemplateChild("Arrow") as Path;
			path.RenderTransform = new RotateTransform(ArrowRotateAngle, 3, 2);

			if (HoverBrush != null)
			{
				Border hoverBorder = GetTemplateChild("HoverBorder") as Border;
				hoverBorder.Background = HoverBrush;
				hoverBorder.Opacity = 0.5;
			}
		}

		#endregion
	}
}
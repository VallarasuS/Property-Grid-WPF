
#region Namespace Imports

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

#endregion

namespace Vasu.Wpf.Controls
{
    /// <summary>
    /// Defines a <see cref="PropertyItemsControl"/>.
    /// </summary>
    [ToolboxItem(false)]
	public class PropertyItemsControl : ListView
	{
		#region Ctor

		static PropertyItemsControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(PropertyItemsControl), new FrameworkPropertyMetadata(typeof(PropertyItemsControl)));
		}

		#endregion

		#region Dependency Properties

		public PropertyItem SelectedItem
		{
			get { return (PropertyItem)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register("SelectedItem", typeof(PropertyItem), typeof(PropertyItemsControl), new UIPropertyMetadata(null));

		public object SelectedValue
		{
			get { return (object)GetValue(SelectedValueProperty); }
			set { SetValue(SelectedValueProperty, value); }
		}

		public static readonly DependencyProperty SelectedValueProperty =
			DependencyProperty.Register("SelectedValue", typeof(object), typeof(PropertyItemsControl), new UIPropertyMetadata(null));

		#endregion

		#region Overrides

		protected override DependencyObject GetContainerForItemOverride()
		{
			return new PropertyItem();
		}

		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is PropertyItem;
		}

		protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
		{
			base.PrepareContainerForItemOverride(element, item);
		}

		protected override void OnPreviewMouseLeftButtonDown(System.Windows.Input.MouseButtonEventArgs e)
		{
			base.OnPreviewMouseLeftButtonDown(e);

			var item = (PropertyItem)VisualTreeHelperExt.FindParentOfType<PropertyItem>((DependencyObject)Mouse.DirectlyOver);

			if (item == null) return;

			SelectedItem = item;

			SelectedValue = ItemContainerGenerator.ItemFromContainer(SelectedItem);
		}
		
		#endregion
	}
}

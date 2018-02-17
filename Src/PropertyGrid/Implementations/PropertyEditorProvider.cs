
#region Namespace Imports

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace Vasu.Wpf.Controls
{
    /// <summary>
    /// Implements methods to decide visual editor for the <see cref="PropertyField"/>.
    /// </summary>
    internal static class PropertyEditorProvider
	{
		public static PropertyEditor GetFieldEditor(PropertyField field)
		{
			if (field == null) return null;

			FrameworkElement element = null;

			if (!field.PropertyInfo.CanWrite)
			{
				var textEditor = new TextBox();
				textEditor.Text = (field.Value == null) ? string.Empty : field.Value.ToString();
				textEditor.IsReadOnly = true;
				textEditor.Foreground = Brushes.Gray;
				textEditor.BorderThickness = new Thickness(0);

				element = textEditor;
			}
			else if (field.PropertyType.IsEnum)
			{
				var comboEditor = new ComboBox();
				comboEditor.ItemsSource = Enum.GetValues(field.PropertyType);
				comboEditor.SelectedValue = field.Value;
				comboEditor.BorderThickness = new Thickness(0);

				element = comboEditor;
			}
			else if (field.PropertyType == typeof(Boolean))
			{
				var comboEditor = new ComboBox();
				comboEditor.Items.Add(true);
				comboEditor.Items.Add(false);
				comboEditor.SelectedValue = field.Value;
				comboEditor.BorderThickness = new Thickness(0);

				element = comboEditor;
			}
			else if (field.PropertyType.IsPrimitive || field.PropertyType == typeof(String))
			{
				var textEditor = new TextBox();
				textEditor.Text = (field.Value == null) ? string.Empty : field.Value.ToString();
				textEditor.BorderThickness = new Thickness(0);

				element = textEditor;
			}
			else
			{
				var textBlock = new TextBlock();
				textBlock.Text = field.Value == null ? string.Empty : field.Value.ToString();

				element = textBlock;
			}

			return new PropertyEditor(element, field);
		}
	}
}

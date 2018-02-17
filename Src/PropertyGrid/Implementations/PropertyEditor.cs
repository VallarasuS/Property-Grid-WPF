
#region Namespace Imports

using System;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace Vasu.Wpf.Controls
{
    /// <summary>
    /// Defines a <see cref="PropertyEditor"/> used in <see cref="PropertyGrid"/>.
    /// </summary>
    internal class PropertyEditor : IDisposable
	{
		#region Ctor

		public PropertyEditor(FrameworkElement editor, PropertyField field)
		{
			Init(editor, field);
		}

		private void Init(FrameworkElement editor, PropertyField field)
		{
			VisualEditor = editor;

			Field = field;

			WireEditor();
		}

		#endregion

		#region Memebers

		public FrameworkElement VisualEditor
		{
			get;
			private set;
		}

		public PropertyField Field { get; private set; }

		#endregion

		#region Implementations

		private void WireEditor()
		{
			if (VisualEditor is TextBox)
				(VisualEditor as TextBox).TextChanged += OnTextChanged;
			else if (VisualEditor is ComboBox)
				(VisualEditor as ComboBox).SelectionChanged += OnSelectionChanged;
		}

		private void UnWireEditor()
		{
			if (VisualEditor is TextBox)
				(VisualEditor as TextBox).TextChanged -= OnTextChanged;
			else if (VisualEditor is ComboBox)
				(VisualEditor as ComboBox).SelectionChanged += OnSelectionChanged;
		}

		#endregion

		#region Events and Handlers

		/// <summary>
		/// Raised when the value is changed by the <see cref="PropertyGrid"/>.
		/// </summary>
		public event ValueChangedEventHandler RequestValueChange;

		private void RaiseRequestValueChange(object newValue)
		{
			var handler = RequestValueChange;

			if (handler != null)
				handler(this, new ValueChangedEventArgs(Field.Name, Field.PropertyDescriptor, Field.PropertyInfo, Field.SourceObject, newValue, Field.Value));
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			var textBox = sender as TextBox;

			if (textBox == null) return;

			RaiseRequestValueChange(textBox.Text);
		}

		private void OnSelectionChanged(object sender, EventArgs e)
		{
			var comboBox = sender as ComboBox;

			if (comboBox == null) return;

			RaiseRequestValueChange(comboBox.SelectedValue);
		}

		#endregion

		#region IDisposable

		public void Dispose()
		{
			UnWireEditor();

			VisualEditor = null;

			Field = null;
		}

		#endregion
	}
}

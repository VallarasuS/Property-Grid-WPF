
#region Namespace Imports

using System;
using System.ComponentModel;
using System.Reflection;

#endregion

namespace Vasu.Wpf.Controls
{
    public class ValueChangedEventArgs : EventArgs
	{
		public ValueChangedEventArgs(string name, PropertyDescriptor propertyDescriptor, PropertyInfo propertyInfo, object source, object newValue, object oldValue)
		{
			Name = name;
			PropertyDescriptor = propertyDescriptor;
			PropertyInfo = propertyInfo;
			Source = source;
			NewValue = newValue;
			OldValue = oldValue;
		}

		public object NewValue { get; private set; }

		public object OldValue { get; private set; }

		public string Name { get; private set; }

		public object Source { get; private set; }

		public PropertyDescriptor PropertyDescriptor { get; private set; }

		public PropertyInfo PropertyInfo { get; private set; }
	}

	public delegate void ValueChangedEventHandler(object sender, ValueChangedEventArgs e);
}


#region Namespace Imports

using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

#endregion

namespace Vasu.Wpf.Controls
{
    public class PropertyField : ModelBase, IDisposable
	{
		#region Ctor

		public PropertyField()
		{

		}

		#endregion

		#region Members

        public string Name
        {
            get 
            {
                return (PropertyDescriptor == null)
                    ? (PropertyInfo == null)
                    ? string.Empty
                    : PropertyInfo.Name
                    : PropertyDescriptor.Name;
            }
        }

        public PropertyInfo PropertyInfo
        {
            get { return propertyInfo; }
            set 
            {
                if (value == propertyInfo) return;

                propertyInfo = value;

                RaisePropertyChanged("PropertyInfo");
            }
        }

		public PropertyDescriptor PropertyDescriptor
		{
			get { return propertyDescriptor; }
            private	set 
			{
				if (propertyDescriptor == value) return;

                RemoveValueChanged();

				propertyDescriptor = value;

                if (!isListening)
                    AddValueChanged();

				RaisePropertyChanged("PropertyDescriptor");
                RaisePropertyChanged("Name");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("Value");
			}
		}

        public string CategoryName
        {
            get { return PropertyDescriptor.Category; }
        }

		public Object SourceObject
		{
			get { return sourceObject; }
			set 
			{
				if (sourceObject == value) return;

                PropertyDescriptor = TypeDescriptor.GetProperties(value)
                    .OfType<PropertyDescriptor>()
                    .FirstOrDefault(d => d.Name == PropertyInfo.Name);

                RemoveValueChanged();

				sourceObject = value;

                if (!isListening)
                    AddValueChanged();

				RaisePropertyChanged("SourceObject");
			}
		}

		public Object Value
		{
			get { return this.value; }
			set 
			{
				if (this.value == value) return;

				this.value = value;

				if (!ignoreValueChange)
					RaisePropertyChanged("Value");
			}
		}

		public Type PropertyType
		{
			get 
			{
                return (PropertyInfo == null) ? Type.EmptyTypes[0] : PropertyInfo.PropertyType;
                //return (PropertyDescriptor == null) ? Type.EmptyTypes[0] : PropertyDescriptor.PropertyType;
			}
		}

		public String Description
		{
			get 
			{
                return (PropertyDescriptor == null)
                    ? (PropertyInfo == null || PropertyInfo.Attributes == PropertyAttributes.None)
                    ? string.Empty
                    : (PropertyInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)[0] as DescriptionAttribute).Description
                    : PropertyDescriptor.Description;
			}
		}

		public Object VisualEditor
		{
			get 
			{
                return (PropertyEditor == null) ? null : PropertyEditor.VisualEditor;
			}
		}

        public PropertyGrid PropertyGrid { get; set; }

        internal PropertyEditor PropertyEditor
        {
            get 
            {
                if (propertyEditor == null)
                {
                    propertyEditor = PropertyEditorProvider.GetFieldEditor(this);
                    propertyEditor.RequestValueChange += (s, e) => { PropertyGrid.RaiseValueChanged(e); };
                }

                return propertyEditor; 
            }
        }

		#endregion

		#region Implementations

        /// <summary>
        /// Raised when PropertyValue changed from external sources.
        /// </summary>
        /// <param name="sender">sender whose value is changed.</param>
        /// <param name="e">event arguments associated with the event.</param>
        /// <remarks>Unless the source objects implements <see cref="INotifyPropertyChanged"/> or any change notification mechanism, this is of no use. </remarks>
		private void OnValueChanged(Object sender, EventArgs e)
		{
			ignoreValueChange = true;

			Value = PropertyDescriptor.GetValue(SourceObject);
			
			ignoreValueChange = false;
		}

        /// <summary>
        /// Starts listenting to value changes by adding an event handler.
        /// </summary>
		private void AddValueChanged()
		{
            if (SourceObject == null || PropertyDescriptor == null) return;

			PropertyDescriptor.AddValueChanged(SourceObject, OnValueChanged);

			isListening = true;
		}

        /// <summary>
        /// Stops listenting to the value changes.
        /// </summary>
		private void RemoveValueChanged()
		{
			if (SourceObject == null || PropertyDescriptor == null) return;

			PropertyDescriptor.RemoveValueChanged(SourceObject, OnValueChanged);

			isListening = false;
		}

		#endregion

		#region IDisposable

		/// <summary>
		/// Releases any references held by <see cref="PropertyField"/>.
		/// </summary>
		public void Dispose()
		{
            RemoveValueChanged();

            PropertyEditor.Dispose();

			SourceObject = null;
		}

		#endregion

		#region Fields

		private PropertyDescriptor propertyDescriptor = null;
		private Object sourceObject = null;
		private Object value = null;
		private bool isListening = false;
		private bool ignoreValueChange = false;
        private PropertyEditor propertyEditor = null;
        private PropertyInfo propertyInfo = null;

        #endregion
	}
}


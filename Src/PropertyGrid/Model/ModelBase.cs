
#region Namespace Imports

using System.ComponentModel;

#endregion

namespace Vasu.Wpf.Controls
{
    public class ModelBase : INotifyPropertyChanged
	{
		#region Ctor

		public ModelBase()
		{ }

		#endregion

		#region ** INotifyPropertyChanged

		public void RaisePropertyChanged(string propertyName)
		{
			var handler = PropertyChanged;
			
			if (handler == null) return;

			handler(this, new PropertyChangedEventArgs(propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion
	}
}

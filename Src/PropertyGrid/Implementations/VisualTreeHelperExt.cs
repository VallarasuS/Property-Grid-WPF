using System.Windows;
using System.Windows.Media;

namespace Vasu.Wpf.Controls
{
    public static class VisualTreeHelperExt
	{
		public static Visual FindParentOfType<T>(DependencyObject source)
		{
			while (source!= null && source.GetType() != typeof(T))
			{
				source = VisualTreeHelper.GetParent(source);
			}

			return source as Visual;
		}
	}
}

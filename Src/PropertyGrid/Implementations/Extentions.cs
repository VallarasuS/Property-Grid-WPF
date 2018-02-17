
#region Namespace Imports

using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Vasu.Wpf.Controls
{
    public static class Extentions
	{
		public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
		{
			foreach (T item in collection)
				action.DynamicInvoke(item);
		}

		public static IEnumerable<T> AsEnumerable<T>(this IList list)
		{
			var enumerator = list.GetEnumerator();

			enumerator.Reset();

			while (enumerator.MoveNext())
			{
				if (enumerator.Current is T)
					yield return (T)enumerator.Current;
				else
					continue;
			}
		}
	}
}

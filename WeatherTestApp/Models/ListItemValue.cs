using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherTestApp
{
	// Represents one item in our list.
	public class ListItemValue : IComparable<ListItemValue>, IListItemValue
	{
		public string CompareMark { get; set; }

		public string Title { get; set; }

		public ListItemValue() { }

		int IComparable<ListItemValue>.CompareTo(ListItemValue value)
		{
			return CompareMark.CompareTo(value.CompareMark);
		}

		public void setComparedProp(string comparedProp) {
			CompareMark = comparedProp;
		}
	}
}
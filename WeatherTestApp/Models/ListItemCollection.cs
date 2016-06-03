using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherTestApp
{
	// Represents a group of items in our list.
	public class ListItemCollection : ObservableCollection<ListItemValue>, IListItemCollection<ListItemValue>
	{
		// Data used to populate our list.
		public List<ListItemValue> ListItemValues { get; set; }

		public string Title { get; set; }

		public string LongTitle { get { return "" + Title; } }

		public ListItemCollection() {
			ListItemValues = new List<ListItemValue> ();
		}

		public void setTitle(string title) {
			Title = title;
		}

		public List<ListItemValue> getSortedData() {
			var items = ListItemValues;

			if (items != null) {
				items.Sort();
			}

			return items;
		}

		public void addItemToCollection(ListItemValue newValue) {
			ListItemValues.Add(newValue);
		}

		public void addItemsToCollection(List<ListItemValue> newItemValues) {
			foreach(ListItemValue itemValue in newItemValues) {
				ListItemValues.Add (itemValue);
			}
		}
	}
}

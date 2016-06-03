using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace WeatherTestApp
{
	public class CitiesCollection : ListItemCollection
	{
		public CitiesCollection () {
			
		}

		// Get list view data redy to display
		public static ObservableCollection<CitiesCollection> getListViewCollection(Dictionary<string, List<CityValue>> dataList) {
			var allCollectGroups = new ObservableCollection<CitiesCollection>();

			foreach(KeyValuePair<string, List<CityValue>> pair in dataList) {
				CitiesCollection collect = new CitiesCollection ();
				collect.setTitle (pair.Key);
				collect.addItemsToCollection (new List<ListItemValue> (pair.Value));

				foreach (var item in collect.getSortedData()) {					
					var collectGroup = allCollectGroups.FirstOrDefault(g => g.Title == item.CompareMark);

					if (collectGroup == null)
					{
						collectGroup = new CitiesCollection ();
						collectGroup.setTitle (pair.Key);

						collectGroup.Add(item);

						allCollectGroups.Add(collectGroup);
					} else {
						collectGroup.Add(item);
					}
				}
			}

			return allCollectGroups;
		}
	}
}


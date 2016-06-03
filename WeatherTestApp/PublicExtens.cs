using System;
using Xamarin.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace WeatherTestApp
{
	public static class PublicExtens
	{
		public static void FilterLocations (this ListView listView, List<CityValue> collect, string filterProp, string filter)
		{
			listView.BeginRefresh ();

			DataModel<CityValue> dataModel = new DataModel<CityValue> ();

			if (string.IsNullOrWhiteSpace (filter)) {
				dataModel.parseDataToSections (collect, filterProp);

				listView.ItemsSource = CitiesCollection.getListViewCollection(dataModel.getData());
			} else {
				List<CityValue> newDataCollect = new List<CityValue> ();

				foreach(CityValue cityVal in collect) {
					if (Regex.IsMatch(cityVal.Name.ToLower(), "^" + filter.ToLower())) {
						newDataCollect.Add (cityVal);
					}
				}

				dataModel.parseDataToSections (newDataCollect, filterProp);

				listView.ItemsSource = CitiesCollection.getListViewCollection (dataModel.getData());
			}

			listView.EndRefresh ();
		}
	}
}


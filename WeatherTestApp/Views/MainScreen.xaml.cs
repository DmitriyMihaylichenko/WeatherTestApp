using System;
using System.Collections.Specialized;
using System.Collections.Generic;

using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;

namespace WeatherTestApp
{
	public partial class MainScreen : ContentPage
	{
		public MainScreen ()
		{
			InitializeComponent ();

			CollectionManager.CitiesCollectionReady += () => {
				itemListView.ItemsSource = CollectionManager.getCitiesReadyCollection (CollectionManager.citiesCollection, Config.groupingProp);
			};

			CollectionManager.loadCitiesFromFileToCollect (Config.cityListFile, CollectionManager.citiesCollection);

			searchBar.TextChanged += (sender, e) => {
				//actIndicator.IsRunning = true;

				itemListView.FilterLocations (CollectionManager.citiesCollection, Config.groupingProp, searchBar.Text);

				//actIndicator.IsRunning = false;
			};
				
			searchBar.SearchButtonPressed += (sender, e) => {
				//actIndicator.IsRunning = true;

				itemListView.FilterLocations (CollectionManager.citiesCollection, Config.groupingProp, searchBar.Text);

				//actIndicator.IsRunning = false;
			};
		}

		// Clear search bar
		public void clearSearch() {
			searchBar.Text = string.Empty;
		}

		void OnItemTapped (object sender, ItemTappedEventArgs ea) {
			CityValue selectedCity = (CityValue)ea.Item;

			DetailScreen detailScr = new DetailScreen (selectedCity);

			Navigation.PushAsync (detailScr);

			((ListView)sender).SelectedItem = null;
		}
	}
}


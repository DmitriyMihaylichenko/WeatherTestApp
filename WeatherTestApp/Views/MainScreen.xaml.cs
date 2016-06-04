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

			showItemsList (false);
			showLoadingInd (true);

			CollectionManager.CitiesCollectionReady += () => {
				Device.BeginInvokeOnMainThread (() => {
					itemListView.ItemsSource = CollectionManager.getCitiesReadyCollection (CollectionManager.citiesCollection, Config.groupingProp);
				
					itemListView.FadeTo(1, 1000, Easing.Linear);

					showLoadingInd(false);
				});
			};

			CollectionManager.loadCitiesFromFileToCollect (Config.cityListFile, CollectionManager.citiesCollection);

			searchBar.TextChanged += (sender, e) => {
				itemListView.FilterLocations (CollectionManager.citiesCollection, Config.groupingProp, searchBar.Text);
			};
				
			searchBar.SearchButtonPressed += (sender, e) => {
				itemListView.FilterLocations (CollectionManager.citiesCollection, Config.groupingProp, searchBar.Text);
			};
		}

		// Show|Hide Items List View
		public void showItemsList(bool isShowing) {
			if (isShowing) {
				itemListView.Opacity = 1;
			} else {
				itemListView.Opacity = 0;
			}
		}

		// Show|Hide loading indicator
		public void showLoadingInd(bool isLoading) {
			if (isLoading) {
				actIndicator.IsRunning = true;
			} else {
				actIndicator.IsRunning = false;
				actIndicator.IsVisible = false;
			}
		}

		// Clear search bar
		public void clearSearch() {
			
		}

		void OnItemTapped (object sender, ItemTappedEventArgs ea) {
			CityValue selectedCity = (CityValue)ea.Item;

			DetailScreen detailScr = new DetailScreen (selectedCity);

			Navigation.PushAsync (detailScr);

			((ListView)sender).SelectedItem = null;
		}
	}
}


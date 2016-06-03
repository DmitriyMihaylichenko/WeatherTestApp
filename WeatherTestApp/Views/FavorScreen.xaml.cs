using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WeatherTestApp
{
	public partial class FavorScreen : ContentPage
	{
		public FavorScreen ()
		{
			InitializeComponent ();

			FavoritesManager.LoadFavorFromStorageToCollect ((favorCollect) => {
				CollectionManager.favorCollection = favorCollect;

				updateListView();
			});

			searchBar.TextChanged += (sender, e) => {
				//actIndicator.IsRunning = true;

				itemListView.FilterLocations (CollectionManager.favorCollection, Config.groupingProp, searchBar.Text);

				//actIndicator.IsRunning = false;
			};

			searchBar.SearchButtonPressed += (sender, e) => {
				//actIndicator.IsRunning = true;

				itemListView.FilterLocations (CollectionManager.favorCollection, Config.groupingProp, searchBar.Text);

				//actIndicator.IsRunning = false;
			};
		}

		// Clear search bar
		public void clearSearch() {
			searchBar.Text = string.Empty;
		}

		// Update ListView when activated
		public void updateListView() {
			itemListView.ItemsSource = CollectionManager.getCitiesReadyCollection (CollectionManager.favorCollection, Config.groupingProp);
		}

		// On item selected
		void OnItemTapped (object sender, ItemTappedEventArgs ea) {
			CityValue selectedCity = (CityValue)ea.Item;

			DetailScreen detailScr = new DetailScreen (selectedCity);

			Navigation.PushAsync (detailScr);

			((ListView)sender).SelectedItem = null;
		}
	}
}


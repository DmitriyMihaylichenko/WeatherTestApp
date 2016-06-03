using System;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

using CrossPlatformLibrary.Maps;

namespace WeatherTestApp
{
	public partial class DetailScreen : ContentPage
	{
		private CityValue _selectedCity;
		public CityValue selectedCity {
			get {
				return _selectedCity;
			}

			set {
				_selectedCity = value;

				cityCaption.Text = $"{_selectedCity.Name} , {_selectedCity.Country}";
				geoCoordsLabel.Text = $"[{_selectedCity.Coord.Lon} : {_selectedCity.Coord.Lat}]";
			}
		}

		private WeatherState _weatherState;
		public WeatherState weatherState {
			get {
				return _weatherState;
			}

			set {
				_weatherState = value;

				Device.BeginInvokeOnMainThread (() => {
					mainLabel.Text = _weatherState.Main;
					tempLabel.Text = _weatherState.Temperature;
					dateLabel.Text = _weatherState.Date;
					windSpeedLabel.Text = _weatherState.WindSpeed;
					windDegLabel.Text = _weatherState.WindDeg;
					cloudinessLabel.Text = _weatherState.Clouds;
					pressureLabel.Text = _weatherState.Pressure;
					humidityLabel.Text = _weatherState.Humidity;
					sunriseLabel.Text = _weatherState.Sunrise;
					sunsetLabel.Text = _weatherState.Sunset;

					weatherMan.sendImageRequest(_weatherState.IconUrl);
				});
			}
		}

		public WeatherManager weatherMan;

		public bool cityInFavorCollect;

		public DetailScreen (CityValue currCity)
		{
			InitializeComponent ();

			this.selectedCity = currCity;

			weatherMan = new WeatherManager ();

			weatherMan.onWeatherResponse += OnWeatherResponse;
			weatherMan.onImageResponse += OnImageResponse;

			weatherMan.sendWeatherRequest (currCity.Id.ToString());

			setupAddButton ();
		}

		// Customize Add To Favorites Button
		public void setupAddButton() {
			string btnAddToFavorText = string.Empty;

			if (FavoritesManager.isCityInCollect (selectedCity, CollectionManager.favorCollection)) {
				btnAddToFavorText = "Remove from favorites";

				addToFavor.BackgroundColor = Color.Red;
			} else {
				btnAddToFavorText = "Add to favorites";

				addToFavor.BackgroundColor = Color.Green;
			}

			addToFavor.Text = btnAddToFavorText;
		}

		// One city selected
		public async void OnAddToFavourClick(object sender, EventArgs ergs) {
			string message = string.Empty;

			if (FavoritesManager.isCityInCollect (selectedCity, CollectionManager.favorCollection)) {
				message = "removed from favorites";

				FavoritesManager.remFromFavor (selectedCity, CollectionManager.favorCollection);
			} else {
				message = "added to favorites";

				FavoritesManager.addToFavor (selectedCity, CollectionManager.favorCollection);
			}

			setupAddButton ();

			DisplayAlert("My Favorites", $"{_selectedCity.Name} {message}", "OK");
		}

		public void OnWeatherResponse(WeatherState weatherState) {
			this.weatherState = weatherState;
		}

		public void OnImageResponse(byte[] bytes) {
			Device.BeginInvokeOnMainThread (async () => {
				weatherImage.Source = ConvertToImage(bytes);
			});
		}

		// Convert byte array to image
		public ImageSource ConvertToImage(byte[] bytes) {
			ImageSource retSource = null;

			if (bytes != null) {
				retSource = ImageSource.FromStream(() => new MemoryStream(bytes));
			}

			return retSource;
		}
	}
}


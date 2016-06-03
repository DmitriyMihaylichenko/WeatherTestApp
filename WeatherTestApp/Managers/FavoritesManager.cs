using System;
using System.IO;
using System.Text;
using System.Json;
using System.Threading.Tasks;
using System.Net;
using System.Linq;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WeatherTestApp
{
	public class FavoritesManager
	{
		// Save favorites collection to local storage
		public static async void SaveFavorToStorageFromCollect(List<CityValue> collect) {
			var fileService = DependencyService.Get<ISaveAndLoad> ();

			string jsonCollect = await CollectionToJSON (collect);

			await fileService.SaveTextAsync (Config.storageFileName, jsonCollect);
		}

		// Load collection from storage
		public static async void LoadFavorFromStorageToCollect(Action<List<CityValue>> callback) {
			string lines = string.Empty;

			try {
				lines = await LoadFavorJSONFromStorage();
			} catch {
				
			}

			List<CityValue> collect = await JSONToCollection (lines);

			callback (collect);
		}

		// Load favorites json file from local storage
		private static async Task<string> LoadFavorJSONFromStorage() {
			var fileService = DependencyService.Get<ISaveAndLoad> ();

			return await fileService.LoadTextAsync(Config.storageFileName);
		}

		// Convert collection to json string
		public static Task<string> CollectionToJSON(List<CityValue> collect) {
			return Task.Run (() => {
				string resString = string.Empty;

				foreach(CityValue city in collect) {
					resString += "{\"_id\":"+city.Id+",\"name\":\""+city.Name+"\",\"country\":\""+city.Country+"\",\"coord\":{\"lon\":"+city.Coord.Lon+",\"lat\":"+city.Coord.Lat+"}}\n";
				}

				return resString;
			});
		}

		// Convert json string to collection list
		private static Task<List<CityValue>> JSONToCollection(string input) {
			return Task.Run (() => {
				List<CityValue> list = new List<CityValue>();

				using (Stream stream = GenerateStreamFromString(input)) {
					using (var streamReader = new StreamReader (stream, Encoding.UTF8)) {
						string line = string.Empty;

						while ((line = streamReader.ReadLine ()) != null) {
							var json = JsonValue.Parse(line);

							int id = Convert.ToInt32(json ["_id"].ToString());

							string name = json ["name"];
							string country = json["country"];

							JsonValue coordCont = json ["coord"];

							float lon = float.Parse (coordCont ["lon"].ToString());
							float lat = float.Parse (coordCont ["lat"].ToString());				

							CityValue.CityCoord coord = new CityValue.CityCoord (lon, lat); 

							CityValue cityVal = new CityValue (id, name, country, coord);

							list.Add (cityVal);
						}
					}
				}

				return list;
			});				
		}

		// Generate stream from string
		public static Stream GenerateStreamFromString(string str) {
			MemoryStream stream = new MemoryStream();

			StreamWriter writer = new StreamWriter(stream);

			writer.Write(str);

			writer.Flush();

			stream.Position = 0;

			return stream;
		}

		// Check is city in collect
		public static bool isCityInCollect(CityValue city, List<CityValue> collect) {
			bool present = false;

			foreach(CityValue currCity in collect) {
				if (city.Id == currCity.Id) {
					present = true;

					break;
				}
			}

			return present;
		}

		// Add new city to favorites list
		public static void addToFavor(CityValue city, List<CityValue> collect) {
			collect.Add (city);

			SaveFavorToStorageFromCollect (collect);

			//string str = await FavoritesManager.CollectionToJSON (CollectionManager.citiesCollection);

			//Global.Log (str);
		}

		// Remove city from favorites list
		public static void remFromFavor(CityValue city, List<CityValue> collect) {
			collect.Remove (city);

			SaveFavorToStorageFromCollect (collect);
		}
	}
}


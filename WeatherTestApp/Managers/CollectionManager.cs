using System;
using System.IO;
using System.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.Linq;

namespace WeatherTestApp
{
	public class CollectionManager
	{
		public static event Action CitiesCollectionReady;
		public static event Action FavorCollectionReady;

		public static List<CityValue> citiesCollection;
		public static List<CityValue> favorCollection;

		// Initialize collection manager
		public static void init() {
			citiesCollection = new List<CityValue> ();
			favorCollection = new List<CityValue> ();
		}

		// Get ready to display grouped collection
		public static ObservableCollection<CitiesCollection> getCitiesReadyCollection(List<CityValue> collect, string groupBy) {
			DataModel<CityValue> dataModel = new DataModel<CityValue> ();

			dataModel.parseDataToSections (collect, groupBy);

			return CitiesCollection.getListViewCollection(dataModel.getData());
		}

		// Load cities from file
		public static async void loadCitiesFromFileToCollect(string fileName, List<CityValue> collect) {
			Task.Run (() => {
				var list = new List<string> ();

				var assembly = typeof(CollectionManager).GetTypeInfo().Assembly;

				//foreach (var res in assembly.GetManifestResourceNames())
				//	System.Diagnostics.Debug.WriteLine("found resource: " + res);

				Stream stream = assembly.GetManifestResourceStream($"WeatherTestApp.Resources.{fileName}");

				using (var streamReader = new StreamReader (stream, Encoding.UTF8)) {				
					string line = string.Empty;

					while ((line = streamReader.ReadLine ()) != null) {
						list.Add (line);
					}
				}

				parseDataFromJSONToCollect (list, collect);

				CitiesCollectionReady?.Invoke ();
			});
		}

		// Load data from file in JSON format and parse to collection
		public static void parseDataFromJSONToCollect(List<string> lines, List<CityValue> collect) {
			foreach (string line in lines) {
				var json = JsonValue.Parse(line);

				int id = Convert.ToInt32(json ["_id"].ToString());

				string name = json ["name"];
				string country = json["country"];

				JsonValue coordCont = json ["coord"];

				float lon = float.Parse (coordCont ["lon"].ToString());
				float lat = float.Parse (coordCont ["lat"].ToString());				

				CityValue.CityCoord coord = new CityValue.CityCoord (lon, lat); 

				CityValue cityVal = new CityValue (id, name, country, coord);

				collect.Add (cityVal);
			}
		}
	}
}


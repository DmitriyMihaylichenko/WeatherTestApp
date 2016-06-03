using System;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Json;
using System.Text.RegularExpressions;

namespace WeatherTestApp
{
	public class WeatherManager
	{
		public event Action<WeatherState> onWeatherResponse;
		public event Action<byte[]> onImageResponse;

		public WeatherManager ()
		{
		}

		// Request to get weather image
		public void sendImageRequest(string requestUrl) {
			try {
				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);

				request.Method = "GET";
				request.Accept = "image/*";

				request.BeginGetResponse(ImageResponseCallback, request);
			} catch (Exception ex) {
				Global.Log (ex.Message);
			}
		}

		// Response callback weather image
		private void ImageResponseCallback(IAsyncResult result) {
			try {
				var request = (HttpWebRequest)result.AsyncState;
				var response = request.EndGetResponse(result);

				using (var stream = response.GetResponseStream()) {
					byte[] totalBytes = ReadFully(stream);

					onImageResponse?.Invoke(totalBytes);
				}
			} catch (Exception ex) {
				Global.Log (ex.Message);
			}
		}
		
		// Read image bytes from stream
		public static byte[] ReadFully(Stream input) {
			byte[] buffer = new byte[16 * 1024];

			using (MemoryStream ms = new MemoryStream()) {
				int read;

				while ((read = input.Read(buffer, 0, buffer.Length)) > 0) {
					ms.Write(buffer, 0, read);
				}

				return ms.ToArray();
			}
		}

		// Send request to get weather state
		public void sendWeatherRequest(string cityCode) {
			try {
				string requestUrl = Config.requestUrl + "?id=" + cityCode + "&APPID=" + Config.apiKey;

				HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);

				request.Method = "GET";
				request.Accept = "application/json";

				request.BeginGetResponse(WeatherResponseCallback, request);
			} catch (Exception ex) {
				Global.Log (ex.Message);
			}
		}

		// Response for get weather request
		private void WeatherResponseCallback(IAsyncResult result) {
			try {
				var request = (HttpWebRequest)result.AsyncState;
				var response = request.EndGetResponse(result);

				using (var stream = response.GetResponseStream()){
					using (var reader = new StreamReader(stream)){
						string contentJSON = reader.ReadToEnd();

						WeatherState weatherState = parseWeather(contentJSON);

						onWeatherResponse?.Invoke(weatherState);
					}
				}
			} catch (Exception ex) {
				Global.Log (ex.Message);
			}
		}

		// Parse weather state to object
		private WeatherState parseWeather(string input) {
			JsonValue json = JsonValue.Parse(input);

			JsonValue weatherCont = json["weather"][0];

			string main = Regex.Replace(weatherCont ["main"].ToString(), "\"", string.Empty);
			string description = weatherCont ["description"].ToString ();
			string iconFile = Regex.Replace(weatherCont ["icon"].ToString (), "\"", string.Empty);

			string icon = $"http://api.openweathermap.org/img/w/{iconFile}.png";

			JsonValue mainCont = json ["main"];

			float tempKelv = float.Parse(mainCont["temp"].ToString());

			string tempCels = Math.Round(tempKelv - 273.15f).ToString() + " \u2103";

			string pressure = mainCont ["pressure"].ToString () + " hpa";
			string humidity = mainCont ["humidity"].ToString () + " %";

			JsonValue sysCont = json ["sys"];

			float sunriseUnix = float.Parse (sysCont ["sunrise"].ToString ());
			string sysSunrise = new DateTime (1970, 1, 1, 0, 0, 0, 0).AddSeconds (sunriseUnix).ToLocalTime ().ToString ("H:mm");

			float sunsetUnix = float.Parse (sysCont ["sunset"].ToString ());
			string sysSunset = new DateTime (1970, 1, 1, 0, 0, 0, 0).AddSeconds (sunsetUnix).ToLocalTime ().ToString ("H:mm");

			JsonValue windCont = json["wind"];

			string windSpeed = windCont ["speed"].ToString() + " m/s";
			string windDeg = windCont["deg"].ToString() + " \u00B0";

			float dateUnix = float.Parse(json ["dt"].ToString ());
			string date = new DateTime (1970, 1, 1, 0, 0, 0, 0).AddSeconds (dateUnix).ToLocalTime ().ToString ();

			JsonValue cloudsCont = json["clouds"];

			string clouds = cloudsCont ["all"].ToString() + " %";

			return new WeatherState () {
				Temperature = tempCels,
				Pressure = pressure,
				Humidity = humidity,
				Sunrise = sysSunrise,
				Sunset = sysSunset,
				WindSpeed = windSpeed,
				WindDeg = windDeg,
				Date = date,
				Clouds = clouds,
				Main = main,
				Description = description,
				IconUrl = icon
			};
		}

		public string getTimeFromStamp(float stamp) {
			return new DateTime (1970, 1, 1, 0, 0, 0, 0).AddSeconds (stamp).ToLocalTime ().ToString ();
		}
	}
}


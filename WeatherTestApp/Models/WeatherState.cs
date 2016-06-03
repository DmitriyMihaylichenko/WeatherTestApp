using System;

namespace WeatherTestApp
{
	public class WeatherState
	{
		public string Main { get; set; }
		public string Description { get; set; }
		public string IconUrl { get; set; }
		public string Temperature { get; set; }
		public string Pressure { get; set; }
		public string Humidity { get; set; }
		public string Sunrise { get; set; }
		public string Sunset { get; set; }
		public string WindSpeed { get; set; }
		public string WindDeg { get; set; }
		public string Date { get; set; }
		public string Clouds { get; set; }

		public WeatherState ()
		{
		}
	}
}


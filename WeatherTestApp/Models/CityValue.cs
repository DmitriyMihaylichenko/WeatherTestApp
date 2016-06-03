using System;

namespace WeatherTestApp
{
	public class CityValue : ListItemValue
	{
		public struct CityCoord {
			public float Lon;
			public float Lat;

			public CityCoord (float lon, float lat) {
				this.Lon = lon;
				this.Lat = lat;
			}
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public string Country { get; set; }
		public CityCoord Coord { get; set; }

		public CityValue (int id, string name, string country, CityCoord coord)
		{
			this.Id = id;
			this.Name = name;
			this.Country = country;
			this.Coord = coord;

			this.Title = name;

			setComparedProp(country);
		}
	}
}


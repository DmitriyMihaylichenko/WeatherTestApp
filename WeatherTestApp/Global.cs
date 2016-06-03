using System;
using System.Diagnostics;

namespace WeatherTestApp
{
	public class Global
	{
		public static void Log(object obj) {
			Debug.WriteLine (obj);
		}
	}
}


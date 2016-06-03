using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace WeatherTestApp
{
	public class App : Application
	{
		public App () {
			CollectionManager.init();

			MasterPage masterPage = new MasterPage ();

			this.MainPage = new NavigationPage(masterPage){
				BackgroundColor = Color.Aqua
			};
		}

		protected override void OnStart () {}
		protected override void OnSleep () {}
		protected override void OnResume () {}
	}
}


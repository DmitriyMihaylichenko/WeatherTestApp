using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace WeatherTestApp
{
	public partial class MasterPage : TabbedPage
	{
		MainScreen mainScreen;
		FavorScreen favorScreen;

		public MasterPage ()
		{
			InitializeComponent ();

			mainScreen = new MainScreen();
			favorScreen = new FavorScreen ();

			Title = "Cities List";

			Children.Add (mainScreen);
			Children.Add (favorScreen);

			CurrentPageChanged += (object sender, EventArgs e) => {
				var i = Children.IndexOf(CurrentPage);

				if(1 == i) {
					favorScreen.updateListView();
				}
			};
		}

		protected override void OnAppearing() {            
			base.OnAppearing();

			mainScreen.clearSearch ();
			favorScreen.clearSearch ();

			favorScreen.updateListView ();
		}
	}
}


using System;
using System.Collections.Generic;

namespace WeatherTestApp
{
	public interface IListItemCollection<T>
	{
		List<T> ListItemValues { get; set; }

		List<T> getSortedData();

		void addItemsToCollection (List<T> itemsList);
	}
}


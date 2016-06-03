using System;
using System.Reflection;
using System.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace WeatherTestApp
{
	public class DataModel<T> where T : ListItemValue
	{
		private Dictionary<string, List<T>> data { get; set; }

		public DataModel ()
		{
			data = new Dictionary<string, List<T>> ();
		}

		// Parse data into model
		public void parseDataToSections(List<T> dataList, string markProp) {
			Dictionary<string, List<T>> outData = new Dictionary<string, List<T>> ();

			List<string> sectionsArr = new List<string>();

			foreach(T elem in dataList) {
				string markPropVal = GetPropValue (elem, markProp).ToString();

				if (markPropVal != string.Empty) {
					if (!sectionsArr.Contains (markPropVal)) {
						sectionsArr.Add (markPropVal);
					}
				}
			}

			foreach(string sect in sectionsArr) {
				List<T> currList = new List<T> ();

				foreach (T elem in dataList) {
					string markPropVal = GetPropValue (elem, markProp).ToString();

					if (sect == markPropVal) {
						currList.Add (elem);
					}
				}

				outData.Add (sect, currList);
			}

			setData (outData);
		}

		// Get value of property
		public object GetPropValue(object sourceObj, string propName)
		{
			PropertyInfo propInfo = sourceObj.GetType ().GetRuntimeProperty(propName);

			return propInfo.GetValue (sourceObj);
		}

		// Set data for current model
		public void setData(Dictionary<string, List<T>> data) {
			this.data = data;
		}

		// Get data from current model
		public Dictionary<string, List<T>> getData() {
			return data;
		}

		// Get list of data
		public List<T> getDataList() {
			List<T> allDataCollect = new List<T> ();

			foreach(KeyValuePair<string, List<T>> pair in data) {
				foreach(T cityVal in pair.Value) {
					allDataCollect.Add (cityVal);
				}
			}

			return allDataCollect;
		}
	}
}


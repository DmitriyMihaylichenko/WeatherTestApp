using System;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

using WeatherTestApp.iOS;
using WeatherTestApp;

using Foundation;


[assembly: Dependency (typeof(SaveAndLoad_iOS))]

namespace WeatherTestApp.iOS
{
	public class SaveAndLoad_iOS : ISaveAndLoad
	{
		public static string DocumentsPath {
			get {
				var documentsDirUrl = NSFileManager.DefaultManager.GetUrls (NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User).Last ();
				return documentsDirUrl.Path;
			}
		}

		public async Task SaveTextAsync (string filename, string text) {
			string path = CreatePathToFile (filename);
			using (StreamWriter sw = File.CreateText(path))
				await sw.WriteAsync(text);
		}

		public async Task<string> LoadTextAsync (string filename) {
			string path = CreatePathToFile (filename);
			using (StreamReader sr = File.OpenText(path))
				return await sr.ReadToEndAsync();
		}

		public bool FileExists (string filename) {
			return File.Exists (CreatePathToFile (filename));
		}

		static string CreatePathToFile(string fileName) {
			return Path.Combine (DocumentsPath, fileName);
		}
	}
}
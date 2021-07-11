using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
using System.IO;
using Xamarin.Forms;
using NoteIT;

[assembly: Dependency(typeof(NoteDatabase))]

namespace NoteIT
{
  public  class NoteDatabase : ISQLiteDb 
    {
		// Stellt Verbindung zur Datenbank her und richtet diese ein
		public SQLiteAsyncConnection GetConnection()
		{
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			var path = Path.Combine(documentsPath, "MeineDatenBank1.db2");

			return new SQLiteAsyncConnection(path);
		}
	}
}

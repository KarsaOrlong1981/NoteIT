using SQLite;

namespace NoteIT
{
    public interface ISQLiteDb
    {
        //Ruft die Datenbank auf
        SQLiteAsyncConnection GetConnection();
    }
}

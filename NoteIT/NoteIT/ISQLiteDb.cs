using SQLite;

namespace NoteIT
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}

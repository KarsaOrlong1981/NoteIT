


using SQLite;



namespace NoteIT
{
    public class Note
    {
       
        [PrimaryKey, AutoIncrement]
        
       
      public int ID { get; set; }
      public string Text { get; set; }
      public string Date { get; set; }
      public string ColorBG { get;set; }
        
    }
}

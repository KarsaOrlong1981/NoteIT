using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SQLite;
using System.Collections.ObjectModel;
using Android.Content;
using Xamarin.Essentials;
using System.IO;
using Microsoft.AppCenter.Analytics;
using System.Xml;


namespace NoteIT
{
    
   
   


    public partial class ListPage4 : ContentPage
    {
        SQLiteAsyncConnection connection;
        DateTimeOffset date;
        ObservableCollection<Note> note;
        ObservableCollection<Note> saveNote;
        string text;
        string newColorBG;
        
        bool isNoEntry;
        bool sortByColor;

        public ListPage4(string noteText,string colorBG,bool entry)
        {
            InitializeComponent();


            
            //verhindern das bei jedem neu Initialisieren der Seite, immer wieder die gleiche Notiz erstellt wird
            isNoEntry = false;

            isNoEntry = entry;
            if(entry == true)
            {
                text = noteText;
                newColorBG = colorBG;
                date = DateTimeOffset.Now;
            }
            else
            {
                text = "";
                newColorBG = "";
                date = DateTimeOffset.Now;
            }
            saveNote = new ObservableCollection<Note>();
            //DatenBank Aufrufen
            connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        //Verbindung zur Datenbank herstellen und Daten einlagern.
        protected override async void OnAppearing()
        {
          

            await connection.CreateTableAsync<Note>();

            var _note = await connection.Table<Note>().ToListAsync();
           
                    
                note = new ObservableCollection<Note>(_note);

            
         
            

            
                  
                collectionView.ItemsSource = note;
            base.OnAppearing();
            //Wenn die Notiz leer ist oder nicht neu Angelegt wurde, keinen neuen Eintrag in der Datenbank vornehmen
            if(isNoEntry == true)
            OnAdd();
            isNoEntry = false;
        }
      
        //Die methode muss aus der Seite NewNotePage3 aufgerufen werden um die Eingetragenen Daten zu übernehmen.
        async void OnAdd()
        {

            var noteAn = new Note
            {
                Text = text,
                Date = Convert.ToString(date.ToString("F")),
                ColorBG = newColorBG
            };
        
         

         await connection.InsertAsync(noteAn);

            //Die Daten der liste übergeben um sie Sichtbar zu machen
            note.Add(noteAn);

    }


        async void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int counter = 0;
            // wenn eine Notiz gewählt wurde, soll das SelectionChanged Ereignis
            // nicht doppelt ausgelöst werden wenn eine Aktion ausgeführt wurden ist.
            if (collectionView.SelectedItem == null)
                return;
            string action = await DisplayActionSheet("Wähle eine Option:", "Abbrechen", "Notiz Löschen", "Notiz Bearbeiten", "Erinnerung Aktivieren", "Notiz Teilen");

            if (action == "Notiz Löschen")
            {
                //einer String Var curr den Aktuellen Inhaltstext übergeben
                string curr = (e.CurrentSelection.FirstOrDefault() as Note)?.Text;
                // über eine Schleife jedes element in note mit dem Text vergleichen um die aktuelle Position per zaehler zu ermitteln
                for (int i = 0; i < note.Count; i++)
                {
                    if (note[i].Text == curr)
                        break;
                    else
                        counter++;
                }
                //die aktuelle position übergeben um das gewählte element zu Löschen
                DeleteNote(counter);
               

                    collectionView.SelectedItem = null;

            }
            if (action == "Notiz Bearbeiten")
            {
                //einer String Var curr den Aktuellen Inhaltstext übergeben
                string curr  =  (e.CurrentSelection.FirstOrDefault() as Note)?.Text;
                // über eine Schleife jedes element in note mit dem Text vergleichen um die aktuelle Position per zaehler zu ermitteln
                for (int i = 0; i< note.Count; i++)
                {
                    if (note[i].Text == curr)
                        break;
                    else
                        counter++;
                }
                //Wenn dieser text nicht gleich mit dem alten ist darf gelöscht werden
                text = note[counter].Text;
                newColorBG = note[counter].ColorBG;
                NewNotePage3 send = new NewNotePage3(newColorBG);
                send.SendData(text, newColorBG);
                //Hier darf nur gelöscht werden wenn die Notiz auch wirklich geändert wurde
                DeleteNote(counter);
                await Navigation.PushAsync(send);
                Navigation.RemovePage(this);
                collectionView.SelectedItem = null;
            }
            if (action == "Notiz Teilen")
            {
                
                string curr = (e.CurrentSelection.FirstOrDefault() as Note)?.Text;
               
                ShareNote(curr);
                collectionView.SelectedItem = null;
            }
            if (action == "Erinnerung Aktivieren")
            {
                NoteMemory();
                collectionView.SelectedItem = null;
            }
        }

      

        // Methode zum Löschen eines Eintrags
        async void DeleteNote(int value)
        {

            var theNote = note[value];
            await connection.DeleteAsync(theNote);
            note.Remove(theNote);
            if (sortByColor == true)
                WriteSortList();
            sortByColor = false;
        }
        void ChangeNote()
        {

        }
      
        //Ermöglicht das Teilen über andere Apps
        async void ShareNote(string message)
        {
          
            await Share.RequestAsync(new ShareTextRequest
            {
              
                Text = message,
                Title = "Share!"
            }) ;
        }
        

       
        void NoteMemory()
        {
            
        }
        async void BackTOADD()
        {
            NewNotePage3 send = new NewNotePage3(newColorBG);
            await Navigation.PushAsync(send);
            Navigation.RemovePage(this);
        }
        private void ToolbarItemADD_Clicked(object sender, EventArgs e)
        {
            BackTOADD();
        }

        private void ToolbarItemClearAll_Clicked(object sender, EventArgs e)
        {
            AskAllDelete();
        }
        async void AskAllDelete()
        {
            string answer = await DisplayActionSheet("Wirklich alle Notizen Löschen ?", "Cancel", "Ja, alle Notizen Löschen");
            sortByColor = false;

            if (answer == "Ja, alle Notizen Löschen")
            {
                int zaehler = 0;
                foreach (Note element in note)
                {

                   
                    DeleteNote(zaehler);
                    zaehler++;

                }
                collectionView.ItemsSource = note;
            }
        }

        void ToolbarItemColor_Clicked(object sender, EventArgs e)
        {
            sortByColor = true;
            WriteSortList();

            collectionView.ItemsSource = saveNote;
            
        }
        void WriteSortList()
        {

            
                saveNote.Clear();//Immer löschen, um wieder eine neue anzahl an elementen hinzufügen zu können
                int counter = 0;
                //Leer Eintraege erstellen um saveNote die gleiche anzahl an Counts zu übergeben wie note,
                //so kann saveNote die daten von note aufnehmen und speichern
                var noteAns = new Note
                {
                    Text = "",
                    Date = "",
                    ColorBG = ""
                };

                for (int i = 0; i < note.Count; i++)
                {
                    saveNote.Add(noteAns);
                }

                for (int i = 0; i < note.Count; i++)
                {
                    if (note[i].ColorBG == "Gray")
                    {
                        saveNote[counter] = note[i];
                        counter++;
                    }
                }
                for (int i = 0; i < note.Count; i++)
                {
                    if (note[i].ColorBG == "CornflowerBlue")
                    {
                        saveNote[counter] = note[i];
                        counter++;
                    }
                }
                for (int i = 0; i < note.Count; i++)
                {
                    if (note[i].ColorBG == "Green")
                    {
                        saveNote[counter] = note[i];
                        counter++;
                    }
                }
                for (int i = 0; i < note.Count; i++)
                {
                    if (note[i].ColorBG == "DarkGoldenrod")
                    {
                        saveNote[counter] = note[i];
                        counter++;
                    }
                }
                for (int i = 0; i < note.Count; i++)
                {
                    if (note[i].ColorBG == "Violet")
                    {
                        saveNote[counter] = note[i];
                        counter++;
                    }
                }
                for (int i = 0; i < note.Count; i++)
                {
                    if (note[i].ColorBG == "DarkRed")
                    {
                        saveNote[counter] = note[i];
                        counter++;
                    }
                }
            
            

        }
      



      
       
    }
}

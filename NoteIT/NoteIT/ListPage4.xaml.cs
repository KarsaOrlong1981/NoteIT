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
        int id;
        bool isNoEntry;
        bool sortByColor;
        bool noteGetEdit;


        public ListPage4(string noteText,string colorBG,bool entry)
        {
            InitializeComponent();


            sortByColor = false;
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
            //Wenn die Notiz leer ist oder nicht neu Angelegt wurde, keinen neuen Eintrag in der
            //Datenbank vornehmen
            if(isNoEntry == true)
            OnAdd(text,newColorBG);
            isNoEntry = false;
        }
        //************************************** Methoden ********************************************
        //Methode zum Eintrag der neuen Notiz in die DatenBank und zur Ausgabe
        //Die methode muss aus der Seite NewNotePage3 aufgerufen werden um die Eingetragenen Daten zu
        //übernehmen.
        async void OnAdd(string txt,string clor)
        {

            var noteAn = new Note
            {
                Text = txt,
                Date = Convert.ToString(date.ToString("F")),
                ColorBG = clor
            };
        
         

         await connection.InsertAsync(noteAn);

            //Die Daten der liste übergeben um sie Sichtbar zu machen
            note.Add(noteAn);

    }

        //Text to Speech 
        private async void TxtSpeech(string text)
        {

            try
            {
                await TextToSpeech.SpeakAsync(text, new SpeechOptions
                {
                    Volume = .75F

                });
            }
            catch (Exception)
            {
                await DisplayAlert("Sorry", "Das gerät unterstützt kein Text-to-Speech", "Verstanden");
            }
        }
        //Methode für alle notizen zu Löschen
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
       
         // Methode zum Löschen einer Notiz
        async void DeleteNote(int value)
        {

            var theNote = note[value];
            await connection.DeleteAsync(theNote);
            note.Remove(theNote);
            if (sortByColor == true)
                WriteSortList();
            
        }
       
      
        //Ermöglicht das Teilen einer Notiz über andere Apps
        async void ShareNote(string message)
        {
          
            await Share.RequestAsync(new ShareTextRequest
            {
              
                Text = message,
                Title = "Share!"
            }) ;
        }
        

       
        //Wird aufgerufen wenn aus der Notizen bestandsliste heraus Neue Notiz hinzufügen gewählt wird
        async void BackTOADD()
        {
            bool emptyBool = false;
            NewNotePage3 send = new NewNotePage3(newColorBG, emptyBool);
            await Navigation.PushAsync(send);
            Navigation.RemovePage(this);
        }
        // Methode die alle Notizen nach farbe sortiert aber nur auf dem Display , nicht in der Datenbank
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
        void ChangeNote()
        {

        }
        void NoteMemory()
        {

        }
        //************************ Ereignisse *************************
        //Wenn Add gewählt wird
        private void ToolbarItemADD_Clicked(object sender, EventArgs e)
        {
            BackTOADD();
        }
        //Wenn alle löschen gewählt wird
        private void ToolbarItemClearAll_Clicked(object sender, EventArgs e)
        {
            AskAllDelete();
        }
       
        //Wenn sortieren nach farbe gewählt wird
        void ToolbarItemColor_Clicked(object sender, EventArgs e)
        {
            sortByColor = true;
            WriteSortList();

            collectionView.ItemsSource = saveNote;
            
        }
        //Wenn eine Notiz angetippt wird
        async void collectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            int counter = 0;
            // wenn eine Notiz gewählt wurde, soll das SelectionChanged Ereignis
            // nicht doppelt ausgelöst werden wenn eine Aktion ausgeführt wurden ist.
            if (collectionView.SelectedItem == null)
                return;
            string action = await DisplayActionSheet("Wähle eine Option:", "Abbrechen", "Notiz Löschen", "Text-to-Speech", "Notiz Bearbeiten", "Erinnerung Aktivieren", "Notiz Teilen");
            if (action == "Abbrechen")
                collectionView.SelectedItem = null;

            if (action == "Text-to-Speech")
            {
                string txt = (e.CurrentSelection.FirstOrDefault() as Note)?.Text;
                TxtSpeech(txt);
                collectionView.SelectedItem = null;
            }
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
                noteGetEdit = true;
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

                text = note[counter].Text;
                newColorBG = note[counter].ColorBG;
                NewNotePage3 send = new NewNotePage3(newColorBG, noteGetEdit);
                send.SendData(text, newColorBG);



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
               
                collectionView.SelectedItem = null;
            }
        }
       
        
    }
}

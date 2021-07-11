using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace NoteIT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewNotePage3 : ContentPage
    {

        string newColor;
        bool editNote;
        public NewNotePage3(string color,bool getEdit)
        {
            InitializeComponent();
            newColor = color;
            editNote = getEdit;
            if (newColor == "DarkRed")
                radio_DarkRed.IsChecked = true;
            if (newColor == "Green")
                radio_Green.IsChecked = true;
            if (newColor == "DarkGoldenrod")
                radio_DGrod.IsChecked = true;
            if (newColor == "Violet")
                radio_Violet.IsChecked = true;
            if (newColor == "CornflowerBlue")
                radio_CFB.IsChecked = true;
            if (newColor == "Gray")
                radio_Gray.IsChecked = true;

            if (editNote == true)

                DisplayAlert("Achtung", "Wenn Sie die Seite verlassen ohne \"Notiz hinzufügen\" zu drücken, \n geht die Notiz für immer verloren", "Verstanden.");

        }
        //********************* Methoden **************************
        // Methode zur übergabe der gewählten Hintergrundfarben und dem Text
        // der Notizen und übergibt die bestätigung das ein Eintrag vorliegt
        async void CallPage4GetEntry(string color)
        {
            bool entRy = true;
            ListPage4 callAndGet = new ListPage4(noteEntry.Text, color, entRy);

            await Navigation.PushAsync(callAndGet);
            Navigation.RemovePage(this);

        }
        //Methode die über ListPage4 aufgerufen wird um die aktuellen Daten der
        //zu Bearbeitenden Notiz zu übergeben.
        public void SendData(string txt, string clor)
        {

            noteEntry.Text = txt;
            newColor = clor;
        }
        //************************* Ereignisse ***************************
        private void btn_ADD_Clicked(object sender, EventArgs e)
        {
            if(noteEntry.Text == null)
            {
                DisplayAlert("Fehlende Eingabe !!!", "Bitte einen Eintrag hinzufügen", "Verstanden.");
            } 
            else
            {
                if (radio_DarkRed.IsChecked == true)
                    newColor = "DarkRed";
                if (radio_Green.IsChecked == true)
                    newColor = "Green";
                if (radio_DGrod.IsChecked == true)
                    newColor = "DarkGoldenrod";
                if (radio_Violet.IsChecked == true)
                    newColor = "Violet";
                if (radio_CFB.IsChecked == true)
                    newColor = "CornflowerBlue";
                if (radio_Gray.IsChecked == true)
                    newColor = "Gray";

                CallPage4GetEntry(newColor);
                noteEntry.Text = "";
            }
           
        }
       

        private void ToolbarItemListe_Clicked(object sender, EventArgs e)
        {
            CallPage4();
        }
        //Die Bestandsliste an notizen Aufrufen und dabei Leere strings übergeben
        //damit kein neuer Datenbank eintrag erstellt wird, dafür wird dann auf der Seite listPage4 gesorgt
        async void CallPage4()
        {
            string none = "";
            bool entry = false;
            ListPage4 call = new ListPage4(none, none,entry);
            await Navigation.PushAsync(call);

        }
    }
}
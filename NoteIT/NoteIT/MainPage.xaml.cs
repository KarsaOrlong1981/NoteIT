using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NoteIT
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        async void CallPage2()
        {
            OptionsPage2 call = new OptionsPage2();
            await Navigation.PushAsync(call);
        }
       

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            
            CallPage2();
        }

        private void ToolbarItem_Clicked_1(object sender, EventArgs e)
        {
           // "Bild von Jess Bailey auf Pixabay"
        }
    }
}
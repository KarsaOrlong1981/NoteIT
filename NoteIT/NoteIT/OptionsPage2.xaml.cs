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
    public partial class OptionsPage2 : ContentPage
    {
        public OptionsPage2()
        {
            InitializeComponent();
        }

        private void btn_Hinzufuegen_Clicked(object sender, EventArgs e)
        {
          
            CallPage3();
        }

        private void btn_Bestand_Clicked(object sender, EventArgs e)
        {
            
            CallPage4();
        }
        async void RemoveThisPage()
        {
            await Navigation.PopAsync();
        }

        async void CallPage3()
        {
            string color = "Black";
            NewNotePage3 call = new NewNotePage3(color);
            await Navigation.PushAsync(call);
        }
        async void CallPage4()
        {
            string none = "";
            bool entry = false;
            ListPage4 call = new ListPage4(none,none,entry);
            await Navigation.PushAsync(call);

        }
    }
}
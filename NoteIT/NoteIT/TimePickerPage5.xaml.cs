using Plugin.LocalNotifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteIT
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TimePickerPage5 : ContentPage
    {
        DateTime newDateTime;
        string memoryTime;
        string memoryDate;
        string cllor;
        string msg;
        public TimePickerPage5(string txt,string color)
        {
            InitializeComponent();
            msg = txt;
            cllor = color;
        }
        async  void CallOnADD(string txt,string color,string memory)
        {
            string none = "";

            bool entry = false;
            ListPage4 call = new ListPage4(none, none, entry);
            await Navigation.PushAsync(call);
            call.OnAdd(txt,color,memory);

        }
        async void CallPage4()
        {
           
            string none = "";
         
            bool entry = false;
            ListPage4 call = new ListPage4(none, none, entry);
            await Navigation.PushAsync(call);
        }
        
        async void NotificationOK()
        {
            string min;
            if (TimePicker24.Time.Minutes == 0)
                min = Convert.ToString(TimePicker24.Time.Minutes) + "0";
            else
                min = Convert.ToString(TimePicker24.Time.Minutes);


            memoryTime = Convert.ToString(TimePicker24.Time.Hours) + ":" + min;
            memoryDate = Convert.ToString(datePicker.Date.Day) + "." + Convert.ToString(datePicker.Date.Month) + "." + Convert.ToString(datePicker.Date.Year);
            string msgMemory = "Erinnerung aktiv:" + memoryDate + "--" + memoryTime + " Uhr";
            string action =  await DisplayActionSheet("Ihre Erinnerung wird am " + memoryDate + " um " + memoryTime + " ausgelöst.", null, null, "OK");
            if (action == "OK")
                CallOnADD(msg, cllor, msgMemory);

        }
        private void buttonAddNotification_Clicked(object sender, EventArgs e)
        {
            string hour = Convert.ToString(TimePicker24.Time.Hours);
            string minutes = Convert.ToString(TimePicker24.Time.Minutes);
            string seconds = Convert.ToString(TimePicker24.Time.Seconds);
            string year = Convert.ToString(datePicker.Date.Year);
            string month = Convert.ToString(datePicker.Date.Month);
            string day = Convert.ToString(datePicker.Date.Day);

            newDateTime = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), Convert.ToInt32(day), Convert.ToInt32(hour), Convert.ToInt32(minutes), Convert.ToInt32(seconds));
           
            CrossLocalNotifications.Current.Show("Ihre Erinnerung an: ", msg, 0, newDateTime);
            NotificationOK();
        }
    }
}
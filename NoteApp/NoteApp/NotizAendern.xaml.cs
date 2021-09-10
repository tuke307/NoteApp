using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NoteApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotizAendern : ContentPage
    {      
        // name das Objektes festlegen 
         Notiz retNotiz;

        public NotizAendern(Notiz selNotiz)
        {   
            // iitialiern der Seite 
            InitializeComponent();

            if (selNotiz == null)
            {
                BtnDelete.IsEnabled = false;
            }
            else
            {
                // Einfügen der Eigenschaften / einfügen in das textfeld und in die Überschrift 
                TextFeld.Text = selNotiz.Beschreibung;
                Ueberschrift.Text = selNotiz.Titel;
            }
        } 


        /// <summary>
        /// Variable damit die abfrage zum Löschen Funktioniert 
        /// </summary>
        bool löschen = true;

        /// <summary>
        /// Wird aufgerufem wenn aufspeicher grdrückt wird.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Speichern_Button(object sender, EventArgs e)
        {
            // neues Objekt erstellen (mit der Notiz klassen)
            retNotiz = new Notiz();

            // Werte werden vom Textfeld und der Überschrift in das Objekt einfügen 
            retNotiz.Titel = Ueberschrift.Text;
            retNotiz.Beschreibung = TextFeld.Text;

            // Trieggern des events damit das objekt au die andere seite geschickt wird 
            OnNotizSave();

            // Zurück zur MainPaige
            this.Navigation.PopToRootAsync();
        }

       
        public delegate void SetNotizSaveEventHandler(object source, Notiz args);
        public event SetNotizSaveEventHandler NotizSave;
        /// <summary>
        /// das event um das objekt auf die andere seit zu schicken.
        /// </summary>
        protected virtual void OnNotizSave()
        {      
                // das Objekt auf die andere seite schicken           
                NotizSave(this, retNotiz); 
            
        }

      
        /// <summary>
        /// wird aufgerufen wenn die Notiz gelöscht werden soll. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void LöschenButton(object sender, EventArgs e)
        {
            // PopUp
            löschen = await DisplayAlert("Sicher?", "Das du Fortfahren willst?", "ja", "Nein");

            // herausfinden ob der Benutzer auf ja oder nein gecklikt hat 
            if (löschen)
            {
                // Triggern das Löschen Events 
                OnNotizDelete(); 

                // zurück auf die adere Seite Naivigieren 
                this.Navigation.PopToRootAsync();
            }
        }

        public delegate void SetNotizDeleteEventHandler(object source);
        public event SetNotizDeleteEventHandler NotizDelete;
        /// <summary>
        /// event zum Löschen der Notiz.
        /// </summary>
        protected virtual void OnNotizDelete()
        {
                // Triggern des Löschen events der anderen Seite 
                NotizDelete(this);          
        }
    }
}
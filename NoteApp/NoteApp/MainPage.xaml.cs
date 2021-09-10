using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 
using Xamarin.Forms;
using System.Xml.Serialization;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace NoteApp
{      
    public partial class MainPage : ContentPage 
    {
        /// <summary>
        /// sagt das die ListView mit den werten der klasse Notiz funktioniert.
        /// </summary>
        ObservableCollection<Notiz> Notizen;

        /// <summary>
        /// Variable um zu wissen welches Item angetipt wurde bzw. bearbeitet werden soll.
        /// </summary>
        int RichtigesItem;

        /// <summary>
        /// Variable zum festlegen ob einen Neue Notiz angelegt wir oder eine bestehende bearbeitet wird .
        /// </summary>
        bool HinzufuegenErsetzen = true;

             /// <summary>
             /// Startmetode wenn eine Neue MainPaige erstellt wird (konstruktor).
             /// </summary>
        public MainPage()
        {
            // initialisieren der komponenten
            this.InitializeComponent();

            // checken ob es schon ein dokument gibt 
            if (!File.Exists(Speicher.FileName))
            {
                // neues Dokument erstellen 
               File.Create(Speicher.FileName);     
            }
            // dokument auslesen 
            string jsonData = System.IO.File.ReadAllText(Speicher.FileName);

            // De-serialize to object or create new list jasn in Notiz datei ändern 
            List<Notiz> notizList = JsonConvert.DeserializeObject<List<Notiz>>(jsonData)
                                    ?? new List<Notiz>();

            // trigger der fragen um Berechtigungen 
            _ = CheckPermissionAsync();

            // sagt das die ListView mit den werten der klasse Notiz funktioniert 
            Notizen = new ObservableCollection<Notiz>(notizList);

            // legt fest das die ListView Itemsource nutzt das heist dann Notizn  
            ListTexte.ItemsSource = Notizen;
           
        }       
        /// <summary>
        /// Frage um Berechtigungen der App.
        /// </summary>
        /// <returns></returns>
        public async Task CheckPermissionAsync()
        {
            // frage für berechtigungen 
            await Permissions.RequestAsync<Permissions.StorageWrite>();
             await Permissions.RequestAsync<Permissions.StorageRead>();
        }
        /// <summary>
        /// event wenn ein Item in der ListView angetipt wird.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ListTexte_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Variable auf true setzen damit eine neue Notiz angelegt wird 
            HinzufuegenErsetzen = false;

            // herausfinden welchs Item angetipt wurden 
            RichtigesItem = e.ItemIndex;

            // neues Objekt anlegen 
            Notiz selNotiz = e.Item as Notiz;

            // Verschickt das Item der Listview an die andere Site 
            NotizAendern BearbeitenSeite = new NotizAendern(selNotiz);

            // event für die andere Seite (Speichern)
            BearbeitenSeite.NotizSave += this.OnNotizSave;

            // event für die andere Seite (Löschen)
            BearbeitenSeite.NotizDelete += this.OnNotizDelete;

            // nawigieren zur bearbeiten Seite 
            await Navigation.PushAsync(BearbeitenSeite, true);

        }
        /// <summary>
        /// event zum Löschen. 
        /// </summary>
        /// <param name="source"></param>
        private void OnNotizDelete(object source)
        {
            // Das richtige Item aus der ListView löschen 
            Notizen.RemoveAt(RichtigesItem);
            DatenSpeichern();
          
        }
                /// <summary>
                /// Datenspeiern 
                /// </summary>
        private void DatenSpeichern()
        {
            // speichert die ganze Listview in den Jasn Ordner 
            string jsonData = JsonConvert.SerializeObject(Notizen);
            System.IO.File.WriteAllText(Speicher.FileName, jsonData);
        }


        /// <summary>
        /// event zu speichern und ersetzen(bearbeiten).
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args">Die Notiz die von der anderen seite wieder gekommen ist</param>
        private void OnNotizSave(object source, Notiz args)
        {
            // Beschreibung darf leer sein, Titel nicht. Deswegen nur Titel überprüft  
            if (!string.IsNullOrEmpty(args.Titel))              
            {
                // herausfinden ob die Variable true oder false ist 
                if (HinzufuegenErsetzen)
                {
                    // true = Neue notiz anlegen 
                    Notizen.Add(args);
                }
                else
                {
                    // fals = bestehende Notiz ersetzen (bearbeiten) 
                    Notizen[RichtigesItem] = args;
                }
                // Update json data string
                DatenSpeichern();
            }
        }
        /// <summary>
        /// wird aufgerufen wenn eine neue notizerstell werden soll.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void NeueNotizButton(object sender, EventArgs e)
        {
            // setzt Variable auf true 
            HinzufuegenErsetzen = true;
         
            // Objekt Verschicken 
            NotizAendern NotizErstellenSeite = new NotizAendern(null);

            // event für die andere seite (Speichern)
            NotizErstellenSeite.NotizSave += this.OnNotizSave;

            // auf die Notiz erstellen Seite leiten 
            await Navigation.PushAsync(NotizErstellenSeite, true);
        }        
    }
}
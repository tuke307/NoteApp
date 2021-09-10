using System;
using System.Collections.Generic;
using System.Text;

namespace NoteApp  // die Klasse 
{
    public class Notiz
    {
        /// <summary>
        /// details zur Notiz.
        /// </summary>
        public string Beschreibung { get; set; }
        /// <summary>
        /// Titel der Notiz.
        /// darf nur ein Wort bzw ein Zeile. 
        /// </summary>
        public string Titel { get; set; }
        public bool IsNullOrEmpty { get; internal set; }

        /// <summary>
        /// Konstruktor zum erstellen einer neuen Notiz.
        /// </summary>
        public Notiz()
        {

        }
    }
  
   
}

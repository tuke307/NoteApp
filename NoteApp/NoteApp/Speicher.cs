using System;
using System.IO;

namespace NoteApp
{
    public static class Speicher
    {
        /// <summary>
        /// datei.
        /// </summary>
        private static string File = "NoteApp.json";

        /// <summary>
        /// dareiname wo es gspeichert werden soll.
        /// </summary>
        public static string FileName =
            Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            File);
    }
}
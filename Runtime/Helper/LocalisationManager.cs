using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JelleVer.Localisation
{
    /// <summary>
    /// Manages the localisation as a static class.
    /// </summary>
    public class LocalisationManager : MonoBehaviour
    {
        private static int currentLanguage = 0;
        private static string currentLanguageString = "";
        private static Dictionary<string, string>[] localisedStrings;
        private static CSVLoader csvLoader;
        private static bool isInit;

        private static void Init()
        {
            csvLoader = new CSVLoader();
            csvLoader.LoadCSV();
            UpdateDictionaries();

            isInit = true;
        }

        /// <summary>
        /// Changes the language to the next in line, looping over every language
        /// </summary>
        /// <returns>The current language string</returns>
        public static string ChangeLanguage()
        {
            if (!isInit) Init();
            currentLanguage = (currentLanguage + 1) % localisedStrings.Length;
            localisedStrings[currentLanguage].TryGetValue("LA", out currentLanguageString);
            return currentLanguageString;
        }

        /// <summary>
        /// Get the current language string
        /// </summary>
        /// <returns>The string of the current active language.</returns>
        public static string GetCurrentLanguage()
        {
            return currentLanguageString;
        }

        /// <summary>
        /// Parses the Localisation csv.
        /// </summary>
        public static void UpdateDictionaries()
        {
            localisedStrings = csvLoader.GetAllDictionaryValues();
            localisedStrings[currentLanguage].TryGetValue("LA", out currentLanguageString);
        }

        /// <summary>
        /// returns a localised value of a given key, throws warning if not found
        /// </summary>
        /// <param name="key"></param>
        /// <returns>the correct value of the current language, returns empty if key is empty</returns>
        public static string GetLocalisedValue(string key)
        {
            if (!isInit) Init();
            if (key == "") return key;
            string value = key;

            localisedStrings[currentLanguage].TryGetValue(key, out value);
            
            if (value == null || value == "")
            {
                value = "NO VAL IN: " + currentLanguageString;
                Debug.LogWarning("The key: <color=white><i>" + key + "</i></color>, has no corresponding value in " + currentLanguageString + ". Please add a value or dubblecheck the key");
            }
            return value;
        }
    }
}
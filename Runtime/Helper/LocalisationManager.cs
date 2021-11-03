using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JelleVer.Localisation
{

    public class LocalisationManager : MonoBehaviour
    {
        private static int currentLanguage = 0;
        private static string currentLanguageString = "";
        private static List<Dictionary<string, string>> localisedStrings = new List<Dictionary<string, string>>();
        public static CSVLoader csvLoader;
        public static bool isInit;

        public static void Init()
        {
            csvLoader = new CSVLoader();
            csvLoader.LoadCSV();
            UpdateDictionaries();

            isInit = true;
        }

        public static void ChangeLanguage()
        {
            currentLanguage = (currentLanguage + 1) % localisedStrings.Count;
            localisedStrings[currentLanguage].TryGetValue("LA", out currentLanguageString);
        }

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

        public static string GetCurrentLanguage()
        {
            return currentLanguageString;
        }

#if UNITY_EDITOR
        public static void Add(string key, string valueNL, string valueEN)
        {
            if (valueNL.Contains("\""))
            {
                valueNL.Replace('"', '\"');
            }
            if (valueEN.Contains("\""))
            {
                valueEN.Replace('"', '\"');
            }

            if (csvLoader == null) csvLoader = new CSVLoader();

            csvLoader.LoadCSV();
            csvLoader.Add(key, valueNL, valueEN);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }

        public static void Edit(string key, string valueNL, string valueEN)
        {
            if (valueNL.Contains("\""))
            {
                valueNL.Replace('"', '\"');
            }
            if (valueEN.Contains("\""))
            {
                valueEN.Replace('"', '\"');
            }

            if (csvLoader == null) csvLoader = new CSVLoader();

            csvLoader.LoadCSV();
            csvLoader.Edit(key, valueNL, valueEN);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }
        public static void Remove(string key)
        {
            if (csvLoader == null) csvLoader = new CSVLoader();

            csvLoader.LoadCSV();
            csvLoader.Remove(key);
            csvLoader.LoadCSV();

            UpdateDictionaries();
        }
#endif

    }
}
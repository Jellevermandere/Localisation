using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

namespace JelleVer.Localisation
{
    //The localisation csv should be located @ "Assets/Resources/localisation.csv"
    public class CSVLoader
    {
        private TextAsset csvFile;
        private char lineSeperator = '\n';
        private char surround = '"';
        private string[] fieldSeperator = { "\",\"" };

        public void LoadCSV()
        {
            csvFile = Resources.Load<TextAsset>("localisation");
        }

        public Dictionary<string, string> GetDictionaryValues(string attrbuteId)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();

            string[] lines = csvFile.text.Split(lineSeperator);
            int attributeIndex = -1;
            string[] headers = lines[0].Split(fieldSeperator, System.StringSplitOptions.None);

            for (int i = 0; i < headers.Length; i++)
            {
                if (headers[i].Contains(attrbuteId))
                {
                    attributeIndex = i;
                    break;
                }
            }

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] fields = CSVParser.Split(line);

                for (int j = 0; j < fields.Length; j++)
                {
                    fields[j] = fields[j].TrimStart(' ', surround);
                    fields[j] = fields[j].TrimEnd(surround);
                }

                if (fields.Length > attributeIndex)
                {
                    var key = fields[0];

                    if (dictionary.ContainsKey(key)) continue;

                    var value = fields[attributeIndex];
                    dictionary.Add(key, value);
                }
            }
            return dictionary;
        }

        public List<Dictionary<string, string>> GetAllDictionaryValues()
        {
            List<Dictionary<string, string>> dictionaryList = new List<Dictionary<string, string>>();

            string[] lines = csvFile.text.Split(lineSeperator);
            string[] headers = lines[0].Split(fieldSeperator, System.StringSplitOptions.None);

            for (int a = 0; a < headers.Length; a++) //go over each language column
            {
                dictionaryList.Add(new Dictionary<string, string>());

                Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

                for (int i = 1; i < lines.Length; i++) //go over each row, skipping the header row
                {
                    string line = lines[i];
                    string[] fields = CSVParser.Split(line);

                    for (int j = 0; j < fields.Length; j++) //go over each word in the row
                    {
                        fields[j] = fields[j].TrimStart(' ', surround);
                        fields[j] = fields[j].TrimEnd(surround);
                    }

                    if (fields.Length > a) // add the value if it's far enough away in the word list
                    {
                        var key = fields[0];

                        if (dictionaryList[a].ContainsKey(key)) continue;

                        var value = fields[a];
                        dictionaryList[a].Add(key, value);
                    }
                }
            }

            return dictionaryList;
        }

#if UNITY_EDITOR
        public void Add(string key, string valueNL, string valueEN)
        {
            string append = string.Format("\n\"" + key + "\",\"" + valueNL + "\",\"" + valueEN + "\"");

            File.AppendAllText("Assets/Resources/localisation.csv", append);
            UnityEditor.AssetDatabase.Refresh();
        }

        public void Remove(string key)
        {
            string[] lines = csvFile.text.Split(lineSeperator);
            string[] keys = new string[lines.Length];

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                keys[i] = line.Split(fieldSeperator, System.StringSplitOptions.None)[0];
            }
            int index = -1;

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].Contains(key))
                {
                    index = i;
                    break;
                }
            }

            if (index > -1)
            {
                string[] newLines;
                newLines = lines.Where(w => w != lines[index]).ToArray();

                string replaced = string.Join(lineSeperator.ToString(), newLines);
                File.WriteAllText("Assets/Resources/localisation.csv", replaced);
            }
        }

        public void Edit(string key, string newValueNL = "", string newValueEN = "")
        {
            Remove(key);
            Add(key, newValueNL, newValueEN);
        }
#endif

    }
}
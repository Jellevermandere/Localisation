using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JelleVer.Localisation
{
    public class TextLocaliser : MonoBehaviour
    {
        [SerializeField]
        private bool UpdateAtStart = true;
        private Text textObject;
        private TMP_Text tmpTextObject;

        private string localKey = "";

        // Start is called before the first frame update
        void Start()
        {
            if (UpdateAtStart) UpdateText();
        }

        /// <summary>
        /// Set the text component to a localised value
        /// </summary>
        /// <param name="key">the localisation key to search for the correct value</param>
        public void UpdateText(string key = "")
        {
            if (!TryGetText())
            {
                Debug.LogWarning(gameObject.name + ": no Text or TMP_Text component attached to this gameobject to localise.");
                return;
            }

            if (localKey != "" && key == "") key = localKey;

            if (key == "")
            {
                if (textObject) key = textObject.text;
                else if (tmpTextObject) key = tmpTextObject.text;
            }

            localKey = key;

            if (textObject) textObject.text = LocalisationManager.GetLocalisedValue(localKey);
            else if (tmpTextObject) tmpTextObject.text = LocalisationManager.GetLocalisedValue(localKey);
        }

        bool TryGetText()
        {
            if (!textObject)
            {
                if (TryGetComponent(out Text text))
                {
                    textObject = text;
                    return true;
                }
                else return false;
            }
            else if (!tmpTextObject)
            {
                if (TryGetComponent(out TMP_Text tmpText))
                {
                    tmpTextObject = tmpText;
                    return true;
                }
                else return false;
            }
            return true;
        }
    }
}
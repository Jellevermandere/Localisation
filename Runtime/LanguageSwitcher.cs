using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JelleVer.Localisation
{
    /// <summary>
    /// Enables you to switch languages and update the scene
    /// </summary>
    public class LanguageSwitcher : MonoBehaviour
    {

        /// <summary>
        /// Change the language to the next in line, also changes all the text values in the scene to match the current language.
        /// </summary>
        public void ChangeLanguage()
        {
            LocalisationManager.ChangeLanguage();

            object[] textLocalisers = FindObjectsOfType(typeof(TextLocaliser));

            foreach (var item in textLocalisers)
            {
                ((TextLocaliser)item).UpdateText();
            }


        }

    }
}

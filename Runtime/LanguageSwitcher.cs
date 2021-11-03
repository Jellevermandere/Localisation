using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace JelleVer.Localisation
{
    public class LanguageSwitcher : MonoBehaviour
    {

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

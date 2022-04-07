using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Auteur : Kusunga Malcom<br>
/// Description : Cette classe permet d'obtenir la valeur sélectionnée par le joueur dans la liste déroulante concernant la langue
/// </summary>
public class LanguageDropdown : MonoBehaviour
{  
    /// <summary>
    /// Liste déroulante des différentes langues
    /// </summary>
    [SerializeField] private TMP_Dropdown dd;

    /// <summary>
    /// Variable contenant la langue sélectionnée
    /// </summary>
    private string language;
    
    /// <summary>
    /// Variable contenant l'indice de la case sélectionnée par le joueur dans la liste déroulante.
    /// </summary>
    private int ddValue;
    
    void Start()
    {   
        //Récupération de la liste déroulante
        TMP_Dropdown dd = GetComponent<TMP_Dropdown>();

        //Mise à jour de la liste déroulante en fonction de la langue de base du jeu
        if(PlayerPrefs.GetString("Language").CompareTo("French") == 0)
        {
            dd.ClearOptions();
            List<string> options = new List<string> {"French", "English"};
            dd.AddOptions(options);
        }
    }

    void Update()
    {   
        //Récupération de l'indice de la case de la liste déroulante sélectionnée par le joueur
        ddValue = dd.value;
        //Récupération de la valeur présente dans la case sélectionnée par le joueur dans la liste déroulante 
        language = dd.options[ddValue].text;
    }

    /// <summary>
    /// Renvoi la valeur sélectionnée par le joueur dans la liste déroulante.
    /// </summary>
    /// <returns>
    /// La valeur entière sélectionnée par le joueur.
    /// </returns>
    public string getLanguage(){

        return language;
    }
}
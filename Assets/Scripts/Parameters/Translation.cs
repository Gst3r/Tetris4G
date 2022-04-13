using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary> 
/// Auteur : Kusunga Malcom<br>
/// Description : Cette classe contient l'ensemble des methodes necessaires a la traduction du jeu.
/// </summary>
public sealed class Translation : MonoBehaviour
{   
    /// <summary> 
    /// Tableau contenant les differentes langues possibles
    /// </summary>
    public static readonly SystemLanguage[] Languages = { SystemLanguage.English, SystemLanguage.French };

    /// <summary> 
    /// Dictionnaire contenant l'ensemble des traductions
    /// </summary>
    private static Dictionary<string, string> Translations = null;

    /// <summary> 
    /// Liste déroulante contenant les différentes langues
    /// </summary>
    [SerializeField] private LanguageDropdown ddLang;

    /// <summary> 
    /// Chaine de caracteres contenant la langue actuelle du jeu
    /// </summary>
    private string actualLanguage;

    /// <summary> 
    /// Chaine de caracteres contenant la langue du jeu avant un possible changement dans la liste deroulante
    /// </summary>
    private string previousLanguage;

    /// <summary> 
    /// Attribut contenant le nom d'un fichier contenant une traduction
    /// </summary>
    private static SystemLanguage language;


    private void Start()
    {
        // Chargement de la langue du jeu
        InitializeLanguage();
    }

    private void Update()
    {
        // Verifie si un changement du parametre langue a lieu
        CheckLanguage();
    }

    /// <summary>
    /// Methode permettant d'initialiser le jeu dans une certaine langue
    /// </summary>
    private void InitializeLanguage()
    {   
        // Vefifie Si le joueur a deja defini des parametres pour la langue
        if(PlayerPrefs.HasKey("Language"))
        {
            // Recuperation de la langue selectionnee par le joueur
            actualLanguage = PlayerPrefs.GetString("Language");
            
            // Selection du fichier de traduction
            if(actualLanguage.CompareTo("French") == 0)
            {
                language = SystemLanguage.French;
            }else{
                language = SystemLanguage.English;
            }

        }else{
            // Selection de l'anglais comme langue de base
            actualLanguage = "English";
            language = SystemLanguage.English;
            PlayerPrefs.SetString("Language","English");
        }
        
        previousLanguage = actualLanguage;
    }

    /// <summary>
    /// Methode permettant de verifier si le joueur selectionne une nouvelle langue
    /// </summary>
    private void CheckLanguage()
    {   
        if(ddLang.getLanguage() != null)
        {   
            // Recuperation de la langue selectionnee dans la liste deroulante
            actualLanguage = ddLang.getLanguage();

            // Verification que la langue selectionnee est differente de la langue actuelle
            if(actualLanguage.CompareTo(previousLanguage) != 0)
            {
                // Selection du fichier de traduction
                if(actualLanguage.CompareTo("French") == 0)
                {
                    PlayerPrefs.SetString("Language","French");
                    language = SystemLanguage.French;
                }else{
                    PlayerPrefs.SetString("Language","English");
                    language = SystemLanguage.English;
                }

                previousLanguage = actualLanguage;

                // Mise a jour du dictionnaire contenant une traduction pour l'ensemble des mots
                Translations = null;
                LoadDictionnary();
            }
        }
    }

    /// <summary> 
    /// Auteur: Kusunga Malcom
    /// Methode qui permet de recuperer le dictionnaire afin d'effectuer la traduction
    /// </summary>
    private static void LoadDictionnary()
    {
        // Verifie si le dictionnaire a deja ete initialise
        if (Translations != null)
            return;

        // Creation du dictionnaire
        Translations = new Dictionary<string, string>(); 
   
        // Recuperation du fichier contenant la traduction dans une certaine langue
        var data = Resources.Load<TextAsset>($"Translations/{language}");

        // Lecture du fichier contenant les traductions
        if (data != null)
            ParseFile(data.text);
    }

    /// <summary> 
    /// Auteur: Kusunga Malcom
    /// Methode qui retourne la traduction d'un mot
    /// </summary>
    /// <param name="key">
    /// Le mot dont pour lequel la traduction est renvoyée
    /// </param>
    public static string Get(string key)
    {   
        //Vérifie que la dictionnaire est initialisé
        LoadDictionnary();

        //Récupération de la traduction
        if (Translations.ContainsKey(key))
            return Translations[key];

        //Renvoi le mot dans le cas le mot serait le meme entre les deux differentes langues
        return key;
    }

    /// <summary> 
    /// Auteur: Kusunga Malcom
    /// Methode qui lit le fichier contenant l'ensemble des tractuctions pour une langue
    /// </summary>
    /// <param name="data">
    /// Le fichier contenant la traduction pour une langue
    /// </param>
    public static void ParseFile(string data)
    {
        using (var stream = new StringReader(data))
        {   
            //Lecture de la premiere ligne
            var line = stream.ReadLine();

            //Tableau contenant un mot et sa traduction
            var wordsTab = new string[2];

            //Mot et sa traduction
            var key = string.Empty;
            var value = string.Empty;

            //Lecture du fichier
            while (line != null)
            {   
                //Saute les lignes correspondant à des commentaires
                if (line.StartsWith(";") || line.StartsWith("["))
                {
                    line = stream.ReadLine();
                    continue;
                }

                //Recuperation du mot et de sa traduction
                wordsTab = line.Split('=');

                if (wordsTab.Length == 2)
                {   
                    //Recuperation du mot 
                    key = wordsTab[0].Trim();

                    //Recuperation de sa traduction
                    value = wordsTab[1].Trim();

                    if (value == string.Empty)
                        continue;
                    
                    //Enregistrement de la traduction dans le dictionnaire
                    if (Translations.ContainsKey(key))
                        Translations[key] = value;
                    else
                        Translations.Add(key, value);
                }

                //Lecture de la nouvelle ligne
                line = stream.ReadLine();
            }
        }
    }
}
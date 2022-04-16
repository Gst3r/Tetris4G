using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;

/// <summary> 
/// Auteur : Kusunga Malcom<br>
/// Description : Cette classe contient l'ensemble des méthodes necessaires à la traduction du jeu.
/// </summary>
public sealed class Translation : MonoBehaviour
{   
    /// <summary> 
    /// Tableau contenant les differentes langues possibles
    /// </summary>
    public static readonly SystemLanguage[] Languages = { SystemLanguage.English, SystemLanguage.French, SystemLanguage.Korean};

    /// <summary> 
    /// Dictionnaire contenant l'ensemble des traductions
    /// </summary>
    private static Dictionary<string, string> Translations = null;

    /// <summary> 
    /// Liste déroulante contenant les différentes langues
    /// </summary>
    [SerializeField] private LanguageDropdown ddLang;

    /// <summary> 
    /// Chaîne de caractères contenant la langue actuelle du jeu
    /// </summary>
    private string actualLanguage;

    /// <summary> 
    /// Chaîne de caractères contenant la langue du jeu avant un possible changement dans la liste deroulante
    /// </summary>
    private string previousLanguage;

    /// <summary> 
    /// Attribut contenant le nom d'un fichier contenant une traduction
    /// </summary>
    private static SystemLanguage language;

    /// <summary> 
    /// Booleen permettant de définir si le dictionnaire contenant les traductions est initialisé
    /// </summary>
    private static bool isInitialize = false;

    private void Start()
    {
        // Chargement de la langue du jeu
        InitializeLanguage();
    }

    private void Update()
    {
        // Vérifie si un changement du paramètre langue a lieu
        CheckLanguage();
    }

    /// <summary>
    /// Méthode permettant d'initialiser le jeu dans une certaine langue
    /// </summary>
    private void InitializeLanguage()
    {   
        // Véfifie Si le joueur a déjà défini des paramètres pour la langue
        if(PlayerPrefs.HasKey("Language"))
        {
            // Récupération de la langue selectionnée par le joueur
            actualLanguage = PlayerPrefs.GetString("Language");

            // Sélection du fichier de traduction
            if(actualLanguage.CompareTo("French") == 0)
            {
                language = SystemLanguage.French;
            }else if (actualLanguage.CompareTo("English") == 0){
                language = SystemLanguage.English;
            }else{
                language = SystemLanguage.Korean;
            }
        }else{
            // Sélection de la langue en fonction de la langue du téléphone de l'utilisateur
            // Si la langue est disponible dans l'application elle sera selectionnée sinon le jeu sera en anglais
            if (Array.IndexOf<SystemLanguage>(Languages, Application.systemLanguage) == -1)
            {
                language = SystemLanguage.English;
                actualLanguage = "English";
                PlayerPrefs.SetString("Language","English");
            }else{
                language = Languages[Array.IndexOf<SystemLanguage>(Languages, Application.systemLanguage)];
                //Sélection selon la langue détectée
                switch(language)
                {
                    case SystemLanguage.English:actualLanguage = "English";
                                                PlayerPrefs.SetString("Language","English");
                                                break;
                    case SystemLanguage.French:actualLanguage = "French";
                                                PlayerPrefs.SetString("Language","French");
                                                break;
                    case SystemLanguage.Korean:actualLanguage = "Korean";
                                                PlayerPrefs.SetString("Language","Korean");
                                                break;
                }
            }
        }

        previousLanguage = actualLanguage;
        isInitialize = true;
    }

    /// <summary>
    /// Méthode permettant de vérifier si le joueur sélectionne une nouvelle langue
    /// </summary>
    private void CheckLanguage()
    {   
        if(ddLang.getLanguage() != null)
        {   
            // Récuperation de la langue selectionnée dans la liste deroulante
            actualLanguage = ddLang.getLanguage();

            // Verification que la langue selectionnée est differente de la langue actuelle
            if(actualLanguage.CompareTo(previousLanguage) != 0)
            {
                // Sélection du fichier de traduction
                if(actualLanguage.CompareTo("French") == 0)
                {
                    PlayerPrefs.SetString("Language","French");
                    language = SystemLanguage.French;
                }else if(actualLanguage.CompareTo("English") == 0){
                    PlayerPrefs.SetString("Language","English");
                    language = SystemLanguage.English;
                }else{
                    PlayerPrefs.SetString("Language","Korean");
                    language = SystemLanguage.Korean;
                }

                previousLanguage = actualLanguage;

                // Mise à jour du dictionnaire contenant une traduction pour l'ensemble des mots
                Translations = null;
                LoadDictionnary();
            }
        }
    }

    /// <summary> 
    /// Auteur: Kusunga Malcom
    /// Méthode qui permet de récupérer le dictionnaire afin d'effectuer la traduction
    /// </summary>
    private static void LoadDictionnary()
    {
        if(isInitialize){
            // Vérifie si le dictionnaire à déjà été initialisé
            if (Translations != null)
                return;

            // Création du dictionnaire
            Translations = new Dictionary<string, string>();

            // Récuperation du fichier contenant la traduction dans une certaine langue
            var data = Resources.Load<TextAsset>($"Translations/{language}");

            // Lecture du fichier contenant les traductions
            if (data != null)
                ParseFile(data.text);
        }
    }

    /// <summary> 
    /// Auteur: Kusunga Malcom
    /// Méthode qui retourne la traduction d'un mot
    /// </summary>
    /// <param name="key">
    /// Le mot dont pour lequel la traduction est renvoyée
    /// </param>
    public static string Get(string key)
    {   
        if(isInitialize){
            // Vérifie que la dictionnaire est initialisé
            LoadDictionnary();

            // Récupération de la traduction
            if (Translations.ContainsKey(key))
                return Translations[key];

        }

        // Renvoi le mot dans le cas le mot serait le même entre les deux differentes langues
        return key;
    }

    /// <summary> 
    /// Auteur: Kusunga Malcom
    /// Méthode qui lit le fichier contenant l'ensemble des tractuctions pour une langue
    /// </summary>
    /// <param name="data">
    /// Le fichier contenant la traduction pour une langue
    /// </param>
    public static void ParseFile(string data)
    {
        using (var stream = new StringReader(data))
        {   
            //Lecture de la première ligne
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

                //Récupération du mot et de sa traduction
                wordsTab = line.Split('=');

                if (wordsTab.Length == 2)
                {   
                    //Récupération du mot 
                    key = wordsTab[0].Trim();

                    //Récupération de sa traduction
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
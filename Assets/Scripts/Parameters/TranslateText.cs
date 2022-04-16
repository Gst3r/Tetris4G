using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary> 
/// Auteur : Kusunga Malcom<br>
/// Description : Cette classe permet de traduire l'ensemble des textes présents dans le jeu.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public sealed class TranslateText : MonoBehaviour
{
    /// <summary> 
    /// Attribut contenant un texte qui sera traduit si la langue change
    /// </summary>
    private TMP_Text text;

    private void Start()
    {
        //Récupération du texte
        text = GetComponent<TMP_Text>();

        //Traduction du texte
        text.text = Translation.Get(text.text);
    }

    private void Update()
    {
        //Traduction du texte en fonction de la langue selectionnée par le joueur
        text.text = Translation.Get(text.text);
    }
}
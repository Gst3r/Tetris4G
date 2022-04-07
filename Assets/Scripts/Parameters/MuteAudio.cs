using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Auteur : Kusunga Malcom<br>
/// Description : Cette classe permet la gestion du son du jeu a travers l'utilisation d'un toggle
/// </summary>
public class MuteAudio : MonoBehaviour
{
    /// <summary> 
    /// Attribut contenant le Toggle permettant de couper le son du jeu
    /// </summary>
    [SerializeField] private GameObject toggle;

    private void Start()
    {
        //Recuperation du Toggle Audio present dans l'interface des parametres
        Toggle audioToggle = GetComponent<Toggle>();

        //Verifie si des parametres ont ete definis
        if(PlayerPrefs.HasKey("Audio"))
        {
            AudioListener.volume = PlayerPrefs.GetInt("Audio");
        }else{
            AudioListener.volume = 1;
        }

        //Mise a jour visuel du Toggle en fonction de l'audio
        if(toggle.activeInHierarchy)
        {   
            if(AudioListener.volume == 0)
                audioToggle.isOn = false;
        } 
    }

    /// <summary>
    /// Methode permettant de modfier l'audio du jeu
    /// </summary>
    /// <param name="unMuted">
    /// Booleen definissant si l'on coupe ou active l'audio
    /// </param>
    public void ToggleAudioOnValueChanged(bool unMuted)
    {   
        //Activation de l'audio
        if(unMuted)
        {
            AudioListener.volume = 1;
            PlayerPrefs.SetInt("Audio",1);
        }
        //Desactivation de l'audio
        else
        {
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("Audio",0);
        }
    }
}
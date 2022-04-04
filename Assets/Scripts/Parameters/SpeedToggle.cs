using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Auteur : Kusunga Malcom<br>
/// Description : Cette classe permet la gestion de l'accélération de la gravité
/// </summary>
public class SpeedToggle : MonoBehaviour
{
    private void Start()
    {
        //Recuperation du Toggle Speed present dans l'interface de pause
        Toggle speedToggle = GetComponent<Toggle>();

        //Vérifie si l'augmentation de la gravité est bloquée
        if(BoardManager.GetLockSpeed())
        {
            speedToggle.isOn = true;
        }
    }

    /// <summary>
    /// Methode permettant de bloquer l'augmentation de la gravité
    /// </summary>
    /// <param name="unlock">
    /// Booleen definissant si l'on bloque ou active l'augmentation de la gravité
    /// </param>
    public void ToggleSpeedOnValueChanged(bool lockS)
    {   
        //Bloque l'augmentation de la gravité 
        if(lockS)
        {
            BoardManager.SetLockSpeed(true);
        }
        //Active l'augmentation de la gravité
        else
        {
            BoardManager.SetLockSpeed(false);
        }
    }
}
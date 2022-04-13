using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auteur : Sterlingot Guillaume
/// Description : Cette classe permet d'initialiser le panel des objectifs et de le remplir au lancement du jeu 
/// </summary>
public class GoalsInitialize : MonoBehaviour
{

    /// <summary>
    /// Attribut contenant le script de gestion des objectifs côté interface 
    /// </summary>
    [SerializeField] private GoalsUI goalsUI;

    // Start is called before the first frame update
    void Start()
    {
        goalsUI.InitializeElements();
        goalsUI.FillPanel();
    }
}

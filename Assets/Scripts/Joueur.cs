using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Auteur : Seghir Nassima<br>
/// Description : Cette classe permet de gérér le profil d'un joueur 
/// </summary>

[System.Serializable]
public class Joueur : MonoBehaviour
{
    /// <summary> 
    /// Attribut contenant le pseudonyme du joueur 
    /// </summary>
    public string name;


    /// <summary> 
    /// Attribut contenant le plus haut score atteint par le joueur  
    /// </summary>
    public float highscore;


    /// <summary> 
    /// Constructeur de l'objet Joueur   
    /// </summary>
    public Joueur(string name, float highscore)
    {
        this.name=name; 
        this.highscore=highscore; 

    }




  









    

   
}

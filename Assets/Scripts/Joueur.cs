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
    private string pseudo;


    /// <summary> 
    /// Attribut contenant le plus haut score atteint par le joueur  
    /// </summary>
    private float highScore;


    /// <summary> 
    /// Constructeur de l'objet Joueur   
    /// </summary>
    public Joueur(string pseudo, float highScore)
    {
        this.pseudo=pseudo; 
        this.highScore=highScore; 

    }




    /// <summary> 
    /// Méthode qui enregistre le score du joueur 
    /// Auteur:Seghir Nassima  
    /// </summary>
     private void storeScore(Joueur joueur)
    { 




    }









    

   
}

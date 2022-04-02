using System;
using System.Collections.Generic;

/// <summary> 
/// Auteur : Nassima Seghir<br>
/// Description : Cette classe permet de stocker la liste des scores 
/// </summary>


[Serializable]
public class ScoreData
{
    /// <summary> 
    /// Attribut contenant la liste des scores des joueurs qui sera chargée en début de partie 
    /// </summary>
    public List<Joueur> scores;

    /// <summary> 
    /// Constructeur de la classe, permet d'initialiser la liste des scores des joueurs 
    /// </summary>
    public ScoreData()
    {
        scores= new List<Joueur>();
    }
}
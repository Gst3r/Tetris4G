using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary> 
/// Auteur : Nassima Seghir<br>
/// Description : Cette classe permet de gérer les scores du jeu  
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public ScoreManager() { }


      
    /// <summary> 
    /// Attribut contenant une référence de la liste des scores 
    /// </summary>
    private ScoreData sd ; 
    void Awake()
    { 
        int a= PlayerPrefs.GetInt("entier"); 
        Debug.Log("Awake"); 
        Debug.Log(a); 
        //chargement du json 
        var json = PlayerPrefs.GetString("scores", "{}"); 
        Debug.Log(json);
        //initialisation de la liste des scores à partir du json 
        sd = JsonUtility.FromJson<ScoreData>(json); //a rajouter apres
       // sd= new ScoreData(); 
       
    }
   

    
    /// <summary> 
    /// Méthode qui retourne la liste des scores dans l'ordre decroissant 
    /// Auteur:Seghir Nassima  
    /// </summary>
    public IEnumerable<Joueur> GetHighScores()
    {
        return sd.scores.OrderByDescending(x => x.highscore);
    }

    /// <summary> 
    /// Méthode qui ajoute un score à la liste de score 
    /// Auteur: Seghir Nassima  
    /// </summary>
    public void AddScore(Joueur score)
    {
       
        sd.scores.Add(score);
     
    }

    private void OnDestroy()
    {
       Debug.Log("destruction"); 
       SaveScore();
    }

    /// <summary> 
    /// Méthode qui permet l'enregitrement du score 
    /// Auteur: Seghir Nassima  
    /// </summary>
    public void SaveScore()
    {
       
        var json = JsonUtility.ToJson(sd);
        Debug.Log(json);
        PlayerPrefs.SetString("scores", json);
        PlayerPrefs.Save(); 
    }
}
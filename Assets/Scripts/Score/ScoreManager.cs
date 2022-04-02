using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 
/// Auteur : Nassima Seghir<br>
/// Description : Cette classe permet de gérer les scores du jeu  
/// </summary>
public class ScoreManager : MonoBehaviour
{

    /// <summary> 
    /// Attribut contenant le score de la partie en cours 
    /// </summary>
    [SerializeField] private static float score=0;

    /// <summary> 
    /// Attribut contenant le score de la partie en cours sous forme de texte
    /// </summary>
    [SerializeField] private Text scoreText;

    public ScoreManager() { }
      
    /// <summary> 
    /// Attribut contenant une référence de la liste des scores 
    /// </summary>
    private ScoreData sd ; 


   


   /* void Awake()
    { 
       
        //chargement du json 
        var json = PlayerPrefs.GetString("scores", "{}"); 
        Debug.Log(json);
        //initialisation de la liste des scores à partir du json 
        sd = JsonUtility.FromJson<ScoreData>(json); 
    }*/

 
    /// <summary> 
    /// Méthode qui permet de charger les scores
    /// Auteur:Seghir Nassima  
    /// </summary>
    public ScoreManager Loading(String key)
    {
       
        //chargement du json    
        var json = PlayerPrefs.GetString(key, "{}"); 
        Debug.Log(json);
        //initialisation de la liste des scores à partir du json 
        this.sd = JsonUtility.FromJson<ScoreData>(json);
        return this; 
    }
   
    /// <summary>
    /// Auteur : Seghir Nassima 
    /// Méthode permettant d'incrémenter le score selon le nombre de lignes éliminées
    /// </summary>
    public void IncrementScore(int nmbLines)
    {
        switch(nmbLines)
        {
            case 1: 
                score+=40; 
                break; 

            case 2:
                score+=100; 
                break; 

            case 3: 
                score+=300; 
                break; 

            case 4:
                score+=1200; 
                break; 

            default: 
                 break;  
        }
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

    /// <summary> 
    /// Méthode qui permet de changer l'affichage du score sur l'interface
    /// Auteur: Sterlingot Guillaume
    /// </summary>
    public void ChangeScore()
    {
        scoreText.text= score.ToString(); //permet de mettre à jour le score affiché 
    }
     

    /// <summary> 
    /// Méthode qui permet l'enregitrement du score 
    /// Auteur: Seghir Nassima  
    /// </summary>
    public void SaveScore(string key)
    {
       
        var json = JsonUtility.ToJson(sd);
        Debug.Log(json);
        PlayerPrefs.SetString(key, json);
        
    }
    /// <summary> 
    /// getter de l'objet sd
    /// </summary>
    public ScoreData GetScoreData()
    {
        return sd; 
    }  
}
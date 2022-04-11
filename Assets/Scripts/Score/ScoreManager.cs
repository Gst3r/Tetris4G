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
    [SerializeField] private static int score;
    
    /// <summary> 
    /// Attribut contenant le noombre de ligne détruite de la partie en cours 
    /// </summary>
    [SerializeField] private static int nbLines;

    /// <summary> 
    /// Attribut contenant le score de la partie en cours sous forme de texte
    /// </summary>
    [SerializeField] private Text scoreTextInGame;

    /// <summary> 
    /// Attribut contenant le score de la partie en cours sous forme de texte
    /// </summary>
    [SerializeField] private Text scoreTextEndGame;
      
    /// <summary> 
    /// Attribut contenant une référence de la liste des scores 
    /// </summary>
    private ScoreData sd ; 

    public ScoreManager(){}

    private void Start(){
        score = 0;
        nbLines = 0;
    }
 
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
    /// Auteur : Seghir Nassima, Bae Jin-Young
    /// Méthode permettant d'incrémenter le score selon le nombre de lignes éliminées
    /// </summary>
    public void IncrementScore(int nmbLines, bool isBonus, bool isMalus)
    {
        if (!isBonus && !isMalus)
        {
            switch (nmbLines)
            {
                case -1:
                    score += -40;
                    break;

                case 1:
                    score += 40;
                    break;

                case 2:
                    score += 100;
                    break;

                case 3:
                    score += 300;
                    break;

                case 4:
                    score += 1200;
                    break;

                default:
                    break;
            }
        }
        
        if (isBonus)
        {
            switch (nmbLines)
            {

                case 1:
                    score += 80;
                    break;

                case 2:
                    score += 200;
                    break;

                case 3:
                    score += 600;
                    break;

                case 4:
                    score += 2400;
                    break;

                default:
                    break;
            }
        }

        if (isMalus)
        {
            switch (nmbLines)
            {

                case 1:
                    score -= 40;
                    break;

                case 2:
                    score -= 100;
                    break;

                case 3:
                    score -= 300;
                    break;

                case 4:
                    score -= 1200;
                    break;

                default:
                    break;
            }
        }

        ChangeScore();
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
    /// Auteur: Seghir Nassima
    /// </summary>
    public void ChangeScore()
    {
        scoreTextInGame.text= score.ToString(); //permet de mettre à jour le score affiché en jeu 
        scoreTextEndGame.text= score.ToString(); //permet de mettre à jour le score affiché à la fin de la partie
    
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

    /// <summary> 
    /// getter du score
    /// </summary>
    public static int GetScore()
    {
        return score; 
    } 
    
    /// <summary> 
    /// getter du nomre de ligne
    /// </summary>
    public static int GetNbLines()
    {
        return nbLines; 
    } 
}
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
    [SerializeField] private static int score = 0;

    /// <summary> 
    /// Attribut contenant le noombre de ligne détruite de la partie en cours 
    /// </summary>
    [SerializeField] private static int nbLines = 0;

    /// <summary> 
    /// Attribut contenant le noombre de ligne détruite de la partie en cours 
    /// </summary>
    [SerializeField] private static int time = 0;

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

    private void Start(){
        score = 0;
        nbLines = 0;
        time = 0;
    }
 
    /// <summary> 
    /// Méthode qui permet de charger les scores
    /// Auteur:Seghir Nassima  
    /// </summary>
    public ScoreManager Loading(String key)
    {
       
        //chargement du json    
        var json = PlayerPrefs.GetString(key, "{}"); 
        //initialisation de la liste des scores à partir du json 
        this.sd = JsonUtility.FromJson<ScoreData>(json);
        return this; 
    }

    /// <summary>
    /// Auteur : Seghir Nassima, Bae Jin-Young
    /// Méthode permettant d'incrémenter le score selon le nombre de lignes éliminées
    /// </summary>
    /// <param name="isBonus">
    /// Booleen mis a TRUE si les lignes sont de nature "bonus"
    /// </param>
    /// <param name="isCol">
    /// Booleen mis a TRUE si la ligne complété est une colonne, FALSE sinon
    /// </param>
    /// <param name="isMalus">
    /// Booleen mis a TRUE si les lignes sont de nature "malus"
    /// </param>
    /// <param name="nmbLines">
    /// Le nombre de lignes supprimées
    /// </param>
    public void IncrementScore(int nmbLines, bool isCol, bool isBonus, bool isMalus)
    {
        int scoreAdding = isCol?80:40; // Le score ajouté est 80 si le joueur a complété une colonne, 40 sinon

          // si les lignes sont standards
          if (!isBonus && !isMalus)
        {
            switch (nmbLines)
            {
                case 1:
                    score += scoreAdding;
                    break;
                case 2:
                    score += scoreAdding*2+20;
                    break;
                case 3:
                    score += scoreAdding*7+20;
                    break;
                case 4:
                    score += scoreAdding*30;
                    break;
                default:
                    break;
            }
        }

          //si les lignes sont bonus, doubles des points standards
         if (isBonus)
        {
            switch (nmbLines)
            {
                case 1:
                    score += scoreAdding*2;
                    break;
                case 2:
                    score += scoreAdding*4+20;
                    break;
                case 3:
                    score += scoreAdding*14+20;
                    break;
                case 4:
                    score += scoreAdding*60;
                    break;
                default:
                    break;
            }
        }

         //si les lignes sont malus, le negatifs de points standards
         if (isMalus)
        {
            switch (nmbLines)
            {
                case 1:
                    score -= scoreAdding;
                    break;
                case 2:
                    score -= scoreAdding*2+20;
                    break;
                case 3:
                    score -= scoreAdding*7+20;
                    break;
                case 4:
                    score -= scoreAdding*30;
                    break;
                default:
                    break;
            }
        }

        //le score est mis a jour
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
        //Debug.Log(json);
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
    /// getter du nombre de ligne
    /// </summary>
    public static int GetNbLines()
    {
        return nbLines; 
    } 

    /// <summary> 
    /// getter du temps écoulé
    /// </summary>
    public static int GetTime()
    {
        return nbLines; 
    }

    /// <summary> 
    /// setter du score
    /// </summary>
    public static void SetScore(int value)
    {
        score = value; 
    } 

    /// <summary> 
    /// setter du nombre de ligne
    /// </summary>
    public static void SetNbLines(int value)
    {
        nbLines = value; 
    } 

    /// <summary> 
    /// setter du temps écoulé
    /// </summary>
    public static void SetTime(int value)
    {
        time = value; 
    }
}
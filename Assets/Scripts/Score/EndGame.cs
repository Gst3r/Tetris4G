using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI; 
using TMPro; 

/// <summary> 
/// Auteur : Nassima Seghir<br>
/// Description : Cette classe permet de gérer les options du menu de fin de jeu 
/// </summary>
public class EndGame : MonoBehaviour
{
    /// <summary> 
    /// Attribut faisant référence à la zone de saisie du menu de fin de jeu 
    /// </summary>
    [SerializeField] private TMP_InputField inputField; 

    /// <summary> 
    /// Attribut faisant référence au text d'affichage du score
    /// </summary>
    [SerializeField] private TextMeshProUGUI textScore; 


    /// <summary> 
    /// Attribut qui permettra de modifier la liste des scores
    /// </summary>
    private ScoreManager scoreManager; 

    private void Awake()
    {
      
        scoreManager= new ScoreManager(); 
        
        //initialisation des composants 
        inputField= transform.Find("InputFieldPseudo").GetComponent<TMP_InputField>();
        textScore= transform.Find("TextScore").GetComponent<TextMeshProUGUI>();

        //affichage du score
        textScore.text = "Score: " + ScoreManager.GetScore().ToString();

      

    }

    /// <summary> 
    /// Méthode qui s'execute lors d'un clique sur le bouton submit 
    /// Auteur:Seghir Nassima
    /// </summary>
    public void Submit()
    {
         switch(ModeController.GetMode()){ 
            case Mode.MARATHON:
              
               scoreManager.Loading("scores"); 

               break; 
            case Mode.SPRINT:
               
                scoreManager.Loading("sprint");

               break; 
            case Mode.ULTRA:
                scoreManager.Loading("ultra"); 
               break; 
            default:    
                Debug.Log("t'as pris marathon");
               break; 
        }


        //On commence par charger la liste des scores: 
        //scoreManager.Loading("scores"); 

        //la variable list va contenir la liste des Joueur 
        List<Joueur> list= scoreManager.GetScoreData().scores; 

        bool changed= false; 

        foreach (Joueur joueur in list.ToList())
        {
            //si jamais le joueur introduit un pseudo déja present dans la liste
            //et que le nouveau highscore est meilleur, on écrase son dernier score 
            if(joueur.name == inputField.text)
            {
                if(joueur.highscore<BoardManager.GetScore())
                {
                    joueur.highscore=BoardManager.GetScore(); 
                    scoreManager.GetScoreData().scores=list; 
                    Debug.Log("It already exists");
                    changed = true; 
                    break; 
                }
            }
           
           

        } if(!changed) //si il s'agit d'un nouveau pseudo, on crée une nouvelle entrée     
                {    
                  
                    Debug.Log("première occurence."); 
                    scoreManager.AddScore(new Joueur(name:inputField.text, highscore:BoardManager.GetScore()));

                }

        //on termine par enregistrer les modifications : 
        //scoreManager.SaveScore("scores"); 

         switch(ModeController.GetMode()){ 
            case Mode.MARATHON:
               Debug.Log("t'as pris marathon");
               scoreManager.SaveScore("scores"); ; 

               break; 
            case Mode.SPRINT:
                Debug.Log("t'as pris sprint");
                scoreManager.SaveScore("sprint"); 
                
               break; 
            case Mode.ULTRA:
                 Debug.Log("t'as pris ultra");
               break; 
            default:    
                Debug.Log("t'as pris marathon");
               break; 
        }


     
    }

}

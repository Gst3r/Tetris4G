using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI; 
using TMPro; 







/// <summary> 
/// Auteur : Nassima Seghir<br>
/// Description : Cette classe permet de gérer lesoptions du menu de fin de jeu 
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

   

     

   


  
    


    private void Awake()
    {
       
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
        
        Scoring.pseudo[Scoring.i]=(inputField.text);
        Scoring.score[Scoring.i]=ScoreManager.GetScore();
        Scoring.i++; 
        
      
       
     
     


       //Scoring.pseudo=inputField.text;
       //Scoring.score=BoardManager.getScore(); 
       
       

      

      
       

       

       

        



        
        



    }

}

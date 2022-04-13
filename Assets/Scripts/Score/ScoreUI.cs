using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary> 
/// Auteur : Nassima Seghir<br>
/// Description : Cette classe permet de générer les lignes du tableau de scores   
/// </summary>
public class ScoreUI : MonoBehaviour
{
    /// <summary> 
    /// Attribut faisant référence à une ligne du tableau des scores 
    /// </summary>
    public RowUI rowUi;

    /// <summary> 
    /// Attribut servant à modifier le tableau des scores
    /// </summary>
    public ScoreManager scoreManager; 

   

    void Start()
    {
        
        scoreManager = new ScoreManager();
       
       
        //chargement de la liste des scores 
        scoreManager.Loading("marathonn");  
       

        //partie qui permet de modifier l'interface et ajouter des lignes dans le tableau des scores
        var scores = scoreManager.GetHighScores().ToArray();
        int max=0; 

        if(scores.Length>10)
        {
            max=10; 
        }
        else{
            max=scores.Length; 
        }
        for (int i = 0; i < max; i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.name.text = scores[i].name;
            row.score.text = scores[i].highscore.ToString();
        }
    }


  
       
        
}

   

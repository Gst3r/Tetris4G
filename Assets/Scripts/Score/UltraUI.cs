using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary> 
/// Auteur : Nassima Seghir<br>
/// Description : Cette classe permet de générer les lignes du tableau de scores   
/// </summary>
public class UltraUI : MonoBehaviour
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
        //chargement de la liste des scores 
        scoreManager.Loading("ultra");  

        //partie qui permet de modifier l'interface et ajouter des lignes dans le tableau des scores
        var scores = scoreManager.GetHighScores().ToArray();
        for (int i = 0; i < scores.Length; i++)
        {
            var row = Instantiate(rowUi, transform).GetComponent<RowUI>();
            row.rank.text = (i + 1).ToString();
            row.name.text = scores[i].name;
            row.score.text = scores[i].highscore.ToString();
        }
    }

   
}
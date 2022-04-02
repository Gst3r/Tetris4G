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

    public Mode mode; 

   /*void Update()
   {
        switch(mode){ 
            case Mode.MARATHON:
               Debug.Log("t'as pris marathon");
               mode=Mode.MARATHON; 
               scoreManager.Loading("scores");
               break; 
            case Mode.SPRINT:
                Debug.Log("t'as pris sprint");
                mode=Mode.SPRINT; 
                scoreManager.Loading("sprint");
               

               break; 
            case Mode.ULTRA:
                 Debug.Log("t'as pris ultra");
                 mode=Mode.ULTRA; 
               break; 
            default:    
                Debug.Log("t'as pris marathon");
                scoreManager.Loading("scores");
               break; 
        }
   }*/

    
    
    void Start()
    {
        

       
       
        //chargement de la liste des scores 
        scoreManager.Loading("sprint");  
       
       /* Debug.Log("La clé est de "); 
        Debug.Log(PlayerPrefs.GetInt("key"));
        if(PlayerPrefs.GetInt("key")==0)
        {
             scoreManager.Loading("scores");
        }
        if(PlayerPrefs.GetInt("key")==1){
             scoreManager.Loading("sprint");  
        }*/
       

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

    public void HandleInputData(int val)
    {
        if(val==0)
        {
           
           

            

        }
        if(val==1)
        {
           
           
        }
        if(val==2)
        {
           
           
        }
       
        
    }

   
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI; 
using TMPro; 
using System.IO; 

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
    [SerializeField] private Text scoreValue; 





    /// <summary> 
    /// Attribut qui permettra de modifier la liste des scores
    /// </summary>
    private ScoreManager scoreManager; 


    /// <summary> 
    /// Attribut contenant le message affiché lors du partage de la capture du score 
    /// </summary>
    private string shareMessage; 








    private void Awake()
    {
      
        scoreManager= new ScoreManager(); 
        
        //initialisation des composants 
        inputField= transform.Find("InputFieldPseudo").GetComponent<TMP_InputField>();
       

        //affichage du score
        scoreValue.text=" "+ScoreManager.GetScore().ToString();

      

    }

    /// <summary> 
    /// Méthode qui s'execute lors d'un clique sur le bouton submit 
    /// Auteur:Seghir Nassima
    /// </summary>
    public void Submit()
    {
        //on commence par load la liste des scores selon le mode:
         switch(ModeController.GetMode()){ 
            case Mode.MARATHON:
                scoreManager.Loading("marathonn"); 
                break; 
            case Mode.SPRINT:  
                scoreManager.Loading("sprint");
                break; 
            case Mode.ULTRA:
                scoreManager.Loading("ultra"); 
                break; 
            default:    
                scoreManager.Loading("marathon"); 
                break; 
        }

        //la variable list va contenir la liste des Joueur 
        List<Joueur> list= scoreManager.GetScoreData().scores; 

        bool changed= false; 

        foreach (Joueur joueur in list.ToList())
        {
            Debug.Log(inputField.text); 
            //si jamais le joueur introduit un pseudo déja present dans la liste
            //et que le nouveau highscore est meilleur, on écrase son dernier score 
            if(joueur.name == inputField.text)
            {
               
                if(joueur.highscore<ScoreManager.GetScore())
                {
                    joueur.highscore=ScoreManager.GetScore(); 
                    scoreManager.GetScoreData().scores=list; 
                   
                    changed = true; 
                    break; 
                }
                changed=true; 
            }
           
           

        } if(!changed) //si il s'agit d'un nouveau pseudo, on crée une nouvelle entrée     
                {    
                  
                   
                    scoreManager.AddScore(new Joueur(name:inputField.text, highscore:ScoreManager.GetScore()));

                }

        //on termine par enregistrer les modifications : 
        //scoreManager.SaveScore("scores"); 

         switch(ModeController.GetMode()){ 
            case Mode.MARATHON:
                scoreManager.SaveScore("marathonn"); 
                break; 
              
            case Mode.SPRINT:
                scoreManager.SaveScore("sprint"); 
                break;
               
            case Mode.ULTRA:
                scoreManager.SaveScore("ultra"); 
                break;    
            
            default:    
                scoreManager.SaveScore("marathon");
                break; 
        }
    }

    /// <summary> 
    /// Méthode qui s'execute lors d'un clique sur le bouton share 
    /// Auteur:Seghir Nassima
    /// </summary>
    public void share()
    {
        shareMessage= "I can't believe I scored "+ ScoreManager.GetScore().ToString()+" points in Tetris4G!!!";

        StartCoroutine(TakeScreenshotAndShare());


    }



    /// <summary> 
    /// Méthode qui permet d'effectuer une capture d'écran 
    /// Auteur:Seghir Nassima
    /// </summary>
    private IEnumerator TakeScreenshotAndShare()
{
	yield return new WaitForEndOfFrame();

	Texture2D ss = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
	ss.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0 );
	ss.Apply();

	string filePath = Path.Combine( Application.temporaryCachePath, "shared img.png" );
	File.WriteAllBytes( filePath, ss.EncodeToPNG() );

	// To avoid memory leaks
	Destroy( ss );

	new NativeShare().AddFile( filePath )
		.SetSubject( "Tetris4G" ).SetText(shareMessage)
		.SetCallback( ( result, shareTarget ) => Debug.Log( "Share result: " + result + ", selected app: " + shareTarget ) )
		.Share();

	
}

}

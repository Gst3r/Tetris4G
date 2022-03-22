using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Auteur : Sterlingot Guillaume, Kusunga Malcom, Bae Jin-Young, Seghir Nassima<br>
/// Descrption : Cette classe permet la gestion des interrations liées aux différentes actions du joueur.
/// </summary>
public class Controller : MonoBehaviour
{

    /// <summary> 
    /// Attribut contenant le plateau de jeu 
    /// </summary>
    [SerializeField] private BoardManager board;
    
    /// <summary> 
    /// Attribut contenant la pièce courante qui est présente sur le plateau de jeu 
    /// </summary>
    [SerializeField] private Piece activePiece;

    public Vector2 startPos;
    
    public Vector2 direction;

    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public GameObject endGamePanel;

    /// <summary> 
    /// Booléen indiquant si le Game Over a été detecter
    /// </summary>
    private bool gameIsOver;

    private void Start() {
        //cette commande permet de reprendre la progression normale du temps
        Time.timeScale=0f;
        LaunchCount();

        gameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);
            bool stayOnScreen = false;
            Debug.Log(touch.pressure);
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    if(touch.pressure>200){
                        stayOnScreen=true;
                        Shift(touch);
                    }else{
                        stayOnScreen=false;
                    }
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    if(!stayOnScreen)
                        activePiece.Rotate();
                    break;
            }
        }  

        checkGameOver(); 
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de choisir automatiquement le déplacement adapté selon la gravité (fonctionnalités tactile)
    /// </summary>
    public void Shift(Touch touch){
        /*switch(board.GetGravity()){
            case Gravity.HAUT:  if(){

                                }else{

                                }
                                 break;
            case Gravity.BAS:   if(){

                                }else{

                                }
                                break;
            case Gravity.GAUCHE:if(){

                                }else{

                                }
                                break;
            case Gravity.DROITE:if(){

                                }else{

                                }
                                break;
            default:break;
        }*/  
    }

    /// <summary> 
    /// Méthode qui permet de vérifier si la partie est perdue 
    /// Auteur: Malcom Kusunga
    /// </summary>
    private void checkGameOver(){
        if(!gameIsOver){
            if(board.GameOver()){
                gameIsOver = true;
                //Arrêt du temps lors de l'ouverture de l'interface de fin de jeu 
                Time.timeScale=0f;
                endGamePanel.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Variable contenant l'animateur lié aux animations exécutées lors de l'appui sur un bouton.
    /// </summary>
    public Animator animator;
    /// <summary>
    /// Variable contenant la durée de l'animation s'activant lors d'un chargement vers la grille de jeu.
    /// </summary>
    public float launchTime = 2.35f;
    //Interface du décompte
    public GameObject countPanel;

    /// <summary>
    /// Méthodes permettant de lancer le decompte lors d'un retour vers l'interface de la grille de jeu
    /// </summary>
    public void LaunchCount(){
        StartCoroutine(LoadCount());
    }

    /// <summary>
    /// Coroutine lié au lancement de l'animation lors du décompte
    /// </summary>
    /// <returns>
    /// Génere une pause de 2.35 secondes.
    /// </returns>
    IEnumerator LoadCount()
    {
        //Démarage de l'animation
        countPanel.SetActive(true);
        
        //Génération de la pause
        float ms = Time.unscaledDeltaTime;
        while(ms <= launchTime){
            ms += Time.unscaledDeltaTime;
            yield return null;
        }
        
        //Fermeture de l'interface du décompte
        countPanel.SetActive(false);

        //cette commande permet de reprendre la progression normale du temps
        Time.timeScale=1f;
    }
}
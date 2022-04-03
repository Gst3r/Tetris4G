using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Auteur : Sterlingot Guillaume, Kusunga Malcom, Bae Jin-Young, Seghir Nassima<br>
/// Description : Cette classe permet la gestion des interrations liées aux différentes actions du joueur.
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

    //-----------------------------------------FIN DE JEU--------------------------------------------------------

    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public GameObject endGamePanel;

    //-----------------------------------------COMPTEUR--------------------------------------------------------

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

    //-----------------------------------------MODE DE JEU--------------------------------------------------------

    /// <summary>
    /// Interface permettant d'utiliser la méthode qui lance le mode de jeu peut importe le mode choisit
    /// </summary>
    private IMode mode;

    /// <summary>
    /// Attribut permettant de différencier les différents modes de jeu
    /// </summary>
    private Mode gameMode;

    //-------------------------------------------------------------------------------------------------------------

    /// <summary> 
    /// Vecteur de nombre réel à deux dimensions qui enregistre la position de départ du doigt lorsqu'il entre en contact avec l'écran
    /// </summary>
    private Vector2 startPos;

    /// <summary> 
    /// Booléen indiquant TRUE si le joueur cherche à tourner le tetromino (analyse du comportement du doigt sur l'écran), FALSE sinon
    /// </summary>
    private static bool wantToRotate;

    /// <summary> 
    /// Un nombre réel qui indique une échelle de vitesse du doigt qui se déplace sur l'écran
    /// </summary>
    private float fast;

    private void Start() {
        SetController();
        LaunchCount();
        wantToRotate = true;
        this.fast = 0f;
        mode.StartExecute();
        /*this.gameIsOver = false;
        this.direction = new Vector2Int();
        this.prevDirection = new Vector2Int();
        this.startPos = new Vector2();*/
    }

    // Update is called once per frame
    void Update()
    {
        mode.Execute();
        touchSensitive();
        board.Clear(activePiece);
        board.Set(activePiece);
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant de paramétrer certains attributs de la classe "Controller"
    /// </summary>
    public void SetController(){
        Time.timeScale=0f; //Cette commande permet de reprendre la progression normale du temps
        gameMode = ModeController.GetMode();

        // Initialisation du mode de jeu 
        switch(gameMode){ // On récupère le mode de jeu qui a été paramétré dans la classe Select mode de la scène "Menu Principale"
            case Mode.MARATHON:
                mode = GetComponent<MarathonManager>(); // On récupère le script associé au mode de jeu MARATHON présent dans le script qui contient le controller (le GameManager)
                break;
            case Mode.SPRINT:
                mode = GetComponent<SprintManager>(); // Même chose mais dans le cas du mode de jeu SPRINT
                break;
            case Mode.ULTRA:
                mode = GetComponent<UltraManager>(); // Même chose mais dans le cas du mode de jeu ULTRA
                break;
            default:    
                mode = GetComponent<MarathonManager>(); // En cas d'erreur de transmission, le script lancé automatiquement est celui des règles classique du Tetris4G  
                break;
        }
    }

    /// <summary>
    /// Méthodes permettant de lancer le decompte lors d'un retour vers l'interface de la grille de jeu
    /// </summary>
    public void LaunchCount(){
        StartCoroutine(LoadCount());
    }

    /// <summary>
    /// Auteur : Kusunga Malcom<br>
    /// Description : Coroutine lié au lancement de l'animation lors du décompte
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

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode condensant les fonctionnalités tactiles
    /// </summary>
    public void touchSensitive(){
        if(!PauseMenu.GetGameIsPausing()&&!IMode.GetGameIsOver()){
            touchSensitiveRotate();
            touchSensitiveShift();
        }
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de choisir automatiquement le déplacement adapté selon la gravité (fonctionnalités tactile)
    /// </summary>
    public void touchSensitiveShift(){
        if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);
            
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Stationary:
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    Vector2Int diff = Vector2Int.FloorToInt(touch.position - startPos);
                    float compareFast = Mathf.Abs(touch.deltaPosition.x);
                    if(compareFast>19)
                        fast = 19;
                    else if(compareFast>17)
                        fast = 17;
                    else if(compareFast>15)
                        fast = 15;
                    else if(compareFast>13)
                        fast = 13;
                    else if(compareFast>11)
                        fast = 11;
                    else if(compareFast>9)
                        fast = 9;
                    else if(compareFast>7)
                        fast = 7;
                    else
                        fast = 3;

                    Debug.Log(fast);

                    if(touch.deltaPosition.x>0){
                        if(Time.frameCount%(21-Mathf.Abs((int)fast))==0){
                            activePiece.RightShift();
                        }
                    }else{
                        if(Time.frameCount%(21-Mathf.Abs((int)fast))==0){
                            activePiece.LeftShift();
                        }
                    }
                    //Shift(direction, touch);
                    wantToRotate=false;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    break;
            }
        }else
            wantToRotate=true;
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de tourner le tetromino courant (fonctionnalités tactile)
    /// </summary>
    public void touchSensitiveRotate(){
        if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);
            
            if(touch.phase.Equals(TouchPhase.Ended)&&wantToRotate){
                activePiece.Rotate();
            }
        }
    }

/*
    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de choisir automatiquement le déplacement adapté selon la gravité (fonctionnalités tactile)
    /// </summary>
    public void Shift(Vector2Int direction, Touch touch){
        switch(board.GetGravity()){
            case Gravity.HAUT:  if(touch.altitudeAngle==0f)
                                    activePiece.Move(new Vector2Int(direction.x, 0));
                                else 
                                    activePiece.Move(new Vector2Int(0, direction.y));
                                break;
            case Gravity.BAS:   if(touch.altitudeAngle==0f)
                                    activePiece.Move(new Vector2Int(direction.x, 0));
                                else 
                                    activePiece.Move(new Vector2Int(0, direction.y));
                                break;
            case Gravity.GAUCHE:if(touch.altitudeAngle==0f)
                                    activePiece.Move(new Vector2Int(direction.x, 0));
                                else 
                                    activePiece.Move(new Vector2Int(0, direction.y));
                                break;
            case Gravity.DROITE:if(touch.altitudeAngle==0f)
                                    activePiece.Move(new Vector2Int(direction.x, 0));
                                else 
                                    activePiece.Move(new Vector2Int(0, direction.y));
                                break;
            default:break;
        }
    }*/

    public Piece GetActivePiece(){
        return activePiece;
    }

    public BoardManager GetBoard(){
        return board;
    }

    public static void SetWantToRotate(bool wantToRot){
        wantToRotate = wantToRot;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Auteur : Sterlingot Guillaume, Kusunga Malcom, Bae Jin-Young, Seghir Nassima<br>
/// Description : Cette classe permet la gestion des interrations liées aux différentes actions du joueur.
/// </summary>
public class Controller : MonoBehaviour
{
    //-----------------------------------------IN GAME----------------------------------------------------------
    /// <summary> 
    /// Attribut contenant le plateau de jeu 
    /// </summary>
    [SerializeField] private BoardManager board;
    
    /// <summary> 
    /// Attribut contenant la pièce courante qui est présente sur le plateau de jeu 
    /// </summary>
    [SerializeField] private Piece activePiece;

    //-----------------------------------------END GAME-------------------------------------------------------

    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public GameObject endGamePanel;

    //-----------------------------------------COUNT--------------------------------------------------------

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

    //-----------------------------------------GAME MODE---------------------------------------------------------------------

    /// <summary>
    /// Interface permettant d'utiliser la méthode qui lance le mode de jeu peut importe le mode choisit
    /// </summary>
    private IMode mode;

    /// <summary>
    /// Attribut permettant de différencier les différents modes de jeu
    /// </summary>
    private static Mode gameMode;

    //------------------------------------------TOUCH SENSITIVE------------------------------------------------------------

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

    /// <summary> 
    /// Booléen qui indique TRUE si le joueur reste appuyé sur l'écran après qu'une pièce ce soit posé, sinon FALSE
    /// </summary>
    private static bool stayOnScreen;

    //----------------------------------------------GOALS--------------------------------------------------------------------

    /// <summary> 
    /// Attribut contenant le gestionnaire des objectifs et donc toutes les méthodes qui permettent de le faire
    /// </summary>
    [SerializeField] private GoalsManager goalsManager;

    //---------------------------------------------------------------------------------------------------------------------

    private void Start() {
        SetController();
        LaunchCount();
        mode.StartExecute();
        /*this.gameIsOver = false;*/
    }

    // Update is called once per frame
    void Update()
    {
        board.Set(activePiece);
        mode.Execute();
        goalsManager.GoalsController();
        touchSensitive();
        board.Clear(activePiece);
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant de paramétrer certains attributs de la classe "Controller"
    /// </summary>
    public void SetController(){
        Time.timeScale=0f; //Cette commande permet de reprendre la progression normale du temps
        gameMode = ModeController.GetMode();
        wantToRotate = true;
        stayOnScreen = false;
        this.fast = 0f;

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
        PauseMenu.SetGameIsPausing(false);
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
                    stayOnScreen=false;
                    break;

                case TouchPhase.Stationary:
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    if(!stayOnScreen){
                        Shift(touch);
                    }
                    wantToRotate=false;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    activePiece.RestoreGravity();
                    stayOnScreen=false;
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


    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de choisir automatiquement le déplacement adapté selon la gravité (fonctionnalités tactile)
    /// </summary>
    public void Shift(Touch touch){
        int compareFastX = (int)Mathf.Abs(touch.deltaPosition.x);
        int fastX = FindFast(compareFastX);

        int compareFastY = (int)Mathf.Abs(touch.deltaPosition.y);
        int fastY = FindFast(compareFastY);

        switch(board.GetGravity()){
            case Gravity.HAUT: 
                fast = fastX;
                if(touch.deltaPosition.y>0)
                    activePiece.ModifyGravityT();
                else if(touch.deltaPosition.x>0 && Time.frameCount%(20-fast)==0)
                    activePiece.RightShift();
                else if(touch.deltaPosition.x<0 && Time.frameCount%(20-fast)==0)
                    activePiece.LeftShift();
                break;
            case Gravity.BAS:
                fast = fastX;             
                if(touch.deltaPosition.y<0)
                    activePiece.ModifyGravityB();      
                else if(touch.deltaPosition.x>0 && Time.frameCount%(20-fast)==0)
                    activePiece.RightShift();
                else if(touch.deltaPosition.x<0 && Time.frameCount%(20-fast)==0)
                    activePiece.LeftShift();
                break;
            case Gravity.GAUCHE:
                fast = fastY;
                if(touch.deltaPosition.y>0 && Time.frameCount%(18-fast)==0)
                    activePiece.TopShift();
                else if(Time.frameCount%(18-fast)==0)
                    activePiece.BotShift();
                break;
            case Gravity.DROITE:
                fast = fastY;
                if(touch.deltaPosition.y>0 && Time.frameCount%(18-fast)==0)
                    activePiece.TopShift();
                else if(Time.frameCount%(18-fast)==0)
                    activePiece.BotShift();
                break;
            default:break;
        }
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de déterminer une vitesse pour le tactile
    /// </summary>
    public int FindFast(int compareFast){
        int localFast;

        if(compareFast>19)
            localFast = 19;
        else if(compareFast>17)
            localFast = 17;
        else if(compareFast>15)
            localFast = 15;
        else if(compareFast>13)
            localFast = 13;
        else if(compareFast>11)
            localFast = 11;
        else if(compareFast>9)
            localFast= 9;
        else if(compareFast>7)
            localFast = 7;
        else
            localFast = 3;

        return localFast;
    }

    public Piece GetActivePiece(){
        return activePiece;
    }

    public BoardManager GetBoard(){
        return board;
    }

    public static Mode GetGameMode(){
        return gameMode;    
    }

    public static void SetWantToRotate(bool wantToRot){
        wantToRotate = wantToRot;
    }

    public static void SetStayOnScreen(bool value){
        stayOnScreen=value;
    }
}
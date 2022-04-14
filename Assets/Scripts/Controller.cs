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
    private void Update()
    {
        if (activePiece.GetPouvoir() == Pouvoir.Standard)
        {
            board.Set(activePiece, activePiece.GetData().tile);
        }
        else if (activePiece.GetPouvoir() == Pouvoir.Malus)
        {
            board.Set(activePiece, activePiece.GetData().malus_tile);
        }
        else
        {
            board.Set(activePiece, activePiece.GetData().bonus_tile);
        }

        mode.Execute();
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant de paramétrer certains attributs de la classe "Controller"
    /// </summary>
    public void SetController(){
        gameMode = MenuController.GetMode();
        wantToRotate = true;
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
        Time.timeScale=0f;
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
        ButtonManager.SetGameIsLoad(true);
        PauseMenu.SetGameIsPausing(false);
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
}
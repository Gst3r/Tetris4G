using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TutorialManager : MonoBehaviour
{

//------------------------------GAME---------------------------------------------------
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject[] showPanel;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject hidePanel;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject explainPanel;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject goalPanel;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject continueButtonAsGameObject;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private TMP_Text goalText;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private Button continueButton;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private TMP_Text explainText;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject coverPauseMenuPanel;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject coverHoldPanel;

//-------------------------------PAUSE------------------------------------------------------
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject explainPausePanel;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private TMP_Text explainMenuPauseText;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject continuePauseButtonAsGameObject;   
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private TMP_Text pauseGoalText; 
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private GameObject pauseGoalPanel;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField]private Button pauseMenuButton;
    
    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    [SerializeField] private GameObject endGamePanel;

//-------------------------------------MANAGEMENT AND SCHEDULING---------------------------------------------------
    
    [SerializeField]private BoardManager board;

    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public static bool canTouchSensitive=true;

    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public static bool explainPrev=true;

    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public static int startUpdate = 0;

//----------------------------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        SetTutorial();
        StartCoroutine(TutoCoroutine(0.7f, delegate{LaunchTutorial();}));
    }

    void Update(){
        CheckEndTutorial();
        CheckTouchSensitiveTuto();    
        CheckGoalIsDone();           
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de vérifier si le joueur perd la partie avant de réussir tout les objectifs ou non
    /// </summary>
    public void CheckEndTutorial(){
        if(ScoreManager.GetScore()==0 && !board.GetBotIsFull())
            BoardManager.SetGravity(Gravity.BAS);
        else if(ScoreManager.GetScore()==0 && board.GetBotIsFull() && startUpdate!=0){
            startUpdate=0;
            ExplainEndGame();
        }else if(ScoreManager.GetScore()==40 && !board.GetTopIsFull()){
            BoardManager.SetGravity(Gravity.HAUT);
        }else if(ScoreManager.GetScore()==40 && board.GetTopIsFull() && !board.GetBotIsFull()){
            BoardManager.SetGravity(Gravity.BAS);
        }else if(ScoreManager.GetScore()==40 && board.GetTopIsFull() && board.GetBotIsFull() && startUpdate!=0){
            startUpdate=0;
            ExplainEndGame();
        }else if(ScoreManager.GetScore()==80 && board.GetTopIsFull() && board.GetTopIsFull() && board.GetRightIsFull() && board.GetLeftIsFull() && startUpdate!=0){
            startUpdate=0;
            ExplainEndGame();
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de gérer les évenements du tuto en fonction des interactions tactile du joueur
    /// </summary>
    public void CheckTouchSensitiveTuto(){
         if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);
            
            switch (touch.phase){
                // Record initial touch position.
                case TouchPhase.Began:
                    break;

                case TouchPhase.Stationary:
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    TouchSensitive.SetWantToRotate(false);
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    if(startUpdate==-1)
                        startUpdate=2;
                    break;
            }

            CheckRotatePiece(touch);
            CheckShiftPiece(touch);
        }else
            TouchSensitive.SetWantToRotate(true);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de vérifier si un objectif parmis les 3 à faire pendant le tuto est remplit ou non
    /// </summary>
    public void CheckGoalIsDone(){
        if(startUpdate==3 && ScoreManager.GetScore()>=40){
            startUpdate=0;
            StartCoroutine(TutoCoroutine(0.4f,delegate{Explain2Direction();}));
        }else if(startUpdate==4 && ScoreManager.GetScore()>=80){
            startUpdate=0;
            StartCoroutine(TutoCoroutine(0.4f,delegate{Explain4Direction();}));
        }else if(startUpdate==5 && ScoreManager.GetScore()>=120){
            startUpdate=0;
            ExplainEndGame();
        }   
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si le joueur à bien réalisé une rotation du tetromino pour le tuto et lance une procédure lié à l'enchaînement d'action du tuto 
    /// </summary>
    /// <param name="touch">
    /// un attribut permettant de manipuler les informations relatifs aux mouvements du doigts sur le dispositif Android
    /// </param>
    public void CheckRotatePiece(Touch touch){
        //Précondition déterminant si on peut déclencher les fonctionnalités tactiles pour la rotation de la pièce
        if(touch.phase==TouchPhase.Ended && startUpdate==1 && TouchSensitive.GetWantToRotate()){
            explainPanel.SetActive(false);
            hidePanel.SetActive(false);
            goalPanel.SetActive(false);
            showPanel[1].SetActive(false);
            RestartGame();
            startUpdate=0;
            StartCoroutine(TutoCoroutine(1,delegate{TakeItShift();}));
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si le joueur à bien réalisé un déplacement du tetromino pour le tuto et lance une procédure lié à l'enchaînement d'action du tuto 
    /// </summary>
    /// <param name="touch">
    /// un attribut permettant de manipuler les informations relatifs aux mouvements du doigts sur le dispositif Android
    /// </param>
    public void CheckShiftPiece(Touch touch){
        //Précondition déterminant si on peut déclencher les fonctionnalités tactiles pour le déplacement de la pièce
        if(touch.phase==TouchPhase.Moved && startUpdate==2 && !TouchSensitive.GetWantToRotate()){
            explainPanel.SetActive(false);
            hidePanel.SetActive(false);
            goalPanel.SetActive(false);
            showPanel[2].SetActive(false);
            RestartGame();
            startUpdate=0;
            StartCoroutine(TutoCoroutine(1,delegate{TakeItHold();}));
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet d'initialiser les interfaces qui apparaissent au lancement du tutoriel
    /// </summary>
    public void SetTutorial(){
        StopGame();
        explainPrev=true;
        explainPanel.SetActive(false);
        hidePanel.SetActive(false);
        goalPanel.SetActive(false);
        explainPausePanel.SetActive(false);
        coverPauseMenuPanel.SetActive(true);
        coverHoldPanel.SetActive(true);
        pauseGoalPanel.SetActive(false);
        for(int i=0;i<showPanel.Length;i++){
            showPanel[i].SetActive(false);
        }

    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de lancer le tutoriel
    /// </summary>
    public void LaunchTutorial(){
        StopGame();
        ExplainBase();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est la première explication du jeu
    /// </summary>
    public void ExplainBase(){
        continueButton.GetComponent<Button>().onClick.AddListener(() => {
            explainPanel.SetActive(false);
            showPanel[0].SetActive(false);
            hidePanel.SetActive(false);
            RestartGame();
            canTouchSensitive=false;
            StartCoroutine(TutoCoroutine(1, delegate {TakeItRotation();}));
        });
        explainText.text = "This Tetromino appear from this gate at the center ! Gravity will be applied on him and the piece just will move downward... We need to destroy him, but the only mean to get away the piece is to complete a row or column with other piece. I hope that other pieces will past throught the gate.";
        explainPanel.SetActive(true);
        hidePanel.SetActive(true);
        coverPauseMenuPanel.SetActive(true);
        coverHoldPanel.SetActive(true);
        showPanel[0].SetActive(true);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est la première rotation du jeu
    /// </summary>
    public void TakeItRotation(){
        StopGame();
        goalText.text = "Touch 1 time the screen"; 
        explainText.text = "If the tetromino's sens doesn't match with the environment and the choosen strategy, you can rotate him at 90 degrees with one presure on the screen. To turn him at 180 degrees, you need to touch the screen two times and at 270 degrees, three times. The tetromino back to his first position with four pressure on the screen.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        hidePanel.SetActive(true);
        goalPanel.SetActive(true);
        showPanel[1].SetActive(true);
        startUpdate=1;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est le premier déplacement du jeu
    /// </summary>
    public void TakeItShift(){
        StopGame();
        goalText.text = "Slide your finger on the screen"; 
        explainText.text = "You masteries the environment according with the gravity so you can move the tetromino right or left by sliding your finger to the wished direction.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        hidePanel.SetActive(true);
        goalPanel.SetActive(true);
        showPanel[2].SetActive(true);
        if(Input.touchCount>0)
            startUpdate=-1;
        else
            startUpdate=2;
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est la première utilisation de la fonctionnalité hold du jeu
    /// </summary>
    public void TakeItHold(){
        StopGame();
        goalText.text = "Touch the hold zone";
        explainText.text = "The piece can be hold in a storage zone and after can be swap with the the active piece on the board. This option add a management of the active piece.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        coverHoldPanel.SetActive(false);
        showPanel[3].SetActive(true);
        goalPanel.SetActive(true);
        hidePanel.SetActive(true);
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est l'explication de la zone de prévisualisation des pièces du jeu
    /// </summary>
    public void ExplainPrevisualisation(){
        if(explainPrev){
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() => {
                explainPanel.SetActive(false);
                showPanel[4].SetActive(false);
                hidePanel.SetActive(false);
                canTouchSensitive=false;
                ExplainScore();
            });
            explainText.text = "This zone show three next pieces which will spawn on the board in the read sens. Your strategy can be adaptated with next pieces.";
            explainPanel.SetActive(true);
            continueButtonAsGameObject.SetActive(true);
            showPanel[3].SetActive(false);
            showPanel[4].SetActive(true);
            goalPanel.SetActive(false);
            explainPrev=false;
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est une explication du score du jeu
    /// </summary>
    public void ExplainScore(){
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => {
            explainPanel.SetActive(false);
            showPanel[5].SetActive(false);
            hidePanel.SetActive(false);
            canTouchSensitive=false;
            ExplainPauseMenu();
        });
        explainText.text = "The game score is displayed here.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(true);
        showPanel[5].SetActive(true);
        coverPauseMenuPanel.SetActive(true);
        coverHoldPanel.SetActive(true);
        hidePanel.SetActive(true);
        goalPanel.SetActive(false);
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est une explication du menu pause du jeu
    /// </summary>
    public void ExplainPauseMenu(){
        pauseMenuButton.onClick.AddListener(() => {
            showPanel[6].SetActive(false);
            pauseGoalText.text = "Press one of the menu's button";
            explainMenuPauseText.text = "From the pause menu, you can continue the tutorial, restart the tutorial or back to main menu to play a real game. You have also an option which allow you to stop the tetrominoes speed.";
            hidePanel.SetActive(false);
            explainPausePanel.SetActive(true);
            explainPanel.SetActive(false);
            continuePauseButtonAsGameObject.SetActive(false);
            goalPanel.SetActive(false);
            coverHoldPanel.SetActive(false);
            pauseGoalPanel.SetActive(true);
            canTouchSensitive=true;
        });

        goalText.text = "Touch the pause menu icon";
        explainText.text = "This icon represents the pause menu.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        showPanel[6].SetActive(true);
        coverPauseMenuPanel.SetActive(false);
        hidePanel.SetActive(true);
        goalPanel.SetActive(true);
    }       

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet d'attribuer au bouton continuer du menu pause la capacité de reprendre les objectifs en cours
    /// </summary>
    public void ContinueGoal(){
        if(startUpdate==3)
            Goal1();
        else if(startUpdate==4)
            Goal2();
        else if(startUpdate==5)
            Goal3();
        else 
            Goal1();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est le premier objectif du jeu
    /// </summary>
    public void Goal1(){
        goalText.text = "Make one complete line";
        hidePanel.SetActive(false);
        explainPausePanel.SetActive(false);
        explainPanel.SetActive(false);
        continuePauseButtonAsGameObject.SetActive(false);
        goalPanel.SetActive(true);
        pauseGoalPanel.SetActive(false);
        coverHoldPanel.SetActive(false);
        startUpdate=3;
        RestartGame();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui explique au joueur qu'il existe en faîte 2 gravités dans le jeu
    /// </summary>
    public void Explain2Direction(){
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => {
            explainPanel.SetActive(false);
            canTouchSensitive=true;
            Time.timeScale=1f;
            StartCoroutine(TutoCoroutine(0.4f,delegate{Goal2();}));
        });
        StopGame();
        explainText.text = "Our analyse tell us that pieces can also spawn in a different gravity... The gravity can be to the top or to the bottom.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(true);
        goalPanel.SetActive(false);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est le deuxieme objectif du jeu
    /// </summary>
    public void Goal2(){
        goalText.text = "Make another complete line";
        hidePanel.SetActive(false);
        explainPausePanel.SetActive(false);
        explainPanel.SetActive(false);
        continuePauseButtonAsGameObject.SetActive(false);
        goalPanel.SetActive(true);
        pauseGoalPanel.SetActive(false);
        startUpdate=4;
        RestartGame();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui explique au joueur qu'il existe en faîte 4 gravités dans le jeu
    /// </summary>
    public void Explain4Direction(){
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => {
            explainPanel.SetActive(false);
            hidePanel.SetActive(false);
            canTouchSensitive=false;
            StartCoroutine(TutoCoroutine(0.4f,delegate{Goal3();}));
        });
        StopGame();
        explainText.text = "We come with bad news... Unfortunately, the piece can sustain 4 gravities, to the top, to the bottom, to the right and to the left. Good luck to stop there progress.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(true);
        hidePanel.SetActive(true);
        goalPanel.SetActive(false);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est le troisième objectif du jeu
    /// </summary>
    public void Goal3(){
        goalText.text = "Make a last complete line to end the tutorial";
        hidePanel.SetActive(false);
        explainPausePanel.SetActive(false);
        explainPanel.SetActive(false);
        continuePauseButtonAsGameObject.SetActive(false);
        goalPanel.SetActive(true);
        pauseGoalPanel.SetActive(false);
        startUpdate=5;
        RestartGame();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui explique le menu de fin de jeu au joueur
    /// </summary>
    public void ExplainEndGame(){
        pauseGoalText.text = "Press one of the menu's button";
        explainMenuPauseText.text = "From the end game menu, you restart the tutorial or back to main menu to play a real game. You have also an option which allow you to register and share your score out of the game.";
        explainPanel.SetActive(false);
        continueButtonAsGameObject.SetActive(true);
        explainPausePanel.SetActive(true);
        continuePauseButtonAsGameObject.SetActive(false);
        goalPanel.SetActive(false);
        pauseGoalPanel.SetActive(true);

        IMode.SetGameIsOver(true);
        //Arrêt du temps lors de l'ouverture de l'interface de fin de jeu 

        endGamePanel.SetActive(true);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet d'arrêter les fonctionnalités tactiles et internes au jeu
    /// </summary>
    public void StopGame(){
        Time.timeScale=0f;
        canTouchSensitive=false;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de lancer ou reprendre les fonctionnalités tactiles et internes au jeu
    /// </summary>
    public void RestartGame(){
        canTouchSensitive=true;
        Time.timeScale=1f;
    }


    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette coroutine permet de stopper les instructions du tutoriel un certain temps spécifié 
    /// </summary>
    /// <param name="nbSeconds">
    /// un entier qui indique le nombre de seconde durant lesquelles le jeu va reprendre
    /// </param>
    /// <param name="method">
    /// une action qui peut être décrite par une procédure/fonction ou une lambda expression 
    /// </param>
    IEnumerator TutoCoroutine(float nbSeconds, Action method){
        float ms = Time.deltaTime;
        while(ms <= nbSeconds){
            ms += Time.deltaTime;
            yield return null;
        }

        method();
    }
}

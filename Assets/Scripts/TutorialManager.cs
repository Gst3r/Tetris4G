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
    [SerializeField]private GameObject tutoCharactere;

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
    public static bool activeAccelerate=true;


    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public static bool activeMove=true;

    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public static int startUpdate = 0;

    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public static int prevScore = 0;

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
            Time.timeScale=0;
            ExplainEndGame();
        }else if(startUpdate==5 && !board.GetTopIsFull()){
            BoardManager.SetGravity(Gravity.HAUT);
        }else if(startUpdate==5 && board.GetTopIsFull() && !board.GetBotIsFull()){
            BoardManager.SetGravity(Gravity.BAS);
        }else if(startUpdate==5 && board.GetTopIsFull() && board.GetBotIsFull() && startUpdate!=0){
            startUpdate=0;
            Time.timeScale=0;
            ExplainEndGame();
        }else if(startUpdate==6 && board.GetTopIsFull() && board.GetTopIsFull() && board.GetRightIsFull() && board.GetLeftIsFull() && startUpdate!=0){
            startUpdate=0;
            Time.timeScale=0;
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
                    else if(startUpdate==-2)
                        startUpdate=3;
                    break;
            }

            CheckRotatePiece(touch);
            CheckShiftPiece(touch);
            CheckAcceleratePiece(touch);
        }else
            TouchSensitive.SetWantToRotate(true);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de vérifier si un objectif parmis les 3 à faire pendant le tuto est remplit ou non
    /// </summary>
    public void CheckGoalIsDone(){
        if(startUpdate==4 && ScoreManager.GetScore()>=40){
            startUpdate=0;
            prevScore=ScoreManager.GetScore();
            StartCoroutine(TutoCoroutine(0.4f,delegate{Explain2Direction();}));
        }else if(startUpdate==5 && ScoreManager.GetScore()>=40+prevScore){
            startUpdate=0;
            prevScore=ScoreManager.GetScore();
            Debug.Log(prevScore);
            StartCoroutine(TutoCoroutine(0.4f,delegate{Explain4Direction();}));
        }else if(startUpdate==6 && ScoreManager.GetScore()>=40+prevScore){
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
            goalPanel.SetActive(false);
            tutoCharactere.SetActive(false);
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
            goalPanel.SetActive(false);
            tutoCharactere.SetActive(false);
            RestartGame();
            startUpdate=0;
            StartCoroutine(TutoCoroutine(1,delegate{TakeItAccelerate();}));
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de déterminer si le joueur à bien réalisé une accéleration du tetromino pour le tuto et lance une procédure lié à l'enchaînement d'action du tuto 
    /// </summary>
    /// <param name="touch">
    /// un attribut permettant de manipuler les informations relatifs aux mouvements du doigts sur le dispositif Android
    /// </param>
    public void CheckAcceleratePiece(Touch touch){
        //Précondition déterminant si on peut déclencher les fonctionnalités tactiles pour la rotation de la pièce
        if(startUpdate==3 && (int)touch.deltaPosition.x==0f && (int)touch.deltaPosition.y<=-7f){
            explainPanel.SetActive(false);
            goalPanel.SetActive(false);
            tutoCharactere.SetActive(false);
            RestartGame();
            startUpdate=0;        
            StartCoroutine(TutoCoroutine(2f,delegate{TakeItHold();}));
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet d'initialiser les interfaces qui apparaissent au lancement du tutoriel
    /// </summary>
    public void SetTutorial(){
        StopGame();
        explainPrev=true;
        activeAccelerate=false;
        activeMove=false;
        explainPanel.SetActive(false);
        goalPanel.SetActive(false);
        explainPausePanel.SetActive(false);
        coverPauseMenuPanel.SetActive(true);
        coverHoldPanel.SetActive(true);
        pauseGoalPanel.SetActive(false);

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
            RestartGame();
            canTouchSensitive=false;
            tutoCharactere.SetActive(false);
            StartCoroutine(TutoCoroutine(1, delegate {TakeItRotation();}));
        });
        explainText.text = "This Tetromino appears from this gate in the center ! Gravity will be applied on it and the piece will move downward... We need to destroy it, but the only way to do so is to complete a row or column with other pieces.";
        explainPanel.SetActive(true);
        coverPauseMenuPanel.SetActive(true);
        tutoCharactere.SetActive(true);
        coverHoldPanel.SetActive(true);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est la première rotation du jeu
    /// </summary>
    public void TakeItRotation(){
        StopGame();
        goalText.text = "Touch the screen 1 time"; 
        explainText.text = "If the tetromino's orientation doesn't match the environment and the choosen strategy, you can rotate it at 90 degrees with one presure on the screen. To turn him at 180 degrees, you need to touch the screen two times and at 270 degrees, three times. The tetromino is back to its first position by the fourth touch.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        tutoCharactere.SetActive(true);
        goalPanel.SetActive(true);
        startUpdate=1;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est le premier déplacement du jeu
    /// </summary>
    public void TakeItShift(){
        StopGame();
        activeMove=true;
        goalText.text = "Slide the piece right or left"; 
        explainText.text = "You masteries the environment according with the gravity. As a result, you can move the tetromino left and right by sliding your finger to the wished direction.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        tutoCharactere.SetActive(true);
        goalPanel.SetActive(true);
        if(Input.touchCount>0)
            startUpdate=-1;
        else
            startUpdate=2;
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est la première utilisation de la fonctionnalité hold du jeu
    /// </summary>
    public void TakeItAccelerate(){
        StopGame();
        activeAccelerate=true;
        goalText.text = "Swipe down the piece to the bottom";
        explainText.text = "The piece can be accelerate if you swipe on the gravity's direction.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        tutoCharactere.SetActive(true);
        goalPanel.SetActive(true);
        TouchSensitive.SetWantToAccelerate(true);
        if(Input.touchCount>0)
            startUpdate=-2;
        else
            startUpdate=3;
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est la première utilisation de la fonctionnalité hold du jeu
    /// </summary>
    public void TakeItHold(){
        StopGame();
        goalText.text = "Touch the hold zone";
        explainText.text = "The piece can be hold in a storage zone. Then, it can be swapped with the active piece on the board. This option is available once by spawn piece and helps to manage the active piece.";
        explainPanel.SetActive(true);
        tutoCharactere.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        coverHoldPanel.SetActive(false);
        goalPanel.SetActive(true);
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
                tutoCharactere.SetActive(false);
                canTouchSensitive=false;
                ExplainScore();
            });
            explainText.text = "This zone shows the next three pieces which will spawn on the board from left to right. Your strategy can be adaptated with these.";
            explainPanel.SetActive(true);
            tutoCharactere.SetActive(true);
            continueButtonAsGameObject.SetActive(true);
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
            tutoCharactere.SetActive(false);
            canTouchSensitive=false;
            ExplainPauseMenu();
        });
        explainText.text = "The game score is displayed here.";
        explainPanel.SetActive(true);
        tutoCharactere.SetActive(true);
        continueButtonAsGameObject.SetActive(true);
        coverPauseMenuPanel.SetActive(true);
        coverHoldPanel.SetActive(true);
        goalPanel.SetActive(false);
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est une explication du menu pause du jeu
    /// </summary>
    public void ExplainPauseMenu(){
        pauseMenuButton.onClick.AddListener(() => {
            pauseGoalText.text = "Press one of the menu's button";
            explainMenuPauseText.text = "From the pause menu, you can continue the tutorial, restart it or go back to the main menu to play a real game. You also have the option to stop the tetrominoe's speed.";
            explainPausePanel.SetActive(true);
            explainPanel.SetActive(false);
            continuePauseButtonAsGameObject.SetActive(false);
            tutoCharactere.SetActive(false);
            goalPanel.SetActive(false);
            coverHoldPanel.SetActive(false);
            pauseGoalPanel.SetActive(true);
            canTouchSensitive=true;
        });

        goalText.text = "Touch the pause menu icon";
        explainText.text = "This icon represents the pause menu.";
        explainPanel.SetActive(true);
        tutoCharactere.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        coverPauseMenuPanel.SetActive(false);
        goalPanel.SetActive(true);
    }       

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet d'attribuer au bouton continuer du menu pause la capacité de reprendre les objectifs en cours
    /// </summary>
    public void ContinueGoal(){
        if(startUpdate==4)
            Goal1();
        else if(startUpdate==5)
            Goal2();
        else if(startUpdate==6)
            Goal3();
        else 
            Goal1();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est le premier objectif du jeu
    /// </summary>
    public void Goal1(){
        goalText.text = "Make at least one complete line";
        explainPausePanel.SetActive(false);
        explainPanel.SetActive(false);
        continuePauseButtonAsGameObject.SetActive(false);
        goalPanel.SetActive(true);
        tutoCharactere.SetActive(false);
        pauseGoalPanel.SetActive(false);
        coverHoldPanel.SetActive(false);
        startUpdate=4;
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
            canTouchSensitive=false;
            tutoCharactere.SetActive(false);
            Time.timeScale=1f;
            StartCoroutine(TutoCoroutine(0.4f,delegate{Goal2();}));
        });
        StopGame();
        explainText.text = "As advertised in the title of the game, the tetrominoes can also be drawn by a different gravity... Now, the gravity can be the top or the bottom one.";
        explainPanel.SetActive(true);
            tutoCharactere.SetActive(true);
        continueButtonAsGameObject.SetActive(true);
        goalPanel.SetActive(false);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est le deuxieme objectif du jeu
    /// </summary>
    public void Goal2(){
        goalText.text = "Make at least another complete line";
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
    /// Description : Cette procédure définit une suite d'action qui explique au joueur qu'il existe en faîte 4 gravités dans le jeu
    /// </summary>
    public void Explain4Direction(){
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => {
            explainPanel.SetActive(false);
            tutoCharactere.SetActive(false);
            canTouchSensitive=false;
            Time.timeScale=1f;
            StartCoroutine(TutoCoroutine(0.4f,delegate{Goal3();}));
        });
        StopGame();
        explainText.text = "Bad news... Unfortunately, the pieces can sustain 4 gravities, the top, the bottom, the right and the left one. Good luck !!";
        explainPanel.SetActive(true);
        tutoCharactere.SetActive(true);
        continueButtonAsGameObject.SetActive(true);
        goalPanel.SetActive(false);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui est le troisième objectif du jeu
    /// </summary>
    public void Goal3(){
        goalText.text = "Make a last complete line to end the tutorial";
        explainPausePanel.SetActive(false);
        explainPanel.SetActive(false);
        continuePauseButtonAsGameObject.SetActive(false);
        goalPanel.SetActive(true);
        pauseGoalPanel.SetActive(false);
        startUpdate=6;
        RestartGame();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette procédure définit une suite d'action qui explique le menu de fin de jeu au joueur
    /// </summary>
    public void ExplainEndGame(){
        pauseGoalText.text = "Press one of the menu's button";
        explainMenuPauseText.text = "From the end game menu, you can restart the tutorial or go back to the main menu to play a real game. You have the option to register and share your score.";
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

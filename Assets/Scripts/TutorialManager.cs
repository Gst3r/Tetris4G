using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class TutorialManager : MonoBehaviour
{

//------------------------------GAME---------------------------------------------------
    [SerializeField]private GameObject[] showPanel;
    [SerializeField]private GameObject hidePanel;
    [SerializeField]private GameObject explainPanel;
    [SerializeField]private GameObject goalPanel;
    [SerializeField]private GameObject continueButtonAsGameObject;
    [SerializeField]private TMP_Text goalText;
    [SerializeField]private Button continueButton;
    [SerializeField]private TMP_Text explainText;

//-------------------------------PAUSE------------------------------------------------------
    [SerializeField]private GameObject explainPausePanel;
    [SerializeField]private GameObject coverPauseMenuPanel;
    [SerializeField]private GameObject hideUIPanel;
    [SerializeField]private TMP_Text explainMenuPauseText;
    [SerializeField]private GameObject continuePauseButtonAsGameObject;   
    [SerializeField]private TMP_Text pauseGoalText; 
    [SerializeField]private GameObject pauseGoalPanel;

//-------------------------------------MANAGEMENT AND SCHEDULING---------------------------------------------------
    private bool screenIsTouch = false;

    public static bool scriptIsActive = false;

    public static bool canTouchSensitive=true;

    public static int startUpdate = 0;

    // Start is called before the first frame update
    void Start()
    {
        canTouchSensitive=false;
        scriptIsActive = true;
        SetTutorial();
        StartCoroutine(TutoCoroutine(0.7f, delegate{LaunchTutorial();}));
    }

    void Update(){
        BoardManager.SetGravity(Gravity.BAS);
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
                    break;
            }

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
        }else
            TouchSensitive.SetWantToRotate(true);
        
        if(startUpdate==3 && ScoreManager.GetScore()>=40){
            startUpdate=0;
            StartCoroutine(TutoCoroutine(0.7f,delegate{Explain2Direction();}));
        }

        if(startUpdate==4 && ScoreManager.GetScore()>=80){
            startUpdate=0;
            StartCoroutine(TutoCoroutine(0.7f,delegate{Explain4Direction();}));
        }

        if(startUpdate==5 && ScoreManager.GetScore()>=120){
            startUpdate=0;
            ExplainEndGame();
        }
    }

    public void SetTutorial(){
        explainPanel.SetActive(false);
        hidePanel.SetActive(false);
        hideUIPanel.SetActive(false);
        goalPanel.SetActive(false);
        explainPausePanel.SetActive(false);
        coverPauseMenuPanel.SetActive(false);
        for(int i=0;i<showPanel.Length;i++){
            showPanel[i].SetActive(false);
        }

    }

    public void LaunchTutorial(){
        StopGame();
        ExplainBase();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
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
        hideUIPanel.SetActive(true);
        showPanel[0].SetActive(true);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
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
    /// Description : Cette méthode permet de 
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
        startUpdate=2;
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void TakeItHold(){
        StopGame();
        goalText.text = "Touch the hold zone";
        explainText.text = "The piece can be hold in a storage zone and after can be swap with the the active piece on the board. This option add a management of the active piece.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(false);
        showPanel[3].SetActive(true);
        goalPanel.SetActive(true);
        coverPauseMenuPanel.SetActive(true);
        hidePanel.SetActive(true);
        hideUIPanel.SetActive(false);
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void ExplainPrevisualisation(){
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
        coverPauseMenuPanel.SetActive(true);
        hidePanel.SetActive(true);
        goalPanel.SetActive(false);  
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void ExplainModeInformation(){

    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
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
        hidePanel.SetActive(true);
        goalPanel.SetActive(false);
    } 

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void ExplainPauseMenu(){
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
    /// Description : Cette méthode permet de 
    /// </summary>
    public void ExplainPauseMenuDeeply(){
        showPanel[6].SetActive(false);
        pauseGoalText.text = "Press one of the menu's button";
        explainMenuPauseText.text = "From the pause menu, you can continue the tutorial, restart the tutorial or back to main menu to play a real game. You have also an option which allow you to stop the tetrominoes speed.";
        hidePanel.SetActive(false);
        explainPausePanel.SetActive(true);
        continuePauseButtonAsGameObject.SetActive(false);
        coverPauseMenuPanel.SetActive(false);
        goalPanel.SetActive(false);
        pauseGoalPanel.SetActive(true);
        canTouchSensitive=true;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void Goal1(){
        goalText.text = "Make one complete line";
        hidePanel.SetActive(false);
        explainPausePanel.SetActive(false);
        explainPanel.SetActive(false);
        continuePauseButtonAsGameObject.SetActive(false);
        coverPauseMenuPanel.SetActive(false);
        goalPanel.SetActive(true);
        pauseGoalPanel.SetActive(false);
        startUpdate=3;
        RestartGame();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void Explain2Direction(){
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => {
            explainPanel.SetActive(false);
            hidePanel.SetActive(false);
            coverPauseMenuPanel.SetActive(false);
            canTouchSensitive=true;
            Goal2();
        });
        StopGame();
        explainText.text = "Our analyse tell us that pieces can also spawn in a different gravity... The gravity can be to the top or to the bottom.";
        explainPanel.SetActive(true);
        continueButtonAsGameObject.SetActive(true);
        hidePanel.SetActive(true);
        goalPanel.SetActive(false);
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void Goal2(){
        goalText.text = "Make another complete line";
        hidePanel.SetActive(false);
        explainPausePanel.SetActive(false);
        explainPanel.SetActive(false);
        continuePauseButtonAsGameObject.SetActive(false);
        coverPauseMenuPanel.SetActive(false);
        goalPanel.SetActive(true);
        pauseGoalPanel.SetActive(false);
        startUpdate=4;
        RestartGame();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void Explain4Direction(){
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(() => {
            explainPanel.SetActive(false);
            hidePanel.SetActive(false);
            coverPauseMenuPanel.SetActive(false);
            canTouchSensitive=true;
            Goal3();
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
    /// Description : Cette méthode permet de 
    /// </summary>
    public void Goal3(){
        goalText.text = "Make a last complete line to end the tutorial";
        hidePanel.SetActive(false);
        explainPausePanel.SetActive(false);
        explainPanel.SetActive(false);
        continuePauseButtonAsGameObject.SetActive(false);
        coverPauseMenuPanel.SetActive(false);
        goalPanel.SetActive(true);
        pauseGoalPanel.SetActive(false);
        startUpdate=5;
        RestartGame();
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void ExplainEndGame(){

    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void EndTutorial(){

    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
    /// </summary>
    public void StopGame(){
        Time.timeScale=0f;
        canTouchSensitive=false;
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Cette méthode permet de 
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
    IEnumerator TutoCoroutine(float nbSeconds, Action method){
        float ms = Time.deltaTime;
        while(ms <= nbSeconds){
            ms += Time.deltaTime;
            yield return null;
        }

        method();
    }
}

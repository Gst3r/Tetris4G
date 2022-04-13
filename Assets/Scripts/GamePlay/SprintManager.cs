using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Auteur : Sterlingot Guillaume<br>
/// Cette classe contient l'ensemble des méthodes utilisées pour l'exécution du mode de jeu Sprint
/// </summary>
public class SprintManager : IMode
{

    /// <summary> 
    /// Temps en seconde qui s'est écoulé depuis le début de la partie
    /// </summary>
    private static float countSec = 0f;

    /// <summary> 
    /// Temps en seconde dont dispose le joueur pour scorer
    /// </summary>
    private static float countMax = 301f;

    /// <summary> 
    /// Panel contenant toute l'affichage conçernant le temps restant avant la fin de la partie
    /// </summary>
    [SerializeField]private GameObject sprintTimePanel;

    private void LastUpdate(){
        closeSprintTimePanel();
    }
     
    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode de départ du mode de jeu Sprint qui initialise les paramètres du mode de jeu
    /// </summary>
    public override void StartExecute(){
        this.controller = GameObject.Find("GameManager").GetComponent<Controller>();
        this.board = controller.GetBoard();
        this.activePiece = controller.GetActivePiece();

        countSec = 0;
        this.activePiece.SetStepDelay(0.4f);
        this.activePiece.SetBufferedStepDelay(this.activePiece.GetStepDelay());
        this.sprintTimePanel.GetComponent<Text>().text = "5:00";
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant d'exécuter le mode de jeu Sprint
    /// </summary>
    public override void Execute(){
        CountTime();
        displaySprintTimePanel();
        CheckGameOver();
    }
    
    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Méthode permettant la gestion du compteur du Sprint mode
    /// </summary>
    public void CountTime(){
        if(!PauseMenu.GetGameIsPausing()){
            countSec+=Time.deltaTime;
            if((int)countSec!=0){
                this.sprintTimePanel.GetComponent<Text>().text = ((int)(countMax-countSec)/60).ToString()+":"+((int)(countMax-countSec)%60).ToString();  
            }
        }
    }

    /// <summary>
    /// Auteur : Kusunga Malcom, Sterlingot Guillaume<br>
    /// Méthode permettant de déterminer si la partie est terminée
    /// </summary>
    /// <returns>
    /// Booléen qui retourne TRUE si tous les côtés sont remplis et le temps limite est dépassé, FALSE sinon
    /// </returns>
    public override bool GameOver(){
        // un côté non rempli signifie que la partie continue
        if ((board.GetTopIsFull() && board.GetBotIsFull() && board.GetLeftIsFull() && board.GetRightIsFull()) || (int)countSec>=(int)countMax)
            return true;
        return false;
    }

    public void displaySprintTimePanel(){
        sprintTimePanel.SetActive(true);
    }

    public void closeSprintTimePanel(){
        sprintTimePanel.SetActive(false);
    }


    public static int GetScore()
    {
        return BoardManager.GetTotalLinesCleared(); 
    }
}

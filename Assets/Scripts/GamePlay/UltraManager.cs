using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Auteur : Sterlingot Guillaume<br>
/// Cette classe contient l'ensemble des méthodes utilisées pour l'exécution du mode de jeu Ultra
/// </summary>
public class UltraManager : IMode
{
    /// <summary> 
    /// Nombre entier qui représente le nombre de ligne restante à détruire
    /// </summary>
    private static int maxLine=20;

    /// <summary> 
    /// Nombre entier qui représente le nombre de ligne détruite
    /// </summary>
    private static int countLine=0;

    /// <summary> 
    /// Temps en seconde qui s'est écoulé depuis le début de la partie
    /// </summary>
    private static float countTime;

    /// <summary> 
    /// Temps en seconde dont dispose le joueur pour scorer
    /// </summary>
    private static int goalScore = 601;

    /// <summary> 
    /// Panel contenant toute l'affichage conçernant le temps restant avant la fin de la partie
    /// </summary>
    [SerializeField]private GameObject ultraTimePanel;

    /// <summary> 
    /// Panel contenant toute l'affichage conçernant le nombre de ligne qu'il reste à détruire
    /// </summary>
    [SerializeField]private GameObject ultraLinePanel;
    
    private void LastUpdate(){
        closeUltraGoalPanel();
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode de départ du mode de jeu Ultra qui initialise les paramètres du mode de jeu
    /// </summary>
    public override void StartExecute(){
        this.soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();
        this.controller = GameObject.Find("GameManager").GetComponent<Controller>();
        this.board = controller.GetBoard();
        this.activePiece = controller.GetActivePiece();
        countTime=0f;
        countLine=0;
        this.activePiece.SetStepDelay(0.5f);
        this.activePiece.SetBufferedStepDelay(this.activePiece.GetStepDelay());
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant d'exécuter le mode de jeu Sprint
    /// </summary>
    public override void Execute(){
        CountTime();
        CountLine();
        displayUltraGoalPanel();
        CheckGameOver();
    }

    
    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Méthode permettant de compter le nombre de ligne restante à détruire
    /// </summary>
    public void CountLine(){
        if(!PauseMenu.GetGameIsPausing()){
            countLine=BoardManager.GetTotalLinesCleared();
            this.ultraLinePanel.GetComponent<Text>().text = (maxLine-countLine).ToString();  
            ScoreManager.SetNbLines(countLine);
        }
    }

    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Méthode permettant de compter le temps qui s'écoule depuis le début de la partie
    /// </summary>
    public void CountTime(){
        if(!PauseMenu.GetGameIsPausing()){
            countTime+=Time.deltaTime;
            if((int)countTime!=0){
                this.ultraTimePanel.GetComponent<Text>().text = ((int)(countTime)/60).ToString()+":"+((((int)countTime)%60<10)?"0":"")+((int)(countTime)%60).ToString();  
            }
            ScoreManager.SetTime((int)countTime);
        }
    }

    /// <summary>
    /// Auteur : Kusunga Malcom, Sterlingot Guillaume<br>
    /// Méthode permettant de déterminer si la partie est terminée
    /// </summary>
    /// <returns>
    /// Booléen qui retourne TRUE si tous les côtés sont remplis et le score visé est atteind, FALSE sinon
    /// </returns>
    public override bool GameOver(){
        // un côté non rempli signifie que la partie continue
        if ((board.GetTopIsFull() && board.GetBotIsFull() && board.GetLeftIsFull() && board.GetRightIsFull()) || (int)BoardManager.GetTotalLinesCleared()>=(int)maxLine || countTime>=goalScore)
            return true;
        return false;
    }

    public void displayUltraGoalPanel(){
        ultraLinePanel.SetActive(true);
        ultraTimePanel.SetActive(true);
    }

    public void closeUltraGoalPanel(){
        ultraLinePanel.SetActive(false);
        ultraTimePanel.SetActive(false);
    }

    public static int GetTime()
    {
        return (int)countTime; 
    }

    public static int GetLine()
    {
        return (int)countLine; 
    }
}

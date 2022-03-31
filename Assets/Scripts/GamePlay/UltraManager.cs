using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auteur : Sterlingot Guillaume<br>
/// Cette classe contient l'ensemble des méthodes utilisées pour l'exécution du mode de jeu Ultra
/// </summary>
public class UltraManager : IMode
{
    /// <summary> 
    /// Temps en seconde qui s'est écoulé depuis le début de la partie
    /// </summary>
    private static float countScore = 0;

    /// <summary> 
    /// Temps en seconde dont dispose le joueur pour scorer
    /// </summary>
    private static float goalScore = 11f;

    /// <summary> 
    /// Panel contenant toute l'affichage conçernant le temps restant avant la fin de la partie
    /// </summary>
    [SerializeField]private GameObject ultraGoalPanel;

    private void Start(){
        this.controller = GameObject.Find("GameManager").GetComponent<Controller>();
        this.board = controller.GetBoard();
        this.activePiece = controller.GetActivePiece();
        countScore = 0;
        this.activePiece.SetStepDelay(0.7f);
    }
    
    private void LastUpdate(){
        closeUltraGoalPanel();
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant d'exécuter le mode de jeu Sprint
    /// </summary>
    public override void Execute(){
        displayUltraGoalPanel();
        AccelerateGravity();
        CheckGameOver();
    }

    /* PARLER AVEC NASSIMA POUR LE SCORE
    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Méthode permettant la gestion du compteur du Sprint mode
    /// </summary>
    public void CountScore(){
        if(!PauseMenu.GetGameIsPausing()){
            countScore+=Time.deltaTime;
            this.sprintTimePanel.GetComponent<Text>().text = ((int)(countMax-countSec)).ToString();
        }
    }*/

    /// <summary>
    /// Auteur : Kusunga Malcom, Sterlingot Guillaume<br>
    /// Méthode permettant de déterminer si la partie est terminée
    /// </summary>
    /// <returns>
    /// Booléen qui retourne TRUE si tous les côtés sont remplis et le score visé est atteind, FALSE sinon
    /// </returns>
    public override bool GameOver(){
        // un côté non rempli signifie que la partie continue
        if ((board.GetTopIsFull() && board.GetBotIsFull() && board.GetLeftIsFull() && board.GetRightIsFull()) || (int)countScore>=(int)goalScore)
            return true;
        return false;
    }

    public void displayUltraGoalPanel(){
        ultraGoalPanel.SetActive(true);
    }

    public void closeUltraGoalPanel(){
        ultraGoalPanel.SetActive(false);
    }
}

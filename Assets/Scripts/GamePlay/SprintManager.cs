using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auteur : Sterlingot Guillaume<br>
/// Cette classe contient l'ensemble des méthodes utilisées pour l'exécution du mode de jeu Sprint
/// </summary>
public class SprintManager : IMode
{

    /// <summary> 
    /// Temps en seconde qui s'est écoulé depuis le début de la partie
    /// </summary>
    private static float countSec;

    /// <summary> 
    /// Temps en seconde dont dispose le joueur pour scorer
    /// </summary>
    private static float countMax = 10f;

    private void Start(){
        this.controller = GameObject.Find("GameManager").GetComponent<Controller>();
        this.board = controller.GetBoard();
        this.activePiece = controller.GetActivePiece();
        this.activePiece.SetStepDelay(0.7f);
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant d'exécuter le mode de jeu Sprint
    /// </summary>
    public override void Execute(){
        //CountTime();
        AccelerateGravity();
        CheckGameOver();
    }
    
    /// <summary>
    /// Auteur : Sterlingot Guillaume<br>
    /// Méthode permettant la gestion du compteur du Sprint mode
    /// </summary>
    public static void CountTime(){
        if(!PauseMenu.GetGameIsPausing()){
            countSec+=Time.deltaTime;
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
        if ((board.GetTopIsFull() && board.GetBotIsFull() && board.GetLeftIsFull() && board.GetRightIsFull()) || (int)countSec==(int)countMax)
            return true;
        return false;
    }
}

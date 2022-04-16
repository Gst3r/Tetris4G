using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auteur : Sterlingot Guillaume<br>
/// Cette classe contient l'ensemble des méthodes utilisées pour l'exécution du mode de jeu Marathon
/// </summary>
public class MarathonManager : IMode
{

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode de départ du mode de jeu Marathon qui initialise les paramètres du mode de jeu
    /// </summary>
    public override void StartExecute(){
        this.soundManager = GameObject.Find("GameManager").GetComponent<SoundManager>();
        this.controller = GameObject.Find("GameManager").GetComponent<Controller>();
        this.board = controller.GetBoard();
        this.activePiece = controller.GetActivePiece();
        this.activePiece.SetStepDelay(0.8f);
        this.activePiece.SetBufferedStepDelay(this.activePiece.GetStepDelay());
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant d'exécuter le mode de jeu Marathon 
    /// </summary>
    public override void Execute(){
        AccelerateGravity();
        CheckGameOver();
    }

    public static float GetScore()
    {
        return ScoreManager.GetScore(); 

    }
}

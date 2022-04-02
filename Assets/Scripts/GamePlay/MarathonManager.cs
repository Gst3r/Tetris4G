using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auteur : Sterlingot Guillaume<br>
/// Cette classe contient l'ensemble des méthodes utilisées pour l'exécution du mode de jeu Marathon
/// </summary>
public class MarathonManager : IMode
{
    private void Start(){
        this.controller = GameObject.Find("GameManager").GetComponent<Controller>();
        this.board = controller.GetBoard();
        this.activePiece = controller.GetActivePiece();
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
        return BoardManager.GetScore(); 

    }
}

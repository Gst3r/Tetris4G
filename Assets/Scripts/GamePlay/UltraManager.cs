using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auteur : <br>
/// Cette classe contient l'ensemble des méthodes utilisées pour l'exécution du mode de jeu Ultra
/// </summary>
public class UltraManager : IMode
{
    /// <summary> 
    /// Attribut indiquant si la gravité a déjà été changé une fois ou non      
    /// </summary>
    private float justOne;

    private void Start(){
        this.justOne = -1;
        this.controller = GameObject.Find("GameManager").GetComponent<Controller>();
        this.board = controller.GetBoard();
        this.activePiece = controller.GetActivePiece();
    }
    
    /// <summary> 
    /// Auteur : <br>
    /// Description : Méthode permettant d'exécuter le mode de jeu Ultra
    /// </summary>
    public override void Execute(){

    }
}

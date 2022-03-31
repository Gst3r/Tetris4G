using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Auteur : Sterlingot Guillaume<br>
/// Description : Cette enumération permet de distinguer les différents modes de jeu diponible
/// </summary>
public enum Mode{
    MARATHON,
    SPRINT,
    ULTRA,
}

/// <summary>
/// Auteur : Sterlingot Guillaume, Kusunga Malcom<br>
/// Description : Cette classe permet l'interfacage entre les différents mode de jeu et le contrôleur.
/// </summary>
public abstract class IMode : MonoBehaviour
{
    /// <summary> 
    /// Attribut contenant la pièce courante qui est présente sur le plateau de jeu 
    /// </summary>
    protected Controller controller;

    /// <summary> 
    /// Attribut contenant le plateau de jeu 
    /// </summary>
    protected BoardManager board;

    /// <summary> 
    /// Attribut contenant la pièce courante qui est présente sur le plateau de jeu 
    /// </summary>
    protected Piece activePiece;

    /// <summary> 
    /// Booléen indiquant si le Game Over a été detecter
    /// </summary>
    protected bool gameIsOver;

    /// <summary> 
    /// Attribut indiquant si la gravité a déjà été changé une fois ou non      
    /// </summary>
    private float justOne;

    public void Start(){
        this.justOne = -1;
        this.activePiece.SetStepDelay(1f);
    }

    public abstract void Execute();

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant d'accélérer la vitesse du tétromino avec le temps
    /// </summary>
    public void AccelerateGravity(){
        float stepDelay = this.activePiece.GetStepDelay();
        
        if(((int)Time.realtimeSinceStartup)%5!=0)
            justOne=0;

        // On décrémente petit à petit la gravité toute les 5 secondes selon le modulo cité dans la condition
        if(((int)Time.realtimeSinceStartup)%5==0 && justOne==0){ 
            stepDelay -= 0.01f; 
            justOne=1; // On indique qu'on veut qu'il rentre une fois et on avorte la condition grace à l'attribut justOne
        }
    }

    /// <summary> 
    /// Méthode qui permet de vérifier si la partie est perdue 
    /// Auteur: Malcom Kusunga
    /// </summary>
    public void CheckGameOver(){
        if(!gameIsOver){
            if(GameOver()){
                gameIsOver = true;
                //Arrêt du temps lors de l'ouverture de l'interface de fin de jeu 
                Time.timeScale=0f;
                controller.endGamePanel.SetActive(true);
            }
        }
    }

    /// <summary>
    /// Auteur : Kusunga Malcom<br>
    /// Méthode permettant de déterminer si la partie est terminée
    /// </summary>
    /// <returns>
    /// Booléen qui retourne TRUE si tous les côtés sont remplis, FALSE sinon
    /// </returns>
    public virtual bool GameOver()
    {
        // un côté non rempli signifie que la partie continue
        if (board.GetTopIsFull() && board.GetBotIsFull() && board.GetLeftIsFull() && board.GetRightIsFull())
            return true;
        return false;
    }
}



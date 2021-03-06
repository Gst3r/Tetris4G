using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auteur : Sterlingot Guillaume<br>
/// Description : Cette enumération permet de distinguer les différents modes de jeu disponibles
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
    /// Attribut contenant le manager du son
    /// </summary>
    protected SoundManager soundManager;

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
    protected static bool gameIsOver;

    /// <summary> 
    /// Attribut indiquant si la gravité a déjà été changé une fois ou non      
    /// </summary>
    private float justOne;
    
    [SerializeField] protected GameObject endGamePanel;


    public void Start(){
        this.justOne = -1;
        BoardManager.SetLockSpeed(false);
        gameIsOver=false;
    }

    public abstract void StartExecute();
    public abstract void Execute();

    /// <summary> 
    /// Auteur : Sterlingot Guillaume<br>
    /// Description : Méthode permettant d'accélérer la vitesse du tétromino avec le temps
    /// </summary>
    public void AccelerateGravity(){
        if(!BoardManager.GetLockSpeed() && !PauseMenu.GetGameIsPausing() && !Piece.GetGravityIsModified())
        {    
            if(((int)Time.realtimeSinceStartup)%5!=0)
                justOne=0;

            // On décrémente petit à petit la gravité toute les 5 secondes selon le modulo cité dans la condition
            if(((int)Time.realtimeSinceStartup)%5==0 && justOne==0 && this.activePiece.GetStepDelay()>=0.45f){
                this.activePiece.SetStepDelay(this.activePiece.GetStepDelay()-0.0025f);
                this.activePiece.SetBufferedStepDelay(this.activePiece.GetStepDelay());
                justOne=1; // On indique qu'on veut qu'il rentre une fois et on avorte la condition grace à l'attribut justOne
            }
        } 
    }

    /// <summary> 
    /// Auteur: Malcom Kusunga<br>
    /// Description : Méthode qui permet de vérifier si la partie est perdue
    /// </summary>
    public void CheckGameOver(){
        if(!gameIsOver){
            if(GameOver()){
                gameIsOver = true;
                //Arrêt du temps lors de l'ouverture de l'interface de fin de jeu 
                Time.timeScale=0f;

                endGamePanel.SetActive(true);

                //Lancement du son lié à la fin de partie
                soundManager.PlaySound(soundManager.m_gameOverSound,0.5f);
            }
        }
    }

    /// <summary>
    /// Auteur : Kusunga Malcom<br>
    /// Description : Méthode permettant de déterminer si la partie est terminée
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

    public static bool GetGameIsOver(){
        return gameIsOver;
    }

    public static void SetGameIsOver(bool value){
        gameIsOver=value;
    }
}
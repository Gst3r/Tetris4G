using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Auteur : Sterlingot Guillaume, Kusunga Malcom, Bae Jin-Young, Seghir Nassima<br>
/// Descrption : Cette classe permet la gestion des interrations liées aux différentes actions du joueur.
/// </summary>
public class Controller : MonoBehaviour
{
    /// <summary> 
    /// Attribut contenant le plateau de jeu 
    /// </summary>
    public BoardManager board;

    /// <summary> 
    /// Attribut contenant le panel de fin de jeu
    /// </summary>
    public GameObject endGamePanel;

    /// <summary> 
    /// Booléen indiquant si le Game Over a été detecter
    /// </summary>
    private bool gameIsOver;

    private void Start() {
        gameIsOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkGameOver();    
    }

    /// <summary> 
    /// Méthode qui permet de vérifier si la partie est perdue 
    /// Auteur: Malcom Kusunga
    /// </summary>
    private void checkGameOver(){
        if(!gameIsOver){
            if(board.GameOver()){
                gameIsOver = true;
                //Arrêt du temps lors de l'ouverture de l'interface de fin de jeu 
                Time.timeScale=0f;
                endGamePanel.SetActive(true);
            }
        }
    }
}
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
    [SerializeField] private BoardManager board;
    
    /// <summary> 
    /// Attribut contenant la pièce courante qui est présente sur le plateau de jeu 
    /// </summary>
    [SerializeField] private Piece activePiece;

    public Vector2 startPos;
    public Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    activePiece.Rotate();
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    break;
            }
        }   
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de choisir automatiquement le déplacement adapté selon la gravité (fonctionnalités tactile)
    /// </summary>
    public void Shift(Touch touch){
        /*switch(board.GetGravity()){
            case Gravity.HAUT:  if(){

                                }else{

                                }
                                 break;
            case Gravity.BAS:   if(){

                                }else{

                                }
                                break;
            case Gravity.GAUCHE:if(){

                                }else{

                                }
                                break;
            case Gravity.DROITE:if(){

                                }else{

                                }
                                break;
            default:break;
        }*/
    }
}
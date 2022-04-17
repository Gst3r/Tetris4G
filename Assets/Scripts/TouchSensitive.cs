using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSensitive : MonoBehaviour
{
    /// <summary> 
    /// Attribut contenant le plateau de jeu 
    /// </summary>
    [SerializeField] private BoardManager board;

    /// <summary> 
    /// Attribut contenant la pièce courante qui est présente sur le plateau de jeu 
    /// </summary>
    [SerializeField] private Piece activePiece;

//-----------------------------------------------------TOUCH SENSITIVE SCHEDULING------------------------------------------------------------------- 
    /// <summary> 
    /// Un nombre réel qui indique une échelle de vitesse du doigt qui se déplace sur l'écran
    /// </summary>
    private float fast;

    /// <summary> 
    /// Booléen indiquant TRUE si le joueur cherche à tourner le tetromino (analyse du comportement du doigt sur l'écran), FALSE sinon
    /// </summary>
    private static bool wantToRotate;

    /// <summary> 
    /// Booléen indiquant TRUE si la pièce n'est pas entrain de bouger, FALSE sinon
    /// </summary>
    private static bool isNotMoved;

    /// <summary> 
    /// Booléen indiquant TRUE si le joueur cherche à déplacer le tetromino (analyse du comportement du doigt sur l'écran), FALSE sinon
    /// </summary>
    private static bool wantToShift;

    /// <summary> 
    /// Booléen indiquant TRUE si le joueur cherche à accélerer le tetromino (analyse du comportement du doigt sur l'écran), FALSE sinon
    /// </summary>
    private static bool wantToAccelerate;

    /*/// <summary> 
    /// Booléen indiquant TRUE si le joueur cherche à ralentir le tetromino (analyse du comportement du doigt sur l'écran), FALSE sinon
    /// </summary>
    private static bool wantToSlow;*/
//---------------------------------------------------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        TouchSensitiveProcess();
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode condensant les fonctionnalités tactiles
    /// </summary>
    public void TouchSensitiveProcess(){
        //Condition de déclenchement des fonctionnalités tactiles selon l'état du jeu
        // Si il est en pause, si il est en game over, si il n'est pas lancé, si le tutoriel l'autorise
        if(!PauseMenu.GetGameIsPausing()&&!IMode.GetGameIsOver()&&ButtonManager.GetGameIsLoad()&&TutorialManager.canTouchSensitive){
            TouchSensitiveRotate();
            TouchSensitiveShift();
        }
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de choisir automatiquement le déplacement adapté selon la gravité (fonctionnalités tactile)
    /// </summary>
    public void TouchSensitiveShift(){
        if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);
            
            // Si le doigt du joueur n'est pas en mouvement sur l'écran l'objectif du joueur est interprété comme étant inconnue et donc les booléens sont automatiquement remis à TRUE
            // Si le doigt du joueur est en mouvement on considère qu'il ne cherche pas à tourner la pièce
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    wantToShift=true;
                    wantToAccelerate=true;
                    //wantToSlow=true;
                    break;

                case TouchPhase.Stationary:
                    wantToShift=true;
                    wantToAccelerate=true;
                    //wantToSlow=true;
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    if(TutorialManager.activeMove)
                        Shift(touch);
                    wantToRotate=false;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    wantToShift=true;
                    wantToAccelerate=true;
                    //wantToSlow=true;
                    activePiece.RestoreGravity();
                    break;
            }
        }else// Si le doigt du joueur ne touche pas l'écran on considère qu'il peut décider de réaliser une rotation de tetromino 
            wantToRotate=true;
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de tourner le tetromino courant (fonctionnalités tactile)
    /// </summary>
    public void TouchSensitiveRotate(){
        if(Input.touchCount>0){ //Si le joueur touche l'écran
            Touch touch = Input.GetTouch(0); // on récupère les informations liés au contact entre le doigt et l'écran dans une variable
            if(touch.phase.Equals(TouchPhase.Ended)&&wantToRotate){ // On vérifie si le joueur lève bien son doigt de l'écran et ne le met pas en mouvement grâce au booléen
                wantToRotate=false;
                activePiece.Rotate(); // On lance une rotation de la pièce
                wantToRotate=true;
            }
        }
    }


    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de choisir automatiquement le déplacement adapté selon la gravité (fonctionnalités tactile)
    /// </summary>
    public void Shift(Touch touch){
        int compareFastX = (int)Mathf.Abs(touch.deltaPosition.x);
        int fastX = FindFast(compareFastX);

        int compareFastY = (int)Mathf.Abs(touch.deltaPosition.y);
        int fastY = FindFast(compareFastY);

        switch(board.GetGravity()){
            case Gravity.HAUT:
                if(touch.deltaPosition.x>0 && Time.frameCount%(20-fastX)==0 && wantToShift){
                    wantToAccelerate=false;
                    //wantToSlow=false;
                    activePiece.RightShift();
                }else if(touch.deltaPosition.x<0 && Time.frameCount%(20-fastX)==0 && wantToShift){
                    wantToAccelerate=false;
                    //wantToSlow=false;
                    activePiece.LeftShift();
                }else if(touch.azimuthAngle==0f && Time.frameCount%(20-fastY)==0 && wantToAccelerate && touch.deltaPosition.y>0){
                    wantToShift=false;
                    //wantToSlow=false;
                    activePiece.TopShift();
                }/*else if(touch.azimuthAngle==0f && Time.frameCount%(20-fastX)==0 && wantToSlow && touch.deltaPosition.y<0){
                    wantToShift=false;
                    activePiece.ModifyGravityT();
                }*/
                    
                break;
            case Gravity.BAS:      
                if(touch.deltaPosition.x>0 && Time.frameCount%(20-fastX)==0 && wantToShift){
                    //wantToSlow=false;
                    wantToAccelerate=false;
                    activePiece.RightShift();
                }else if(touch.deltaPosition.x<0 && Time.frameCount%(20-fastX)==0 && wantToShift){
                    //wantToSlow=false;
                    wantToAccelerate=false;
                    activePiece.LeftShift();
                }else if(touch.azimuthAngle==0f && Time.frameCount%(20-fastY)==0 && wantToAccelerate && touch.deltaPosition.y<0 && TutorialManager.activeAccelerate){
                    //wantToSlow=false;
                    wantToShift=false;
                    activePiece.BotShift();
                }/*else if(touch.azimuthAngle==0f && Time.frameCount%(20-fastX)==0 && wantToSlow && touch.deltaPosition.y>0){
                    wantToShift=false;
                    activePiece.ModifyGravityB();
                }*/
                    
                break;
            case Gravity.GAUCHE:
                if(touch.deltaPosition.y>0 && Time.frameCount%(18-fastY)==0 && wantToShift){
                    wantToAccelerate=false;
                    //wantToSlow=false;
                    activePiece.TopShift();
                }else if(Time.frameCount%(18-fastY)==0 && wantToShift){
                    wantToAccelerate=false;
                    //wantToSlow=false;
                    activePiece.BotShift();
                }else if(touch.azimuthAngle==0f && Time.frameCount%(20-fastX)==0 && wantToAccelerate && touch.deltaPosition.x<0){
                    wantToShift= false;
                    //wantToSlow=false;
                    activePiece.LeftShift();
                }/*else if(touch.azimuthAngle==0f && Time.frameCount%(21-fastX)==0 && wantToSlow && touch.deltaPosition.x>0){
                    wantToShift=false;
                    activePiece.ModifyGravityL();
                }*/
                 
                break;
            case Gravity.DROITE:
                fast = fastY;
                if(touch.deltaPosition.y>0 && Time.frameCount%(18-fastY)==0 && wantToShift){
                    wantToAccelerate=false;
                    //wantToSlow=false;
                    activePiece.TopShift();
                }else if(Time.frameCount%(18-fastY)==0 && wantToShift){
                    wantToAccelerate=false;
                    //wantToSlow=false;
                    activePiece.BotShift();
                }else if(touch.azimuthAngle==0f && Time.frameCount%(20-fastX)==0 && wantToAccelerate && touch.deltaPosition.x>0){
                    wantToShift= false;
                    //wantToSlow=false;
                    activePiece.RightShift();
                }/*else if(touch.azimuthAngle==0f && Time.frameCount%(21-fastX)==0 && wantToSlow && touch.deltaPosition.x<0){
                    wantToShift=false;
                    activePiece.ModifyGravityR();
                }*/
                break;
            default:break;
        }
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de déterminer une vitesse pour le tactile
    /// </summary>
    public int FindFast(int compareFast){
        int localFast;

        if(compareFast>19)
            localFast = 19;
        else if(compareFast>17)
            localFast = 17;
        else if(compareFast>15)
            localFast = 13;
        else if(compareFast>13)
            localFast = 10;
        else if(compareFast>11)
            localFast = 7;
        else if(compareFast>9)
            localFast= 5;
        else if(compareFast>7)
            localFast = 2;
        else if(compareFast>5)
            localFast = -2;
        else if(compareFast>3)
            localFast = -5;
        else
            localFast = -8;

        return localFast;
    }

    public static bool GetWantToRotate(){
        return wantToRotate;
    }

    public static void SetWantToRotate(bool value){
        wantToRotate = value;
    }

    public static bool GetWantToAccelerate(){
        return wantToAccelerate;
    }

    public static void SetWantToAccelerate(bool value){
        wantToRotate = value;
    }
}

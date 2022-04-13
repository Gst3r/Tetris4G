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

    /// <summary> 
    /// Vecteur de nombre réel à deux dimensions qui enregistre la position de départ du doigt lorsqu'il entre en contact avec l'écran
    /// </summary>
    private Vector2 startPos;

    /// <summary> 
    /// Booléen indiquant TRUE si le joueur cherche à tourner le tetromino (analyse du comportement du doigt sur l'écran), FALSE sinon
    /// </summary>
    private static bool wantToRotate;

    /// <summary> 
    /// Un nombre réel qui indique une échelle de vitesse du doigt qui se déplace sur l'écran
    /// </summary>
    private float fast;

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
            
            switch (touch.phase)
            {
                // Record initial touch position.
                case TouchPhase.Began:
                    break;

                case TouchPhase.Stationary:
                    break;

                // Determine direction by comparing the current touch position with the initial one.
                case TouchPhase.Moved:
                    Shift(touch);
                    wantToRotate=false;
                    break;

                // Report that a direction has been chosen when the finger is lifted.
                case TouchPhase.Ended:
                    activePiece.RestoreGravity();
                    break;
            }
        }else
            wantToRotate=true;
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode permettant de tourner le tetromino courant (fonctionnalités tactile)
    /// </summary>
    public void TouchSensitiveRotate(){
        if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);
            if(touch.phase.Equals(TouchPhase.Ended)&&wantToRotate){
                activePiece.Rotate();
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
                fast = fastX;
                
                if(touch.deltaPosition.x>0 && Time.frameCount%(20-fast)==0)
                    activePiece.RightShift();
                else if(touch.deltaPosition.x<0 && Time.frameCount%(20-fast)==0)
                    activePiece.LeftShift();
                //else if(touch.azimuthAngle==0f)
                //    activePiece.ModifyGravityT();
                    
                break;
            case Gravity.BAS:
                fast = fastX;             
                if(touch.deltaPosition.x>0 && Time.frameCount%(20-fast)==0)
                    activePiece.RightShift();
                else if(touch.deltaPosition.x<0 && Time.frameCount%(20-fast)==0)
                    activePiece.LeftShift();
                //else if(touch.azimuthAngle==0f)
                //    activePiece.ModifyGravityB();
                break;
            case Gravity.GAUCHE:
                fast = fastY;
                if(touch.deltaPosition.y>0 && Time.frameCount%(18-fast)==0)
                    activePiece.TopShift();
                else if(Time.frameCount%(18-fast)==0)
                    activePiece.BotShift();
                break;
            case Gravity.DROITE:
                fast = fastY;
                if(touch.deltaPosition.y>0 && Time.frameCount%(18-fast)==0)
                    activePiece.TopShift();
                else if(Time.frameCount%(18-fast)==0)
                    activePiece.BotShift();
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
            localFast = 15;
        else if(compareFast>13)
            localFast = 13;
        else if(compareFast>11)
            localFast = 11;
        else if(compareFast>9)
            localFast= 9;
        else if(compareFast>7)
            localFast = 7;
        else
            localFast = 3;

        return localFast;
    }

    public static bool GetWantToRotate(){
        return wantToRotate;
    }
    public static void SetWantToRotate(bool wantToRot){
        wantToRotate = wantToRot;
    }
}

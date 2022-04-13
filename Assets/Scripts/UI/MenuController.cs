using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    /// <summary>
    /// Attribut permettant de différencdier les différents modes de jeu
    /// </summary>
    protected static Mode gameMode = Mode.MARATHON;
    public static bool firstGame;

    void Awake(){
        Debug.Log(firstGame);
        if(firstGame==null){
            firstGame=false;
            SceneManager.LoadScene("Tutorial");
        }
    }
    
    public void SetModeMarathon(){
        gameMode = Mode.MARATHON;
    }

    public void SetModeSprint(){
        gameMode = Mode.SPRINT;
    }

    public void SetModeUltra(){
        gameMode = Mode.ULTRA;
    }

    public static Mode GetMode(){
        return gameMode;
    }
}

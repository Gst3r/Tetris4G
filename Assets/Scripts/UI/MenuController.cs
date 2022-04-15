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

    void Awake(){
        if(PlayerPrefs.GetInt("firstGame", 0)!=1){
            PlayerPrefs.SetInt("firstGame",1);
            SceneManager.LoadScene("Tutorial");
        }
    }
    void Start(){
        TutorialManager.canTouchSensitive = true;
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

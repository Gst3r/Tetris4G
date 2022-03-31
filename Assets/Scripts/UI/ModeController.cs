using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeController : MonoBehaviour
{
    /// <summary>
    /// Attribut permettant de différencier les différents modes de jeu
    /// </summary>
    protected static Mode gameMode;

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

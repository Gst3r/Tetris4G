using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Auteur : Malcom Kusunga<br>
/// Descrption : Cette classe permet la gestion différentes actions relatives à l’utilisation des boutons (changement de scène, chargement d’interface, ...).
/// </summary>
public class ButtonManager : MonoBehaviour
{   
    /// <summary> 
    /// Attribut contenant le manager du son 
    /// </summary>
    [SerializeField] private SoundManager soundManager;

    /// <summary>
    /// Variable contenant l'animateur lié aux animations exécutées lors de l'appui sur un bouton.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Variable contenant la durée de l'animation s'activant lors d'une pression sur un bouton.
    /// </summary>
    private float pressTime = 0.38f;

    /// <summary>
    /// Variable contenant le bouton start.
    /// </summary>
    [SerializeField] private GameObject startButton;

    /// <summary>
    /// Variable contenant le bouton highscores.
    /// </summary>
    [SerializeField] private GameObject HighScoresButton;

    /// <summary>
    /// Variable contenant le bouton Goals.
    /// </summary>
    [SerializeField] private GameObject GoalsButton;
    
    /// <summary>
    /// Variable contenant le bouton Goals.
    /// </summary>
    [SerializeField] private GameObject TutorialButton;

    /// <summary>
    /// Variable contenant le panel des modes.
    /// </summary>
    [SerializeField] private GameObject modePanel;

    /// <summary>
    /// Variable contenant le panel highscores.
    /// </summary>
    [SerializeField] private GameObject HighScoresPanel;

    /// <summary>
    /// Variable contenant le panel Goals.
    /// </summary>
    [SerializeField] private GameObject GoalsPanel;

    /// <summary>
    /// Variable contenant le panel paramètres.
    /// </summary>
    [SerializeField] private GameObject ParametersPanel;

    /// <summary>
    /// Variable contenant le dropdown des modes de jeu
    /// </summary>
    [SerializeField] private GameObject modeDropDown;

    /// <summary>
    /// Booléen indiquant TRUE si le jeu est lancé, FALSE sinon
    /// </summary>
    private static bool gameIsLoad = false;

    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();   
    }
    
    /// <summary>
    /// Méthode permettant de jouer le son lié à l'utilisation du bouton.
    /// </summary>
    public void PlaySound(AudioClip sound){
        if(soundManager.m_fxEnabled)
        {
            soundManager.PlaySound(sound);
        }
    }

    /// <summary>
    /// Méthode permettant de changer de scène.
    /// </summary>
    public void LaunchScene(string scene){
        if(Time.timeScale == 0f){
            Time.timeScale=1f; 
        }
        StartCoroutine(LoadGame(scene));
    }

    /// <summary>
    /// Coroutine de changement de scène liée au lancement de l'animation (menu principal vers interface de jeu ou inversement).
    /// </summary>
    /// <returns>
    /// Génere une pause de 0.38 secondes.
    /// </returns>
    IEnumerator LoadGame(string scene)
    {
        //Démarage de l'animation
        animator.SetTrigger("Press");

        //Génération de la pause
        float ms = Time.deltaTime;
        while(ms <= pressTime){
            ms += Time.deltaTime;
            yield return null;
        }
        
        //Chargement de la scène
        SceneManager.LoadScene(scene);
    }


    /// <summary>
    /// Méthode permettant de lancer une animation pour les boutons.
    /// </summary>
    public void LaunchStart(){
        if(Time.timeScale == 0f){
            Time.timeScale=1f; 
        }

        StartCoroutine(LaodStart());
    }

    /// <summary>
    /// Coroutine de changement de scène liée au lancement de l'animation (menu principal vers interface de jeu ou inversement).
    /// </summary>
    /// <returns>
    /// Génere une pause de 0.38 secondes.
    /// </returns>
    IEnumerator LaodStart()
    {
        //Démarage de l'animation
        animator.SetTrigger("Press");
        
        //Génération de la pause
        float ms = Time.deltaTime;
        while(ms <= pressTime){
            ms += Time.deltaTime;
            yield return null;
        }

        //Modification de l'interface
        startButton.SetActive(false);
        TutorialButton.SetActive(false);
        HighScoresButton.SetActive(false);
        GoalsButton.SetActive(false);
        modePanel.SetActive(true);;
    }


    /// <summary>
    /// Méthode permettant de lancer une animation pour les boutons.
    /// </summary>
    public void LaunchHighscores(){
        StartCoroutine(LoadHighscores());
    }

    /// <summary>
    /// Méthode permettant de lancer une animation pour les boutons.
    /// </summary>
    public void LaunchGoals(){
        StartCoroutine(LoadGoals());
    }

    /// <summary>
    /// Coroutine de changement de scène liée au lancement de l'animation (menu principal vers interface de jeu ou inversement).
    /// </summary>
    /// <returns>
    /// Génere une pause de 0.38 secondes.
    /// </returns>
    IEnumerator LoadHighscores()
    {
        //Démarage de l'animation
        animator.SetTrigger("Press");
        
        //Génération de la pause
        float ms = Time.deltaTime;
        while(ms <= pressTime){
            ms += Time.deltaTime;
            yield return null;
        }

        //Modification de l'interface
        startButton.SetActive(false);
        HighScoresButton.SetActive(false);
        GoalsButton.SetActive(false);
        TutorialButton.SetActive(false);
        HighScoresPanel.SetActive(true);
        modeDropDown.SetActive(true);
    }


    /// <summary>
    /// Coroutine de changement de scène liée au lancement de l'animation (menu principal vers interface de jeu ou inversement).
    /// </summary>
    /// <returns>
    /// Génere une pause de 0.38 secondes.
    /// </returns>
    IEnumerator LoadGoals()
    {
        //Démarage de l'animation
        animator.SetTrigger("Press");
        
        //Génération de la pause
        float ms = Time.deltaTime;
        while(ms <= pressTime){
            ms += Time.deltaTime;
            yield return null;
        }

        //Modification de l'interface
        startButton.SetActive(false);
        HighScoresButton.SetActive(false);
        GoalsButton.SetActive(false);
        TutorialButton.SetActive(false);
        GoalsPanel.SetActive(true);
    }

    /// <summary>
    /// Méthode permettant de lancer une animation pour les boutons.
    /// </summary>
    public void QuitPamameters()
    {
        StartCoroutine(LaodQuitPamameters());
    }

    /// <summary>
    /// Coroutine de changement de scène liée au lancement de l'animation (menu principal vers interface de jeu ou inversement).
    /// </summary>
    /// <returns>
    /// Génere une pause de 0.38 secondes.
    /// </returns>
    IEnumerator LaodQuitPamameters()
    {
        //Démarage de l'animation
        animator.SetTrigger("Press");
        
        //Génération de la pause
        float ms = Time.deltaTime;
        while(ms <= pressTime){
            ms += Time.deltaTime;
            yield return null;
        }

        //Modification de l'interface
        ParametersPanel.SetActive(false);
    }
    
    /// <summary>
    /// Méthode permettant de définir si le jeu est lancé
    /// </summary>
    public static void SetGameIsLoad(bool value){
        gameIsLoad=value;
    }

    /// <summary>
    /// Méthode permettant de récupérer l'attribut indiquant si le jeu est lancé
    /// </summary>
    public static bool GetGameIsLoad(){
        return gameIsLoad;
    }
}
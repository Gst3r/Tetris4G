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
    /// Variable contenant la source audio.
    /// </summary>
    public AudioSource audioSource;

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
    /// Variable contenant le panel des modes.
    /// </summary>
    [SerializeField] private GameObject modePanel;

    /// <summary>
    /// Variable contenant le panel highscores.
    /// </summary>
    [SerializeField] private GameObject HighScoresPanel;

    void Start()
    {
        //Récupétation de la source audio en relation avec le bouton
        audioSource = GetComponent<AudioSource>();    
    }
    
    /// <summary>
    /// Méthodes permettant de jouer le son lié à l'utilisation du bouton.
    /// </summary>
    public void PlaySound(AudioClip sound){
        this.audioSource.PlayOneShot(sound);
    }

    /// <summary>
    /// Méthodes permettant de changer de scène.
    /// </summary>
    public void LaunchScene(string scene){
        if(Time.timeScale == 0f){
            Time.timeScale=1f; 
        }
        StartCoroutine(LoadGame(scene));
    }

    /// <summary>
    /// Coroutine de changement de scène lié au lancement de l'animation (menu principal vers interface de jeu ou inversement).
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
    /// Méthodes permettant de lancer une animation pour les boutons.
    /// </summary>
    public void LaunchStart(){
        if(Time.timeScale == 0f){
            Time.timeScale=1f; 
        }
        StartCoroutine(LaodStart());
    }

    /// <summary>
    /// Coroutine de changement de scène lié au lancement de l'animation (menu principal vers interface de jeu ou inversement).
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
        HighScoresButton.SetActive(false);
        modePanel.SetActive(true);
    }


    /// <summary>
    /// Méthodes permettant de lancer une animation pour les boutons.
    /// </summary>
    public void LaunchHighscores(){
        if(Time.timeScale == 0f){
            Time.timeScale=1f; 
        }
        StartCoroutine(LaodHighscores());
    }

    /// <summary>
    /// Coroutine de changement de scène lié au lancement de l'animation (menu principal vers interface de jeu ou inversement).
    /// </summary>
    /// <returns>
    /// Génere une pause de 0.38 secondes.
    /// </returns>
    IEnumerator LaodHighscores()
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
        HighScoresPanel.SetActive(true); 
    }
}
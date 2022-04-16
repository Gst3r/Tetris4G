using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> 
/// Auteur : Seghir Nassima, Malcom Kusunga<br>
/// Descrption : Cette classe permet la gestion des événements liés aux boutons du menu Pause 
/// </summary>
public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Attribut contenant le menu Pause 
    /// </summary>
    [SerializeField] GameObject pauseMenu; 

    /// <summary>
    /// Variable contenant l'animateur lié aux animations exécutées lors de l'appui sur un bouton.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Variable contenant la durée de l'animation s'activant lors d'une pression sur un bouton.
    /// </summary>
    private float pressTime = 0.38f;
    
    /// <summary>
    /// Booléen indiquant TRUE si le jeu est en pause, FALSE sinon.
    /// </summary>
    private static bool gameIsPausing;

    /// <summary>
    /// Attribut contenant une instance du controller
    /// </summary>
    public Controller controller;

    public void Start(){
        gameIsPausing=false;
    }
 
    /// <summary> 
    /// Auteur : Seghir Nassima <br>
    /// Méthode qui permet de mettre le jeu en état de pause 
    /// </summary>
    public void Pause()
    {
        pauseMenu.SetActive(true); 
        gameIsPausing=true;
        Time.timeScale=0f; //cette commande permet de d'arreter la progression du temps 
    }
    
    /// <summary> 
    /// Auteurs : Seghir Nassima,Malcom Kusunga <br>
    /// Méthode qui permet de reprendre la partie 
    /// </summary>
    public void Resume()
    {   
        StartCoroutine(ResumeGame());
    }

    /// <summary>
    /// Auteur: Malcom Kusunga
    /// Coroutine liée au lancement de l'animation lorsque le bouton pour continuer la partie est selectionné
    /// </summary>
    /// <returns>
    /// Génere une pause de 0.38 secondes.
    /// </returns>
    IEnumerator ResumeGame()
    {
        //Démarage de l'animation
        animator.SetTrigger("Press");
        //Génération de la pause
        float ms = Time.unscaledDeltaTime;
        while(ms <= pressTime){
            ms += Time.unscaledDeltaTime;
            yield return null;
        }
        
        //Fermeture du Menu pause
        pauseMenu.SetActive(false);

        //Lancement du décompte
        controller.LaunchCount();
    }
    
    /// <summary> 
    /// Auteur : Seghir Nassima <br>
    /// Méthode qui permet de commencer une nouvelle partie 
    /// </summary>
    public void Restart()
    {
        gameIsPausing=false;
        Time.timeScale=1f; //cette commande permet de reprendre la progression normale du temps
        SceneManager.LoadScene(1); 
    }

    /// <summary> 
    /// Méthode qui permet de récupérer la variable indiquant lorsque le jeu est en pause
    /// </summary>
    public static bool GetGameIsPausing(){
        return gameIsPausing;
    }
    
    /// <summary> 
    /// Méthode qui permet de modifier la variable indiquant lorsque le jeu est en pause
    /// </summary>
    public static void SetGameIsPausing(bool value){
        gameIsPausing = value;
    }
}
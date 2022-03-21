using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary> 
/// Auteur : Seghir Nassima<br>
/// Descrption : Cette classe permet la gestion des événements liés aux boutons du menu Pause 
/// </summary>


public class PauseMenu : MonoBehaviour
{
    /// <summary>
    /// Attribut contenant le menu Pause 
    /// </summary>
    [SerializeField] GameObject pauseMenu; 
    

    /// <summary> 
    /// Méthode qui permet de mettre le jeu en état de pause 
    /// Auteur:Seghir Nassima
    /// </summary>
    public void Pause()
    {
        pauseMenu.SetActive(true); 
        Time.timeScale=0f; //cette commande permet de d'arreter la progression du temps 

    }
    

    /// <summary> 
    /// Méthode qui permet de reprendre la partie 
    /// Auteur:Seghir Nassima
    /// </summary>
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale=1f; //cette commande permet de reprendre la progression normale du temps


    }
    
    /// <summary> 
    /// Méthode qui permet de commencer une nouvelle partie 
    /// Auteur:Seghir Nassima
    /// </summary>
    public void Restart()
    {
        Time.timeScale=1f; //cette commande permet de reprendre la progression normale du temps
        SceneManager.LoadScene(1); 


    }
   
}

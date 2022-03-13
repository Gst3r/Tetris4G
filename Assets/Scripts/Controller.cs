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
    /// Variable contenant l'animateur lié aux animations exécutées lors de l'appui sur un bouton.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Variable contenant la durée de l'animation s'activant lors d'un chargement vers la grille de jeu.
    /// </summary>
    public float launchTime = 3f;

    //Interface du décompte
    public GameObject countPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Méthodes permettant de lancer le decompte lors d'un retour vers l'interface de la grille de jeu
    /// </summary>
    public void LaunchCount(){
        StartCoroutine(LoadCount());
    }

    /// <summary>
    /// Coroutine lié au lancement de l'animation lors du décompte
    /// </summary>
    /// <returns>
    /// Génere une pause de 3 secondes.
    /// </returns>
    IEnumerator LoadCount()
    {
        //Démarage de l'animation
        countPanel.SetActive(true);
        
        //Génération de la pause
        float ms = Time.deltaTime;
        while(ms <= launchTime){
            ms += Time. deltaTime;
            yield return null;
        }
        
        //Fermeture de l'interface du décompte
        countPanel.SetActive(false);
    }
}
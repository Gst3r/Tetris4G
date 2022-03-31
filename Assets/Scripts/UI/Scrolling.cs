using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Auteur : Malcom Kusunga<br>
/// Description : Cette classe permet la gestion du background.
/// </summary>
public class Scrolling : MonoBehaviour
{
    /// <summary>
    /// Attribut contenant l'image sur laquelle on applique un effet de mouvement
    /// </summary>
    [SerializeField] private RawImage img;
    
    /// <summary>
    /// Variable contenant la vitesse de déroulement de l'image de fond
    /// </summary>
    [SerializeField] private float abs, ord;
    
    /// <summary>
    /// Booleen permettant de définir lorque le sens dans lequel le background va défiler doit changer
    /// </summary>
    private bool rotate = false;

    /// <summary>
    /// Variable contenant la durée avant le changement de sens de défilement du background
    /// </summary>
    private float nextRotation = 10f;

    /// <summary>
    /// Variable contenant le temps en secondes
    /// </summary>
    private float time;

    private void Awake() {
        time = (int)Time.time;
    }

    void Update()
    {
        //Sens de départ
        if(!rotate){
            //Mise à jour de la position de l'image : effet de mouvement
            img.uvRect = new Rect(img.uvRect.position + new Vector2(abs,ord) * Time.deltaTime,img.uvRect.size);
            time = (int)Time.time;
            //Vérifie si le changement de sens soit être effectué
            if(time > nextRotation){
                rotate = true;
                nextRotation = time + 10f;
            }
        //Sens inverse
        }else{
            //Mise à jour de la position de l'image : effet de mouvement
            img.uvRect = new Rect(img.uvRect.position + new Vector2(-abs,-ord) * Time.deltaTime,img.uvRect.size);
            time = (int)Time.time;
            //Vérifie si le changement de sens soit être effectué
            if(time > nextRotation){
                rotate = false;
                nextRotation = time + 10f;
            }
        }  
    }
}
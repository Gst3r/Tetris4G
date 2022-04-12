using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary> 
/// Auteur : Nassima Seghir<br>
/// Description : Cette classe permet gérer les affichages activés par la liste déroulante des modes   
/// </summary>
public class DropDownManager : MonoBehaviour
{
     /// <summary> 
    /// Attribut faisant référence au panel ultra
    /// </summary>
    [SerializeField] GameObject PanelUltra; 

    /// <summary> 
    /// Attribut faisant référence au panel sprint
    /// </summary>
    [SerializeField] GameObject PanelSprint;

    /// <summary> 
    /// Attribut faisant référence au panel marathon
    /// </summary>
    [SerializeField] GameObject PanelMarathon;

    
    /// <summary> 
    /// Méthode qui permet de gérer les événements liés à laséléction d'un mode dans la liste
    /// Auteur:Seghir Nassima
    /// </summary>
    public void HandleInputData(int val)
    {
        if(val==0) //si marathon est séléctionné 
        {
            PanelMarathon.SetActive(true); 
            PanelUltra.SetActive(false);
            PanelSprint.SetActive(false);   
        }
        if(val==1) //si sprint est séléctionné
        {
            PanelMarathon.SetActive(false);  
            PanelUltra.SetActive(false);
            PanelSprint.SetActive(true);      
        }
        if(val==2) //si ultra est séléctionné 
        {
            PanelMarathon.SetActive(false);
            PanelSprint.SetActive(false);  
            PanelUltra.SetActive(true);
            
            
           
        }   
        
    }  
}

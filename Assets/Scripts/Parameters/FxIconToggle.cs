using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Auteur : Kusunga Malcom<br>
/// Description : Cette classe permet la gestion du son des effets sonores du jeu à travers l'utilisation d'un toggle
/// </summary>
[RequireComponent(typeof(Image))]
public class FxIconToggle : MonoBehaviour {

	/// <summary>
    /// Le sprite utiisé lorsque le Toggle est à True
    /// </summary>
	public Sprite m_iconTrue;

	/// <summary>
    /// Le sprite utiisé lorsque le Toggle est à False
    /// </summary>
	public Sprite m_iconFalse;

	/// <summary>
    /// L'état de base du toggle
    /// </summary>
	public bool m_defaultIconState = true;

	/// <summary>
    /// Le composant Image présent dans le toggle
    /// </summary>
	private Image m_image;

	void Start () {
		//Récupération de l'image dans le toggle
		m_image = GetComponent<Image>();
		m_image.sprite = (m_defaultIconState) ? m_iconTrue : m_iconFalse;

		//Vérifie si des paramètres ont été définis
        if(PlayerPrefs.HasKey("AudioFx"))
        {
			//Application des paramètres enregistrés
            if(PlayerPrefs.GetInt("AudioFx") == 1)
			{
				m_defaultIconState = true;
				//Mise à jour du toggle
				ToggleIcon(m_defaultIconState);
			}else{
				m_defaultIconState = false;
				//Mise à jour du toggle
				ToggleIcon(m_defaultIconState);
			}	
        }else{
            m_defaultIconState = true;
        }
	}

	/// <summary>
    /// Méthode permettant de modfier l'etat du toggle
    /// </summary>
    /// <param name="state">
    /// Booleen definissant si le toggle est actif ou non
    /// </param>
	public void ToggleIcon(bool state)
	{
		m_image.sprite = (state) ? m_iconTrue : m_iconFalse;
	}		
}
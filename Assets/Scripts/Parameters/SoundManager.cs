using UnityEngine;
using System.Collections;

/// <summary> 
/// Auteur : Kusunga Malcom<br>
/// Description : Cette classe permet la gestion des différents son présents dans le jeu
/// </summary>
public class SoundManager : MonoBehaviour {

	/// <summary>
    /// Booléen qui determine si la musique est activée
    /// </summary>
	public bool m_musicEnabled = true;

	/// <summary>
    /// Booléen qui determine si les effets sonores sont actifs
    /// </summary>
	public bool m_fxEnabled = true;

	/// <summary>
    /// Niveau de volume de la musique de fond
    /// </summary>
	[Range(0,1)] public float m_musicVolume = 0.075f;

	/// <summary>
    /// Niveau de volume des effets sonores
    /// </summary>
	[Range(0,1)] public float m_fxVolume = 0.3f;

	/// <summary>
    /// Clip audio de l'effet sonore lié à la destruction d'une ligne
    /// </summary>
	public AudioClip m_clearRowSound;

	/// <summary>
    /// Clip audio de l'effet sonore lié au dépot d'une pièce
    /// </summary>
	public AudioClip m_dropSound;

	/// <summary>
    /// Clip audio lié à la fin de la partie
    /// </summary>
	public AudioClip m_gameOverSound;

	/// <summary>
    /// Source audio de la musique de fond
    /// </summary>
	public AudioSource m_musicSource;

	/// <summary>
    /// Liste de l'ensemble des musique de fond
    /// </summary>
	public AudioClip[] m_musicClips;

	/// <summary>
    /// Toggle permettant de couper la musique
    /// </summary>
	public MusicIconToggle musicIconToggle;

	/// <summary>
    /// Toggle permettant de couper les effets sonores
    /// </summary>
	public FxIconToggle fxIconToggle;

	private void Start () {
		//Vérifie si des paramètres ont été définis
        if(PlayerPrefs.HasKey("Audio"))
        {
			//Application des paramètres enregistrés
            if(PlayerPrefs.GetInt("Audio") == 1)
			{
				m_musicEnabled = true;
			}else{
				m_musicEnabled = false;
			}
        }else{
            m_musicEnabled = true;
			PlayerPrefs.SetInt("Audio",1);
        }

		//Vérifie si des paramètres ont été définis
        if(PlayerPrefs.HasKey("AudioFx"))
        {
			//Application des paramètres enregistrés
            if(PlayerPrefs.GetInt("AudioFx") == 1)
			{
				m_fxEnabled = true;
			}else{
				m_fxEnabled = false;
			}
        }else{
            m_fxEnabled = true;
			PlayerPrefs.SetInt("AudioFx",1);
        }

		//Joue un morceau aléatoire
		PlayBackgroundMusic(GetRandomClip(m_musicClips));
	}

	/// <summary>
    /// Auteur : Kusunga Malcom <br>
    /// Methode permettant sélectionner aléatoirement la musique de fond
    /// </summary>
	/// <returns>
    /// La musique de fond selectionnée dans la liste
    /// </returns>
	public AudioClip GetRandomClip(AudioClip[] clips)
	{
		AudioClip randomClip = clips[Random.Range(0, clips.Length)];
		return randomClip;
	}

	/// <summary>
    /// Auteur : Kusunga Malcom <br>
    /// Methode permettant le lancement de la musique de fond des l'apparition de la scène
    /// </summary>
	public void PlayBackgroundMusic(AudioClip musicClip)
	{
		//Retourne si la musique est désactivée ou si musicSource ou musicClip ne sont pas définis
		if (!m_musicEnabled || !musicClip || !m_musicSource)
		{
			return;
		}

		//Si un son est en train d'être joué, on le stoppe
		m_musicSource.Stop();

		m_musicSource.clip = musicClip;

		//Définie le volume de la musique
		m_musicSource.volume = m_musicVolume;

		//La musique se répète à l'infini
		m_musicSource.loop = true;

		//Lancement de la musique
		m_musicSource.Play();        
	} 

	/// <summary>
    /// Auteur : Kusunga Malcom <br>
    /// Méthode permettant d'activer/desactiver la musique lorsque le joueur utilise le Toggle lié à la musique 
    /// </summary>
	private void UpdateMusic()
	{	
		//Si le paramètre de gestion de la musique a été modifié
		if (m_musicSource.isPlaying != m_musicEnabled) 
		{
			//Active/desactive la musique en fonction de l'option selectionnée par le joueur
			if (m_musicEnabled) 
			{
				PlayBackgroundMusic (GetRandomClip(m_musicClips));
			}
			else {
				m_musicSource.Stop();
			}
		}
	}

	/// <summary>
    /// Auteur : Kusunga Malcom <br>
    /// Methode permettant d'activer/desactiver la musique de fond
    /// </summary>
	public void ToggleMusic()
	{
		//Inversement de la valeur de la variable booléenne
		m_musicEnabled = !m_musicEnabled;
		UpdateMusic();

		//Mise à jour de l'affichage du toggle
		if(musicIconToggle)
		{
			musicIconToggle.ToggleIcon(m_musicEnabled);
		}

		//Modification des paramètres utilisateurs
		if(m_musicEnabled)
		{
			PlayerPrefs.SetInt("Audio",1);
		}else{
			PlayerPrefs.SetInt("Audio",0);
		}
	}

	/// <summary>
    /// Auteur : Kusunga Malcom <br>
    /// Méthode permettant d'activer/desactiver le son des effets sonores
    /// </summary>
	public void ToggleFX()
	{
		//Inversement de la valeur de la variable booléenne
		m_fxEnabled = !m_fxEnabled;

		//Mise à jour de l'affichage du toggle
		if(fxIconToggle)
		{
			fxIconToggle.ToggleIcon(m_fxEnabled);
		}

		//Modification des paramètres utilisateurs
		if(m_fxEnabled)
		{
			PlayerPrefs.SetInt("AudioFx",1);
		}else{
			PlayerPrefs.SetInt("AudioFx",0);
		}
	}

	/// <summary>
    /// Auteur : Kusunga Malcom <br>
    /// Methode permettant de jouer un son avec un multiplicateur de volume
    /// </summary>
	public void PlaySound (AudioClip clip, float volMultiplier = 1.0f)
	{	
		//Vérifie que le volume des effets est bien activée
		if (m_fxEnabled && clip) {
			AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position, Mathf.Clamp(m_fxVolume*volMultiplier,0.05f,1f));
		}
	}
}
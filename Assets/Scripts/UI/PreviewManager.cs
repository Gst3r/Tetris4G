using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Auteur : Jin-Young BAE
/// Classe permettant de contr�ler la zone de pr�visualisation
/// </summary>
public class PreviewManager : MonoBehaviour
{
    /// <summary>
    /// Tableau des sprites des t�trominos
    /// </summary>
    public Sprite[] next;

    /// <summary>
    /// Le sprite actuel de la troisi�me pi�ce pr�visualis�e
    /// </summary>
    private GameObject preview3;

    /// <summary>
    /// Le sprite actuel de la deuxi�me pi�ce pr�visualis�e
    /// </summary>
    private GameObject preview2;

    /// <summary>
    /// Le sprite actuel de la premi�re pi�ce pr�visualis�e
    /// </summary>
    private GameObject preview1;

    /// <summary>
    /// L'indice du prochain t�tromino � jouer 
    /// </summary>
    private int nextPiece;

    /// <summary>
    /// L'indice de la deuxi�me pi�ce pr�visualis�e
    /// </summary>
    private int indice_preview2;

    /// <summary>
    /// L'indice de la troisi�me pi�ce pr�visualis�e
    /// </summary>
    private int indice_preview3;


    void Start()
    {
        //premi�re apparition de la troisi�me pi�ce pr�visualis�e
        preview3 = GameObject.Find("Preview3");
        RandomSprite(preview3);
        Debug.Log(indice_preview3);

        //premi�re apparition de la deuxi�me pi�ce pr�visualis�e
        preview2 = GameObject.Find("Preview2");
        RandomSprite(preview2);

        //premi�re apparition de la premi�re pi�ce pr�visualis�e
        preview1 = GameObject.Find("Preview1");
        RandomSprite(preview1);

    }

    /// <summary>
    /// M�thode permettant la g�n�ration et l'affichage d'un sprite al�atoire dans le tableau next
    /// </summary>
    /// <param name="preview">
    /// Le GameObject dont le sprite sera modifi�
    /// </param>
    public void RandomSprite(GameObject preview)
    {
        //g�n�ration d'un nombre al�atoire
        int random = Random.Range(0, next.Length);

        //r�cuperer l'indice du sprite correspondant :
        //si preview est la premi�re pi�ce pr�visualis�e
        if (preview.Equals(preview1))
        {
            nextPiece = random;
        }
        //si preview est la deuxi�me pi�ce pr�visualis�e
        else if (preview.Equals(preview2))
        {
            indice_preview2 = random;
        }
        //si preview est la troisi�me pi�ce pr�visualis�e
        else if (preview.Equals(preview3))
        {
            indice_preview3 = random;
        }

        //affichage du sprite
        Sprite sprite = next[random];
        ChangeSprite(preview, sprite);
    }

    /// <summary>
    /// M�thode permettant de changer le sprite d'un GameObject par un autre
    /// </summary>
    /// <param name="preview">
    /// Le GameObject dont le Sprite sera modifi�
    /// </param>
    /// <param name="sprite">
    /// Le Sprite qui remplacera celui du preview
    /// </param>
    public void ChangeSprite(GameObject preview, Sprite sprite)
    {
        preview.GetComponent<Image>().sprite = sprite;
    }

    /// <summary>
    /// Methode permettant de changer les sprites de la zone de pr�visualisation
    /// </summary>
    public void ChangePreview()
    {
        //on r�cup�re l'indice du preview2 dans nextPiece
        nextPiece = indice_preview2;
        //on change le sprite du preview1 par celui du preview2
        ChangeSprite(preview1, preview2.GetComponent<Image>().sprite);

        //on remplace l'indice du preview2 par celui du preview3
        indice_preview2 = indice_preview3;
        //on change le sprite du preview2 par celui du preview3
        ChangeSprite(preview2, preview3.GetComponent<Image>().sprite);

        //g�n�ration al�atoire de la troisi�me pi�ce pr�visualis�e
        RandomSprite(preview3);

    }

    /// <summary>
    /// Getter du nombre nextPiece
    /// </summary>
    /// <returns>
    /// nextPiece l'indice du prochain t�tromino � apparaitre sur le board
    /// </returns>
    public int GetNextPiece()
    {
        return nextPiece;
    }

}

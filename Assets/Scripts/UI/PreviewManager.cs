using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Auteur : Jin-Young BAE
/// Classe permettant de contrôler la zone de prévisualisation
/// </summary>
public class PreviewManager : MonoBehaviour
{
    /// <summary>
    /// Tableau des sprites des tétrominos
    /// </summary>
    public Sprite[] next;

    /// <summary>
    /// Le sprite actuel de la troisième pièce prévisualisée
    /// </summary>
    private GameObject preview3;

    /// <summary>
    /// Le sprite actuel de la deuxième pièce prévisualisée
    /// </summary>
    private GameObject preview2;

    /// <summary>
    /// Le sprite actuel de la première pièce prévisualisée
    /// </summary>
    private GameObject preview1;

    /// <summary>
    /// L'indice du prochain tétromino à jouer 
    /// </summary>
    private int nextPiece;

    /// <summary>
    /// L'indice de la deuxième pièce prévisualisée
    /// </summary>
    private int indice_preview2;

    /// <summary>
    /// L'indice de la troisième pièce prévisualisée
    /// </summary>
    private int indice_preview3;


    void Start()
    {
        //première apparition de la troisième pièce prévisualisée
        preview3 = GameObject.Find("Preview3");
        RandomSprite(preview3);
        //Debug.Log(indice_preview3);

        //première apparition de la deuxième pièce prévisualisée
        preview2 = GameObject.Find("Preview2");
        RandomSprite(preview2);

        //première apparition de la première pièce prévisualisée
        preview1 = GameObject.Find("Preview1");
        RandomSprite(preview1);
        Debug.Log(nextPiece);

    }

    /// <summary>
    /// Méthode permettant la génération et l'affichage d'un sprite aléatoire dans le tableau next
    /// </summary>
    /// <param name="preview">
    /// Le GameObject dont le sprite sera modifié
    /// </param>
    public void RandomSprite(GameObject preview)
    {
        //génération d'un nombre aléatoire
        int random = Random.Range(0, next.Length);

        //récuperer l'indice du sprite correspondant :
        //si preview est la première pièce prévisualisée
        if (preview.Equals(preview1))
        {
            nextPiece = random;
        }
        //si preview est la deuxième pièce prévisualisée
        else if (preview.Equals(preview2))
        {
            indice_preview2 = random;
        }
        //si preview est la troisième pièce prévisualisée
        else if (preview.Equals(preview3))
        {
            indice_preview3 = random;
        }

        //affichage du sprite
        Sprite sprite = next[random];
        ChangeSprite(preview, sprite);
    }

    /// <summary>
    /// Méthode permettant de changer le sprite d'un GameObject par un autre
    /// </summary>
    /// <param name="preview">
    /// Le GameObject dont le Sprite sera modifié
    /// </param>
    /// <param name="sprite">
    /// Le Sprite qui remplacera celui du preview
    /// </param>
    public void ChangeSprite(GameObject preview, Sprite sprite)
    {
        preview.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    /// <summary>
    /// Methode permettant de changer les sprites de la zone de prévisualisation
    /// </summary>
    public void ChangePreview()
    {
        //on récupère l'indice du preview2 dans nextPiece
        nextPiece = indice_preview2;
        //on change le sprite du preview1 par celui du preview2
        ChangeSprite(preview1, preview2.GetComponent<SpriteRenderer>().sprite);

        //on remplace l'indice du preview2 par celui du preview3
        indice_preview2 = indice_preview3;
        //on change le sprite du preview2 par celui du preview3
        ChangeSprite(preview2, preview3.GetComponent<SpriteRenderer>().sprite);

        //génération aléatoire de la troisième pièce prévisualisée
        RandomSprite(preview3);

    }

    /// <summary>
    /// Getter du nombre nextPiece
    /// </summary>
    /// <returns>
    /// nextPiece l'indice du prochain tétromino à apparaitre sur le board
    /// </returns>
    public int GetNextPiece()
    {
        return nextPiece;
    }

}

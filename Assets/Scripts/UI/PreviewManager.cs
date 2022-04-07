using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Auteur : Jin-Young BAE
/// Classe permettant de controler la zone de previsualisation
/// </summary>
public class PreviewManager : MonoBehaviour
{
    /// <summary>
    /// Tableau des sprites des tetrominoes
    /// </summary>
    public Sprite[] next;

    /// <summary>
    /// Le sprite actuel de la troisieme piece previsualisee
    /// </summary>
    private GameObject preview3;

    /// <summary>
    /// Le sprite actuel de la deuxieme piece previsualisee
    /// </summary>
    private GameObject preview2;

    /// <summary>
    /// Le sprite actuel de la premiere piece previsualisee
    /// </summary>
    private GameObject preview1;

    /// <summary>
    /// L'indice du prochain tetromino a jouer 
    /// </summary>
    private int nextPiece;

    /// <summary>
    /// L'indice de la deuxieme piece previsualisee
    /// </summary>
    private int indice_preview2;

    /// <summary>
    /// L'indice de la troisieme piece previsualisee
    /// </summary>
    private int indice_preview3;


    void Start()
    {
        //premiere apparition de la troisieme piece previsualisee
        preview3 = GameObject.Find("Preview3");
        RandomSprite(preview3);

        //premiere apparition de la deuxieme piece previsualisee
        preview2 = GameObject.Find("Preview2");
        RandomSprite(preview2);

        //premiere apparition de la premiere piece previsualisee
        preview1 = GameObject.Find("Preview1");
        RandomSprite(preview1);

    }

    /// <summary>
    /// Methode permettant la generation et l'affichage d'un sprite aleatoire dans le tableau next
    /// </summary>
    /// <param name="preview">
    /// Le GameObject dont le sprite sera modifie
    /// </param>
    public void RandomSprite(GameObject preview)
    {
        //generation d'un nombre aleatoire
        int random = Random.Range(0, next.Length);

        //recuperer l'indice du sprite correspondant :
        //si preview est la premiere piece previsualisee
        if (preview.Equals(preview1))
        {
            nextPiece = random;
        }
        //si preview est la deuxieme piece previsualisee
        else if (preview.Equals(preview2))
        {
            indice_preview2 = random;
        }
        //si preview est la troisieme piece previsualisee
        else if (preview.Equals(preview3))
        {
            indice_preview3 = random;
        }

        //affichage du sprite
        Sprite sprite = next[random];
        ChangeSprite(preview, sprite);
    }

    /// <summary>
    /// Methode permettant de changer le sprite d'un GameObject par un autre
    /// </summary>
    /// <param name="preview">
    /// Le GameObject dont le Sprite sera modifie
    /// </param>
    /// <param name="sprite">
    /// Le Sprite qui remplacera celui du preview
    /// </param>
    public void ChangeSprite(GameObject preview, Sprite sprite)
    {
        preview.GetComponent<Image>().sprite = sprite;
    }

    /// <summary>
    /// Methode permettant de changer les sprites de la zone de previsualisation
    /// </summary>
    public void ChangePreview()
    {
        //on recupere l'indice du preview2 dans nextPiece
        nextPiece = indice_preview2;
        //on change le sprite du preview1 par celui du preview2
        ChangeSprite(preview1, preview2.GetComponent<Image>().sprite);

        //on remplace l'indice du preview2 par celui du preview3
        indice_preview2 = indice_preview3;
        //on change le sprite du preview2 par celui du preview3
        ChangeSprite(preview2, preview3.GetComponent<Image>().sprite);

        //generation aleatoire de la troisieme piece previsualisee
        RandomSprite(preview3);

    }

    /// <summary>
    /// Getter du nombre nextPiece
    /// </summary>
    /// <returns>
    /// nextPiece l'indice du prochain tetromino a apparaitre sur le board
    /// </returns>
    public int GetNextPiece()
    {
        return nextPiece;
    }

}

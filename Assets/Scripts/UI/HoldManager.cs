using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Auteur : Jin-Young BAE
/// Description : Script permettant la gestion de la zone de Hold
/// </summary>
public class HoldManager : MonoBehaviour
{

    /// <summary>
    /// Tableau des sprites des t�trominoes + un sprite vide
    /// </summary>
    public Sprite[] tetrominoes;

    /// <summary>
    /// La zone Hold
    /// </summary>
    public GameObject hold;

    /// <summary>
    /// La piece active sur le board
    /// </summary>
    private Piece activePiece;

    /// <summary>
    /// L'indice du tetromino dans la zone de hold
    /// </summary>
    private int hold_indice;

    /// <summary>
    /// Le gestionnaire de la zone de previsualisation
    /// </summary>
    private PreviewManager previewManager;

    /// <summary>
    /// Le gestionnaire du board
    /// </summary>
    private BoardManager board;

    /// <summary>
    /// Booleen repr�sentant le status du hold qui ne peut �tre chang� qu'une fois avant un Lock de la piece active
    /// </summary>
    private bool changed;

    // Start is called before the first frame update
    void Start()
    {
        //recherche des diff�rents �l�ments n�cessaires � ce script
        //hold = GameObject.Find("HoldPiece");
        activePiece = FindObjectOfType<Piece>();
        previewManager = FindObjectOfType<PreviewManager>();
        board = FindObjectOfType<BoardManager>();

        //initialisation du booleen
        changed = false;

    }

    /// <summary>
    /// M�thode permettant de stocker la piece active dans la zone de hold
    /// </summary>
    public void ChangeHold()
    {
        if (changed == false)
        {
            if (hold.GetComponent<Image>().sprite == tetrominoes[7])
            {
                //Debug.Log("1");
                hold_indice = activePiece.GetIndice();
                ChangeSprite(hold_indice);

                NewSpawn(previewManager.GetNextPiece());
                previewManager.ChangePreview();

            }

            else
            {
                int indice_activePiece = activePiece.GetIndice();
                ChangeSprite(indice_activePiece);

                NewSpawn(hold_indice);
                hold_indice = indice_activePiece;

            }

            changed = true;
        }
    }

    /// <summary>
    /// M�thode permettant de changer le sprite de la zone de hold
    /// </summary>
    /// <param name="indice">
    /// L'indice du sprite qui va �tre stock�
    /// </param>
    public void ChangeSprite(int indice)
    {
        hold.GetComponent<Image>().sprite = tetrominoes[indice];
    }

    /// <summary>
    /// Methode permettant d'afficher une nouvelle piece sur le board apr�s avoir effacer la piece actuelle
    /// </summary>
    /// <param name="indice">
    /// L'indice de la nouvelle piece
    /// </param>
    public void NewSpawn(int indice)
    {
        activePiece.Remove();
        board.SpawnPiece(indice, activePiece.GetPiecePosition());
    }

    /// <summary>
    /// Methode permettant de remettre le booleen changed � false, apr�s le positionnement d'une piece
    /// </summary>
    public void SetStatusHold()
    {
        changed = false;
    }

}

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
    /// Tableau des sprites des tetrominoes + un sprite vide
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
    /// Booleen representant le status du hold qui ne peut etre change qu'une fois avant un Lock de la piece active
    /// </summary>
    private bool changed;

    // Start is called before the first frame update
    void Start()
    {
        //recherche des differents elements necessaires a ce script
        activePiece = FindObjectOfType<Piece>();
        previewManager = FindObjectOfType<PreviewManager>();
        board = FindObjectOfType<BoardManager>();

        //initialisation du booleen
        changed = false;

    }

    /// <summary>
    /// Methode permettant de stocker la piece active dans la zone de hold
    /// </summary>
    public void ChangeHold()
    {
        //si le joueur n'a pas deja change la piece de la zone de hold
        if (changed == false)
        {
            //si la case du hold ne contient aucune piece
            if (hold.GetComponent<Image>().sprite == tetrominoes[7])
            {
                //stockage de l'indice de la piece active dans hold_indice
                hold_indice = activePiece.GetIndice();
                //changement du sprite du hold
                ChangeSprite(hold_indice);

                //generation de la prochaine piece de la zone de previsualisation
                NewSpawn(previewManager.GetNextPiece());
                previewManager.ChangePreview();

            }

            //si la case contient une piece
            else
            {
                //on recupere l'indice de la piece active dans une variable
                int indice_activePiece = activePiece.GetIndice();
                //changement du sprite du hold
                ChangeSprite(indice_activePiece);

                //generation de la piece qui etait dans le hold
                NewSpawn(hold_indice);
                hold_indice = indice_activePiece;

            }

            //le joueur a change la piece du hold, il ne peut plus le refaire tant qu'une piece soit posee
            changed = true;
        }
    }

    /// <summary>
    /// Methode permettant de changer le sprite de la zone de hold
    /// </summary>
    /// <param name="indice">
    /// L'indice du sprite qui va etre stocke
    /// </param>
    public void ChangeSprite(int indice)
    {
        hold.GetComponent<Image>().sprite = tetrominoes[indice];
    }

    /// <summary>
    /// Methode permettant d'afficher une nouvelle piece sur le board apres avoir effacer la piece actuelle
    /// </summary>
    /// <param name="indice">
    /// L'indice de la nouvelle piece
    /// </param>
    public void NewSpawn(int indice)
    {
        activePiece.Remove();
        board.SpawnPiece(indice, Pouvoir.Standard);
    }

    /// <summary>
    /// Methode permettant de remettre le booleen changed a false, apres le positionnement d'une piece
    /// </summary>
    public void SetStatusHold()
    {
        changed = false;
    }

}

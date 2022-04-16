using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary> 
/// Auteur : Kusunga Malcom<br>
/// Description : Cette classe permet de controler la "ghost piece" du tertomino actuellement controllé par le joueur. 
/// </summary>
public class GhostPiece : MonoBehaviour
{
    /// <summary> 
    /// Attribut contenant l'apparance des tiles de la ghost piece
    /// </summary>
    [SerializeField] private Tile tile;

    /// <summary> 
    /// Attribut contenant le plateau de jeu principal
    /// </summary>
    [SerializeField] private BoardManager board;

    /// <summary> 
    /// Attribut contenant la pièce actuellement sur la grille de jeu
    /// </summary>
    [SerializeField] private Piece pieceOnBoard;

    /// <summary> 
    /// Attribut contenant le plateau de jeu secondaire (utilisé pour la ghost piece)
    /// </summary>
    [SerializeField] private Tilemap tilemap {get; set;}
    
    /// <summary> 
    /// Attribut contenant les cellules de la ghost piece
    /// </summary>
    public Vector3Int[] cells { get; private set; }

    /// <summary> 
    /// Attribut contenant la position de la ghost piece 
    /// </summary>
    public Vector3Int position { get; private set; }

    private void Awake()
    {   
        //Récupération de la Tilemap
        this.tilemap = GetComponentInChildren<Tilemap>();

        //Initialise la liste des cellules de la ghost piece
        this.cells = new Vector3Int[4];
    }

    private void LateUpdate() {
        //Retire la ghost piece de la gille de jeu
        ClearGhost();

        //Mise à jour des cellules de la ghost piece
        CopyPieceOnBoard();
        
        //Place la pièce à l'extremite de la grille dans le sens de la gravite
        ProjectPiece();

        //Place la ghost piece dans gille de jeu
        SetGhost();
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom
    /// Méthode permettant de définir la ghost piece sur la grille de jeu secondaire
    /// </summary>
    public void SetGhost()
    {
        //Parcours des cellules de la ghost piece
        for (int i = 0; i < this.cells.Length; i++)
        {
            //Récupération de la position de la cellule
            Vector3Int tilePosition = this.cells[i] + this.position;
            //Enregistrement de la cellule sur la grille de jeu
            this.tilemap.SetTile(tilePosition, this.tile);
        }
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom
    /// Méthode permettant de supprimer la ghost piece de la grille de jeu secondaire
    /// </summary>
    public void ClearGhost()
    {
        //Parcours des cellules de la ghost piece
        for (int i = 0; i < this.cells.Length; i++)
        {
            //Récupération de la position de la cellule
            Vector3Int tilePosition = this.cells[i] + this.position;
            //Suppression de la cellule sur la grille de jeu
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom
    /// Méthode permettant de supprimer la ghost piece de la grille de jeu 
    /// </summary>
    public void CopyPieceOnBoard()
    {
        //Parcours des cellules de la ghost piece
        for (int i = 0; i < this.cells.Length; i++)
        {
            //Copie des données de la pièce sur la grille dans la ghost piece
            this.cells[i] = this.pieceOnBoard.cells[i];
        }
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom, Seghir Nassima, Bae Jin-Young
    /// Méthode permettant de projeter la ghost piece dans le sens exercé par la gravité  
    /// </summary>
    public void ProjectPiece()
    {
        //Récupération de la position de la piece actuellement présente sur la grille de jeu
        Vector3Int position = this.pieceOnBoard.position;
        
        //GRAVITE BAS
        if(board.GetGravity() == Gravity.BAS){

            //Récupération de la ligne sur laquelle se trouve la piece actuelle
            int currentRow = position.y;

            //Récupération de la dernière ligne de la grille de jeu
            int lastRow = -this.board.GetSize().y / 2 - 1;

            //Efface temporairement la piece actuelle de la grille de jeu au cas où il y aurait superposition avec la ghost piece
            this.board.Clear(this.pieceOnBoard);

            for(int row = currentRow; row >= lastRow; row--)
            {
                position.y = row;

                //Vérification de la possible position pour la ghost piece
                if(this.board.ValiderPosition(this.pieceOnBoard,position)){
                    this.position = position;
                }else{
                    break;
                }
            }

            //Replace la pièce actuelle dans la grille de jeu
            if (this.pieceOnBoard.GetPouvoir() == Pouvoir.Standard)
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().tile);
            }
            else if (this.pieceOnBoard.GetPouvoir() == Pouvoir.Malus)
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().malus_tile);
            }
            else
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().bonus_tile);
            }
        }

        //GRAVITE HAUT
        else if(board.GetGravity() == Gravity.HAUT){

            //Récupération de la ligne sur laquelle se trouve la piece actuelle
            int currentRow = position.y;

            //Récupération de la première ligne de la grille de jeu
            int firstRow = this.board.GetSize().y / 2 + 1;

            //Efface temporairement la piece actuelle de la grille de jeu au cas où il y aurait superposition avec la ghost piece
            this.board.Clear(this.pieceOnBoard);

            for(int row = currentRow; row <= firstRow; row++)
            {
                position.y = row;

                //Vérification de la possible position pour la ghost piece
                if(this.board.ValiderPosition(this.pieceOnBoard,position)){
                    this.position = position;
                }else{
                    break;
                }
            }

            //Replace la pièce actuelle dans la grille de jeu
            if (this.pieceOnBoard.GetPouvoir() == Pouvoir.Standard)
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().tile);
            }
            else if (this.pieceOnBoard.GetPouvoir() == Pouvoir.Malus)
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().malus_tile);
            }
            else
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().bonus_tile);
            }
        }

        //GRAVITE GAUCHE
        else if(board.GetGravity() == Gravity.GAUCHE){

            //Récupération de la colonne sur laquelle se trouve la pièce actuelle
            int currentCol = position.x;

            //Récupération de la première ligne de la grille de jeu
            int firstCol = -this.board.GetSize().x / 2 - 1;

            //Efface temporairement la pièce actuelle de la grille de jeu au cas où il y aurait superposition avec la ghost piece
            this.board.Clear(this.pieceOnBoard);

            for(int col = currentCol; col >= firstCol; col--)
            {
                position.x = col;

                //Vérification de la possible position pour la ghost piece
                if(this.board.ValiderPosition(this.pieceOnBoard,position)){
                    this.position = position;
                }else{
                    break;
                }
            }

            //Replace la pièce actuelle dans la grille de jeu
            if (this.pieceOnBoard.GetPouvoir() == Pouvoir.Standard)
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().tile);
            }
            else if (this.pieceOnBoard.GetPouvoir() == Pouvoir.Malus)
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().malus_tile);
            }
            else
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().bonus_tile);
            }
        }

        //GRAVITE DROITE
        else if(board.GetGravity() == Gravity.DROITE){

            //Récupération de la colonne sur laquelle se trouve la pièce actuelle
            int currentCol = position.x;

            //Récupération de la première ligne de la grille de jeu
            int lastCol = this.board.GetSize().x / 2 - 1;

            //Efface temporairement la pièce actuelle de la grille de jeu au cas où il y aurait superposition avec la ghost piece
            this.board.Clear(this.pieceOnBoard);

            for(int col = currentCol; col <= lastCol; col++)
            {
                position.x = col;

                //Vérification de la possible position pour la ghost piece
                if(this.board.ValiderPosition(this.pieceOnBoard,position)){
                    this.position = position;
                }else{
                    break;
                }
            }

            //Replace la pièce actuelle dans la grille de jeu
            if (this.pieceOnBoard.GetPouvoir() == Pouvoir.Standard)
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().tile);
            }
            else if (this.pieceOnBoard.GetPouvoir() == Pouvoir.Malus)
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().malus_tile);
            }
            else
            {
                this.board.Set(this.pieceOnBoard, this.pieceOnBoard.GetData().bonus_tile);
            }
        }
    }
}
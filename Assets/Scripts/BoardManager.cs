using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary> 
/// Auteur : Sterlingot Guillaume<br>
/// Description : Cette enumération permet de distinguer les quatres gravités existantes dans le jeu
/// </summary>
public enum Gravity 
{
    HAUT,
    BAS,
    GAUCHE,
    DROITE
}

/// <summary> 
/// Auteur : Sterlingot Guillaume, Bae Jin-Young, Nassima Seghir, Malcom Kusunga<br>
/// Description : Cette classe permet de la gestion de l'ensemble de la grille de jeu
/// </summary>
public class BoardManager : MonoBehaviour
{
    /// <summary> 
    /// Attribut contenant le plateau de jeu 
    /// </summary>
    [SerializeField] private Tilemap board { get; set; }

    /// <summary> 
    /// Attribut contenant la pièce courante qui est présente sur le plateau de jeu 
    /// </summary>
    [SerializeField] private Piece activePiece;

     /// <summary> 
    /// Attribut contenant la position d'apparition initiale de la pièce 
    /// </summary>
    [SerializeField] private Vector3Int spawnPosition;


    
    /// <summary> 
    /// Attribut contenant la gravité courante exercée
    /// </summary>
    private Gravity gravity;

    /// <summary> 
    /// Attribut indiquant le nombre de ligne du plateau de jeu 
    /// </summary>
    private int nb_row;

    
    /// <summary> 
    /// Attribut indiquant le nombre de colonne du plateau de jeu 
    /// </summary>
    private int nb_col;


    /// <summary> 
    /// Attribut indiquant la taille totale du plateau de jeu
    /// </summary>
    private Vector2Int size;

    /// <summary> 
    /// Tableau des données des tetrominoes
    /// </summary>
    [SerializeField] private TetrominoData[] tetrominoes; 


    /// <summary> 
    /// Attribut définissant le rectangle qui délimite la grille de jeu 
    /// </summary>
    public RectInt Bornes {
        get{
             Vector2Int position = new Vector2Int(-size.x / 2, -size.y / 2);
             return new RectInt(position, size);
        }
    }




    private async void Awake()
    {
        SetupBoard();
        for(int i=0; i< this.tetrominoes.Length; i++)
        {
           this.tetrominoes[i]=TetrominoBuilder.Build(this.tetrominoes[i]); 

        }
    }

    private void Start(){
        chooseRandomGravity();
        SpawnPiece();
    }

    private void Update()
    {
        /*if(HaveCollision()){
            ClearLine();
            SpawnPiece();
        }*/
    }

    /// <summary> 
    /// Méthode qui permet de générer une piece aléatoirement 
    /// </summary>
    public void SpawnPiece(){
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data= this.tetrominoes[random]; 

        this.activePiece.Initialize(this, spawnPosition, data); 
        Set(activePiece); 
    }

    /// <summary> 
    /// Méthode qui permet de fixer la piece sur la grille de jeu 
    /// </summary>
    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            board.SetTile(tilePosition, piece.data.tile);
        }
    }

    /// <summary> 
    /// Méthode qui permet d'effacer la piece de la grille de jeu  
    /// </summary>
    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            board.SetTile(tilePosition, null);
        }
    }

    /// <summary> 
    /// Méthode qui permet de détecter si une collision a lieu entre un tetromino et le bord/une pièce de la grille de jeu
    /// </summary>
    public bool HaveCollision(){
        return false;
    }
    
    /// <summary> 
    /// Méthode qui permet de détruire la ligne complété et d'appliquer la gravité sur toute la grille
    /// </summary>
    public bool ClearLine(){
        return false;
    }

    /// <summary> 
    /// Méthode qui permet d'initialiser les paramètres du plateau de jeu
    /// </summary>
    public void SetupBoard(){
        this.board = GetComponentInChildren<Tilemap>();
        this.activePiece=GetComponentInChildren<Piece>();
        this.nb_col = 14;
        this.nb_row = 22;
        this.size = new Vector2Int(14,22);
    }

    /// <summary> 
    /// Méthode qui permet de vérifier si une ligne a été complétée dans la grille de jeu
    /// </summary>
    /// <returns>
    /// un booléen qui indique TRUE si une ligne a été complétée sur la grille de jeu, FALSE sinon
    /// </returns>
    public bool RowIsComplete(int row)
    {
        RectInt bornes = Bornes;

        for (int col = bornes.xMin; col < bornes.xMax; col++)
        {
            Vector3Int cur_pos = new Vector3Int(col, row, -2);

            if (!board.HasTile(cur_pos))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary> 
    /// Méthode qui permet de détruire la ligne complétée et d'appliquer la gravité sur toute la grille
    /// </summary>
    public void ClearRow(int row)
    {
        RectInt bornes = Bornes;

        //Destruction de la ligne complète 
        for (int col = bornes.xMin; col < bornes.xMax; col++)
        {
            Vector3Int cur_pos = new Vector3Int(col, row, -2);
            board.SetTile(cur_pos, null);
        }

        //Application de la gravité
        switch (gravity)
        {
            //si la gravité s'applique vers le bas
            case Gravity.BAS:
                while (row < -2)
                {
                    for (int col = bornes.xMin; col < bornes.xMax; col++)
                    {
                        //position de la cellule à déplacer
                        Vector3Int position = new Vector3Int(col, row + 1, -2);
                        TileBase previous = board.GetTile(position);
                        board.SetTile(position, null);

                        //nouvelle position
                        Vector3Int new_position = new Vector3Int(col, row, -2);
                        board.SetTile(new_position, previous);
                    }
                    row++;
                }
                break;

            // si la gravité s'applique vers le haut
            case Gravity.HAUT:
                while (row > 2)
                {
                    for (int col = bornes.xMin; col < bornes.xMax; col++)
                    {
                        //position de la cellule à déplacer
                        Vector3Int position = new Vector3Int(col, row - 1, -2);
                        TileBase previous = board.GetTile(position);
                        board.SetTile(position, null);

                        //nouvelle position
                        Vector3Int new_position = new Vector3Int(col, row, -2);
                        board.SetTile(new_position, previous);
                    }
                    row--;
                }
                break;

        }
    }

    /// <summary> 
    /// Méthode qui permet de vérifier si une colonne a été complétée dans la grille de jeu
    /// </summary>
    /// <returns>
    /// un booléen qui indique TRUE si une colonne a été complétée sur la grille de jeu, FALSE sinon
    /// </returns>
    public bool ColIsComplete(int col)
    {
        RectInt bornes = Bornes;

        for (int row = bornes.yMin; row < bornes.yMax; row++)
        {
            Vector3Int cur_pos = new Vector3Int(col, row, -2);

            if (!board.HasTile(cur_pos))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary> 
    /// Méthode qui permet de détruire la colonne complétée et d'appliquer la gravité sur toute la grille
    /// </summary>
    public void ClearCol(int col)
    {
        RectInt bornes = Bornes;

        // Destruction de la colonne complète
        for (int row = bornes.yMin; row < bornes.yMax; row++)
        {
            Vector3Int cur_pos = new Vector3Int(col, row, -2);
            board.SetTile(cur_pos, null);
        }

        // Application de la gravité
        switch (gravity)
        {
            //si la gravité s'applique vers la gauche
            case Gravity.GAUCHE:
                while (col < 2)
                {
                    for (int row = bornes.yMin; row < bornes.yMax; row++)
                    {
                        //position de la cellule à déplacer
                        Vector3Int position = new Vector3Int(col + 1, row, -2);
                        TileBase previous = board.GetTile(position);
                        board.SetTile(position, null);

                        //nouvelle position 
                        Vector3Int new_position = new Vector3Int(col, row, -2);
                        board.SetTile(new_position, previous);
                    }
                    col++;
                }
                break;

            //si la gravité s'applique vers la droite
            case Gravity.DROITE:
                while (col > -2)
                {
                    for (int row = bornes.yMin; row < bornes.yMax; row++)
                    {
                        //position de la cellule à déplacer
                        Vector3Int position = new Vector3Int(col - 1, row, -2);
                        TileBase previous = board.GetTile(position);
                        board.SetTile(position, null);

                        //nouvelle position
                        Vector3Int new_position = new Vector3Int(col, row, -2);
                        board.SetTile(new_position, previous);
                    }
                    col--;
                }
                break;
        }
    }

    /// <summary>
    /// Méthode permettant de détruire une ligne/colonne complétée
    /// </summary>
    public void ClearCompleteLine()
    {
        RectInt bornes = Bornes;

        int row = bornes.yMin;
        while (row < bornes.yMax)
        {

            if (RowIsComplete(row))
            {
                ClearRow(row);
            }

            else
            {
                row++;
            }
        }

        int col = bornes.xMin;
        while (col < bornes.xMax)
        {

            if (ColIsComplete(col))
            {
                ClearCol(col);
            }

            else
            {
                col++;
            }
        }

    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode qui permet la sélection aléatoire d'une des quatres gravités existantes
    /// </summary>
    public void chooseRandomGravity(){
        int randomNumber = Random.Range(0,4);
        switch (randomNumber)
        {
            case 0: this.gravity = Gravity.HAUT;
                    break;
            case 1: this.gravity = Gravity.BAS;
                    break;
            case 2: this.gravity = Gravity.GAUCHE;
                    break;
            case 3: this.gravity = Gravity.DROITE;
                    break;
            default: this.gravity = Gravity.BAS;
                    break;
        }
    }

    /// <summary> 
    /// Auteur : Seghir Nassima 
    /// Description : Méthode qui permet de vérifier si une position est valide 
    /// </summary>
    /// <returns>
    /// un booléen qui indique FALSE si la poisition est occupée par un tetromino, 
    /// FALSE si la position est au-dela des limites de la grille, TRUE sinon
    /// </returns>

    public bool validerPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = Bornes;

        // la méthode vérifie la validité de chaque cellule 
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            // au-dela des limites donc FALSE
            if (!bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            // occupé par une autre tuile donc FALSE 
            if (board.HasTile(tilePosition)) {
                return false;
            }
        }

        return true;
    }

    public bool ValideTilePos(Vector3Int tilePos){
        RectInt bounds = Bornes;

        if (!bounds.Contains((Vector2Int)tilePos))
            return false;
        
        return true;
    }

    public Gravity GetGravity(){
        return gravity;
    }
}
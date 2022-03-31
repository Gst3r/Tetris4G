using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


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
//------------------------------------------------------------------BOARD------------------------------------------------

    /// <summary> 
    /// Attribut contenant le plateau de jeu 
    /// </summary>
    [SerializeField] private Tilemap board;

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

    
//------------------------------------------------------------------SCORE-------------------------------------------------

    /// <summary> 
    /// Attribut contenant le score de la partie en cours 
    /// </summary>
    [SerializeField] private float score=0;

    /// <summary> 
    /// Attribut contenant le score de la partie en cours sous forme de texte
    /// </summary>
    [SerializeField] private Text scoreText;

//------------------------------------------------------------------COTE COMPLET-----------------------------------------
   
    /// Booléen permettant de determiner si le haut de la grille est complet 
    /// </summary>
    private bool topIsFull { get; set; }

    /// <summary> 
    /// Booléen permettant de determiner si le bas de la grille est complet
    /// </summary>
    private bool botIsFull { get; set; }

    /// <summary> 
    /// Booléen permettant de determiner si la gauche de la grille est complete
    /// </summary>
    private bool leftIsFull { get; set; }

    /// <summary> 
    /// Booléen permettant de determiner si la droite de la grille est complete
    /// </summary>
    private bool rightIsFull { get; set; }
   
//---------------------------------------------------------------------------------------------------------------------

    private async void Awake()
    {
        SetupBoard();
        for(int i=0; i< this.tetrominoes.Length; i++)
        {
           this.tetrominoes[i]=TetrominoBuilder.Build(this.tetrominoes[i]); 

        }
    }

    private void Start(){
        topIsFull = false;
        botIsFull = false;
        leftIsFull = false;
        rightIsFull = false;
        chooseRandomGravity();
        SpawnPiece();
    }

    private void Update()
    {

        //scoreText.text= score.ToString(); //permet de mettre à jour le score affiché 
        /*if(HaveCollision()){
            ClearLine();
            SpawnPiece();
        }*/
    }

//-------------------------------------------------------------------------------------------------------------

    /// <summary> 
    /// Méthode qui permet de générer une piece aléatoirement 
    /// Auteur:Seghir Nassima
    /// </summary>
    public void SpawnPiece(){
        int random = Random.Range(0, tetrominoes.Length);
        TetrominoData data= this.tetrominoes[random]; 

        

        this.activePiece.Initialize(this, spawnPosition, data); 
        Set(activePiece); 
    }

    /// <summary> 
    /// Méthode qui permet de fixer la piece sur la grille de jeu 
    /// Auteur:Seghir Nassima
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
    /// Auteur:Seghir Nassima 
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
    /// Méthode qui permet d'initialiser les paramètres du plateau de jeu
    /// </summary>
    public void SetupBoard(){
        this.board = GetComponentInChildren<Tilemap>();
        this.activePiece=GetComponentInChildren<Piece>();
        this.size = new Vector2Int(16,22);
    }

    /// <summary> 
    /// Auteur : Jin-Young BAE
    /// Méthode qui permet de vérifier si une ligne a été complétée dans la grille de jeu
    /// </summary>
    /// <returns>
    /// un booléen qui indique TRUE si une ligne a été complétée sur la grille de jeu, FALSE sinon
    /// </returns>
    public bool RowIsComplete(int row)
    {
        RectInt bornes = Bornes;

        // pour toutes les cellules d'une ligne :
        for (int col = bornes.xMin; col < bornes.xMax; col++)
        {
            Vector3Int cur_pos = new Vector3Int(col, row, -2);

            // si la cellule n'est pas remplie, la ligne n'est pas complète
            if (!board.HasTile(cur_pos))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary> 
    /// Auteur : Jin-Young BAE
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

        //si la ligne se trouve sur la partie basse de l'écran
        if (row < 0)
        {
            while (row < -3)
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
        }

        // si la ligne se trouve sur la partie haute de l'écran
        if (row > 0)
        {
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

        }
    }

    /// <summary> 
    /// Auteur : Jin-Young BAE
    /// Méthode qui permet de vérifier si une colonne a été complétée dans la grille de jeu
    /// </summary>
    /// <returns>
    /// un booléen qui indique TRUE si une colonne a été complétée sur la grille de jeu, FALSE sinon
    /// </returns>
    public bool ColIsComplete(int col)
    {
        RectInt bornes = Bornes;

        // pour toutes les cellules d'une colonne :
        for (int row = bornes.yMin; row < bornes.yMax; row++)
        {
            Vector3Int cur_pos = new Vector3Int(col, row, -2);

            // si une cellule n'est pas remplie, la colonne n'est pas complète
            if (!board.HasTile(cur_pos))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary> 
    /// Auteur : Jin-Young BAE
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

        //si la colonne se trouve sur la partie gauche de l'écran
        if (col < 0)
        {
            while (col < -3)
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
        }

        //si la colonne se trouve sur la partie droite de l'écran
        if (col > 0)
        {
            while (col > 2)
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
        }
    }
    /// <summary>
    /// Auteur : Seghir Nassima 
    /// Méthode permettant d'incrémenter le score selon le nombre de lignes éliminées
    /// </summary>
    public void incrementScore(int nmbLines)
    {
        switch(nmbLines)
        {
            case 1: 
                score+=40; 
                break; 

            case 2:
                score+=100; 
                break; 

            case 3: 
                score+=300; 
                break; 

            case 4:
                score+=1200; 
                break; 

            default: 
                 break;  
        }

    }


    /// <summary>
    /// Auteur : Jin-Young BAE
    /// Méthode permettant de détruire une ligne/colonne complétée
    /// </summary>
    public void ClearCompleteLine()
    {
        RectInt bornes = Bornes;
        int nbLinesCleared=0; //correspond au nombre de lignes effacées au total

        // les lignes :
        int row = bornes.yMin;
        while (row < bornes.yMax)
        {
            // si une ligne est complète, elle est détruite
            if (RowIsComplete(row))
            {
                ClearRow(row);
                nbLinesCleared++; 
            }

            // sinon on passe à la prochaine ligne
            else
            {
                row++;
            }
        }

        // les colonnes :
        int col = bornes.xMin;
        while (col < bornes.xMax)
        {
            // si une colonne est complète, elle est détruite
            if (ColIsComplete(col))
            {
                ClearCol(col);
                nbLinesCleared++;
            }

            else
            {
                col++;
            }
        }
        incrementScore(nbLinesCleared); 
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

    /// <summary> 
    /// Auteur : Sterlingot Guillaume 
    /// Description : Méthode qui permet de vérifier si la position d'une tile est valide 
    /// </summary>
    /// <returns>
    /// un booléen qui indique FALSE si la position est occupée par un tetromino, 
    /// FALSE si la position est au-dela des limites de la grille, TRUE sinon
    /// </returns>
    public bool ValideTilePos(Vector3Int tilePos){
        RectInt bounds = Bornes;// On récupère les bornes du plateau

        //Si la position de la tile dépace les limites où se confond avec une autre tile alors on retourne FALSE pour indiquer que la position est incorrect
        if (!bounds.Contains((Vector2Int)tilePos) || board.HasTile(tilePos)) 
            return false;
        //Sinon on retourne TRUE pour indiquer que la position est correct
        return true;
    }

    /// <summary>
    /// Auteur : Jin-Young BAE
    /// Description : Méthode permettant de vérifier si un côté de l'écran est rempli
    /// </summary>
    /// <returns>
    /// Booléen qui return TRUE si un côté est rempli, retourne FALSE sinon
    /// </returns>
    public bool FullSide(int num_row_col, bool gravity_is_vertical)
    {
        // pour vérifier les côtés HAUT et BAS :
        if (gravity_is_vertical)
        {
            int row = num_row_col;
            // parmi les cellules se trouvant à la bordure de la zone d'apparition
            for (int col = -2; col < 2; col++)
            {
                Vector3Int pos = new Vector3Int(col, row, -2);

                // si une cellule est remplie alors on retourne TRUE pour bloquer la gravité
                if (board.HasTile(pos))
                {   
                    //Vérifie si il s'agit de la gravité vers le haut ou la bas
                    if(row == -3){
                        botIsFull = true;
                    }
                    else{
                        topIsFull = true; 
                    }    

                    return true;
                }
            }
        }

        // pour vérifier les côtés DROITE et GAUCHE :
        else
        {
            int col = num_row_col;
            // parmi les cellules se trouvant à la bordure de la zone d'apparition
            for (int row = -2; row < 2; row++)
            {
                Vector3Int pos = new Vector3Int(col, row, -2);

                // si une cellule est remplie alors on retourne TRUE pour bloquer la gravité
                if (board.HasTile(pos))
                {   
                    //Vérifie si il s'agit de la gravité vers la droite ou la gauche
                    if(col == -3){
                        leftIsFull = true;
                    }
                    else{
                        rightIsFull = true;
                    }
                    
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Auteur : Malcom Kusunga
    /// Description : Méthode permettant de vérifier si tous les côtés sont remplis
    /// </summary>
    public void FullSides(){
        //Vérifie si le côté bas est déjà complet
        if(!botIsFull)
            FullSide(-3, true);
        
        //Vérifie si le côté haut est déjà complet
        if(!topIsFull)
            FullSide(2, true);
        
        //Vérifie si le côté gauche est déjà complet
        if(!leftIsFull)
            FullSide(-3, false);
        
        //Vérifie si le côté droit est déjà complet
        if(!rightIsFull)
            FullSide(2, false);
    }


    /// <summary>
    /// Auteur : Jin-Young BAE
    /// Méthode permettant de bloquer les gravités pour les cotés complets
    /// </summary>
    public void StopGravity()
    {
        // bloquer la gravité BAS :
        /*if (FullSide(-3, true) && gravity == Gravity.BAS)
        {
            // rappel du générateur de gravité et rappel de la méthode au cas où le générateur re-sélectionne la gravité BAS
            chooseRandomGravity();
            StopGravity();
        }

        // bloquer la gravité HAUT:
        else if (FullSide(2, true) && gravity == Gravity.HAUT)
        {
            chooseRandomGravity();
            StopGravity();
        }

        // bloquer la gravité GAUCHE :
        else if (FullSide(-3, false) && gravity == Gravity.GAUCHE)
        {
            chooseRandomGravity();
            StopGravity();
        }

        // bloquer la gravité DROITE :
        else if (FullSide(2, false) && gravity == Gravity.DROITE)
        {
            chooseRandomGravity();
            StopGravity();
        }*/

        //Selection de la gravité en fonction des gravités restantes
        if(!topIsFull || !botIsFull || !leftIsFull || !rightIsFull){
            do{
                chooseRandomGravity();
            }while( // bloquer la gravité BAS
                    (FullSide(-3, true) && gravity == Gravity.BAS)  ||

                   // bloquer la gravité HAUT
                   (FullSide(2, true)  && gravity == Gravity.HAUT)  || 

                   // bloquer la gravité GAUCHE
                   (FullSide(-3, false)&& gravity == Gravity.GAUCHE)|| 

                   // bloquer la gravité DROITE
                   (FullSide(2, false) && gravity == Gravity.DROITE));
        }
    }

    /// <summary>
    /// Auteur : Jin-Young BAE
    /// Méthode permettant de nettoyer la zone d'apparition pour laisser la place aux nouvelles pièces
    /// </summary>
    public void ClearApparitionZone()
    {
        // pour les lignes de la zone d'appartition
        for (int row = -2; row < 2; row++)
        {
            // pour les colonnes de la zone d'apparition
            for (int col = -2; col < 2; col++)
            {
                Vector3Int pos = new Vector3Int(col, row, -2);

                // si une cellule est remplie, on la supprime
                if (board.HasTile(pos))
                {
                    board.SetTile(pos, null);
                }
            }
        }
    }
// Getters

    public static float getScore()
    {
        return score; 
    }

    public bool GetTopIsFull(){
        return topIsFull;
    }
    
    public bool GetBotIsFull(){
        return botIsFull;
    }

    public bool GetLeftIsFull(){
        return leftIsFull;
    }
    
    public bool GetRightIsFull(){
        return rightIsFull;
    }
    
    public Gravity GetGravity(){
        return gravity;
    }

    public Piece GetActivePiece(){
        return activePiece;
    }
}
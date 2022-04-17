using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

/// <summary> 
/// Auteurs : Seghir Nassima, Kusunga Malcom, Sterlingot Guillaume, Bae Jin-Young<br>
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
/// Auteurs : Sterlingot Guillaume, Bae Jin-Young, Nassima Seghir, Malcom Kusunga<br>
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
    /// Attribut contenant le manager du score  
    /// </summary>
    [SerializeField] private ScoreManager scoreManager;

    /// <summary> 
    /// Attribut contenant la position d'apparition initiale de la pièce 
    /// </summary>
    [SerializeField] private Vector3Int spawnPosition;

    /// <summary> 
    /// Attribut contenant la gravité courante exercée
    /// </summary>
    private static Gravity gravity;

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

    /// <summary>
    /// Le temps depuis le début de la partie en secondes
    /// </summary>
    private static float time;

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


    /// <summary> 
    /// Attribut contenant le nombre de lignes élinminées au total
    /// </summary>
    private static int  totalLinesCleared =0; 

    /// <summary> 
    /// Attribut indiquant si l'accelération de la gravité est veoruillée    
    /// </summary>
    private static bool lockSpeed;
   
//------------------------------------------------------------------Volume-----------------------------------------

    /// <summary> 
    /// Attribut contenant le manager du son 
    /// </summary>
    [SerializeField] private SoundManager soundManager;

//---------------------------------------------------------------------------------------------------------------------

    private void Awake()
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
        time = 0f;
        chooseRandomGravity();
        if(SceneManager.GetActiveScene().name=="Tutorial"){
            SpawnPiece(3, Pouvoir.Standard);
        }else{
            int random = Random.Range(0, tetrominoes.Length);
            SpawnPiece(random, Pouvoir.Standard);
        }
        
    }

    //-------------------------------------------------------------------------------------------------------------

    /// <summary> 
    /// Méthode qui permet de générer une piece aléatoirement 
    /// Auteur:Seghir Nassima, Bae Jin-Young
    /// </summary>
    /// <param name="random">
    /// l'indice du tetromino dans le tableau de tetrominoes
    /// </param>
    /// <param name="pouvoir">
    /// le pouvoir de la piece
    /// </param>
    public void SpawnPiece(int random, Pouvoir pouvoir)
    {
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, spawnPosition, data, random, pouvoir);

        if (pouvoir == Pouvoir.Standard)
        {
            Set(activePiece, data.tile);
        }
        else if (pouvoir == Pouvoir.Malus)
        {
            Set(activePiece, data.malus_tile);
        }
        else
        {
            Set(activePiece, data.bonus_tile);
        }
    }

    /// <summary> 
    /// Méthode qui permet de fixer la piece sur la grille de jeu 
    /// Auteur:Seghir Nassima
    /// </summary>
    public void Set(Piece piece, Tile tile)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            board.SetTile(tilePosition, tile);
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
    /// Auteurs : Seghir Nassima, Bae Jin-Young
    /// Méthode permettant d'incrémenter le temps à chaque seconde
    /// </summary>
    public void CountTime()
    {
        if (!PauseMenu.GetGameIsPausing())
        {
            time += Time.deltaTime;
   
        }
    }

    /// <summary> 
    /// Auteur : Bae Jin-Young
    /// Méthode qui permet de vérifier si une ligne a été complétée dans la grille de jeu
    /// </summary>
    /// <param name="row">
    /// La ligne que l'on examine
    /// </param>
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
    /// Auteur : Bae Jin-Young
    /// Méthode qui permet de détruire la ligne complétée et d'appliquer la gravité sur toute la grille
    /// </summary>
    /// <param name="row">
    /// La ligne complete a effacer
    /// </param>
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
    /// Auteur : Bae Jin-Young
    /// Méthode qui permet de vérifier si une colonne a été complétée dans la grille de jeu
    /// </summary>
    /// <param name="col">
    /// La colonne que l'on examine
    /// </param>
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
    /// Auteur : Bae Jin-Young
    /// Méthode qui permet de détruire la colonne complétée et d'appliquer la gravité sur toute la grille
    /// </summary>
    /// <param name="col">
    /// La colonne complete a effacer
    /// </param>
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
    /// Auteur : Bae Jin-Young, Seghir Nassima
    /// Méthode permettant de détruire une ligne/colonne complétée
    /// </summary>
    public void ClearCompleteLine()
    {
        RectInt bornes = Bornes;

        // Booleen qui indique si la ligne complété est une ligne ou une colonne
        bool isCol=false;

        //Le nombre de lignes sans bonus ni malus effacées au total
        int nbLinesCleared =0; 
        
        //Le nombre de lignes contenant un bonus
        int nbLinesBonus = 0;

        //Le nombre de lignes contenant un malus
        int nbLinesMalus = 0;

        // les lignes :
        int row = bornes.yMin;
        while (row < bornes.yMax)
        {

            // si une ligne est complète, elle est détruite
            if (RowIsComplete(row))
            {
                isCol=false;

                //on compte le nombre de tuiles standards, bonus et malus dans la ligne supprimee
                int nbTileBonus = 0;
                int nbTileMalus = 0;
                int nbTileNormal = 0;

                for (int colonne = bornes.xMin; colonne < bornes.xMax; colonne++)
                {
                    Vector3Int cur_pos = new Vector3Int(colonne, row, -2);

                    //Incrementation du nombre de cellules bonus
                    if (board.GetTile(cur_pos) == activePiece.GetData().bonus_tile)
                    {
                        nbTileBonus++;
                  
                    }

                    //Incrementation du nombre de cellules malus
                    else if (board.GetTile(cur_pos) == activePiece.GetData().malus_tile)
                    {
                        nbTileMalus++;
                        
                    }

                    //Incrementation du nombre de cellules standards
                    else
                    {
                        nbTileNormal++;
                    }
                }

                //on efface la ligne complète
                ClearRow(row);

                //si la ligne ne contient que des tetrominoes standards, la ligne est consideree comme standard 
                if (nbTileNormal == 16)
                {
                    nbLinesCleared++;
                }

                //si la ligne contient des malus, elle est consideree comme malus
                if (nbTileMalus > 0)
                {
                    nbLinesMalus++;
                }

                //si la ligne contient des bonus, elle est consideree comme bonus
                if (nbTileBonus > 0)
                {
                    nbLinesBonus++;
                }

                //une ligne consideree comme malus ET bonus rapporte le meme score qu'une ligne standard
                 
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
                isCol=true;

                //on compte le nombre de tuiles standards, bonus et malus dans la colonne supprimee
                int nbTileBonus = 0;
                int nbTileMalus = 0;
                int nbTileNormal = 0;
                for (int ligne = bornes.yMin; ligne < bornes.yMax; ligne++)
                {
                    Vector3Int cur_pos = new Vector3Int(col, ligne, -2);

                    //Incrementation du nombre de cellules bonus
                    if (board.GetTile(cur_pos) == activePiece.GetData().bonus_tile)
                    {
                        nbTileBonus++;

                    }

                    //Incrementation du nombre de cellules malus
                    else if (board.GetTile(cur_pos) == activePiece.GetData().malus_tile)
                    {
                        nbTileMalus++;

                    }

                    //Incrementation du nombre de cellules standards
                    else
                    {
                        nbTileNormal++;
                    }
                }

                //on efface la colonne complete
                ClearCol(col);

                //si la colonne ne contient que des tetrominoes standards, elle est consideree comme standard 
                if (nbTileNormal == 22)
                {
                    nbLinesCleared++;
                }

                //si la colonne contient des malus, elle est consideree comme malus
                if (nbTileMalus > 0)
                {
                    nbLinesMalus++;
                }

                //si la colonne contient des bonus, elle est consideree comme bonus
                if (nbTileBonus > 0)
                {
                    nbLinesBonus++;
                }
            }

            //sinon on passe a la prochaine colonne
            else
            {
                col++;
            }
        }
        //Calcul du score selon le nombre et la nature des lignes/colonnes completees 
        scoreManager.IncrementScore(nbLinesCleared, isCol, false, false);
        scoreManager.IncrementScore(nbLinesBonus, isCol, true, false);
        scoreManager.IncrementScore(nbLinesMalus, isCol, false, true);
        totalLinesCleared +=nbLinesCleared;
        
        //Activation de l'effet sonore lorsque un ou plusieurs lignes sont détruites
        if(nbLinesCleared > 0)
        {
            soundManager.PlaySound(soundManager.m_clearRowSound,0.5f);
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
            case 0: gravity = Gravity.HAUT;
                    break;
            case 1: gravity = Gravity.BAS;
                    break;
            case 2: gravity = Gravity.GAUCHE;
                    break;
            case 3: gravity = Gravity.DROITE;
                    break;
            default: gravity = Gravity.BAS;
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
    public bool ValiderPosition(Piece piece, Vector3Int position)
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
    /// Auteur : Bae Jin-Young
    /// Description : Méthode permettant de vérifier si un côté de l'écran est rempli
    /// </summary>
    /// <param name="gravity_is_vertical">
    /// Booleen permettant d'identifier la nature de la gravite, TRUE si la gravite est celle du BAS ou du HAUT
    /// </param>
    /// <param name="num_row_col">
    /// Le numero de la ligne/colonne qu'il faut examiner
    /// </param>
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
                    //Vérifie s'il s'agit de la gravité vers le haut ou le bas
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
                    //Vérifie s'il s'agit de la gravité vers la droite ou la gauche
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
    /// Auteur : Kusunga Malcom 
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
    /// Auteurs : Bae Jin-Young, Kusunga Malcom
    /// Méthode permettant de bloquer les gravités pour les cotés complets
    /// </summary>
    public void StopGravity()
    {
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
    /// Auteur : Bae Jin-Young
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

    /// <summary>
    /// Méthode permettant de retourner la taille de la grille
    /// </summary>
    public Vector2Int GetSize(){
        return size;
    }

    /// <summary>
    /// Méthode permettant de retourner un booleen indiquant lorsque le haut de la grille est complet
    /// </summary>
    public bool GetTopIsFull(){
        return topIsFull;
    }
    
    /// <summary>
    /// Méthode permettant de retourner un booleen indiquant lorsque le bas de la grille est complet
    /// </summary>
    public bool GetBotIsFull(){
        return botIsFull;
    }

    /// <summary>
    /// Méthode permettant de retourner un booleen indiquant lorsque la gauche de la grille est complet
    /// </summary>
    public bool GetLeftIsFull(){
        return leftIsFull;
    }
    
    /// <summary>
    /// Méthode permettant de retourner un booleen indiquant lorsque la droite de la grille est complet
    /// </summary>
    public bool GetRightIsFull(){
        return rightIsFull;
    }
    
    /// <summary>
    /// Méthode permettant de retourner la grivité qui s'exerce
    /// </summary>
    public Gravity GetGravity(){
        return gravity;
    }

    /// <summary>
    /// Méthode permettant de retourner la grivité qui s'exerce
    /// </summary>
    public static void SetGravity(Gravity grav){
        gravity = grav;
    }

    /// <summary>
    /// Méthode permettant de retourner la pièce actuellement présente sur la grille
    /// </summary>
    public Piece GetActivePiece(){
        return activePiece;
    }

    public static int GetTotalLinesCleared(){
        return totalLinesCleared;
    }

    public static bool GetLockSpeed(){
        return lockSpeed;
    }
    
    public static void SetLockSpeed(bool lockState){
        lockSpeed = lockState;
    }

    public Vector3Int GetSpawnPosition(){
        return spawnPosition;
    }

    
    public Tilemap GetBoard(){
        return board;
    }

    public float GetTime()
    {
        return time;
    }
}
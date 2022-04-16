using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary> 
/// Auteurs : Seghir Nassima, Kusunga Malcom, Sterlingot Guillaume, Bae Jin-Young<br>
/// Description : Cette classe permet de controler le tetromino present sur la grille de jeu 
/// </summary>
public class Piece : MonoBehaviour
{   
    /// <summary> 
    /// Attribut contenant le manager du son 
    /// </summary>
    [SerializeField] private SoundManager soundManager;

    /// <summary> 
    /// Attribut contenant le plateau de jeu 
    /// </summary>
    public BoardManager board;

    /// <summary> 
    /// Attribut contenant les donnees du tetromino actuel 
    /// </summary>
    public TetrominoData data { get; private set; }

    /// <summary> 
    /// Attribut contenant les cellules du tetromino actuel
    /// </summary>
    public Vector3Int[] cells { get; private set; }

    /// <summary> 
    /// Attribut contenant la position de la piece actuelle 
    /// </summary>
    public Vector3Int position;

    /// <summary> 
    /// Attribut determinant la cadence de deplacement du tetromino 
    /// </summary>
    [SerializeField] private float stepDelay;

    /// <summary> 
    /// Attribut qui permet de stocker la cadence de deplacement du tetromino dites "normale" 
    /// </summary>
    private float bufferedStepDelay;

    /// <summary> 
    /// Attribut determinant le delai avant le quel la piece se fixe définitivement sur la grille    
    /// </summary>
    [SerializeField] private float lockDelay = 0.5f;

    /// <summary> 
    /// Attribut indiquant l'instant ou la pièce doit se deplacer     
    /// </summary>
    private float stepTime;
    
    /// <summary> 
    /// Attribut indiquant l'instant ou la pièce doit se fixer      
    /// </summary>
    private float lockTime;

    /// <summary>
    /// Attribut contenant le gestionnaire de la zone de prévisualisation
    /// </summary>
    private PreviewManager previewManager;

    /// <summary>
    /// l'indice de la piece active
    /// </summary>
    private int indice_piece;

    /// <summary>
    /// Le gestionnaire de la zone de hold
    /// </summary>
    private HoldManager holdManager;

    /// <summary>
    /// Tableau de booleens pour les pieces "speciales"
    /// </summary>
    private bool[] paliers_booleen;

    /// <summary>
    /// Le pouvoir de la piece
    /// </summary>
    private Pouvoir pouvoir;

    private static bool gravityIsModified;

    /// <summary> 
    /// Méthode qui permet d'initialiser la piece  
    /// </summary>
    public void Initialize(BoardManager board, Vector3Int position, TetrominoData data, int indice, Pouvoir pouvoir)
    {
        this.data = data;
        this.board = board;
        this.position = position;
        this.indice_piece = indice;
        this.pouvoir = pouvoir;
        stepTime = Time.time + stepDelay;
        lockTime = 0f;

        if (cells == null)
        {
            cells = new Vector3Int[data.cellules.Length];
        }

        for (int i = 0; i < cells.Length; i++)
        {
            cells[i] = (Vector3Int)data.cellules[i];
        }
    }

    private void Start(){
        gravityIsModified=false;
        this.previewManager = FindObjectOfType<PreviewManager>();
        this.holdManager = FindObjectOfType<HoldManager>();

        paliers_booleen = new bool[50];

        for (int i = 0; i < 50; i++)
        {
            this.paliers_booleen[i] = false;
        }
    }

    private void Update()
    {
        board.Clear(this);
        lockTime += Time.deltaTime;
        if (Time.time > stepTime)
            ApplyGravity();

        if (pouvoir == Pouvoir.Standard)
        {
            board.Set(this, data.tile);
        }
        else if (pouvoir == Pouvoir.Malus)
        {
            board.Set(this, data.malus_tile);
        }
        else
        {
            board.Set(this, data.bonus_tile);
        }
    }

    /// <summary> 
    /// Méthode qui permet de bouger la piece sur la grille de jeu 
    /// Auteur: Seghir Nassima 
    /// </summary>
    /// <returns>
    /// un booléen qui indique TRUE si le mouvement a bien eu lieu, FALSE sinon
    /// </returns>
    public bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.ValiderPosition(this, newPosition);

        if(valid)
        {
            position = newPosition;
            lockTime = 0f; // a chaque mouvement de la piece il est remis a 0, comme ça quand elle atteint 
            // le bord et qu'elle ne bouge plus, on la lock
           
        }

        return valid;
    }

    /// <summary> 
    /// Méthode qui controle le mouvement de la piece de ligne en ligne
    /// Auteur:Seghir Nassima  
    /// </summary>
    private void ApplyGravity()
    {
        stepTime = Time.time + stepDelay;

        switch (board.GetGravity())
        {
            case Gravity.HAUT:   Move(Vector2Int.up);
                                 break;
            case Gravity.BAS:    Move(Vector2Int.down);
                                 break;
            case Gravity.GAUCHE: Move(Vector2Int.left);
                                 break;
            case Gravity.DROITE: Move(Vector2Int.right);
                                 break;
            default: Move(Vector2Int.down);
                     break;
        }
        
        if (lockTime >= lockDelay) {
           Lock();
        }
    }

    /// <summary> 
    /// Auteurs : Seghir Nassima, Kusunga Malcom, Bae Jin-Young, Sterlingot Guillaume
    /// Méthode qui bloque le mouvement de la piece  
    /// </summary>
    private void Lock()
    {   
        RestoreGravity();

        //selon le pouvoir de la piece, la tuile utilisee est differente
        if (pouvoir == Pouvoir.Standard)
        {
            board.Set(this, data.tile);
        }
        else if (pouvoir == Pouvoir.Malus)
        {
            board.Set(this, data.malus_tile);
        }
        else
        {
            board.Set(this, data.bonus_tile);
        }

        //verification les cotes remplis et des lignes completes
        board.FullSides();
        board.ClearCompleteLine();
        board.ClearApparitionZone();
        board.StopGravity();
        holdManager.SetStatusHold();

        //uniquement en mode Marathon
        if (Controller.GetGameMode() == Mode.MARATHON)
        {
            //toutes les 50 secondes
            int tps = (int)board.GetTime() / 50;

            //apparition d'un malus si tps est un multiple de 2
            if (tps % 2 == 0 && paliers_booleen[tps] == false && tps != 0)
            {
                board.SpawnPiece(previewManager.GetNextPiece(), board.GetSpawnPosition(), Pouvoir.Malus);
                paliers_booleen[tps] = true;

            }

            //apparition d'un bonus si tps n'est pas un multiple de 2
            else if (tps % 2 == 1 && paliers_booleen[tps] == false)
            {
                board.SpawnPiece(previewManager.GetNextPiece(), board.GetSpawnPosition(), Pouvoir.Bonus);
                paliers_booleen[tps] = true;

            }

            // le reste du temps, des pièces standards
            else
            {
                board.SpawnPiece(previewManager.GetNextPiece(), board.GetSpawnPosition(), Pouvoir.Standard);
            }
        }

        //si le mode different de Marathon, apparition de pieces standards
        else
        {
            board.SpawnPiece(previewManager.GetNextPiece(), board.GetSpawnPosition(), Pouvoir.Standard);
        }

        previewManager.ChangePreview();

        //Activation de l'effet sonore losqu'une piece est placée sur le board
        if(soundManager.m_fxEnabled && soundManager.m_dropSound)
        {
            AudioSource.PlayClipAtPoint(soundManager.m_dropSound,Camera.main.transform.position,soundManager.m_fxVolume);
        }
    }

    /// <summary>
    /// Auteur : Bae Jin-Young
    /// Methode permettant d'effacer une piece
    /// </summary>
    public void Remove()
    {
        for (int i = 0; i < cells.Length; i++)
        {
            Vector3Int tilePosition = cells[i] + position;
            board.GetBoard().SetTile(tilePosition, null);
        }

    }

    /// <summary> 
    /// Auteur : Kusunga Malcom 
    /// Description : Méthode permettant le déplacement vers la droite du tetromino actuellement présent sur le plateau
    /// </summary>
    public void RightShift(){
        //Vérification du sens d'application de la gravité
        if(board.GetGravity() == Gravity.HAUT || board.GetGravity() == Gravity.BAS){
            //Suppresion de la position précédente du tétromino sur la grille
            board.Clear(this);
            TouchSensitive.SetWantToRotate(false);
            Move(Vector2Int.right);
        } 
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom
    /// Description : Méthode permettant le déplacement vers la gauche du tetromino actuellement présent sur le plateau
    /// </summary>
    public void LeftShift(){
        //Vérification du sens d'application de la gravité
        if(board.GetGravity() == Gravity.HAUT || board.GetGravity() == Gravity.BAS){
            //Suppresion de la position précédente du tétromino sur la grille
            board.Clear(this);
            TouchSensitive.SetWantToRotate(false);
            Move(Vector2Int.left);
        }
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom
    /// Description : Méthode permettant le déplacement vers le haut du tetromino actuellement présent sur le plateau
    /// </summary>
    public void TopShift(){
        //Vérification du sens d'application de la gravité
        if(board.GetGravity() == Gravity.GAUCHE || board.GetGravity() == Gravity.DROITE){
            //Suppresion de la position précédente du tétromino sur la grille
            board.Clear(this);
            TouchSensitive.SetWantToRotate(false);
            Move(Vector2Int.up);
        }
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom 
    /// Description : Méthode permettant le déplacement vers le bas du tetromino actuellement présent sur le plateau
    /// </summary>
    public void BotShift(){
        //Vérification du sens d'application de la gravité
        if(board.GetGravity() == Gravity.GAUCHE || board.GetGravity() == Gravity.DROITE){
            //Suppresion de la position précédente du tétromino sur la grille
            board.Clear(this);
            TouchSensitive.SetWantToRotate(false);
            Move(Vector2Int.down);
        }
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom
    /// Description : Méthode permettant de modifier la force de la gravité du tetromino actuellement présent sur le plateau 
    ///               Bouton Droit  
    /// </summary>
    public void ModifyGravityR()
    {
        gravityIsModified=true;
        // Conserver la valeur normale de la vitesse des tétrominos pour pouvoir la remettre une fois que le déplacement ou la rotation sont finis
        TouchSensitive.SetWantToRotate(false);

        //Augmentation de la gravité
        if(board.GetGravity() == Gravity.DROITE){
            stepDelay = 0.1f;
        }
        //Diminution de la gravité
        else if(board.GetGravity() == Gravity.GAUCHE){
            stepDelay = 1.5f;
        }
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom
    /// Description : Méthode permettant de modifier la force de la graivté du tetromino actuellement présent sur le plateau 
    ///               Bouton Gauche
    /// </summary>
    public void ModifyGravityL()
    {
        gravityIsModified=true;
        // Conserver la valeur normale de la vitesse des tétrominos pour pouvoir la remettre une fois que le déplacement ou la rotation sont finis
        TouchSensitive.SetWantToRotate(false);

        //Augmenation de la gravité
        if(board.GetGravity() == Gravity.GAUCHE){
            stepDelay = 0.1f;
        }
        //Diminution de la gravité
        else if(board.GetGravity() == Gravity.DROITE){
            stepDelay = 1.5f;
        }
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom
    /// Description : Méthode permettant de modifier la force de la gravité du tetromino actuellement présent sur le plateau 
    ///               Bouton Haut
    /// </summary>
    public void ModifyGravityT()
    {
        gravityIsModified=true;
        // Conserver la valeur normale de la vitesse des tétrominos pour pouvoir la remettre une fois que le déplacement ou la rotation sont finis
        TouchSensitive.SetWantToRotate(false);

        //Augmentation de la gravité
        if(board.GetGravity() == Gravity.HAUT){
            stepDelay = 0.1f;
        }
        //Diminution de la gravité
        else if(board.GetGravity() == Gravity.BAS){
            stepDelay = 1.5f;
        }
    }

    /// <summary> 
    /// Auteur : Kusunga Malcom
    /// Description : Méthode permettant de modifier la force de la gravité du tetromino actuellement présent sur le plateau 
    ///               Bouton Bas
    /// </summary>
    public void ModifyGravityB()
    {
        gravityIsModified=true;
        // Conserver la valeur normale de la vitesse des tétrominos pour pouvoir la remettre une fois que le déplacement ou la rotation sont finis
        TouchSensitive.SetWantToRotate(false);

        //Augmenation de la gravité
        if(board.GetGravity() == Gravity.BAS){
            stepDelay = 0.1f;
        }
        //Diminution de la gravité
        else if(board.GetGravity() == Gravity.HAUT){
            stepDelay = 1.5f;
        }
    }

    /// <summary> 
    /// Auteurs : Sterlingot Guillaume, Kusunga Malcom
    /// Description : Méthode permettant de restaurer la vitesse de déplacement du tetromino actuellement présent sur le plateau
    /// </summary>
    public void RestoreGravity(){
        this.stepDelay = this.bufferedStepDelay;
        gravityIsModified=false;
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode qui permet la rotation de la pièce de tetromino actuellement présente sur le plateau
    /// </summary>
    public void Rotate(){
        Vector2Int/*[]*/ pivot; //On initialise un tableau de pivot
        //pivots = FillPivot(); //On remplie le tableau de pivot dans le bonne ordre

        if(data.tetromino.CompareTo(Tetromino.I)==0)
            pivot = new Vector2Int(1,1);
            //pivots[0] = new Vector2Int(1,1);
        else
            pivot = new Vector2Int(0,0);
            //pivots[0] = new Vector2Int(0,0);

        //On vérifie si le tétromino O est présent en pièce active
        if(!data.tetromino.Equals(Tetromino.O)){
            /*int j=0;
            while(!Pivot(pivots[j])){
                Debug.Log(pivots[j]);
                j++;
            }*/
            board.Clear(this);
            Pivot(pivot);
        }
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode qui permet de remplir le tableau de pivot de la pièce courante sur le board
    /// </summary>
    /// <returns> 
    /// un tableau de vecteur d'entier à deux dimensions contenant les positions de chaque tuile du tétromino qui correspondent à un tableau de pivots
    /// </returns>
    /*public Vector2Int[] FillPivot(){
        Vector2Int[] pivots = new Vector2Int[4];

        if(data.tetromino.CompareTo(Tetromino.I)==0)
            pivots[0] = new Vector2Int(1,1);
        else
            pivots[0] = new Vector2Int(0,0);
        
        int i=1;
        foreach(Vector2Int tile in data.cellules){
            if(!tile.Equals(pivots[0])){
                pivots[i] = tile;
                i++;
            }
        }
        return pivots;
    }*/

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode qui permet de pivoter une pièce
    /// </summary>
    /// <returns> 
    /// un booléen qui indique TRUE si la position de la pièce est valide, FALSE sinon
    /// </returns>
    public bool Pivot(Vector2Int pivot){
        Vector3Int[] newCells = new Vector3Int[data.cellules.Length];
        Array.Copy(cells, newCells, data.cellules.Length);

        for(int i=0;i<4;i++){

            //Calcul de l'écart en x et en y entre la pièce pivot et la pièce à faire tourner autour
            int ecartX = Mathf.Abs(pivot.x - cells[i].x);
            int ecartY = Mathf.Abs(pivot.y - cells[i].y);
            int newX = newCells[i].x, newY = newCells[i].y;

            //Différenciation des quatres rotations effectifs sur le plateau avant que la pièce retourne à son point de départ
            //Pour chaque cas, on calcul les nouvelles coordonnées de la cellules selon les coordonnées du pivot

            //CAS OU ON SE SITUE DANS le VOISINAGE aligné horizontalement ou verticalement avec le pivot
            if(newCells[i].y==pivot.y&&newCells[i].x<pivot.x){ //Condition qui indique la rotation a effectuer
                newX = pivot.x;
                newY = ecartX+newCells[i].y;
            }else if(newCells[i].x==pivot.x&&newCells[i].y>pivot.y){ //Condition qui indique la rotation a effectuer
                newX = ecartY+newCells[i].x;
                newY = pivot.y;
            }else if(newCells[i].y==pivot.y&&newCells[i].x>pivot.x){ //Condition qui indique la rotation a effectuer
                newX = pivot.x;
                newY = newCells[i].y-ecartX;
            }else if(newCells[i].x==pivot.x&&newCells[i].y<pivot.y){ //Condition qui indique la rotation a effectuer
                newX = newCells[i].x - ecartY;
                newY = pivot.y;
            }
            //CAS OU ON SE SITUE DANS le VOISINAGE non aligné avec le pivot
            else if(newCells[i].y>pivot.y&&newCells[i].x<pivot.x)
                newX = newCells[i].x+2*ecartX;
            else if(newCells[i].y>pivot.y&&newCells[i].x>pivot.x)
                newY = newCells[i].y-2*ecartY;
            else if(newCells[i].y<pivot.y&&newCells[i].x>pivot.x)
                newX = newCells[i].x-2*ecartX;
            else
                newY = newCells[i].y+2*ecartY;

            bool valid = board.ValideTilePos(new Vector3Int(newX+position.x, newY+position.y, -2));
            if (valid){
                newCells[i].x = newX;
                newCells[i].y = newY;
            }else{
                return false;
            }
        }

        cells = newCells;
        
        return true;
    }

    public float GetStepDelay()
    {
        return stepDelay;
    }

    public float GetBufferedStepDelay()
    {
        return bufferedStepDelay;
    }

    public static bool GetGravityIsModified(){
        return gravityIsModified;
    }
    
    
    public void SetStepDelay(float stepDelay)
    {
        this.stepDelay = stepDelay;
    }

    public void SetBufferedStepDelay(float bufferedStepDelay){
        this.bufferedStepDelay = bufferedStepDelay;
    }

    public float GetStepTime()
    {
        return stepTime;
    }

    public int GetIndice()
    {
        return indice_piece;
    }

    public Vector3Int GetPiecePosition()
    {
        return position;
    }

    public Pouvoir GetPouvoir()
    {
        return pouvoir;
    }

    public TetrominoData GetData()
    {
        return data;
    }
}
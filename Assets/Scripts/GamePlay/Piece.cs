using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary> 
/// Auteur : Seghir Nassima, Kusunga Malcom, Sterlingot Guillaume, Bae Jin-Young<br>
/// Description : Cette classe permet de controler le tetromino present sur la grille de jeu 
/// </summary>
public class Piece : MonoBehaviour
{
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
    /// Méthode qui permet d'initialiser la piece  
    /// </summary>
    public void Initialize(BoardManager board, Vector3Int position, TetrominoData data)
    {
        this.data = data;
        this.board = board;
        this.position = position;
        stepTime = Time.time + stepDelay;
        lockTime = 0f;

        if (cells == null) {
            cells = new Vector3Int[data.cellules.Length];
        }

        for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector3Int)data.cellules[i]; 
        }
    }

    private void Start()
    {
        this.stepDelay = 1f;
        this.bufferedStepDelay = stepDelay; 
    }

    private void Update()
    {
        board.Clear(this);

        lockTime += Time.deltaTime;

        if (Time.time > stepTime) {
            ApplyGravity();
        }

        board.Set(this);
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
        board.Set(this);
        board.FullSides();
        board.ClearCompleteLine();
        board.ClearApparitionZone();
        board.StopGravity();
        board.SpawnPiece();
    }

    /// <summary> 
    /// Auteur : Malcom Kusunga
    /// Description : Méthode permettant le déplacement vers la droite du tetromino actuellement présent sur le plateau
    /// </summary>
    public void RightShift(){
        //Vérification du sens d'application de la gravité
        if(board.GetGravity() == Gravity.HAUT || board.GetGravity() == Gravity.BAS){
            //Suppresion de la position précédente du tétromino sur la grille
            board.Clear(this);
            Move(Vector2Int.right);
        } 
    }

    /// <summary> 
    /// Auteur : Malcom Kusunga
    /// Description : Méthode permettant le déplacement vers la gauche du tetromino actuellement présent sur le plateau
    /// </summary>
    public void LeftShift(){
        if(board.GetGravity() == Gravity.HAUT || board.GetGravity() == Gravity.BAS){
            board.Clear(this);
            Move(Vector2Int.left);
        }
    }

    /// <summary> 
    /// Auteur : Malcom Kusunga
    /// Description : Méthode permettant le déplacement vers le haut du tetromino actuellement présent sur le plateau
    /// </summary>
    public void TopShift(){
        if(board.GetGravity() == Gravity.GAUCHE || board.GetGravity() == Gravity.DROITE){
            board.Clear(this);
            Move(Vector2Int.up);
        }
    }

    /// <summary> 
    /// Auteur : Malcom Kusunga
    /// Description : Méthode permettant le déplacement vers le bas du tetromino actuellement présent sur le plateau
    /// </summary>
    public void BotShift(){
        if(board.GetGravity() == Gravity.GAUCHE || board.GetGravity() == Gravity.DROITE){
            board.Clear(this);
            Move(Vector2Int.down);
        }
    }

    /// <summary> 
    /// Auteur : Malcom Kusunga
    /// Description : Méthode permettant de modifier la force de la gravité du tetromino actuellement présent sur le plateau 
    ///               Bouton Droit  
    /// </summary>
    public void ModifyGravityR()
    {
        // Conserver la valeur normale de la vitesse des tétrominos pour pouvoir la remettre une fois que le déplacement ou la rotation sont finis
        this.bufferedStepDelay = stepDelay;

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
    /// Auteur : Malcom Kusunga
    /// Description : Méthode permettant de modifier la force de la graivté du tetromino actuellement présent sur le plateau 
    ///               Bouton Gauche
    /// </summary>
    public void ModifyGravityL()
    {
        // Conserver la valeur normale de la vitesse des tétrominos pour pouvoir la remettre une fois que le déplacement ou la rotation sont finis
        this.bufferedStepDelay = stepDelay;

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
    /// Auteur : Malcom Kusunga
    /// Description : Méthode permettant de modifier la force de la gravité du tetromino actuellement présent sur le plateau 
    ///               Bouton Haut
    /// </summary>
    public void ModifyGravityT()
    {
        // Conserver la valeur normale de la vitesse des tétrominos pour pouvoir la remettre une fois que le déplacement ou la rotation sont finis
        this.bufferedStepDelay = stepDelay;

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
    /// Auteur : Malcom Kusunga
    /// Description : Méthode permettant de modifier la force de la gravité du tetromino actuellement présent sur le plateau 
    ///               Bouton Bas
    /// </summary>
    public void ModifyGravityB()
    {
        // Conserver la valeur normale de la vitesse des tétrominos pour pouvoir la remettre une fois que le déplacement ou la rotation sont finis
        this.bufferedStepDelay = stepDelay;

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
    /// Auteur : Sterlingot Guillaume, Kusunga Malcom
    /// Description : Méthode permettant de restaurer la vitesse de déplacement du tetromino actuellement présent sur le plateau
    /// </summary>
    public void RestoreGravity(){
        stepDelay = bufferedStepDelay;
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode qui permet la rotation de la pièce de tetromino actuellement présente sur le plateau
    /// </summary>
    public void Rotate(){
        Vector2Int pivot;
        
        if(data.tetromino.CompareTo(Tetromino.I)==0)
            pivot = new Vector2Int(1,1);
        else
            pivot = new Vector2Int(0,0);
       
        if(!data.tetromino.Equals(Tetromino.O)){
            board.Clear(this);
            Pivot(pivot);
        }
    }
    
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

    public float GetStepDelay(){
        return stepDelay;
    }
    
    public void SetStepDelay(float stepDelay){
        this.stepDelay = stepDelay;
    }
    public float GetStepTime(){
        return stepTime;
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Auteur : Seghir Nassima<br>
/// Description : Cette classe permet de controler le tetromino present sur la grille de jeu 
/// </summary>
public class Piece : MonoBehaviour
{
    /// <summary> 
    /// Attribut contenant le plateau de jeu 
    /// </summary>
    public BoardManager board { get; private set; }

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
    public Vector3Int position { get; private set; }


    /// <summary> 
    /// Attribut determinant la cadence de deplacement du tetromino 
    /// </summary>
    [SerializeField] private float stepDelay = 1f;


    /// <summary> 
    /// Attribut determinant le delai avant le quel la piece se fixe définitivement sur la grille    
    /// </summary>
    [SerializeField] private float lockDelay = 0.5f;

    /// <summary> 
    /// Attribut indiquant l'instant ou la pice doit se deplacer     
    /// </summary>
    private float stepTime;
    
    /// <summary> 
    /// Attribut indiquant l'instant ou la pice doit se fixer      
    /// </summary>
    private float lockTime;
  
    /// <summary> 
    /// Attribut indiquant le nombre de rotation réalisé en partant de la pièce initiale      
    /// </summary>
    private int nbRotate=0;
  
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

    private void Update()
    {
        board.Clear(this);

        lockTime += Time.deltaTime;

         if (Time.time > stepTime) {
            Step();
        }

         board.Set(this);
    }

    /// <summary> 
    /// Méthode qui permet de bouger la piece sur la grille de jeu  
    /// </summary>
    /// <returns>
    /// un booléen qui indique TRUE si le mouvement a a bien eu lieu, FALSE sinon
    /// </returns>
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = board.validerPosition(this, newPosition);

        if (valid)
        {
            position = newPosition;
            lockTime = 0f; // a chaque mouvement de la piece il est remis a 0, comme ça quand elle atteint 
            //le bord et qu'elle ne bouge plus, on la lock 
           
        }

        return valid;
    }

    
    /// <summary> 
    /// Méthode qui controle le mouvement de la piece de ligne en ligne  
    /// </summary>
    private void Step()
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
    /// Méthode qui bloque le mouvement de la piece   
    /// </summary>
    private void Lock()
    {
        board.Set(this);
        board.GetGravity();
        board.SpawnPiece();
    }


    /// <summary> 
    /// Auteur : Malcom Kusunga
    /// Description : Méthode qui permet le déplacement droite de la pièce de tetromino actuellement présente sur le plateau
    /// </summary>
    public void RightShift(){
        
    }

    /// <summary> 
    /// Auteur : Malcom Kusunga
    /// Description : Méthode qui permet le déplacement gauche de la pièce de tetromino actuellement présente sur le plateau
    /// </summary>
    public void LeftShift(){
        
    }

    /// <summary> 
    /// Auteur : Malcom Kusunga
    /// Description : Méthode qui permet le déplacement haut de la pièce de tetromino actuellement présente sur le plateau
    /// </summary>
    public void TopShift(){
        
    }

    /// <summary> 
    /// Auteur : Malcom Kusunga
    /// Description : Méthode qui permet le déplacement bas de la pièce de tetromino actuellement présente sur le plateau
    /// </summary>
    public void BotShift(){
        
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode qui permet la rotation de la pièce de tetromino actuellement présente sur le plateau
    /// </summary>
    public void Rotate(){
        board.Clear(this);
        Pivot();
    }
    
    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode qui permet de pivoter une pièce
    /// </summary>
    /// <returns> 
    /// un booléen qui indique TRUE si la position de la pièce est valide, FALSE sinon
    /// </returns>
    public bool Pivot(){
        GetComponent<RectTransform>().transform.rotation = Quaternion.Euler(GetComponent<RectTransform>().transform.rotation.x, GetComponent<RectTransform>().transform.rotation.y, GetComponent<RectTransform>().transform.rotation.z-90);

        return false;
    }
}


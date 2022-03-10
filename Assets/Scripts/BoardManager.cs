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
/// Auteur : Sterlingot Guillaume, Bae Jin-Young<br>
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
    /// Attribut contenant la pièce courante qui est présente sur le plateau de jeu 
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
    /// Attribut indiquant la taille total du plateau de jeu
    /// </summary>
    private int size;

    private void Awake()
    {
        SetupBoard();
    }

    private void Start(){
        ApplyGravity();
        SpawnPiece();
    }

    private void Update()
    {
        chooseRandomGravity();
        Debug.Log(this.gravity);
        if(HaveCollision()){
            ClearLine();
            SpawnPiece();
            ApplyGravity();
        }
    }

    /// <summary> 
    /// Méthode qui permet de faire apparaître une pièce dans la grille de jeu
    /// </summary>
    public void SpawnPiece(){

    }

    /// <summary> 
    /// Méthode qui permet d'appliquer une des quatres gravités à la grille de jeu
    /// </summary>
    // VERIFIER SI UNE PIECE EST PRESENTE DANS LA GRILLE AVANT, SINON ON INDIQUE UNE ERREURE
    public void ApplyGravity(){

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
        this.nb_col = 14;
        this.nb_row = 22;
        this.size = this.nb_col*this.nb_row;
    }

    /// <summary> 
    /// Méthode qui permet de vérifier si une ligne a été complété dans la grille de jeu
    /// </summary>
    /// <returns>
    /// un booléen qui indique TRUE si une ligne a été complété sur la grille de jeu, FALSE sinon
    /// </returns>
    public bool RowIsComplete(){
        return false;
    }

    /// <summary> 
    /// Méthode qui permet de vérifier si une colonne a été complété dans la grille de jeu
    /// </summary>
    /// <returns>
    /// un booléen qui indique TRUE si une colonne a été complété sur la grille de jeu, FALSE sinon
    /// </returns>
    public bool ColIsComplete(){
        return false;
    }

    /// <summary> 
    /// Auteur : Sterlingot Guillaume
    /// Description : Méthode qui permet la sélection aléatoire d'une des quatres gravités existantes
    /// </summary>
    public void chooseRandomGravity(){
        int randomNumber = Random.Range(0,4);
        switch (randomNumber)
        {
            case 0: this.gravity=Gravity.HAUT;
                    break;
            case 1: this.gravity=Gravity.BAS;
                    break;
            case 2: this.gravity=Gravity.GAUCHE;
                    break;
            case 3: this.gravity=Gravity.DROITE;
                    break;
            default:this.gravity=Gravity.BAS;
                    break;
        }
    }
}

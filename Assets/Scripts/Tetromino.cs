using UnityEngine; 
using UnityEngine.Tilemaps;


/// <summary> 
/// Auteur : Seghir Nassima<br>
/// Description : Cette enumération permet de distinguer les différents tetrominoes 
/// </summary>
public enum Tetromino{
    I,
    O, 
    T,
    J,
    L,
    S,
    Z
    
}

/// <summary> 
/// Auteur : Seghir Nassima<br>
/// Description : Cette structure regroupe les données d'un tetromino 
/// </summary>
[System.Serializable]
public struct TetrominoData
{
    /// <summary> 
    /// Attribut faisant référence au type de tetromino  
    /// </summary>
    public Tetromino tetromino; 

    /// <summary> 
    /// Attribut contenant la tuile utilisée pour construire le tetromino  
    /// </summary>
    public Tile tile; 

    /// <summary> 
    /// Tableau contenant les positions des cellules du tetromino  
    /// </summary>
    public Vector2Int[] cellules ; 
}


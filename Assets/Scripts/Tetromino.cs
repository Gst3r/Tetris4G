using System.Collections;
using System.Collections.Generic;
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

public class TetrominoBuilder : MonoBehaviour
{
   public static readonly Dictionary<Tetromino, Vector2Int[]> Cells = new Dictionary<Tetromino, Vector2Int[]>()
    {
        { Tetromino.I, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 2, 1) } },
        { Tetromino.J, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.L, new Vector2Int[] { new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.O, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.S, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int( 1, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0) } },
        { Tetromino.T, new Vector2Int[] { new Vector2Int( 0, 1), new Vector2Int(-1, 0), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
        { Tetromino.Z, new Vector2Int[] { new Vector2Int(-1, 1), new Vector2Int( 0, 1), new Vector2Int( 0, 0), new Vector2Int( 1, 0) } },
    };
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


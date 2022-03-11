using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public BoardManager board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public Vector3Int position { get; private set; }
  

  //just initialiser notre nouvelle piece 
    public void Initialize(BoardManager board, Vector3Int position, TetrominoData data)
    {
         this.data = data;
        this.board = board;
        this.position = position;

        if (cells == null) {
            cells = new Vector3Int[data.cellules.Length];
        }

         for (int i = 0; i < cells.Length; i++) {
            cells[i] = (Vector3Int)data.cellules[i]; 
        }



    }
}

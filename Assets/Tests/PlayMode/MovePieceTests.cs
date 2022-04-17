using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class MovePieceTests
{
    [UnityTest]
    public IEnumerator RightShiftTest()
    {
        var gameObject = new GameObject();
        var piece = gameObject.AddComponent<Piece>();

        Vector2Int shift = Vector2Int.right;
        Vector3Int destination = piece.GetPiecePosition();
        destination.x += shift.x;
        destination.y += shift.y;

        piece.Move(shift);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;

        Assert.AreEqual(destination,piece.GetPiecePosition());
    }

    [UnityTest]
    public IEnumerator LeftShiftTest()
    {
        var gameObject = new GameObject();
        var piece = gameObject.AddComponent<Piece>();

        Vector2Int shift = Vector2Int.left;
        Vector3Int destination = piece.GetPiecePosition();
        destination.x += shift.x;
        destination.y += shift.y;

        piece.Move(shift);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;

        Assert.AreEqual(destination,piece.GetPiecePosition());
    }


    [UnityTest]
    public IEnumerator TopShiftTest()
    {
        var gameObject = new GameObject();
        var piece = gameObject.AddComponent<Piece>();

        Vector2Int shift = Vector2Int.up;
        Vector3Int destination = piece.GetPiecePosition();
        destination.x += shift.x;
        destination.y += shift.y;

        piece.Move(shift);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;

        Assert.AreEqual(destination,piece.GetPiecePosition());
    }


    [UnityTest]
    public IEnumerator BotShiftTest()
    {
        var gameObject = new GameObject();
        var piece = gameObject.AddComponent<Piece>();

        Vector2Int shift = Vector2Int.down;
        Vector3Int destination = piece.GetPiecePosition();
        destination.x += shift.x;
        destination.y += shift.y;

        piece.Move(shift);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;

        Assert.AreEqual(destination,piece.GetPiecePosition());
    }
}
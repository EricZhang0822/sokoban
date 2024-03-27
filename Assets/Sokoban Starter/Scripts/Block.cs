using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(GridObject))]
public abstract class Block : MonoBehaviour
{
    private static readonly Dictionary<Vector2Int, Block> blockMap = new();

    private GridObject gridObject;

    public bool isChecked = false;

    private void Start()
    {
        gridObject = GetComponent<GridObject>();
        RegisterBlockAtRandomPosition();
    }

    protected void Update()
    {
        isChecked = false;
    }

    private void RegisterBlockAtRandomPosition()
    {
        Vector2Int randomPosition = GenerateRandomPosition();
        while (blockMap.ContainsKey(randomPosition))
        {
            randomPosition = GenerateRandomPosition();
        }
        Position = randomPosition;
    }

    private Vector2Int GenerateRandomPosition()
    {
        int x = UnityEngine.Random.Range(0, (int)GridMaker.reference.dimensions.x);
        int y = UnityEngine.Random.Range(0, (int)GridMaker.reference.dimensions.y);
        return new Vector2Int(x, y);
    }

    public Vector2Int Position
    {
        get => ConvertToIndex(gridObject.gridPosition);
        set
        {
            if (blockMap.TryGetValue(Position, out var block) && block == this)
            {
                blockMap.Remove(Position);
            }
            gridObject.gridPosition = ConvertToLength(value);
            blockMap[Position] = this;
        }
    }

    protected Block GetBlockAt(Vector2Int position)
    {
        return isInBound(position) && blockMap.TryGetValue(position, out var block) ? block : null;
    }

    protected bool isInBound(Vector2Int position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < GridMaker.reference.dimensions.x && position.y < GridMaker.reference.dimensions.y;
    }

    public abstract bool Move(Vector2Int direction, string type);

    protected void ForAdjacentBlocks(Action<Vector2Int> action)
    {
        var adjacentOffsets = new Vector2Int[] { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
        foreach (var offset in adjacentOffsets)
        {
            action(offset);
        }
    }

    protected void SuccessMove(Vector2Int direction)
    {
        isChecked = true;
        Vector2Int oldPosition = Position;
        Position += direction;

        ForAdjacentBlocks(offset =>
        {
            var adjacentBlock = GetBlockAt(oldPosition + offset);
            if (adjacentBlock == null || offset == direction || offset == -direction) return;

            adjacentBlock.Move(direction, offset == -direction ? "pull" : "stick");
        });
    }

    private Vector2Int ConvertToLength(Vector2Int index) => index + Vector2Int.one;

    private Vector2Int ConvertToIndex(Vector2Int length) => length - Vector2Int.one;
}

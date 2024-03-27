using UnityEngine;

public class WallBlock : Block
{
    public override bool Move(Vector2Int direction, string type)
    {
        return false;
    }
}

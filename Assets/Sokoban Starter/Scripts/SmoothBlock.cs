using UnityEngine;

public class SmoothBlock : Block
{
    public override bool Move(Vector2Int direction, string type)
    {
        if (isChecked) return false;

        if (type != "push") return false;

        Vector2Int posFront = Position + direction;
        if (!isInBound(posFront)) return false;

        Block frontBlock = GetBlockAt(posFront);

        if (frontBlock == null)
        {
            SuccessMove(direction);
            return true;
        }

        if (frontBlock is WallBlock) return false;

        bool pushResult = frontBlock.Move(direction, "push");
        if (isChecked) return false;
        if (pushResult) SuccessMove(direction);
        return pushResult;
    }
}

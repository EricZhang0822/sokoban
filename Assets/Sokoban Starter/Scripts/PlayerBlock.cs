using Unity.VisualScripting;
using UnityEngine;

public class PlayerBlock : Block
{
    // Override the Update method
    new void Update()
    {
        base.Update(); // Call the base class Update method

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            Move(Vector2Int.down, "");
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            Move(Vector2Int.up, "");
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            Move(Vector2Int.left, "");
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            Move(Vector2Int.right, "");
    }

    public override bool Move(Vector2Int direction, string type)
    {
        if (isChecked) return false;

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

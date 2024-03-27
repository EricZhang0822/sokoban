using UnityEngine;

public class ClingyBlock : Block
{
    public override bool Move(Vector2Int direction, string type)
    {
        if (isChecked) return false;

        if (type != "pull") return false;

        SuccessMove(direction);
        return true;
    }
}

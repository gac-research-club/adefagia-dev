using UnityEngine;

public class Functions
{
    public static Vector3 CoordinateToPosition(Vector2Int coordinate, float height, float size)
    {
        var x = coordinate.x * size;
        var z = coordinate.y * size;

        return new Vector3(x, height, z);
    }

    public static Vector2Int PositionToCoordinate(Vector3 position, float size)
    {
        var x = (int) (position.x / size);
        var y = (int) (position.z / size);

        return new Vector2Int(x, y);
    }
}

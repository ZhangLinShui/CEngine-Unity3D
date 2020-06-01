using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public SplinePath path;

    public SplinePath CreatePath()
    {
        return path = new SplinePath(Vector2.zero);
    }

    void Reset()
    {
        CreatePath();
    }
}
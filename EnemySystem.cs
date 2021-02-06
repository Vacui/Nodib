using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemySystem {
    public List<Route> routes = new List<Route>();
}

[System.Serializable]
public class Route {

    [SerializeField] private Vector2 p0 = Vector2.zero;
    [SerializeField] private Vector2 p1 = Vector2.zero;
    [SerializeField] private Vector2 p2 = Vector2.zero;
    [SerializeField] private Vector2 p3 = Vector2.zero;

    public Route(Vector2 point0, Vector2 point1, Vector2 point2, Vector2 point3) {
        p0 = point0;
        p1 = point1;
        p2 = point2;
        p3 = point3;
    }

    public Vector2 GetPoint(int index) {
        index = Mathf.Clamp(index, 0, 3);
        if (index == 0)
            return p0;
        if (index == 1)
            return p1;
        if (index == 2)
            return p2;
        if (index == 3)
            return p3;
        return Vector2.zero;
    }

    public Vector2 GetPosition(float progress, bool invert = false) {
        if (invert)
            return Mathf.Pow(1 - progress, 3) * p3 +
                    3 * Mathf.Pow(1 - progress, 2) * progress * p2 +
                    3 * (1 - progress) * Mathf.Pow(progress, 2) * p1 +
                    Mathf.Pow(progress, 3) * p0;
        else
            return Mathf.Pow(1 - progress, 3) * p0 +
                    3 * Mathf.Pow(1 - progress, 2) * progress * p1 +
                    3 * (1 - progress) * Mathf.Pow(progress, 2) * p2 +
                    Mathf.Pow(progress, 3) * p3;
    }

}
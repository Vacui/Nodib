using UnityEngine;

public class RouteGraphic : MonoBehaviour {

    [SerializeField] private Route route;

    [Header("Components")]
    [SerializeField] private Transform point0 = null;
    [SerializeField] private Transform point1 = null;
    [SerializeField] private Transform point2 = null;
    [SerializeField] private Transform point3 = null;

    private void OnDrawGizmos() {
        route = new Route(point0.position, point1.position, point2.position, point3.position);

        for (float t = 0; t <= 1; t += 0.1f)
            Gizmos.DrawSphere(route.GetPosition(t), 0.1f);

        Gizmos.DrawLine(route.GetPoint(0), route.GetPoint(1));
        Gizmos.DrawLine(route.GetPoint(2), route.GetPoint(3));
    }

    public Route GetRoute() {
        return new Route(point0.position, point1.position, point2.position, point3.position);
    }

}
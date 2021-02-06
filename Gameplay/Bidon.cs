using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Bidon : MonoBehaviour {

    [SerializeField] private RouteGraphic testRoute = null;
    [SerializeField] private List<Route> routes = new List<Route>();
    private int currentRouteIndex = 0;
    private float progress = 0;
    private Vector2 unitPosition = Vector2.zero;

    private bool invertOnEnd = false;
    private bool inverted = false;

    private const float MOVE_RATE = 0.01f;
    [SerializeField] private float moveSpeed = 1;
    [SerializeField] private float startWaitTime = 0;
    private bool isFirstStart = true;
    [SerializeField] private float betweenWaitTime = 0;

    private float currentWaitTime = 0;
    private Coroutine move = null;
    private bool isMoving {
        get { return move != null; }
    }

    private void Awake() {
        if (testRoute != null)
            routes = new List<Route>() { testRoute.GetRoute() };
        Initialize(routes);
    }

    public void Initialize(List<Route> newRoutes) {
        SetRoutes(newRoutes);
        currentRouteIndex = 0;
        OnMoveStart();
    }

    private void SetRoutes(List<Route> newRoutes) {
        routes = newRoutes;
        invertOnEnd = routes.Count == 1;
    }

    private void Update() {
        if (currentWaitTime > 0)
            currentWaitTime -= Time.deltaTime;
        else {
            if (!isMoving) {
                isFirstStart = false;
                OnMoveStart();
            }
        }
    }

    IEnumerator BezierMovement() {
        progress = 0;
        Route currentRoute = routes[currentRouteIndex];

        Vector2 p0;
        Vector2 p1;
        Vector2 p2;
        Vector2 p3;

        if (inverted) {
            p0 = currentRoute.GetPoint(3);
            p1 = currentRoute.GetPoint(2);
            p2 = currentRoute.GetPoint(1);
            p3 = currentRoute.GetPoint(0);
        } else {
            p0 = currentRoute.GetPoint(0);
            p1 = currentRoute.GetPoint(1);
            p2 = currentRoute.GetPoint(2);
            p3 = currentRoute.GetPoint(3);
        }

        while(progress <= 1) {
            if (!GameManager.paused || progress == 0) {
                progress += Time.deltaTime * moveSpeed;

                transform.position = currentRoute.GetPosition(progress, inverted);
            }

            yield return new WaitForSeconds(MOVE_RATE);
        }

        OnMoveEnd();
    }

    public void OnMoveStart() {
        if (isFirstStart)
            currentWaitTime = startWaitTime;
        else
            move = StartCoroutine(BezierMovement());
    }

    public void OnMoveEnd() {
        StopCoroutine(move);
        move = null;

        currentRouteIndex++;

        if (currentRouteIndex < 0 || currentRouteIndex >= routes.Count)
            currentRouteIndex = 0;

        if (invertOnEnd)
            inverted = !inverted;

        if (betweenWaitTime > 0)
            currentWaitTime = betweenWaitTime;
        else
            OnMoveStart();
    }

}
using UnityEngine;

public class ProjectileReflectionEmitter : MonoBehaviour {
    [SerializeField] private LineRenderer myLineRenderer = null;
    [SerializeField] private LayerMask collisionLayerMask = 0;
    [SerializeField] private int maxReflectionCount = 5;
    [SerializeField] private float maxStepDistance = 200;

    private void Awake() {
        myLineRenderer.positionCount = 4;
        myLineRenderer.SetPosition(0, transform.position);
    }

    public void DrawPrediction(Vector2 direction) {
        DrawPredictedReflectionPattern(transform.position, direction, maxReflectionCount);
    }

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining) {
        if (reflectionsRemaining == 0)
            return;

        Vector3 startingPosition = position;

        Ray2D ray = new Ray2D(position, direction);
        RaycastHit2D hit = Physics2D.Raycast(position, direction, maxStepDistance, collisionLayerMask);
        if (hit) {
            direction = Vector3.Reflect(direction.normalized, hit.normal);
            position = hit.point;
        } else {
            position += direction * maxStepDistance;
        }

        myLineRenderer.SetPosition(4 - reflectionsRemaining, position);

        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }
}
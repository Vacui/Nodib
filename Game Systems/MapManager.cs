using System;
using UnityEngine;

public class MapManager : MonoBehaviour {

    public static MapManager instance = null;
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    [Header("Prefabs")]
    [SerializeField] private GameObject endPrefab = null;
    [SerializeField] private GameObject pointPrefab = null;
    [SerializeField] private GameObject nodibPrefab = null;
    [SerializeField] private GameObject bidonPrefab = null;
    [SerializeField] private GameObject bidonRouteDotPrefab = null;
    private const float TIME_BETWEEN_DOTS = 0.1f;

    [Header("Components")]
    private Transform levelParentTransform = null;

    public Level currentLevel = null;

    public static event Action LevelLoaded = null;

    private void Start() {
        CreateLevel();
        // Default boundaries
        FindObjectOfType<Boundaries>().CreateBoundaries();
    }

    public void CreateLevel() {
        Debug.Log("Trying to create level");

        Level newLevel = GameManager.currentLevel;

        if (newLevel != null && !newLevel.isEmpty) {
            Debug.Log("Creating level");
            GameManager.Pause();
            GameManager.ScheduleResume(ref LevelLoaded);
            Camera.main.orthographicSize = newLevel.cameraSize;

            if (levelParentTransform != null)
                Destroy(levelParentTransform.gameObject);

            levelParentTransform = new GameObject("Level").transform;
            levelParentTransform.parent = transform;
            levelParentTransform.localPosition = Vector3.zero;


            // Creating end
            GameManager.endsNum = 1;
            GameObject newEnd = Instantiate(endPrefab, newLevel.nodibPos, Quaternion.identity);
            newEnd.transform.parent = levelParentTransform;

            // Creating nodib
            GameObject nodib = Instantiate(nodibPrefab, newLevel.nodibPos, Quaternion.identity);
            nodib.transform.parent = levelParentTransform;

            // Creating points
            GameManager.pointsNum = newLevel.points.Count;
            foreach (Vector2 point in newLevel.points) {
                GameObject newPoint = Instantiate(pointPrefab, point, Quaternion.identity);

                newPoint.transform.parent = levelParentTransform;
            }

            // Creating Enemy Systems
            foreach (EnemySystem enemySystem in newLevel.enemySystems) {
                Bidon newEnemy = Instantiate(bidonPrefab, Vector3.zero, Quaternion.identity).GetComponent<Bidon>();
                newEnemy.transform.parent = levelParentTransform;
                newEnemy.Initialize(enemySystem.routes);

                foreach (Route route in enemySystem.routes) {
                    Transform newRouteGraphicTransform = new GameObject("Route Graphic").transform;
                    newRouteGraphicTransform.parent = levelParentTransform;
                    newRouteGraphicTransform.position = Vector3.zero;

                    for (float t = 0; t <= 1; t += TIME_BETWEEN_DOTS)
                        NewDot(route.GetPosition(t), newRouteGraphicTransform);

                    if (1 % TIME_BETWEEN_DOTS > 0)
                        NewDot(route.GetPosition(1), newRouteGraphicTransform);
                }
            }

            Debug.Log("Level loaded");
            currentLevel = newLevel;
            LevelLoaded?.Invoke();
            LevelLoaded = null;
        } else
            Debug.LogWarning("Level is null");
    }

    private void NewDot(Vector2 position, Transform parent) {
        GameObject newDot = Instantiate(bidonRouteDotPrefab, position, Quaternion.identity);
        newDot.transform.parent = parent;
        newDot.name = "Route Dot";
    }

}
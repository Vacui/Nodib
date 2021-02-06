using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level{

    public bool isEmpty {
        get { return points.Count <= 0; }
    }

    [Header("Base")]
    [HideInInspector] public int num = 0;
    [Range(5, 10)] public float cameraSize = 5;
    public Vector2 nodibPos = Vector2.zero;
    public List<Vector2> points = new List<Vector2>();
    public List<EnemySystem> enemySystems = new List<EnemySystem>();

    [Header("Narrative")]
    public DialogManager myDialogManager = null;

}
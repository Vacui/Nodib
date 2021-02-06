using UnityEngine;

public class UIHolder : MonoBehaviour {

    public static UIHolder instance = null;

    [HideInInspector] public UIManager_Menu menu = null;
    [HideInInspector] public UIManager_Credits credits = null;
    [HideInInspector] public UIManager_Levels levels = null;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        menu = GetComponent<UIManager_Menu>();
        credits = GetComponent<UIManager_Credits>();
        levels = GetComponent<UIManager_Levels>();
    }

}
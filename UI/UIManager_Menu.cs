using TMPro;
using UnityEngine;

public class UIManager_Menu : UIManager {

    [SerializeField] private TextMeshProUGUI textVersion = null;

    private void OnEnable() {
        UIHolder.instance.credits.OnShowUI += HideUI;
        UIHolder.instance.levels.OnShowUI += HideUI;
    }

    private void OnDisable() {
        UIHolder.instance.credits.OnShowUI -= HideUI;
        UIHolder.instance.levels.OnShowUI -= HideUI;
    }

    private void Awake() {
        if (textVersion != null)
            textVersion.text = "Game version: " + Application.version;
    }

    private void Start() {
        if (!GameManager.fromLevel)
            ShowUI();
    }

    public override void ShowUI() {
        base.ShowUI();
        GameManager.fromLevel = false;
    }

    public void Quit() {
        Application.Quit();
    }

}
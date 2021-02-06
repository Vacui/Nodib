public class UIManager_Credits : UIManager {

    private void OnEnable() {
        UIHolder.instance.menu.OnShowUI += HideUI;
    }

    private void OnDisable() {
        UIHolder.instance.menu.OnShowUI -= HideUI;
    }

    private void Start() {
        HideUI();
    }
}
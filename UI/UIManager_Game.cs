using TMPro;
using UnityEngine;

public class UIManager_Game : UIManager {

    [Header("Components")]
    [SerializeField] private Animator pauseBtn = null;
    [SerializeField] private TextMeshProUGUI textLevelNum = null;
    [SerializeField] private GameObject textLastLevel = null;

    private void Awake() {
        HideUI();
        if (GameManager.currentLevel != null) {
            textLevelNum.text = GameManager.currentLevel.num.ToString();
            if (GameManager.currentLevel.num == GameManager.maxLevel)
                textLastLevel.SetActive(true);
        }
    }

    public override void ShowUI() {
        base.ShowUI();
        GameManager.Pause();
    }

    public override void HideUI() {
        base.HideUI();
        GameManager.Resume();
    }

    public void Resume() {
        HideUI();
        pauseBtn.SetTrigger("pressed");
    }

    public void Restart() {
        GameManager.instance.LoadCurrentLevel();
        Resume();
    }

    public void Menu() {
        GameManager.instance.GoToMenu();
    }

}
using System.Collections.Generic;
using UnityEngine;

public class UIManager_Levels : UIManager {

    [SerializeField] private List<LevelBtn> levelBtns = new List<LevelBtn>();

    private void OnEnable() {
        UIHolder.instance.menu.OnShowUI += HideUI;
    }

    private void OnDisable() {
        UIHolder.instance.menu.OnShowUI -= HideUI;
    }

    private void Start() {
        if (GameManager.fromLevel)
            ShowUI();

        UpdateLevels();
    }

    private void UpdateLevels() {
        int lastLevel = GameManager.GetLastLevel();
        int nextLevel = lastLevel + 1;
        int maxLevel = GameManager.maxLevel;
        int levelNum = 0;

        for (int i = 0; i < levelBtns.Count; i++) {
            levelNum = i + 1;
            levelBtns[i].SetLevelNum(levelNum);

            if (levelNum > nextLevel || levelNum > maxLevel)
                levelBtns[i].Lock();
            else
                levelBtns[i].SetActive(levelNum <= lastLevel + 1, levelNum + 1 <= lastLevel + 1);
        }
    }

    public void Reset() {
        GameManager.instance.Reset();
    }

}
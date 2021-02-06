using System;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(instance);

        if (!PlayerPrefs.HasKey("lastLevel"))
            SetLastLevel(1, true);

        SetLastLevel(8);
    }

    public static bool paused = false;
    public static bool winned = false;
    public static int pointsNum = 0;
    public static int endsNum = 0;
    public static int maxLevel = 8;
    public static Level currentLevel = null;
    public static bool fromLevel = false;

    public static event Action OnGamePause = null;
    public static event Action OnGameResume = null;
    private static int scheduledResumes = 0;

    private void OnEnable() {
        End.Enter += RemoveEnd;
        End.Leave += AddEnd;
        Point.Enter += RemovePoint;
        Nodib.Death += Lost;
    }

    private void OnDisable() {
        End.Enter -= RemoveEnd;
        End.Leave -= AddEnd;
        Point.Enter -= RemovePoint;
        Nodib.Death -= Lost;
    }

    private void Start() {
        GoToMenu(false, false);
    }

    public void GoToMenu(bool toLevelSelection = false, bool useAnimation = true) {
        scheduledResumes = 0;
        fromLevel = toLevelSelection;
        SceneLoader.instance.LoadScene("menu", useAnimation);
        Resume();
    }

    public void LoadLevel(Level level, bool loadScene = true) {
        Debug.Log("Loading level");
        winned = false;
        currentLevel = level;
        if (loadScene)
            SceneLoader.instance.LoadScene("level");
    }
    public void LoadCurrentLevel() {
        LoadLevel(currentLevel, false);
        MapManager.instance.CreateLevel();
    }

    private void RemovePoint() {
        pointsNum--;
    }

    private void RemoveEnd() {
        endsNum--;
        if (pointsNum <= 0)
            Win();
    }

    private void AddEnd() {
        endsNum++;
    }

    private void Win() {
        Debug.LogWarning("Win");
        winned = true;
        SetLastLevel(currentLevel.num);
        GoToMenu(true);
    }

    private void Lost() {
        Debug.LogWarning("Lost");
        LoadCurrentLevel();
    }

    public static void Pause() {
        paused = true;
        OnGamePause?.Invoke();
    }
    
    public static void Resume() {
        if (scheduledResumes <= 0) {
            paused = false;
            OnGameResume?.Invoke();

        }
    }
    public static void ResumeAndClearShedule() {
        if (scheduledResumes > 0) {
            Debug.LogWarning("Resuming from schedule");
            scheduledResumes--;
            if (scheduledResumes <= 0)
                Resume();
        }
    }

    public static void ScheduleResume(ref Action action) {
        action += ResumeAndClearShedule;
        scheduledResumes++;
        Debug.Log("Scheduling resume: " + scheduledResumes);
    }

    public void Reset() {
        SetLastLevel(0, true);
        GoToMenu(true);
    }


    #region player prefs

    public static int GetLastLevel() { return PlayerPrefs.GetInt("lastLevel"); }
    public static void SetLastLevel(int num, bool overrideControl = false) {
        num = Mathf.Clamp(num, 0, maxLevel);
        if (GetLastLevel() < num || overrideControl)
            PlayerPrefs.SetInt("lastLevel", num);
    }

    #endregion

}
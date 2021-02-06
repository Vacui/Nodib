using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelBtn : MonoBehaviour {

    [Header("Components")]
    [SerializeField] private TextMeshProUGUI textLevelNum = null;
    [SerializeField] private GameObject dottedLine = null;
    [SerializeField] private Level myLevel = null;
    [SerializeField] private Button myButton = null;

    [Header("Graphic")]
    [SerializeField] private Color unlockedColor = Color.white;
    [SerializeField] private Image background1Img = null;
    [SerializeField] private Image background2Img = null;
    [SerializeField] private Image lockImg = null;

    public void SetLevelNum(int num) {
        textLevelNum.text = num.ToString();
        myLevel.num = num;
    }

    public void SetActive(bool value, bool isNextActive = false) {
        if (value) {
            dottedLine.SetActive(isNextActive);
            background2Img.color = Color.black;
            background1Img.color = unlockedColor;
            textLevelNum.color = Color.black;
            textLevelNum.enabled = true;
            myButton.onClick.AddListener(LoadLevel);
        } else {
            dottedLine.SetActive(false);
        }
    }

    public void Lock() {
        SetActive(false);
        textLevelNum.enabled = false;
        lockImg.enabled = true;
    }

    private void LoadLevel() {
        if (!GameManager.paused && !SceneLoader.isLoading && GameManager.instance != null) {
            GameManager.instance.LoadLevel(myLevel);
        }
    }

}
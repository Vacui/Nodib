using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager_Dialog : UIManager {

    [SerializeField] private TextMeshProUGUI textContent = null;
    [SerializeField] private Image imageAvatar = null;
    [SerializeField] private RectTransform backgroundTransform = null;
    [SerializeField] private Animator myAnimator = null;
    [SerializeField] private GameObject nextBtn = null;
    private const float WRITING_SPEED = 0.05f;
    private const int MAX_LINE_CHAR = 12;

    private DialogManager currentDialogManager = null;

    public static event Action DialogStart = null;
    public static event Action DialogEnd = null;

    [System.Serializable]
    public struct AvatarSprite {
        public Avatar avatar;
        public Sprite sprite;
    }
    [SerializeField] private List<AvatarSprite> avatarSpriteList = new List<AvatarSprite>();
    private Dictionary<Avatar, Sprite> avatarSpriteDictionary = new Dictionary<Avatar, Sprite>();

    private void Awake() {
        ConvertAvatarSpriteListToDictionary();

        Level newLevel = GameManager.currentLevel;

        if (newLevel != null && newLevel.myDialogManager != null && !newLevel.isEmpty) {
            
            Debug.Log("Starting narrative");
            GameManager.Pause();
            GameManager.ScheduleResume(ref DialogEnd);
            DialogStart?.Invoke();

            ShowUI();

            currentDialogManager = newLevel.myDialogManager;

            NextDialog();

        } else
            HideUI(false);
    }

    private void ConvertAvatarSpriteListToDictionary() {
        foreach (AvatarSprite avatarSprite in avatarSpriteList)
            avatarSpriteDictionary.Add(avatarSprite.avatar, avatarSprite.sprite);
    }

    private void UpdateBackgroundSize() {
        backgroundTransform.sizeDelta = new Vector2(textContent.preferredWidth + 250, textContent.preferredHeight + 100);
    }

    public void NextDialog() {

        StopAllCoroutines();

        DialogData newDialogData = currentDialogManager.GetNextDialogData();

        if (newDialogData != null) {
            myAnimator.SetTrigger("next");
            if (avatarSpriteDictionary[newDialogData.avatar] != imageAvatar.sprite)
                imageAvatar.sprite = avatarSpriteDictionary[newDialogData.avatar];
            StartCoroutine(TextWriting(newDialogData.dialogText));
            if(currentDialogManager.isLastDialog && currentDialogManager.isLastText)
                nextBtn.SetActive(false);
        } else
            CloseDialog();

    }

    private void CloseDialog() {
        HideUI();
        Debug.Log("Closing dialog");
        DialogEnd?.Invoke();
    }

    IEnumerator TextWriting(string text) {
        textContent.text = string.Empty;

        foreach(char c in text) {

            if (c == ' ') {
                string word = text.IndexOf(" ") > -1
                      ? text.Substring(0, text.IndexOf(" "))
                      : text;
                if (textContent.text.Length % MAX_LINE_CHAR + word.Length > MAX_LINE_CHAR)
                    textContent.text += "\n";
                else
                    textContent.text += c;
            } else
                textContent.text += c;

            Canvas.ForceUpdateCanvases();
            UpdateBackgroundSize();
            yield return new WaitForSeconds(WRITING_SPEED);
        }
    }

}
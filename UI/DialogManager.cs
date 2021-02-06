using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogManager {

    [SerializeField] private List<Dialog> dialogs = new List<Dialog>();
    private int currentDialog = 0;
    public bool isEmpty {
        get { return dialogs.Count > 0; }
        private set { isEmpty = value; }
    }
    public bool isLastDialog {
        get { return currentDialog >= dialogs.Count; }
    }
    public bool isLastText {
        get { return dialogs[currentDialog].isLast; }
    }

    public DialogData GetNextDialogData(int index) {
        if (!isLastDialog) {
            string resultText = dialogs[currentDialog].NextDialog();
            if (resultText == string.Empty) {
                currentDialog++;
                return GetNextDialogData();
            } else
                return new DialogData(dialogs[currentDialog].avatar, resultText);
        } else
            return null;
    }
    public DialogData GetNextDialogData() {
        return GetNextDialogData(currentDialog);

    }

}

public enum Avatar {
    Nodib,
    Bidon
}

public class DialogData {
    public Avatar avatar;
    public string dialogText = string.Empty;
    public DialogData(Avatar newAvatar, string newDialogText) {
        avatar = newAvatar;
        dialogText = newDialogText;
    }
}

[System.Serializable]
public class Dialog {
    public Avatar avatar;
    [SerializeField] private List<string> dialogues = new List<string>();
    private int currentDialog = 0;
    public bool isLast {
        get { return currentDialog >= dialogues.Count; }
    }

    public string NextDialog() {
        string result = string.Empty;
        if (!isLast)
            result = dialogues[currentDialog];

        currentDialog++;

        return result;
    }
}
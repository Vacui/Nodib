using System;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [SerializeField] protected GameObject mainUIParent = null;
    private bool isShowed = false;

    public event Action OnShowUI = null;
    public event Action OnHideUI = null;

    public virtual void ShowUI(bool useEvent) {
        isShowed = true;
        if (mainUIParent != null) {
            mainUIParent.SetActive(true);
            if (useEvent)
                OnShowUI?.Invoke();
        } else
            Debug.LogWarning("No reference to UIParent", mainUIParent.gameObject);
    }
    public virtual void ShowUI() { ShowUI(true); }

    public virtual void HideUI(bool useEvent = true) {
        isShowed = false;
        if (mainUIParent != null) {
            mainUIParent.SetActive(false);
            if (useEvent)
                OnHideUI?.Invoke();
        } else
            Debug.LogWarning("No reference to UIParent", gameObject);
    }
    public virtual void HideUI() { HideUI(true); }

    public virtual void ToggleUI() {
        if (isShowed)
            HideUI();
        else
            ShowUI();
    }

}
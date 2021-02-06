using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class Utility {

    #region Obj
    public static Quaternion AimPos(Vector3 originPos, Vector3 targetPos) {
        Vector3 dir = originPos - targetPos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    public static void ResetLocalPosition(Transform objTransform) {
        objTransform.localPosition = Vector3.zero;
    }
    public static void ResetLocalPosition(GameObject obj) {
        ResetLocalPosition(obj.transform);
    }

    public static void RemoveCloneText(GameObject obj) {
        obj.name = obj.name.Replace("(Clone", "");
    }

    public static void SetStateObj(GameObject[] elements, bool state) {
        foreach (GameObject obj in elements) {
            obj.SetActive(state);
            if (obj.tag == UITags.UIEndElement.ToString() && state) {
                if (obj.GetComponent<Animator>() != null) {
                    obj.GetComponent<Animator>().SetTrigger("enter");
                }
            }
        }
    }
    public static void SetStateObj(string tag, bool state) {
        SetStateObj(GameObject.FindGameObjectsWithTag(tag), state);
    }
    #endregion

    #region Space
    public static void AimPos(Transform obj, Vector3 pos) {
        Vector3 dir = pos - obj.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        obj.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    public static Vector3 LerpByDistance(Vector3 A, Vector3 B, float x) {
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }
    public static Vector3 LerpByDistance(Vector3 A, Vector3 B) {
        float x = (B - A).magnitude / 2;
        Vector3 P = x * Vector3.Normalize(B - A) + A;
        return P;
    }

    public static Vector3 GetMouseWorldPosition() {
        Vector3 vec = GetWorldPosition(Input.mousePosition, Camera.main);
        vec.z = 0f;
        return vec;
    }
    public static Vector3 GetMouseWorldPositionWithZ() {
        return GetWorldPosition(Input.mousePosition, Camera.main);
    }

    public static Vector3 GetTouchWorldPosition(Touch touch) {
        return GetWorldPosition(touch.position, Camera.main);
    }

    public static Vector3 GetWorldPosition(Vector3 screenPosition, Camera worldCamera) {
        Vector3 worldPosition = worldCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
    #endregion

    #region UI
    public enum UITags {
        UIMenu,
        UIRestart,
        UIPause,
        UIPauseElement,
        UIResume,
        UIEndElement,
        UISucessElement,
        UIFailElement,
        UILevels,
        UINextLevel
    }

    public static void StoreGameObjectsWithTag(ref GameObject[] array, string tag, bool reset = true) {
        if (reset) {
            array = new GameObject[0];
        }
        array = array.Concat(GameObject.FindGameObjectsWithTag(tag)).ToArray();
    }
    public static void StoreGameObjectsWithTag(ref GameObject[] array, UITags tag, bool reset = true) {
        StoreGameObjectsWithTag(ref array, tag.ToString(), reset);
    }

    public static void SetButton(UITags tag, UnityAction action) {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag.ToString())) {
            Button objBtn = obj.GetComponent<Button>();
            if (objBtn != null) {
                objBtn.onClick.AddListener(action);
            }
        }
    }
    #endregion

    #region Color
    public static Gradient NewGradient(Color color1, Color color2) {
        Gradient tempGradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        GradientColorKey[] colorKey = new GradientColorKey[2];
        colorKey[0].color = color1;
        colorKey[0].time = 0.0f;
        colorKey[1].color = color2;
        colorKey[1].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        GradientAlphaKey[] alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        tempGradient.SetKeys(colorKey, alphaKey);

        return tempGradient;
    }
    #endregion
}
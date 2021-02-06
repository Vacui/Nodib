using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIHyperlink : MonoBehaviour {
    [SerializeField] private string url = string.Empty;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(OpenURL);
    }

    private void OpenURL() {
        Application.OpenURL(url);
    }

}
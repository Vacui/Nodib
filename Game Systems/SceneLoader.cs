using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour{

    public static SceneLoader instance = null;
    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    [SerializeField] public Animator transition;
    private bool isFirstEnd = true;
    private string sceneToLoad = "";

    public static bool isLoading = false;

    public void LoadScene(string name, bool useStartAnimation = true) {
        sceneToLoad = name;
        isFirstEnd = true;
        if (useStartAnimation)
            OnLoadStart();
        else
            OnLoadEnd();
    }

    private void OnLoadStart() {
        isLoading = true;
        StartCoroutine(LoadAnimation("start"));
    }

    private void OnLoadEnd() {
        if (isFirstEnd) {
            isFirstEnd = false;
            SceneManager.LoadScene(sceneToLoad);
            StartCoroutine(LoadAnimation("end"));
        }else
            isLoading = false;
    }

    IEnumerator LoadAnimation(string trigger) {
        transition.SetTrigger(trigger);

        yield return new WaitForSeconds(1);

        OnLoadEnd();
    }
}

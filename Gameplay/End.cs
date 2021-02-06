using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class End : MonoBehaviour
{
    private Collider2D myCollider = null;
    public Vector3 position {
        get { return transform.position; }
    }

    public static event Action Enter = null;
    public static event Action Leave = null;

    private void Awake() {
        myCollider = GetComponent<Collider2D>();
    }

    public void OnEnterStart() {
        Debug.Log("Nodib entering end.", gameObject);
        myCollider.enabled = false;
        if (SoundManager.instance != null)
            SoundManager.instance.PlaySound(Sound.EnterPoint);
    }

    public void OnEnterEnd() {
        Debug.Log("Nodib entered end.", gameObject);
        Enter?.Invoke();
    }

    public void OnLeave() {
        Debug.Log("Nodib leaved end.", gameObject);
        myCollider.enabled = true;
        Leave?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Debug.Log("Leaving end from trigger");
        OnLeave();
    }
}

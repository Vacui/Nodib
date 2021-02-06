using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Point : MonoBehaviour {

    private Collider2D myCollider = null;
    private Animator myAnimator = null;

    public static event Action Enter = null;

    private void Awake() {
        myCollider = GetComponent<Collider2D>();
        myAnimator = GetComponent<Animator>();
    }

    public void OnEnter() {
        Debug.Log("Nodib taking point.", gameObject);
        Enter?.Invoke();
        myCollider.enabled = false;
        myAnimator.SetTrigger("enter");
        gameObject.layer = 0;
        Destroy(gameObject, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Nodib")
            OnEnter();
    }

}
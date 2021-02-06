using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Nodib : MonoBehaviour {

    private static readonly float ENTER_SPEED = 3.5f;
    private static readonly float MAX_VELOCITY = 4;
    private static readonly float PUSH_FORCE_MUTIPLIER = 5;
    private static readonly float MAX_DRAG = 1f;
    public static event Action Death = null;

    [Header("Components")]
    private Camera mainCam = null;
    private Collider2D myCollider2D = null;
    private Rigidbody2D myRigidbody2D = null;
    [SerializeField] private Transform dragSpriteTransform = null;

    [Header("Graphic")]
    [SerializeField] private GameObject deathParticlePrefab = null;

    private bool isDragging = false;
    private bool isReadyToPush = false;
    private bool canDrag = true;
    private Coroutine enter = null;
    private Vector3 startPos = Vector3.zero;
    private Vector3 moveForce;

    private End currentEnd = null;

    private void Awake() {
        mainCam = Camera.main;
        myCollider2D = GetComponent<Collider2D>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnEnable() {
        GameManager.OnGamePause += OnMovePause;
        GameManager.OnGameResume += OnMoveResume;
    }

    private void OnDisable() {
        GameManager.OnGamePause -= OnMovePause;
        GameManager.OnGameResume -= OnMoveResume;
    }

    Vector2 lastVeocity;
    private void Update() {

        if (canDrag && !GameManager.paused && !GameManager.winned) {

            if (Application.isEditor) {
                if (Input.GetMouseButtonDown(0)) {
                    Vector3 inputPos = Utility.GetMouseWorldPosition();
                    OnDragStart(inputPos);
                }
                if (isDragging) {
                    Vector3 inputPos = Utility.GetMouseWorldPosition();
                    OnDrag(inputPos);
                }
                if (Input.GetMouseButtonUp(0)) {
                    Vector3 inputPos = Utility.GetMouseWorldPosition();
                    OnDragEnd();
                }
            }

            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                    OnDragStart(touch);

                if (touch.phase == TouchPhase.Moved)
                    OnDrag(touch);

                if (touch.phase == TouchPhase.Ended)
                    OnDragEnd();
            }

        }

        lastVeocity = myRigidbody2D.velocity;
    }

    #region drag

    private void OnDragStart(Touch touch) {
        OnDragStart(Utility.GetTouchWorldPosition(touch));
    }
    private void OnDragStart(Vector3 worldPosition) {
        Debug.Log("On Drag Start");
        startPos = worldPosition;
        isDragging = true;
    }

    private void OnDrag(Touch touch) {
        OnDrag(Utility.GetTouchWorldPosition(touch));
    }
    private void OnDrag(Vector3 worldPosition) {
        Debug.Log("On Drag");
        Vector3 endPos = worldPosition;
        Debug.DrawLine(startPos, endPos);
        Vector2 direction = startPos - endPos;
        SetMoveForce(direction.normalized * Mathf.Clamp(direction.magnitude, 0, MAX_DRAG));
    }

    private void OnDragEnd() {
        Debug.Log("On Drag End");
        if (moveForce.magnitude >= 0.2f)
            OnMoveStart();
        isDragging = false;
    }

    #endregion

    private void SetMoveForce(Vector2 force) {
        moveForce = force;
        dragSpriteTransform.localPosition = Vector3.zero - moveForce;
    }

    private void FixedUpdate() {
        if (isReadyToPush) {
            myRigidbody2D.AddForce(moveForce * PUSH_FORCE_MUTIPLIER, ForceMode2D.Impulse);
            if (SoundManager.instance != null)
                SoundManager.instance.PlaySound(Sound.Jump);
            ClearMove();
        }
        Vector2 myVelocity = myRigidbody2D.velocity;
        if (myVelocity.magnitude > MAX_VELOCITY)
            myRigidbody2D.velocity = myVelocity.normalized * MAX_VELOCITY;
    }

    #region move

    private void OnMoveStart() {
        myCollider2D.enabled = true;
        isReadyToPush = true;
        canDrag = false;
    }

    private void ClearMove() {
        isReadyToPush = false;
        SetMoveForce(Vector2.zero);
    }

    private Vector2 prevVelocity;
    private void OnMovePause() {
        prevVelocity = myRigidbody2D.velocity;
        myRigidbody2D.velocity = Vector2.zero;
    }

    private void OnMoveResume() {
        myRigidbody2D.velocity = prevVelocity;
    }

    public void OnMoveEnd() {
        myCollider2D.enabled = false;
        myRigidbody2D.velocity = Vector3.zero;
        myRigidbody2D.angularVelocity = 0f;
    }

    #endregion

    private void OnCollisionEnter2D(Collision2D collision) {
        End end = collision.gameObject.GetComponent<End>();
        if (end != null)
            InteractWithEnd(end);
        else {
            Bidon bidon = collision.gameObject.GetComponent<Bidon>();
            if (bidon != null)
                Die();
            else {
                //Bounce
                Debug.Log("Bounce");
                Vector2 direction = Vector2.Reflect(lastVeocity.normalized, collision.contacts[0].normal);
                myRigidbody2D.velocity = direction * Mathf.Max(lastVeocity.magnitude, 0f);
            }
        }
    }

    private void InteractWithEnd(End end) {
        OnMoveEnd();
        currentEnd = end;
        currentEnd.OnEnterStart();
        OnEnterStart();
    }

    #region enter

    IEnumerator LerpEnter(Vector2 startPos, Vector2 endPos) {
        float progress = 0;

        while (progress <= 1 && !GameManager.paused) {
            progress += Time.deltaTime * ENTER_SPEED;
            transform.position = Vector3.Lerp(startPos, endPos, progress);
            yield return new WaitForSeconds(0.01f);
        }

        OnEnterEnd();
    }

    private void OnEnterStart() {
        if (enter != null)
            StopCoroutine(enter);

        enter = StartCoroutine(LerpEnter(transform.position, currentEnd.position));
    }

    private void OnEnterEnd() {
        transform.position = currentEnd.position;
        currentEnd.OnEnterEnd();
        canDrag = true;
    }

    #endregion

    private void Die() {
        Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);
        Death?.Invoke();
        Destroy(gameObject);

        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    public void CreateBoundaries() {
        Vector2 topRightScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        NewBoundary("Top", new Vector2(0, topRightScreen.y), topRightScreen.x * 2, Vector3.right);
        NewBoundary("Bottom", new Vector2(0, -topRightScreen.y), topRightScreen.x * 2, Vector3.right);
        NewBoundary("Top", new Vector2(topRightScreen.x, 0), topRightScreen.y * 2, Vector3.up);
        NewBoundary("Bottom", new Vector2(-topRightScreen.x, 0), topRightScreen.y * 2, Vector3.up);
    }

    private void NewBoundary(string name, Vector2 position, float lenght, Vector3 lenghtVector) {
        GameObject newBoundary = new GameObject(name + " Boundary");
        newBoundary.transform.position = position;
        newBoundary.transform.localScale = (Vector3.one * 0.1f) + (lenghtVector * lenght);
        newBoundary.AddComponent<BoxCollider2D>();
    }
}

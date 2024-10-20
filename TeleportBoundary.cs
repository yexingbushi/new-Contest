using UnityEngine;

public class TeleportBoundary : MonoBehaviour
{
    public enum BoundaryPosition
    {
        Lower,
        Upper,
        Left,
        Right,
    }
    public BoundaryPosition boundaryPosition;

    // 物体触碰该边界时应当被传送的位移
    private Vector2 oppositeDisplacement;
    private Vector2 exitDirection;

    void Start()
    {
        Camera camera= Camera.main;
        float height = 2f * camera.orthographicSize;
        float width = height * camera.aspect;

        if (boundaryPosition == BoundaryPosition.Lower)
        {
            oppositeDisplacement = new Vector2(0, height);
            exitDirection = Vector2.down;
        }
        else if (boundaryPosition == BoundaryPosition.Upper)
        {
            oppositeDisplacement = new Vector2(0, -height);
            exitDirection = Vector2.up;
        }
        else if (boundaryPosition == BoundaryPosition.Left)
        {
            oppositeDisplacement = new Vector2(width, 0);
            exitDirection = Vector2.left;
        }
        else if (boundaryPosition == BoundaryPosition.Right)
        {
            oppositeDisplacement = new Vector2(-width, 0);
            exitDirection = Vector2.right;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out Teleportable teleportable))
        {
            // 如果是原物体
            if (!teleportable.isCopy)
            {
                teleportable.CreateCopy((Vector2)teleportable.transform.position + oppositeDisplacement);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out Teleportable teleportable))
        {
            // 如果是原物体
            if (!teleportable.isCopy && Vector2.Dot(exitDirection, teleportable.GetComponent<Rigidbody2D>().velocity) > 0)
            {
                Destroy(teleportable.gameObject);
            }
            // 如果是复制体
            else if (teleportable.isCopy && Vector2.Dot(exitDirection, teleportable.GetComponent<Rigidbody2D>().velocity) < 0)
            {
                teleportable.isCopy = false;
            }
        }
    }
}

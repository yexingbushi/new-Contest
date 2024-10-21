using UnityEngine;

public class TeleportBoundary : MonoBehaviour
{
    public enum BoundaryPosition
    {
        Lower,   // 下边界
        Upper,   // 上边界
        Left,    // 左边界
        Right,   // 右边界
    }

    public BoundaryPosition boundaryPosition;
    
    // 引用四个边界物体
    public Transform lowerBoundaryObject;
    public Transform upperBoundaryObject;
    public Transform leftBoundaryObject;
    public Transform rightBoundaryObject;

    private Vector2 oppositeDisplacement;
    private Vector2 exitDirection;

    void Start()
    {
        // 获取边界物体的世界坐标
        float leftBoundary = leftBoundaryObject.position.x;
        float rightBoundary = rightBoundaryObject.position.x;
        float lowerBoundary = lowerBoundaryObject.position.y;
        float upperBoundary = upperBoundaryObject.position.y;

        // 根据当前边界位置计算对称位移和退出方向
        if (boundaryPosition == BoundaryPosition.Lower)
        {
            oppositeDisplacement = new Vector2(0, upperBoundary - lowerBoundary);
            exitDirection = Vector2.down;
        }
        else if (boundaryPosition == BoundaryPosition.Upper)
        {
            oppositeDisplacement = new Vector2(0, lowerBoundary - upperBoundary);
            exitDirection = Vector2.up;
        }
        else if (boundaryPosition == BoundaryPosition.Left)
        {
            oppositeDisplacement = new Vector2(rightBoundary - leftBoundary, 0);
            exitDirection = Vector2.left;
        }
        else if (boundaryPosition == BoundaryPosition.Right)
        {
            oppositeDisplacement = new Vector2(leftBoundary - rightBoundary, 0);
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
                // 传送到相对边界位置
                Teleportable newTeleportable = teleportable.CreateCopy((Vector2)teleportable.transform.position + oppositeDisplacement);
                
                // 更新摄像机跟随新物体
                // Camera.main.GetComponent<CameraFollow>().UpdateFollowTarget(newTeleportable.transform);
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

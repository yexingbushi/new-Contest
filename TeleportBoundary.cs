using System.Collections.Generic;
using UnityEngine;

public class TeleportBoundary : MonoBehaviour
{
    // 物体触碰该边界时应当被传送的位移
    public Vector2 oppsiteDisplacement;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out Teleportable teleportable))
        {
            // 如果是原物体
            if (!teleportable.isCopy)
            {
                teleportable.CreateCopy((Vector2)teleportable.transform.position + oppsiteDisplacement);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out Teleportable teleportable))
        {
            // 如果是原物体
            if (!teleportable.isCopy)
            {
                Destroy(teleportable.gameObject);
            }
            // 如果是复制体
            else
            {
                teleportable.isCopy = false;
            }
        }
    }
}

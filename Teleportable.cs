using System.Collections.Generic;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    [HideInInspector] public bool isCopy = false;
    [HideInInspector] public bool hasCopy = false;

    // 在对边位置上复制该物体，保留速度
    public void CreateCopy(Vector2 pos)
    {
        if (!hasCopy)
        {
            hasCopy = true;

            GameObject copy = Instantiate(gameObject, pos, Quaternion.identity);

            copy.GetComponent<Teleportable>().isCopy = true;
            copy.GetComponent<Teleportable>().hasCopy = false;
            copy.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
        }
    }
}
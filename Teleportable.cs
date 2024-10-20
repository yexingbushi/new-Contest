using System.Collections.Generic;
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    [HideInInspector] public bool isCopy = false;

    // 在对边位置上复制该物体，保留速度
    public void CreateCopy(Vector2 pos)
    {
        GameObject copy = Instantiate(gameObject, pos, Quaternion.identity);

        copy.GetComponent<Teleportable>().isCopy = true;
        copy.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity;
    }
}
using UnityEngine;

public class Teleportable : MonoBehaviour
{
    public bool isCopy;

    // 创建复制体的方法，返回新的 Teleportable 对象
    public Teleportable CreateCopy(Vector2 position)
    {
        // 实例化新的 Teleportable 对象
        Teleportable copy = Instantiate(this, position, Quaternion.identity);
        copy.isCopy = true; // 标记为复制体
        return copy; // 返回新的复制体
    }
}

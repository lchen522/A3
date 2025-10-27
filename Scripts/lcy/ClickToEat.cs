using UnityEngine;

// 这个特性会确保任何挂载此脚本的物体都自动添加一个Collider2D组件
// 这是让 OnMouseDown 生效所必需的
[RequireComponent(typeof(Collider2D))]
public class ClickToEat : MonoBehaviour
{
    // 在Unity编辑器中，把你的“被吃掉”的音效文件拖到这里
    public AudioClip eatSound;

    // 当鼠标点击到这个物体的 Collider2D 时，这个函数会被自动调用
    void OnMouseDown()
    {
        // 1. 播放音效
        // 我们使用 PlayClipAtPoint，它会在指定位置创建一个临时的音源来播放声音。
        // 这样做的好处是，即使这个物体马上就消失了，声音也会完整地播放完毕。
        if (eatSound != null)
        {
            AudioSource.PlayClipAtPoint(eatSound, transform.position);
        }

        // 2. 让物体消失
        // gameObject.SetActive(false) 是隐藏物体，之后还可以再让它显示
        // 如果你希望物体被彻底删除，也可以使用 Destroy(gameObject);
        gameObject.SetActive(false);
    }
}
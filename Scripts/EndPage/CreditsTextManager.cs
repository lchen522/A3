// CreditsTextManager.cs
// 这是一个独立的管理器，用于在 "EndScene" 中按顺序显示制作人员名单。
// 它可以管理任意数量的 Text 对象。

using UnityEngine;
using UnityEngine.UI; // <-- 使用旧版 Text
using System.Collections;
using System.Collections.Generic; // <-- 需要这个来使用列表 (List)

public class CreditsTextManager : MonoBehaviour
{
    [Header("Text Elements")]
    [Tooltip("【重要】按你希望它们出现的顺序，将所有 Text 对象拖到这里")]
    public List<Text> creditsList;

    [Header("Typing Settings")]
    [Tooltip("打字机效果的速度")]
    public float typingSpeed = 0.05f;

    [Tooltip("上一段文本显示完毕后，到下一段文本开始前的延迟时间")]
    public float pauseBetweenTexts = 0.5f;

    // --- 私有变量 ---
    private List<string> fullTexts; // 用于存储所有文本的原始内容

    void Start()
    {
        // 1. 初始化列表
        fullTexts = new List<string>();

        // 2. 检查并准备所有文本
        if (creditsList == null || creditsList.Count == 0)
        {
            Debug.LogError("Credits List (制作人员名单列表) 没有被设置！", this);
            return;
        }

        // 3. 遍历所有 Text 对象...
        foreach (Text creditText in creditsList)
        {
            // 存储它们的原始内容
            fullTexts.Add(creditText.text);

            // ...然后清空它们，为打字机效果做准备
            creditText.text = "";
        }

        // 4. 启动主协程
        StartCoroutine(ShowCreditsSequentially());
    }

    // --- 主协程：按顺序显示所有文本 ---
    private IEnumerator ShowCreditsSequentially()
    {
        // 遍历我们存储的每一个文本
        for (int i = 0; i < creditsList.Count; i++)
        {
            // 调用“打字机”协程来显示当前文本
            // 'yield return' 会让这个循环暂停，直到打字机效果完成
            yield return StartCoroutine(TypeText(creditsList[i], fullTexts[i]));

            // 在两段文本之间暂停一下
            yield return new WaitForSeconds(pauseBetweenTexts);
        }

        // --- 所有文本都显示完毕 ---
        Debug.Log("制作人员名单显示完毕。");
        // 你可以在这里添加一个 "返回主页" 按钮的淡入效果 (可选)
    }

    // --- 辅助协程：单个打字机效果 ---
    private IEnumerator TypeText(Text textObject, string fullText)
    {
        // 遍历完整文本中的每一个字符
        foreach (char c in fullText)
        {
            // 将字符一个一个加回到 Text 对象上
            textObject.text += c;

            // 等待 'typingSpeed' 秒
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
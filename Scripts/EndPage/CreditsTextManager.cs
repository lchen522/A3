// CreditsTextManager.cs
// ����һ�������Ĺ������������� "EndScene" �а�˳����ʾ������Ա������
// �����Թ������������� Text ����

using UnityEngine;
using UnityEngine.UI; // <-- ʹ�þɰ� Text
using System.Collections;
using System.Collections.Generic; // <-- ��Ҫ�����ʹ���б� (List)

public class CreditsTextManager : MonoBehaviour
{
    [Header("Text Elements")]
    [Tooltip("����Ҫ������ϣ�����ǳ��ֵ�˳�򣬽����� Text �����ϵ�����")]
    public List<Text> creditsList;

    [Header("Typing Settings")]
    [Tooltip("���ֻ�Ч�����ٶ�")]
    public float typingSpeed = 0.05f;

    [Tooltip("��һ���ı���ʾ��Ϻ󣬵���һ���ı���ʼǰ���ӳ�ʱ��")]
    public float pauseBetweenTexts = 0.5f;

    // --- ˽�б��� ---
    private List<string> fullTexts; // ���ڴ洢�����ı���ԭʼ����

    void Start()
    {
        // 1. ��ʼ���б�
        fullTexts = new List<string>();

        // 2. ��鲢׼�������ı�
        if (creditsList == null || creditsList.Count == 0)
        {
            Debug.LogError("Credits List (������Ա�����б�) û�б����ã�", this);
            return;
        }

        // 3. �������� Text ����...
        foreach (Text creditText in creditsList)
        {
            // �洢���ǵ�ԭʼ����
            fullTexts.Add(creditText.text);

            // ...Ȼ��������ǣ�Ϊ���ֻ�Ч����׼��
            creditText.text = "";
        }

        // 4. ������Э��
        StartCoroutine(ShowCreditsSequentially());
    }

    // --- ��Э�̣���˳����ʾ�����ı� ---
    private IEnumerator ShowCreditsSequentially()
    {
        // �������Ǵ洢��ÿһ���ı�
        for (int i = 0; i < creditsList.Count; i++)
        {
            // ���á����ֻ���Э������ʾ��ǰ�ı�
            // 'yield return' �������ѭ����ͣ��ֱ�����ֻ�Ч�����
            yield return StartCoroutine(TypeText(creditsList[i], fullTexts[i]));

            // �������ı�֮����ͣһ��
            yield return new WaitForSeconds(pauseBetweenTexts);
        }

        // --- �����ı�����ʾ��� ---
        Debug.Log("������Ա������ʾ��ϡ�");
        // ��������������һ�� "������ҳ" ��ť�ĵ���Ч�� (��ѡ)
    }

    // --- ����Э�̣��������ֻ�Ч�� ---
    private IEnumerator TypeText(Text textObject, string fullText)
    {
        // ���������ı��е�ÿһ���ַ�
        foreach (char c in fullText)
        {
            // ���ַ�һ��һ���ӻص� Text ������
            textObject.text += c;

            // �ȴ� 'typingSpeed' ��
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
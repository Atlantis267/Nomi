using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkUI : MonoBehaviour
{
    [Header("UI組件")]
    public TextMeshProUGUI textUI;  // 用于显示文本的 UI 元素
    public Image faceImage;
    //public string displayText = "Hello, World!";  // 要显示的文本内容
    [Header("文本文件")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;

     [Header("頭像")]
    public Sprite face01, face02;

    private void Start()
    {
        // 确保一开始文本是隐藏的
        if (textUI != null)
        {
            textUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Text UI is not assigned!");
        }
    }

    // 当其他 Collider 进入触发器时调用此方法
    private void OnTriggerEnter(Collider other)
    {
        if (textUI != null)
        {
            // 显示文本并设置内容
            textUI.gameObject.SetActive(true);
            textUI.text = displayText;
        }
    }

    // 当其他 Collider 离开触发器时调用此方法
    private void OnTriggerExit(Collider other)
    {
        if (textUI != null)
        {
            // 隐藏文本
            textUI.gameObject.SetActive(false);
        }
    }
}

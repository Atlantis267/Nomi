using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkUI : MonoBehaviour
{
    [Header("UI組件")]
    public TextMeshProUGUI textLabel;  // 用于显示文本的 UI 元素
    public Image faceImage;
    //public string displayText = "Hello, World!";  // 要显示的文本内容
    [Header("文本文件")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;

     [Header("頭像")]
    public Sprite face01, face02;

    bool textFinished;
    bool cancelTyping;
    List<string> textList = new List<string>();

    void Awake()
    {
         GetTextFormFile(textFile);
        index = 0;
    }

    private void OnEnable()
    {
        textFinished = true;
        StartCoroutine(SetTextUI());
    }

    // private void Start()
    // {
    //     // 确保一开始文本是隐藏的
    //     if (textUI != null)
    //     {
    //         textUI.gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("Text UI is not assigned!");
    //     }
    // }

    // 当其他 Collider 进入触发器时调用此方法
    private void OnTriggerEnter(Collider other)
    {
        if (textLabel != null)
        {
            // 显示文本并设置内容
            textLabel.gameObject.SetActive(true);
            //textUI.text = displayText;
             if (textFinished && !cancelTyping){
                StartCoroutine(SetTextUI());
            }
            else if(!textFinished && !cancelTyping){
                cancelTyping = true;
            }
        }
    }

    // 当其他 Collider 离开触发器时调用此方法
    private void OnTriggerExit(Collider other)
    {
        if (textLabel != null)
        {
            // 隐藏文本
            textLabel.gameObject.SetActive(false);
        }
    }

    void GetTextFormFile(TextAsset file)
    {
        textList.Clear();
        index = 0;

        var lineDate = file.text.Split('\n');
        
        foreach(var line in lineDate)
        {
            textList.Add(line);
        }
    }

    IEnumerator SetTextUI(){
        textFinished = false;
        textLabel.text = "";

        switch(textList[index]){
            case "A":
                faceImage.sprite = face01;
                index++;
                break;
            case "B":
                faceImage.sprite = face02;
                index++;
                break;
        }

        int letter = 0;
        while(!cancelTyping && letter <textList[index].Length - 1)
        {
            textLabel.text += textList[index][letter];
            letter++;
            yield return new WaitForSeconds(textSpeed);
        }
        textLabel.text = textList[index];
        cancelTyping = false;
        textFinished = true;
        index++;
    }
}

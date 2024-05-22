using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TalkUI : MonoBehaviour
{
    public GameObject talkUI;

    [Header("UI組件")]
    public TextMeshProUGUI textLabel;  // 用于显示文本的 UI 元素


    [Header("文本文件")]
    public TextAsset textFile;
    public int index;
    public float textSpeed;
    private float timer = 0.0f;
    private float interval = 4.5f;


    bool could;
    bool textFinished;

    List<string> textList = new List<string>();

    private void Start()
    {
        GetTextFormFile(textFile);
        index = 0;
    }

    private void OnEnable()
    {
        textFinished = true;
        //StartCoroutine(SetTextUI());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            could = true;
            this.gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (could)
        {
            talkUI.SetActive(true);
            if (timer >= interval && textFinished)
            {
                StartCoroutine(SetTextUI());
                timer -= interval;
            }
        }
        if (index == textList.Count)
        {
            gameObject.SetActive(false);
            talkUI.SetActive(false);
            index = 0;
            return;
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

    IEnumerator SetTextUI()
    {
        textFinished = false;
        textLabel.text = "";

        for (int i = 0; i < textList[index].Length; i++)
        {
            textLabel.text += textList[index][i];

            yield return new WaitForSeconds(textSpeed);
        }
        textFinished = true;
        index++;
    }
}

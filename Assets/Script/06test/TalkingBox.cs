using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingBox : MonoBehaviour
{
    public GameObject Button;
    public GameObject talkUI;

    // 当其他Collider进入触发器时调用此方法
   private void OnTriggerEnter(Collider other)
    {
        if (talkUI != null)
        {
            // 显示文本并设置内容
            talkUI.gameObject.SetActive(true);
        }
    }

    // 当其他Collider离开触发器时调用此方法
    private void OnTriggerExit(Collider other)
    {
        // 隐藏按钮
        Button.SetActive(false);
        // 确保退出时对话UI也隐藏
        talkUI.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        // 如果按钮处于激活状态且按下了R键
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.R))
        {
            // 显示对话UI
            talkUI.SetActive(true);
        }
    }
}

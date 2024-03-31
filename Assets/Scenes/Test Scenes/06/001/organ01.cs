using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class organ01 : MonoBehaviour
{ 
    private Animator animator;

    void Start()
    {
        // 获取Animator组件
        animator = GetComponent<Animator>();
        
        // 将Animator的默认动画状态设置为循环动画状态
        animator.SetBool("Loop", true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

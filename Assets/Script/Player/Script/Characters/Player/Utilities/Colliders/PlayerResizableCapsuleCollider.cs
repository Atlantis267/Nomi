using System.Collections;
using System;
using UnityEngine;
[Serializable]
public class PlayerResizableCapsuleCollider : ResizableCapsuleCollider
{
    [field: SerializeField] public PlayerTriggerColliderData TriggerColliderData { get; private set; }
    protected override void OnInitialize()
    {
        base.OnInitialize();

        TriggerColliderData.Initialize();
    }
}

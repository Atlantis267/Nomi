using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleColliderData
{
    public CapsuleCollider Collider { get; private set; }
    public Vector3 ColliderCenterInLocalSpace { get; private set; }
    public Vector3 ColliderVerticalEvtents { get; private set; }
    public float skinWidth { get; private set; } = 0.08f;
    public float slopeLimit { get; private set; } = 50f;
    public void Initialize(GameObject gameObject)
    {
        if (Collider != null)
        {
            return;
        }

        Collider = gameObject.GetComponent<CapsuleCollider>();

        UpdateColliderData();
    }
    public void UpdateColliderData()
    {
        ColliderCenterInLocalSpace = Collider.center;

        ColliderVerticalEvtents = new Vector3(0.0f, Collider.bounds.extents.y, 0.0f);
    }
}

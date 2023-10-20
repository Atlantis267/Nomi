using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    public HookScript grapplingGun;
    private Vector3 currentGrapplePosition;
    private LineRenderer lr;
    public GameObject hook;

    private Spring spring;
    public int quality;
    public float damper;
    public float streagth;
    public float velocity;
    public float waveCount;
    public float waveHeight;
    public AnimationCurve affectCurve;
    // Start is called before the first frame update
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        DrawRope();
    }
    void DrawRope()
    {
        if (!grapplingGun.IsGrappling())
        {
            currentGrapplePosition = hook.transform.position;
            spring.Reset();

            if (lr.positionCount >= 0)
            {
                lr.positionCount = 0;
            }
        }
        if (lr.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            lr.positionCount = quality + 1;
        }
        spring.SetDamper(damper);
        spring.SetStrength(streagth);
        spring.Update(Time.deltaTime);

        var grapplePoint = hook.transform.position;
        var gunTipPosition = grapplingGun.caster.position;
        var up = Quaternion.LookRotation((grapplePoint - gunTipPosition).normalized) * Vector3.up;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 100f);

        for (int i = 0; i < quality + 1; i++)
        {
            var delta = i / (float)quality;
            var offect = up * waveHeight * Mathf.Sin(delta * waveCount * Mathf.PI) * spring.Value * affectCurve.Evaluate(delta);

            lr.SetPosition(i, Vector3.Lerp(gunTipPosition, currentGrapplePosition, delta) + offect);
        }
    }
}

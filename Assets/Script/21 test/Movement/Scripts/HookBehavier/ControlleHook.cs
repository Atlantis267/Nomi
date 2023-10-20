using UnityEngine;
using UnityEngine.UI;

public class ControlleHook : MonoBehaviour
{
    //Prefab
    public GameObject hookOBJ;
    public GameObject hookOrign;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public ParticleSystem chargeParticle;
    //public Vector3 target;

    [Header("Grappling")]
    public float maxGrappleDistance;
    public float grappleDelayTime;

    [Header("Cooldown")]
    public float grapplingCd;
    private float grapplingCdTimer = 0f;

    private bool grappling = false;

    [SerializeField]private bool hasCharge;
    [SerializeField] private float timeToCharge;

    private float chargeTime;
    // Use this for initialization
    void Start()
    {
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
        if (chargeParticle.isPlaying) chargeParticle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        ChargeParticle();
        if (grapplingCdTimer >= grapplingCd)
        {
            grappling = true;
        }
        else
        {
            grappling = false;
            grapplingCdTimer += Time.deltaTime;
            grapplingCdTimer = Mathf.Clamp(grapplingCdTimer, 0.0f, grapplingCd);
        }
        Vector3 direction = Vector3.forward;
        Ray theRay = new Ray(transform.position, transform.TransformDirection(direction * maxGrappleDistance));
        Debug.DrawRay(transform.position, transform.TransformDirection(direction * maxGrappleDistance));
        if (Input.GetKey(KeyCode.Mouse0) && grappling)
        {
            chargeTime += Time.deltaTime;
        }
        if(chargeTime >= timeToCharge)
        {
            hasCharge = true;
            //if (!chargeParticle.isPlaying) chargeParticle.Play();
        }
        else
        {
            hasCharge = false;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && grappling && !hasCharge)
        {
            if (!hasCharge)
            {
                    grappling = true;
                    Debug.Log("Something was hit");
                    Hook();
                    grapplingCdTimer = 0.0f;                    
                    //Invoke(nameof(Open), grapplingCd);
                    //target = hit.point;                
            }
            if (chargeParticle.isPlaying) chargeParticle.Stop();
        }
      if (Input.GetKeyUp(KeyCode.Mouse0) && grappling && chargeTime >= timeToCharge)
        {
            if (hasCharge)
            {
                if (Physics.Raycast(theRay, out RaycastHit hit, maxGrappleDistance, whatIsGrappleable))
                {
                    grappling = true;
                    Debug.Log(hit.collider.gameObject.name + "Something was hit");
                    CertainlyHook();
                    grapplingCdTimer = 0.0f;
                    //Invoke(nameof(Open), grapplingCd);
                    //target = hit.point;
                }
            }
            chargeTime = 0f;
            if (chargeParticle.isPlaying) chargeParticle.Stop();
        }
    }

    void Hook()
    {
        if (hookOBJ != null)
        {
            //hookOrign.SetActive(false);
            var hook = Instantiate(hookOBJ, gunTip.position, gunTip.rotation);
            hook.GetComponent<HookScript>().caster = transform;
            hook.GetComponent<HookScript>().range = Mathf.Lerp(4, 12, chargeTime);
        }
        //Creates prefab in front of camera using camera's rotation
    }
    void Open()
    {
        hookOrign.SetActive(true);
    }
    void ChargeParticle()
    {     
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
                if (!chargeParticle.isPlaying) chargeParticle.Play();
        }
    }
    void CertainlyHook()
    {
        if (hookOBJ != null)
        {
            //hookOrign.SetActive(false);
            var hook = Instantiate(hookOBJ, gunTip.position, gunTip.rotation);
            hook.GetComponent<HookScript>().caster = transform;
            hook.GetComponent<HookScript>().range = 50f;
        }
    }
}
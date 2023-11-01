using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSensor : MonoBehaviour
{
    public LayerMask movableLayer;
    public enum NextPlayerMovement
    {
        jump,   
        climbLow,    
        climbHigh,   
        vault        
    }
    public NextPlayerMovement nextMovement = NextPlayerMovement.jump;

    public float lowClimbHeight = 0.5f;

    public float hightClimbHeight = 1.6f;

    public float checkDistance = 1f;

    public float climbAngle = 45f;

    public float bodyHeight = 1f;

    public float valutDistance = 0.2f;

    public float movableObjectHeight = 0.8f;


    float climbDistance;  
    Vector3 ledge;    
    Vector3 climbHitNormal;   
    public Vector3 Ledge { get => ledge; }
    public Vector3 ClimbHitNormal { get => climbHitNormal; }


    private void Start()
    {
        climbDistance = Mathf.Cos(climbAngle) * checkDistance;
    }

    public NextPlayerMovement ClimbDetection(Transform playerTransform, Vector3 inputDirection, float offset)
    {
        float climbOffset = Mathf.Cos(climbAngle) * offset;
        //檢查低位置，檢查高度為lowClimbHeight，檢查距離為checkDistance
        if (Physics.Raycast(playerTransform.position + Vector3.up * lowClimbHeight, playerTransform.forward, out RaycastHit obsHit, checkDistance + offset))
        {
            climbHitNormal = obsHit.normal;
            Debug.Log("低位檢測通过" + obsHit.normal);
            //（玩家方位或輸入方向）角度不符合要求（與牆壁法線的角度大於限制角度），玩家跳躍
            if (Vector3.Angle(-climbHitNormal, playerTransform.forward) > climbAngle || Vector3.Angle(-climbHitNormal, inputDirection) > climbAngle)
            {
                Debug.Log("角度不是正的");
                return NextPlayerMovement.jump;
            }

            //在牆壁法線方向再檢測一次低位，檢查距離是climbDistance
            if (Physics.Raycast(playerTransform.position + Vector3.up * lowClimbHeight, -climbHitNormal, out RaycastHit firstWallHit, climbDistance + climbOffset))
            {
                Debug.Log("低位法線方向檢測通過" + firstWallHit.normal);
                //向上提高一個身位bodyHeight，再檢測一次
                if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimbHeight + bodyHeight), -climbHitNormal, out RaycastHit secondWallHit, climbDistance + climbOffset))
                {
                    Debug.Log("向上一個身位法線方向檢測通過");
                    //向上提高兩個身位bodyHeight，再檢測一次
                    if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimbHeight + bodyHeight * 2), -climbHitNormal, out RaycastHit thirdWallHit, climbDistance + climbOffset))
                    {
                        Debug.Log("向上兩個身位法線方向檢測通過");
                        //向上提高三個身位bodyHeight，再檢測一次，仍舊檢測到障礙，玩家跳躍
                        if (Physics.Raycast(playerTransform.position + Vector3.up * (lowClimbHeight + bodyHeight * 3), -climbHitNormal, climbDistance + offset))
                        {
                            Debug.Log("太高了");
                            return NextPlayerMovement.jump;
                        }

                        //第三個身位沒有檢測到障礙，（從第二個身位向上一個身位的為止）向下檢測，碰撞點即為牆邊，玩家執行高位攀爬
                        else if (Physics.Raycast(thirdWallHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight))
                        {
                            Debug.Log("在第二個身位上方檢測到邊緣");
                            ledge = ledgeHit.point;
                            return NextPlayerMovement.climbHigh;
                        }
                    }

                    //第二個身位沒有檢測到障礙，（從第一個身位向上一個身位的為止）向下檢測，碰撞點即為牆邊，玩家執行低位攀爬
                    else if (Physics.Raycast(secondWallHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight))
                    {
                        Debug.Log("在第一個身位上方檢測到邊緣");
                        ledge = ledgeHit.point;
                        if (ledge.y - playerTransform.position.y > hightClimbHeight)
                        {
                            return NextPlayerMovement.climbHigh;
                        }
                        //處於低位攀爬高度，檢查是否可以翻越，檢測到寬度足夠，則使用低位攀爬
                        else if (Physics.Raycast(secondWallHit.point + Vector3.up * bodyHeight - climbHitNormal * valutDistance, Vector3.down, bodyHeight))
                        {
                            return NextPlayerMovement.climbLow;
                        }
                        else
                        {
                            return NextPlayerMovement.vault;
                        }
                    }
                }
                else if (Physics.Raycast(firstWallHit.point + Vector3.up * bodyHeight, Vector3.down, out RaycastHit ledgeHit, bodyHeight))
                {
                    Debug.Log("在低位上方檢測到邊緣");
                    ledge = ledgeHit.point;
                    //處於低位攀爬高度，檢查是否可以翻越，檢測到寬度足夠，則使用低位攀爬
                    if (Physics.Raycast(firstWallHit.point + Vector3.up * bodyHeight - climbHitNormal * valutDistance, Vector3.down, out ledgeHit, bodyHeight))
                    {
                        return NextPlayerMovement.climbLow;
                    }
                    else
                    {
                        return NextPlayerMovement.vault;
                    }
                }
            }
        }
        Debug.Log("跳吧小姑娘");
        return NextPlayerMovement.jump;
    }

    public MovableObject MovableObjectCheck(Transform playerTransform, Vector3 inputDirection)
    {
        if (Physics.Raycast(playerTransform.position + Vector3.up * movableObjectHeight, playerTransform.forward, out RaycastHit hit, checkDistance, movableLayer))
        {
            climbHitNormal = hit.normal;
            if (Vector3.Angle(-climbHitNormal, playerTransform.forward) > climbAngle || Vector3.Angle(-climbHitNormal, inputDirection) > climbAngle)
            {
                return null;
            }
            MovableObject movableObject;
            if (hit.collider.TryGetComponent<MovableObject>(out movableObject))
            {
                return movableObject;
            }
        }
        return null;
    }
}

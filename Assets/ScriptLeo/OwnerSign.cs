using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnerSign : MonoBehaviour
{
    Transform FollowTarget;
    float offsetY = 0.07f;

    public void Init(Transform target)
    {
        FollowTarget = target;
    }

    private void Update()
    {
        if (FollowTarget != null)
        {
            Vector3 targetPos = FollowTarget.position;
            targetPos.y += offsetY;
            transform.position = targetPos;
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private Vector3 grapplePoint;
    public LayerMask grappleLayer;
    public LayerMask grabbableLayer;
    public Transform gunTip, cam, player;
    public Transform holdPoint; 
    private float maxDistance = 100f;
    private SpringJoint joint;
    private Rigidbody grabbedObject;
    public float grabMaxDistance = 15f; 
    public float moveSpeed = 10f; 
    private Vector3 currentGrapplePosition;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            StartGrapple();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }

        if (Input.GetMouseButtonDown(1)) 
        {
            TryGrabObject();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ReleaseObject();
        }

        if (grabbedObject != null)
        {
            MoveObject();
        }
    }

    void LateUpdate()
    {
        DrawRope();
    }

  
    void StartGrapple()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, grappleLayer))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;

            float distanceFromPoint = Vector3.Distance(player.position, grapplePoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lineRenderer.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

  
    void StopGrapple()
    {
        lineRenderer.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 8f);

        lineRenderer.SetPosition(0, gunTip.position);
        lineRenderer.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling()
    {
        return joint != null;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }


    void TryGrabObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, grabMaxDistance, grabbableLayer))
        {
            Rigidbody rb = hit.rigidbody;

            if (rb != null && !rb.isKinematic)
            {
                grabbedObject = rb;
                grabbedObject.useGravity = false;
                grabbedObject.drag = 10f;
            }
        }
    }


    void ReleaseObject()
    {
        if (grabbedObject != null)
        {
            grabbedObject.useGravity = true;
            grabbedObject.drag = 0f;
            grabbedObject = null;
        }
    }


    void MoveObject()
    {
        Vector3 targetPosition = holdPoint.position;
        Vector3 direction = targetPosition - grabbedObject.position;
        grabbedObject.velocity = direction * moveSpeed;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHookBehavior : MonoBehaviour
{
    [SerializeField] private Transform grappleOriginTransform;
    [SerializeField] private float retractSpeedIncreaseRate = 1.1f;

    private bool isGrappling;
    private Vector3 retractDirection;

    private Camera playerCam;
    private LineRenderer lineRenderer;
    private Animator animator;
    private Rigidbody2D rb;

    //private Coroutine coroutine_removeGrapplingHookAfter

    private readonly string grapplingHookButton = "Fire1";
    private readonly static int maxGrappleHookDistance = 50;

    private void Awake()
    {
        //external references
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;

        //internal references
        lineRenderer = GetComponent<LineRenderer>() as LineRenderer;
        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //check for grappling hook
        if (Input.GetButtonDown(grapplingHookButton))
        {
            Debug.Log("Grappling Hook Button Pressed!");
            ShootGrapplingHook();
        }
        else if (Input.GetButtonUp(grapplingHookButton))
        {
            ReleaseGrapplingHook();
        }

        if (isGrappling)
        {
            HandleGrappling();
        }
    }

    private void ReleaseGrapplingHook()
    {
        //release the Hoook
        isGrappling = false;
        //remove line renderer
        lineRenderer.positionCount = 0;
    }

    private void HandleGrappling()
    {
        //add velocity in this direction

        //update line renderer
        lineRenderer.SetPosition(0, grappleOriginTransform.position);
        animator.SetBool("Down", true);


    }

    private void ShootGrapplingHook()
    {
        Debug.Log("Shooting hook....");
        //get mouse position in the world
        Vector3 cursorClick = playerCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 originPoint = grappleOriginTransform.position;
        retractDirection = cursorClick - originPoint;

        RaycastHit hitInfo;

        GameObject debugPoint = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        debugPoint.transform.position = cursorClick;

        //raycast to this point
        if (Physics.Raycast(originPoint, retractDirection, out hitInfo, maxGrappleHookDistance))
        {
            //switch on collider's tag to do specific things
            switch (hitInfo.collider.gameObject.tag)
            {
                //if enemy
                case "Enemy":
                    Debug.Log("Grappling Hook hit enemy! That is all.");
                    break;

                //if environment
                case "EnvironmentTile":
                    Debug.Log("Grappling Hook hit environment!");
                    break;
            }

            retractDirection = hitInfo.point - originPoint;//may be redundant, but more precise
            isGrappling = true;

            //handle line renderer
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, originPoint);
            lineRenderer.SetPosition(1, hitInfo.point);
        }
        else
        {
            Debug.Log("Raycast hit nothing....");
        }

        
        //if not, mouse position is line renderer endpoint
        
        
        //get direction from current point to endpoint
        //

        //


    }

    
}

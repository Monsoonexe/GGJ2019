using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHookBehavior : MonoBehaviour
{
    [SerializeField] private Transform grappleOriginTransform;
    [SerializeField] private float retractSpeedIncreaseRate = 1.1f;

    private bool isGrappling;
    private Vector2 retractDirection;

    private Vector3 grapplingHookEndpoint;

    private Camera playerCam;
    private LineRenderer lineRenderer;
    private Animator animator;
    private Rigidbody2D rb;
    private Fox_Move moveScript;

    //private Coroutine coroutine_removeGrapplingHookAfter

    private readonly string grapplingHookButton = "Fire1";
    [SerializeField] private int maxGrappleHookDistance = 5;

    private void Awake()
    {
        InitReferences();
    }


    // Start is called before the first frame update
    void Start()
    {
        InitReferences();
    }

    // Update is called once per frame
    void Update()
    {
        //check for grappling hook
        if (Input.GetButtonDown(grapplingHookButton))
        {
            //Debug.Log("Grappling Hook Button Pressed!");
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

    private void InitReferences()
    {
        //external references
        playerCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>() as Camera;

        //internal references
        lineRenderer = GetComponent<LineRenderer>() as LineRenderer;
        rb = GetComponent<Rigidbody2D>() as Rigidbody2D;
        animator = GetComponent<Animator>() as Animator;
        moveScript = GetComponent<Fox_Move>() as Fox_Move;

    }

    private void ReleaseGrapplingHook()
    {
        //release the Hoook
        isGrappling = false;
        //remove line renderer
        lineRenderer.positionCount = 0;
        moveScript.isGrappling = false;
    }

    private void HandleGrappling()
    {
        //rb.velocity = new Vector2(retractDirection.x * Time.deltaTime * retractSpeedIncreaseRate, rb.velocity.y);
        //add velocity in this direction
        rb.velocity = retractDirection * retractSpeedIncreaseRate;
        //Debug.Log(rb.velocity);
        moveScript.isGrappling = true;

        //update line renderer
        lineRenderer.SetPosition(0, new Vector3(grappleOriginTransform.position.x, grappleOriginTransform.position.y, -1));
        animator.SetBool("Down", true);

        //release when reach destination
        float distanceToHook = Vector3.Distance(transform.position, grapplingHookEndpoint);
        if (distanceToHook <= 1f)
        {
            //Debug.Log("Distance limit reached!");
            ReleaseGrapplingHook();
        }
        

    }

    private void ShootGrapplingHook()
    {
        //Debug.Log("Shooting hook....");
        //get mouse position in the world
        Vector2 cursorClick = playerCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 originPoint = grappleOriginTransform.position;
        retractDirection = cursorClick - originPoint;
        
        int layerMask = (gameObject.layer);

        //raycast to this point
        RaycastHit2D hitInfo = Physics2D.Raycast(originPoint, retractDirection, maxGrappleHookDistance, layerMask);

        if (hitInfo.collider)
        {
            //switch on collider's tag to do specific things
            switch (hitInfo.collider.gameObject.tag)
            {
                //if enemy
                case "Enemy":
                    //Debug.Log("Grappling Hook hit enemy! That is all.");
                    break;

                //if environment
                case "EnvironmentTile":
                    //Debug.Log("Grappling Hook hit environment!");

                    //retractDirection = hitInfo.point - originPoint;//may be redundant, but more precise
                    isGrappling = true;

                    grapplingHookEndpoint = hitInfo.point;

                    //handle line renderer
                    lineRenderer.positionCount = 2;
                    lineRenderer.SetPosition(0, new Vector3(originPoint.x, originPoint.y, -1));
                    lineRenderer.SetPosition(1, new Vector3(hitInfo.point.x, hitInfo.point.y, -1));
                    break;

                default:
                    //Debug.Log("No TAG!");
                    break;
            }
        }

        


        //Tilemap tileMap = hitInfo.collider.gameObject.GetComponent<Tilemap>() as Tilemap;
        //if (tileMap && tileMap.HasTile(new Vector3Int(Mathf.RoundToInt(cursorClick.x), Mathf.RoundToInt(cursorClick.y), 0)))
        //{

        //}
           


    }

    
}

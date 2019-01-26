using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingHookBehavior : MonoBehaviour
{
    [SerializeField] private Transform grappleOriginTransform;

    private LineRenderer lineRenderer;
    private readonly string grapplingHookButton = "GrapplingHook";

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>() as LineRenderer;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GrapplingHook()
    {

    }

    
}

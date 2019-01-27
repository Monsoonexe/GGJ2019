using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{

    [SerializeField] private float wanderDistance = 3f;
    [SerializeField] private float wanderSpeed = 3f;

    private int wanderDirection = 1; //1 or negative 1 for direction
    private Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        startingPosition = transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(Time.deltaTime * wanderSpeed * wanderDirection, 0, 0));
        float distanceFromPivot = Vector3.Distance(startingPosition, transform.position);
        if (distanceFromPivot > wanderDistance) wanderDirection *= -1;
    }
}

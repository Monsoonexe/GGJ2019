using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{

    [SerializeField] private Vector3 rotateSpeed;
    private Transform cachedXform;
    // Start is called before the first frame update
    void Start()
    {
        cachedXform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        cachedXform.Rotate(rotateSpeed * Time.deltaTime);
    }
}

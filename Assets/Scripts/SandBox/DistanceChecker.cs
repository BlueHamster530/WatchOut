using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceChecker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject clone;
    [SerializeField]
    float distance;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(transform.position, clone.transform.position);
    }
}

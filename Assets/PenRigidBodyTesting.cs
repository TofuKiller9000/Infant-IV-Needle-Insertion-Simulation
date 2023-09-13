using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenRigidBodyTesting : MonoBehaviour
{
    private Rigidbody penRB; 

    // Start is called before the first frame update
    void Start()
    {
        penRB = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

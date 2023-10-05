using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followStylus : MonoBehaviour
{

    public GameObject Stylus; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(Stylus.transform.position.x + (-0.07f), Stylus.transform.position.y + (-0.75f), Stylus.transform.position.z);
        gameObject.transform.eulerAngles = new Vector3(Stylus.transform.eulerAngles.x, Stylus.transform.eulerAngles.y, Stylus.transform.eulerAngles.z);
    }
}

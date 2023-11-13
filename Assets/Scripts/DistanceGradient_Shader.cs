using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceGradient_Shader : MonoBehaviour
{

    private Material material;
    public int layerMask;
   // public GameObject sphere;
    private Color _color;

    public GameObject[] handJoints;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<MeshRenderer>().sharedMaterial;
        layerMask = LayerMask.GetMask("Deform");
    }

    // Update is called once per frame
    void Update()
    {
        //print("Distance between Sphere and Plane: " + Vector3.Distance(transform.position, sphere.transform.position));
        Debug.DrawLine(handJoints[5].transform.position, transform.TransformDirection(Vector3.down), _color);
        if(Physics.Raycast(handJoints[5].transform.position, transform.TransformDirection(Vector3.down), out var hit, 5, layerMask))
        {
            _color = Color.green;
            //print(CalculateDistance(transform.position, handJoints[0].transform.position));
            StartGradient(hit.point);

        }
        else
        {
            //print(hit.collider.gameObject.name);
            _color = Color.red;
        }
        
    }

    private void StartGradient(Vector3 position)
    {


        // print(handJoints[0].transform.position);
        // material.SetVector("_SpherePosition", sphere.transform.position);
        //  material.SetVector("_SphereHit", position); //stops updating the position when the raycast starts

        material.SetFloat("_Distance", CalculateDistance(transform.position, handJoints[0].transform.position));
        print(material.GetFloat("_Distance"));
        //JOINTS///
        for (int i = 0; i < handJoints.Length; i++)
        {
            
            material.SetVector("_Joint0" + i.ToString() + "Hit", handJoints[i].transform.position);
            //print(i);
            material.SetVector("_Joint0" + i.ToString() + "Position", handJoints[i].transform.position);
        }
        //material.SetVector("_Joint06Position", handJoints[5].transform.position);
       // material.SetVector("_Joint06Hit", handJoints[5].transform.position);
        //print(material.GetVector("_Position"));
    }
    private float CalculateDistance(Vector2 PointA, Vector2 PointB)
    {
        float dist; 
        return dist = Vector2.Distance(PointA, PointB);
    }
}

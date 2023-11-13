using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveRipple_Shader : MonoBehaviour
{
    private Material material;
    private Color previousColor; 
    // Start is called before the first frame update
    void Start()
    {
        //duplicate the material so changes made at runtime are not remembered
        var renderer = GetComponent<MeshRenderer>();
        material = GetComponent<MeshRenderer>().sharedMaterial;
        renderer.material = material;
        //get the material of the object
        previousColor = material.GetColor("_BaseColor");
        material.SetColor("_RippleColor", previousColor);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CastClickRay(); //if we click our mouse, cast a raycast
        }
    }

    private void OnDestroy()
    {
        if(material != null)
        {
            Destroy(material);
        }
    }

    private void CastClickRay()
    {
        var camera = Camera.main;
        var mousePosition = Input.mousePosition;
        //XY are in screen space, while the Z coordinate is in the view space
        var ray = camera.ScreenPointToRay(new Vector3(mousePosition.x, mousePosition.y, camera.nearClipPlane));
        //if our ray hits a collider, and that collider is attached to this game object, start the ripple
        if(Physics.Raycast(ray, out var hit) && hit.collider.gameObject == gameObject)
        {
            StartRipple(hit.point);
        }
    }

    private void StartRipple(Vector3 center)
    {
        Color rippleColor = Color.HSVToRGB(Random.value, 1, 1);
        material.SetVector("_Ripplecenter", center);
        //the time.timesincelevelloaded value is the same as the time node in the shader graph
        material.SetFloat("_RippleStartTime", Time.time);

        material.SetColor("_BaseColor", previousColor);
        material.SetColor("_RippleColor", rippleColor);

        //store the current ripple color so we can set the base color to it next time
        previousColor = rippleColor;
    }
}

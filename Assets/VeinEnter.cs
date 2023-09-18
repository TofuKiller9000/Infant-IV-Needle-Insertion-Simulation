using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Shared;

namespace HurricaneVR.Framework.Core.Stabbing
{
    public class VeinEnter : MonoBehaviour
    {

        public Material enteredMaterial;
        public GameObject topCube; 
        //public GameObject bottomCube; 
       
        private Material defaultMaterial;
        private HapticSurface topCubeSurface; 
        private HapticSurface bottomCubeSurface;
        private bool inVein; 


        private void Awake()
        {
            topCubeSurface.tag = "Touchable";
            inVein = false; 
        }

        private void Update()
        {
            if(inVein)
            {
                topCube.tag = "Touchable";
            }
            else
            {
                topCube.tag = "Untagged";
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains("Stylus"))
            {
                inVein = true;
                gameObject.GetComponent<MeshRenderer>().material = enteredMaterial;
                topCubeSurface.hlStiffness = 0.5f; topCubeSurface.hlDamping = 0.9f; topCubeSurface.hlStaticFriction = 0.2f; topCubeSurface.hlDynamicFriction = 0.2f;
               // bottomCubeSurface.hlStiffness = 0.5f; bottomCubeSurface.hlDamping = 0.9f; bottomCubeSurface.hlStaticFriction = 0.2f; bottomCubeSurface.hlDynamicFriction = 0.2f;
                topCube.tag = "Untagged";
                //bottomCube.tag = "Touchable";

            }
            else
            {
                print(other.name);
            }
        }

        private void OnTriggerExit(Collider other)
        {
                if (other.name.Contains("Stylus"))
                {
                inVein = false; 
                topCubeSurface.hlStiffness = 0f; topCubeSurface.hlDamping = 0f; topCubeSurface.hlStaticFriction = 0f; topCubeSurface.hlDynamicFriction = 0f;
                //bottomCubeSurface.hlStiffness = 0f; bottomCubeSurface.hlDamping = 0f; bottomCubeSurface.hlStaticFriction = 0f; bottomCubeSurface.hlDynamicFriction = 0f;
                gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
                topCube.tag = "Untagged";
                //bottomCube.tag = "Default";

            }
        }

    }
}

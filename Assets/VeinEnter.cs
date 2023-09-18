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
        private Material defaultMaterial;

        private void Start()
        {
            defaultMaterial = GetComponent<Material>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains("Sword"))
            {

                gameObject.GetComponent<MeshRenderer>().material = enteredMaterial;

            }
        }

        private void OnTriggerExit(Collider other)
        {
                if (other.name.Contains("Sword"))
                {

                    gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;

                }
        }

    }
}

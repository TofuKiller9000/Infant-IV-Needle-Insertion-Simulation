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
        public  Material defaultMaterial;
        public string MaskedLayer; 
        public GameObject HallowTube;
        private int layerMsk;

        private void Awake()
        {
            layerMsk = LayerMask.NameToLayer(MaskedLayer);
        }

        private void OnCollisionEnter(Collision collision)
        {
            var layermask = collision.gameObject.layer; 
            if(collision.gameObject.tag == "Touchable" && layermask == layerMsk)
            {
                collision.gameObject.GetComponent<MeshRenderer>().material = enteredMaterial;
                if(HallowTube != null )
                {
                    HallowTube.GetComponent<HallowTube_Manager>().AlignToVein(collision.contacts[0], collision.transform);
                    //HallowTube.transform.parent = null;
                    //HallowTube.transform.SetParent(collision.gameObject.transform, true);
                }

            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Touchable")
            {
                if(defaultMaterial != null)
                {
                    collision.gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
                }
                if (HallowTube != null)
                {
                    //HallowTube.transform.parent = null;
                    //HallowTube.transform.SetParent(gameObject.transform, true);
                }

            }
            else
            {
                print(collision.gameObject.tag);
            }
        }

    }
}

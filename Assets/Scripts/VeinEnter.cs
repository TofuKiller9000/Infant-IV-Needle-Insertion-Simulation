using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HurricaneVR.Framework.Core.Utils;
using HurricaneVR.Framework.Shared;
using UnityEditor;

namespace HurricaneVR.Framework.Core.Stabbing
{
    public class VeinEnter : MonoBehaviour
    {
        //public bool Resistance_A; //Does not parent
        //public bool Resistance_B; //Does Parent

        public string MaskedLayer;
        public GameObject HallowTube;
        private int layerMsk;
        public Transform end; 

        

        private void Awake()
        {
            layerMsk = LayerMask.NameToLayer(MaskedLayer);
        }


        private void OnCollisionEnter(Collision collision)
        {
            var layermask = collision.gameObject.layer; 
            if(collision.gameObject.tag == "Touchable" && layermask == layerMsk)
            {

                //collision.gameObject.GetComponent<MeshRenderer>().material = enteredMaterial;
                if(HallowTube != null )
                {

                    //HallowTube.GetComponent<HallowTube_Manager>().AlignToVein(collision.contacts[0], transform);

                    HallowTube.GetComponent<HallowTube_Manager>().TurnOnResistance(transform);

                }

            }
        }

        private void OnCollisionExit(Collision collision)
        {
            var layermask = collision.gameObject.layer;
            if (collision.gameObject.tag == "Touchable" && layermask == layerMsk)
            {
                //collision.gameObject.GetComponent<MeshRenderer>().material = enteredMaterial;
                if (HallowTube != null)
                {

                    //HallowTube.GetComponent<HallowTube_Manager>().DisalignToVein();

                        HallowTube.GetComponent<HallowTube_Manager>().TurnOffResistance();
                }

            }
        }

    }
}

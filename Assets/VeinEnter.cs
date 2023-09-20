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
        public bool Resistance_A; //Does not parent
        public bool Resistance_B; //Does Parent

        public string MaskedLayer;
        public GameObject HallowTube;
        private int layerMsk;
        

        private void Awake()
        {
            layerMsk = LayerMask.NameToLayer(MaskedLayer);
        }

        private void Start()
        {


            if (Resistance_A)
            {
                Resistance_B = false;
                HallowTube.transform.parent = null;
                InitializeResistance.Trigger = true; 
            }
            else if (Resistance_B)
            {
                Resistance_A = false;
                HallowTube.transform.SetParent(transform, false);
                InitializeResistance.Trigger = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var layermask = collision.gameObject.layer; 
            if(collision.gameObject.tag == "Touchable" && layermask == layerMsk)
            {

                //collision.gameObject.GetComponent<MeshRenderer>().material = enteredMaterial;
                if(HallowTube != null )
                {

                    if(Resistance_A)
                    {
                        HallowTube.GetComponent<HallowTube_Manager>().AlignToVein(collision.contacts[0], collision.transform);
                    }
                    else
                    {
                        HallowTube.GetComponent<HallowTube_Manager>().TurnOnResistance();
                    }
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
                    if(Resistance_A)
                    {
                        HallowTube.GetComponent<HallowTube_Manager>().DisalignToVein();
                    }
                    else
                    {
                        HallowTube.GetComponent<HallowTube_Manager>().TurnOffResistance();
                    }
                }

            }
        }

    }
}

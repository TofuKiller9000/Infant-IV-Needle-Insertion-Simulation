using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeinFind : MonoBehaviour
{

    public GameObject Vein;
    public GameObject Light;

    public Material wrongmat;
    public Material basemat;
    private bool on = false;
    private float mattime = 1;
    private float mattimer = 10000000;
    // Start is called before the first frame update
    void Start()
    {
        float x = Random.Range(-0.1423f, -0.0332f);
        float z = Random.Range(0.01983428f, 0.3604f);
        Vector3 pos = new Vector3 (x, -0.105f, z);
        float rot = Random.Range(-15f, 15f);
        Vein.transform.localPosition = pos;
        Vein.transform.eulerAngles = new Vector3(0, rot, 0);
    }

    // Update is called once per frame
    void Update()
    {

        if (on)
        {
            if(Time.time > mattimer + mattime)
            {
                Light.GetComponent<Renderer>().material = basemat;
                on = false;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "Poker")
        {
            on = true; mattimer = Time.time;
            Light.GetComponent<Renderer>().material = wrongmat;
            float x = Random.Range(-0.14f, -0.033f);
            float z = Random.Range(0.08f, 0.358f);
            Vector3 pos = new Vector3(x, -0.1058f, z);
            float rot = Random.Range(-15f, 15f);
            Debug.Log("Rotation: " + rot);
            Vein.transform.localPosition = pos;
            Vein.transform.eulerAngles = new Vector3(0, rot, 0);
        }
    }
}

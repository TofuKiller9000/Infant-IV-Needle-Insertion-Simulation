using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VRDeformerManager : MonoBehaviour
{
    [SerializeField] private Deformer[] deformers;
    [SerializeField] private int maxDeformers;
    [SerializeField] private GameObject L_Hand;
    [SerializeField] private GameObject R_Hand;
    [SerializeField] private Deformer[] _deformers;


    private Vector4[] shaderBuffer;

    private void Start()
    {
        //L_Hand = GameObject.Find("[LeftHand Controller] Model Parent");
       // R_Hand = GameObject.Find("[RightHand Controller] Model Parent");

        if (L_Hand == null || R_Hand == null)
            Debug.LogError("Hands Not Found for Deformer Manager");
        shaderBuffer = new Vector4[maxDeformers];
        //this needs to get reset after each start, as Unity does not clear it by default

        Deformer[] L_Hand_tran = L_Hand.GetComponentsInChildren<Deformer>();
        Deformer[] R_Hand_tran = R_Hand.GetComponentsInChildren<Deformer>();
        List<Deformer> tempD = new List<Deformer>();

        StartCoroutine(FindDeformers());


        //foreach (Deformer tran in L_Hand_tran)
        //{
        //    print(tran.name);
        //    if (tran.name.Contains("_Deformer"))
        //    {
        //        tempD.Add(tran.GetComponent<Deformer>());
        //    }

        //}
        //foreach (Deformer tran in R_Hand_tran)
        //{
        //    if (tran.name.Contains("_Deformer"))
        //    {
        //        tempD.Add(tran.GetComponent<Deformer>());
        //    }

        //}
        
    }

    private void Update()
    {
        for (int i = 0; i < maxDeformers; i++)
        {
            shaderBuffer[i] = Vector4.zero;
        }

        int numActive = 0;
        foreach (var deformer in deformers)
        {
            if (numActive >= maxDeformers)
            {
                break; //this is bad change this later
            }

            if (deformer.isActiveAndEnabled)
            {
                Vector3 posn = deformer.transform.position;
                shaderBuffer[numActive] = new Vector4(posn.x, posn.y, posn.z, deformer.radius);
                numActive++;
            }

        }



        Shader.SetGlobalInt("_DeformerNum", numActive);
        //print(Shader.GetGlobalInt("_DeformerCount"));
        Shader.SetGlobalVectorArray("_DeformerPS", shaderBuffer);
    }

    IEnumerator FindDeformers()
    {

        yield return new WaitForSeconds(1f);
        _deformers = FindObjectsOfType(typeof(Deformer)) as Deformer[];
        deformers = _deformers;
    }
}

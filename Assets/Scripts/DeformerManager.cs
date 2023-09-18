using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformerManager : MonoBehaviour
{
    [SerializeField] private Deformer[] deformers;
    [SerializeField] private int maxDeformers;

    private Vector4[] shaderBuffer;
    private float[] strengthBuffer; 

    private void Start()
    {
        shaderBuffer = new Vector4[maxDeformers];
        strengthBuffer = new float[maxDeformers];
        //this needs to get reset after each start, as Unity does not clear it by default
    }

    private void Update()
    {
        for (int i = 0; i < maxDeformers; i++)
        {
            shaderBuffer[i] = Vector4.zero;
            strengthBuffer[i] = 1; 
        }

        int numActive = 0;
        foreach (var deformer in deformers)
        {
            if (numActive >= maxDeformers)
            {
                break; //eh
            }

            if (deformer.isActiveAndEnabled)
            {
                Vector3 posn = deformer.transform.position;
                shaderBuffer[numActive] = new Vector4(posn.x, posn.y, posn.z, deformer.radius);
                strengthBuffer[numActive] = deformer.strength;
                numActive++;
            }

        }



        Shader.SetGlobalInt("_DeformerNum", numActive);
        //print(Shader.GetGlobalInt("_DeformerCount"));
        Shader.SetGlobalVectorArray("_DeformerPS", shaderBuffer);
        Shader.SetGlobalFloatArray("_power", strengthBuffer);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DeformerManager : MonoBehaviour
{
    [Header("Deformer Settings")]
    [SerializeField] private Deformer[] deformers;
    [SerializeField] private int maxDeformers;


    private Vector4[] shaderBuffer;
    private float[] strengthBuffer;

    [Header("Haptic Objects")]
    //public HapticPlugin hapticDevice = null;
    //public HapticEffect hapticEffect;

    public float strength; 

    private void Start()
    {
        shaderBuffer = new Vector4[maxDeformers];
        strengthBuffer = new float[maxDeformers];
        //this needs to get reset after each start, as Unity does not clear it by default

        //if (hapticDevice == null)
        //{
        //    hapticDevice = (HapticPlugin)FindObjectOfType(typeof(HapticPlugin));
        //}
        //if (hapticDevice == null)
        //{
        //    Debug.LogError("Missing Required Haptic Device");
        //}
    }

    private void Update()
    {

        //hapticEffect.Magnitude = hapticDevice.touchingDepth / 10;

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

                // strength = Mathf.Lerp(0, deformer.maxStrength, (float)hapticEffect.Magnitude);
                strength = deformer.maxStrength;
                //print(strength);
                strengthBuffer[numActive] = strength;
                numActive++;
            }

        }

        

        Shader.SetGlobalInt("_DeformerNum", numActive);
        //print(Shader.GetGlobalInt("_DeformerCount"));
        Shader.SetGlobalVectorArray("_DeformerPS", shaderBuffer);
        Shader.SetGlobalFloatArray("_power", strengthBuffer);
    }
}

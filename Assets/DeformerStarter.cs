using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformerStarter : MonoBehaviour
{
    public  Deformer[] deformers;
    public static Deformer[] L_deformers;
    public static Deformer[] R_deformers;

    private void Start()
    {
        if(gameObject.name.Contains("Left"))
        {
            for (int i = 0; i < deformers.Length; i++)
            {
                print(i);
                L_deformers[i] = deformers[i];
                
            }
        }
        else if (gameObject.name.Contains("Right"))
        {
            for (int i = 0; i < deformers.Length; i++)
            {
                R_deformers[i] = deformers[i];
            }
        }
    }
}

Shader "DeformationShader/MyTestShader"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map (RGB) Smoothness / Alpha (A)", 2D) = "white" {}
        [MainColor]   _BaseColor("Base Color", Color) = (1, 1, 1, 1)

        [Space(20)]
        [Toggle(_ALPHATEST_ON)] _AlphaTestToggle("Alpha Clipping", Float) = 0
        _Cutoff("Alpha Cutoff", Float) = 0.5

        [Space(20)]
        [Toggle(_SPECULAR_SETUP)] _MetallicSpecToggle("Workflow, Specular (if on), Metallic (if off)", Float) = 0
        [Toggle(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A)] _SmoothnessSource("Smoothness Source, Albedo Alpha (if on) vs Metallic (if off)", Float) = 0
        _Metallic("Metallic", Range(0.0, 1.0)) = 0
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0.5)
        [Toggle(_METALLICSPECGLOSSMAP)] _MetallicSpecGlossMapToggle("Use Metallic/Specular Gloss Map", Float) = 0
        _MetallicSpecGlossMap("Specular or Metallic Map", 2D) = "black" {}
        // Usually this is split into _SpecGlossMap and _MetallicGlossMap, but I find
        // that a bit annoying as I'm not using a custom ShaderGUI to show/hide them.

        [Space(20)]
        [Toggle(_NORMALMAP)] _NormalMapToggle("Use Normal Map", Float) = 0
        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Bump Scale", Float) = 1

            // Not including Height (parallax) map in this example/template

            [Space(20)]
            [Toggle(_OCCLUSIONMAP)] _OcclusionToggle("Use Occlusion Map", Float) = 0
            [NoScaleOffset] _OcclusionMap("Occlusion Map", 2D) = "bump" {}
            _OcclusionStrength("Occlusion Strength", Range(0.0, 1.0)) = 1.0

            [Space(20)]
            [Toggle(_EMISSION)] _Emission("Emission", Float) = 0
            [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
            [NoScaleOffset]_EmissionMap("Emission Map", 2D) = "black" {}

            [Space(20)]
            [Toggle(_SPECULARHIGHLIGHTS_OFF)] _SpecularHighlights("Turn Specular Highlights Off", Float) = 0
            [Toggle(_ENVIRONMENTREFLECTIONS_OFF)] _EnvironmentalReflections("Turn Environmental Reflections Off", Float) = 0
                // These are inverted fom what the URP/Lit shader does which is a bit annoying.
                // They would usually be handled by the Lit ShaderGUI but I'm using Toggle instead,
                // which assumes the keyword is more of an "on" state.

                // Not including Detail maps in this template
    }

        SubShader
            {

            Tags {
                "RenderPipeline" = "UniversalPipeline"
                "RenderType" = "Opaque"
                "Queue" = "Geometry"
            }

            HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            //CBUFFER must include all of the exposed properties except textures (still need to include tiling and offset values)
            //if multiple materials have different values, then you need to expose the attributes
            //_______________________________________________________________________________________________________________________________________
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _BaseColor;
            float4 _EmissionColor;
            float4 _SpecColor;
            float _Metallic;
            float _Smoothness;
            float _OcclusionStrength;
            float _Cutoff;
            float _BumpScale;
            #define MAX_DEFORMERS 24

            float4 _DeformerPS[MAX_DEFORMERS]; //position, size/radius of circle
            int _DeformerNum; //number that is currently in the scene
            float _power[MAX_DEFORMERS];
            CBUFFER_END
            ENDHLSL

                Pass
                {
                    Name "Forward"
                    Tags {"LightMode" = "UniversalForward"}



                    HLSLPROGRAM

                    #pragma vertex LitPassVertex
                    #pragma fragment LitPassFragment

                    ///Keywords//

                    //Material
                    #pragma shader_feature_local _NORMALMAP
                    #pragma shader_feature_local_fragment _ALPHATEST_ON
                    #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
                    #pragma shader_feature_local_fragment _EMISSION
                    #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
                    #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                    #pragma shader_feature_local_fragment _OCCLUSIONMAP
                    #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
                    #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
                    #pragma shader_feature_local_fragment _SPECULAR_SETUP
                    #pragma shader_feature_local _RECEIVE_SHADOWS_OFF

                    //URP
                    #if UNITY_VERSION >= 202120
                        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
                    #else
                        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
                        #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
                    #endif
                    #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
                    #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
                    #pragma multi_compile_fragment _ _SHADOWS_SOFT
                    #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION // v10+ only (for SSAO support)
                    #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING // v10+ only, renamed from "_MIXED_LIGHTING_SUBTRACTIVE"
                    #pragma multi_compile _ SHADOWS_SHADOWMASK

                    // Other
                    #pragma multi_compile_fog
                    #pragma multi_compile_instancing
                    #pragma multi_compile _ DOTS_INSTANCING_ON
                    #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
                                // Unity Keywords
                    #pragma multi_compile _ LIGHTMAP_ON
                    #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                    #pragma multi_compile_fog
                    
                   




                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

                    // ---------------------------------------------------------------------------
                    // Structs
                    // ---------------------------------------------------------------------------
                    struct Attributes //input to the vertex shader; allows us to obtain the per-vertex data from the mesh
                    {
                        float4 positionOS : POSITION; //Vertex position
                        float2 uv : TEXCOORD0; //UVs
                        float4 color : COLOR; //Vertex Color

                        #ifdef _NORMALMAP
                            float4 tangentOS : TANGENT;
                        #endif
                        float2 lightmapUV : TEXCOORD1;
                        float3 normalOS : NORMAL;
                    };
                    struct Varyings //input to the frag shader, and the output of the vertex shader (assuming there is no geometry shader)
                    {

                        float4 positionCS : SV_POSITION; //clip space position from the vertex shader output
                        float2 uv : TEXCOORD0; //interpolate data across triangles
                        DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
                        float3 positionWS : TEXCOORD2; //?

                        #ifdef _NORMALMAP
                            half4 normalWS : TEXCOORD3;
                            half4 tangentWS : TEXCOORD4; 
                            half4 bitangentWS : TEXCOORD5; 
                        #else
                            float3 normalWS : TEXCOORD3; 
                        #endif

                            #ifdef _ADDITIONAL_LIGHTS_VERTEX
                                half4 fogFactorAndVertexLight	: TEXCOORD6; // x: fogFactor, yzw: vertex light
                            #else
                                half  fogFactor					: TEXCOORD6;
                            #endif

                        #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                            float4 shadowCoord 				: TEXCOORD7;
                        #endif

                    
                        float4 color : COLOR;
                    };

                    #include "Assets/Shaders/MyDeformationShader/MyPBRSurface.hlsl"
                    #include "Assets/Shaders/MyDeformationShader/MyPBRInput.hlsl"

                    // ---------------------------------------------------------------------------
                    // Functions
                    // ---------------------------------------------------------------------------

                    //takes in WS and returns a deform position
                    float DeformLength(float3 vertexWS, float3 movementDirectionWS, float3 deformerPositionWS, float deformerRadius, float power)
                    {
                        //movementDirectionWS currently equals the negative of the normalWS vector
                        float3 difference = vertexWS - deformerPositionWS;
                        float distanceToDeformer = length(difference); //returns the mag of the vector
                        float3 deformDirection = difference / distanceToDeformer;

                        float deformDot = dot(movementDirectionWS, deformDirection); //if dot >= 0, has not touched object yet; otherwise, intersecting with object

                        //float stiffness = 1;
                        float deformerLength = power;

                        /*float projectToPlane = dot(difference, movementDirectionWS);
                        float pointOnPlane = deformerPositionWS + projectToPlane * movementDirectionWS;
                        */
                        if (deformDot > 0)
                        {
                            deformerLength = max(0, distanceToDeformer / power) * power;
                        }
                        else
                        {
                            return -1000;
                            //deformerLength = power + distanceToDeformer; 
                        }
                        return ((deformerRadius - deformerLength) * power);
                    }


                    //Vertex Shader
                    //the main thing our vertex shader needs to do is convert the object space position from the mesh into the clip space position
                    Varyings LitPassVertex(Attributes IN)
                    {
                        Varyings OUT;

                        VertexPositionInputs posnInputs = GetVertexPositionInputs(IN.positionOS.xyz); //GetVP computes the position in each of the commonly used spaces
                        #ifdef _NORMALMAP
                            VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS.xyz, IN.tangentOS);
                        #else
                            VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS.xyz);
                        #endif

                        float3 deformDirection = -normalInputs.normalWS;
                        float maxDeformLength = 0;
                        for (int i = 0; i < _DeformerNum; i++)
                        {
                            maxDeformLength = max(maxDeformLength, DeformLength(posnInputs.positionWS, deformDirection, _DeformerPS[i].xyz, _DeformerPS[i].w, _power[i]));
                        }
                        posnInputs.positionWS += maxDeformLength * deformDirection;



                        OUT.positionCS = TransformWorldToHClip(posnInputs.positionWS);
                        OUT.positionWS = posnInputs.positionWS;

                        half3 viewDirWS = GetWorldSpaceViewDirection(posnInputs.positionWS);
                        half3 vertexLight = VertexLighting(posnInputs.positionWS, normalInputs.normalWS);
                        half fogFactor = ComputeFogFactor(posnInputs.positionCS.z);
                
                        #ifdef _NORMALMAP
                            OUT.normalWS = half4(normalInputs.normalWS, viewDirWS.x);
                            OUT.tangentWS = half4(normalInputs.tangentWS, viewDirWS.y);
                            OUT.bitangentWS = half4(normalInputs.bitangentWS, viewDirWS.z);
                        #else
                            OUT.normalWS = NormalizeNormalPerVertex(normalInputs.normalWS); 
                        #endif

                            OUTPUT_LIGHTMAP_UV(IN.lightmapUV, unity_LightmapST, OUT.lightmapUV);
                            OUTPUT_SH(OUT.normalWS.xyz, OUT.vertexSH);

                            #ifdef _ADDITIONAL_LIGHTS_VERTEX
                                OUT.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
                            #else
                                OUT.fogFactor = fogFactor;
                            #endif
                
                            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                                OUT.shadowCoord = GetShadowCoord(posnInputs);
                            #endif
                        OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap); //_ST is added on
                        // Pass through Vertex Colours :
                        OUT.color = IN.color;
                        return OUT;
                    }


                    //Fragment shader
                    // 
                    //responsible for determining the color of the pixel output
                    //SV_TARGET is used to write the fragment/pixel color to the current render target
                    float3 LitPassFragment(Varyings IN) : SV_Target{

                        //SurfaceData
                        SurfaceData surfaceData; 
                        InitializeSurfaceData(IN, surfaceData);

                        //InputData
                        InputData lightingInput; 
                        InitializeInputData(IN, surfaceData.normalTS, lightingInput);

                        half4 color = UniversalFragmentPBR(lightingInput, surfaceData);

                        color.rgb = MixFog(color.rgb, lightingInput.fogCoord);

                        return color; 

                        //return UniversalFragmentPBR(lightingInput, surfaceData);
                    }
                    ENDHLSL
    }

               // UsePass "Universal Render Pipeline/Lit/ShadowCaster"
             //   UsePass "Universal Render Pipeline/Lit/DepthOnly"
                //this can techniqually cause issues later on, as it won't make this shader compatible with SRP Batcher
                Pass
                {
                    Name "ShadowCaster"
                    Tags {"LightMode" = "ShadowCaster"}

                    ZWrite On
                    ZTest LEqual

                    HLSLPROGRAM
                    #pragma vertex DisplacedShadowPassVertex
                    #pragma fragment ShadowPassFragment

                            //Material Keywords
                    #pragma shader_feature_local_fragment _ALPHATEST_ON
                    #pragma shader_feature_local_fragment_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

                    #pragma multi_compile_instancing

                    #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW
                    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
                    #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"

                    Varyings DisplacedShadowPassVertex(Attributes IN)
                    {
                        Varyings output = (Varyings)0; 

                        VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);

                        output.uv = TRANSFORM_TEX(IN.texcoord, _BaseMap);
                        output.positionCS = GetShadowPositionHClip(IN);
                        return output; 
                     }

                    ENDHLSL
                }

                Pass{
                        Name "DepthOnly"
                        Tags { "LightMode" = "DepthOnly" }

                        ColorMask 0
                        ZWrite On
                        ZTest LEqual

                        HLSLPROGRAM
                        #pragma vertex DepthOnlyVertex
                        #pragma fragment DepthOnlyFragment

                        // Material Keywords
                        #pragma shader_feature _ALPHATEST_ON
                        #pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

                        // GPU Instancing
                        #pragma multi_compile_instancing
                        // #pragma multi_compile _ DOTS_INSTANCING_ON

                        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
                        #include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"
                        ENDHLSL
                    }
    }
}

/*
    ██████╗░██╗░██████╗░██████╗░█████╗░██╗░░░░░██╗░░░██╗███████╗  ░██████╗██╗░░██╗░█████╗░██████╗░███████╗██████╗░
    ██╔══██╗██║██╔════╝██╔════╝██╔══██╗██║░░░░░██║░░░██║██╔════╝  ██╔════╝██║░░██║██╔══██╗██╔══██╗██╔════╝██╔══██╗
    ██║░░██║██║╚█████╗░╚█████╗░██║░░██║██║░░░░░╚██╗░██╔╝█████╗░░  ╚█████╗░███████║███████║██║░░██║█████╗░░██████╔╝
    ██║░░██║██║░╚═══██╗░╚═══██╗██║░░██║██║░░░░░░╚████╔╝░██╔══╝░░  ░╚═══██╗██╔══██║██╔══██║██║░░██║██╔══╝░░██╔══██╗
    ██████╔╝██║██████╔╝██████╔╝╚█████╔╝███████╗░░╚██╔╝░░███████╗  ██████╔╝██║░░██║██║░░██║██████╔╝███████╗██║░░██║
    ╚═════╝░╚═╝╚═════╝░╚═════╝░░╚════╝░╚══════╝░░░╚═╝░░░╚══════╝  ╚═════╝░╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░╚══════╝╚═╝░░╚═╝

                █▀▀▄ █──█ 　 ▀▀█▀▀ █──█ █▀▀ 　 ░█▀▀▄ █▀▀ ▀█─█▀ █▀▀ █── █▀▀█ █▀▀█ █▀▀ █▀▀█ 
                █▀▀▄ █▄▄█ 　 ─░█── █▀▀█ █▀▀ 　 ░█─░█ █▀▀ ─█▄█─ █▀▀ █── █──█ █──█ █▀▀ █▄▄▀ 
                ▀▀▀─ ▄▄▄█ 　 ─░█── ▀──▀ ▀▀▀ 　 ░█▄▄▀ ▀▀▀ ──▀── ▀▀▀ ▀▀▀ ▀▀▀▀ █▀▀▀ ▀▀▀ ▀─▀▀
____________________________________________________________________________________________________________________________________________

        ▄▀█ █▀ █▀ █▀▀ ▀█▀ ▀   █░█ █░░ ▀█▀ █ █▀▄▀█ ▄▀█ ▀█▀ █▀▀   ▄█ █▀█ ▄█▄   █▀ █░█ ▄▀█ █▀▄ █▀▀ █▀█ █▀
        █▀█ ▄█ ▄█ ██▄ ░█░ ▄   █▄█ █▄▄ ░█░ █ █░▀░█ █▀█ ░█░ ██▄   ░█ █▄█ ░▀░   ▄█ █▀█ █▀█ █▄▀ ██▄ █▀▄ ▄█
____________________________________________________________________________________________________________________________________________
License:
    The license is ATTRIBUTION 3.0

    More license info here:
        https://creativecommons.org/licenses/by/3.0/
____________________________________________________________________________________________________________________________________________
This shader has NOT been tested on any other PC configuration except the following:
    CPU: Intel Core i5-6400
    GPU: NVidia GTX 750Ti
    RAM: 16GB
    Windows: 10 x64
    DirectX: 11
____________________________________________________________________________________________________________________________________________
*/
Shader "Ultimate 10+ Shaders/Dissolve_URP"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Base Map", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}

        _Cutoff ("Cutoff", Range(0,1)) = 0.25
        _EdgeWidth ("Edge Width", Range(0,1)) = 0.05
        [HDR]_EdgeColor ("Edge Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalRenderPipeline"
            "Queue"="Transparent"
            "RenderType"="TransparentCutout"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float2 uv          : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;

            float4 _Color;
            float4 _EdgeColor;
            float _Cutoff;
            float _EdgeWidth;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                half noise = tex2D(_NoiseTex, i.uv).r;

                // Dissolve clip
                clip(noise - _Cutoff);

                half edge = smoothstep(_Cutoff, _Cutoff + _EdgeWidth, noise);

                half3 baseCol = tex2D(_MainTex, i.uv).rgb * _Color.rgb;
                half3 finalCol = lerp(_EdgeColor.rgb, baseCol, edge);

                InputData inputData = (InputData)0;
                inputData.normalWS = normalize(i.normalWS);
                inputData.viewDirectionWS = normalize(GetWorldSpaceViewDir(i.positionHCS.xyz));

                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = finalCol;
                surfaceData.emission = _EdgeColor.rgb * (1 - edge);
                surfaceData.alpha = 1;
                surfaceData.metallic = 0;
                surfaceData.smoothness = 0.5;

                return UniversalFragmentPBR(inputData, surfaceData);
            }
            ENDHLSL
        }
    }
}

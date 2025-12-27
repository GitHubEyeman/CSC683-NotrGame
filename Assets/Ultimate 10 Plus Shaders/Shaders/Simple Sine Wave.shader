/*
                ░██████╗██╗███╗░░░███╗██████╗░██╗░░░░░███████╗  ░██████╗██╗███╗░░██╗███████╗
                ██╔════╝██║████╗░████║██╔══██╗██║░░░░░██╔════╝  ██╔════╝██║████╗░██║██╔════╝
                ╚█████╗░██║██╔████╔██║██████╔╝██║░░░░░█████╗░░  ╚█████╗░██║██╔██╗██║█████╗░░
                ░╚═══██╗██║██║╚██╔╝██║██╔═══╝░██║░░░░░██╔══╝░░  ░╚═══██╗██║██║╚████║██╔══╝░░
                ██████╔╝██║██║░╚═╝░██║██║░░░░░███████╗███████╗  ██████╔╝██║██║░╚███║███████╗
                ╚═════╝░╚═╝╚═╝░░░░░╚═╝╚═╝░░░░░╚══════╝╚══════╝  ╚═════╝░╚═╝╚═╝░░╚══╝╚══════╝

           ░██╗░░░░░░░██╗░█████╗░██╗░░░██╗███████╗  ░██████╗██╗░░██╗░█████╗░██████╗░███████╗██████╗░
           ░██║░░██╗░░██║██╔══██╗██║░░░██║██╔════╝  ██╔════╝██║░░██║██╔══██╗██╔══██╗██╔════╝██╔══██╗
           ░╚██╗████╗██╔╝███████║╚██╗░██╔╝█████╗░░  ╚█████╗░███████║███████║██║░░██║█████╗░░██████╔╝
           ░░████╔═████║░██╔══██║░╚████╔╝░██╔══╝░░  ░╚═══██╗██╔══██║██╔══██║██║░░██║██╔══╝░░██╔══██╗
           ░░╚██╔╝░╚██╔╝░██║░░██║░░╚██╔╝░░███████╗  ██████╔╝██║░░██║██║░░██║██████╔╝███████╗██║░░██║
           ░░░╚═╝░░░╚═╝░░╚═╝░░╚═╝░░░╚═╝░░░╚══════╝  ╚═════╝░╚═╝░░╚═╝╚═╝░░╚═╝╚═════╝░╚══════╝╚═╝░░╚═╝

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

Shader "Ultimate 10+ Shaders/Simple Sine Wave_URP"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Base Map", 2D) = "white" {}

        _Speed ("Speed", Float) = 1.25
        _Amplitude ("Amplitude", Float) = 1.0

        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalRenderPipeline"
            "RenderType"="Opaque"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

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
            float4 _MainTex_ST;
            float4 _Color;

            float _Speed;
            float _Amplitude;
            float _Smoothness;
            float _Metallic;

            Varyings vert (Attributes v)
            {
                Varyings o;

                float3 pos = v.positionOS.xyz;
                pos.y += sin((_Time.y + pos.x) * _Speed) * _Amplitude;

                o.positionHCS = TransformObjectToHClip(pos);
                o.normalWS = TransformObjectToWorldNormal(v.normalOS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                half3 albedo = tex2D(_MainTex, i.uv).rgb * _Color.rgb;

                InputData inputData = (InputData)0;
                inputData.normalWS = normalize(i.normalWS);
                inputData.viewDirectionWS = normalize(GetWorldSpaceViewDir(i.positionHCS.xyz));

                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.albedo = albedo;
                surfaceData.metallic = _Metallic;
                surfaceData.smoothness = _Smoothness;
                surfaceData.alpha = 1;

                return UniversalFragmentPBR(inputData, surfaceData);
            }
            ENDHLSL
        }
    }
}


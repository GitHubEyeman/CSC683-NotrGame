Shader "Ultimate 10+ Shaders/Plasma_URP"
{
    Properties
    {
        [HDR]_Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Main Tex", 2D) = "white" {}
        _NoiseTex ("Noise", 2D) = "white" {}
        _MovementDirection ("Movement Direction", Vector) = (0,-1,0,0)

        _FadeStart ("Fade Start Distance", Float) = 0
        _FadeEnd ("Fade End Distance", Float) = 0
    }

    SubShader
    {
        Tags { "RenderPipeline"="UniversalRenderPipeline" "Queue"="Overlay" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite On
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _Color;
            float2 _MovementDirection;

            float _FadeStart;
            float _FadeEnd;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = TransformObjectToWorld(v.positionOS.xyz);
                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                float t = _Time.y;
                float2 uvMove = _MovementDirection * t;

                half noise = tex2D(_NoiseTex, i.uv + uvMove * 0.5).r;
                half4 col = tex2D(_MainTex, i.uv + uvMove);

                col *= _Color;
                col.a *= noise;

                // Distance fade
                float dist = distance(i.worldPos, _WorldSpaceCameraPos);
                float fade = saturate((_FadeEnd - dist) / (_FadeEnd - _FadeStart));
                col.a *= fade;

                return col;
            }
            ENDHLSL
        }
    }
}

Shader "URP/Unlit/LaserPlasmaURP"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1, 0, 0, 1)
        _EmissionStrength ("Emission Strength", Range(0,10)) = 5
        _PulseSpeed ("Pulse Speed", Range(0.1,10)) = 3
        _ScrollSpeed ("Scroll Speed", Range(-5,5)) = 1
        _Distortion ("Distortion Amount", Range(0,1)) = 0.3
        _WidthFade ("Width Fade", Range(0,1)) = 0.08
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend One One
        ZWrite Off
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
            };

            float4 _BaseColor;
            float _EmissionStrength;
            float _PulseSpeed;
            float _ScrollSpeed;
            float _Distortion;
            float _WidthFade;

            float hash(float n)
            {
                return frac(sin(n) * 43758.5453);
            }

            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);

                float a = hash(i.x + i.y * 57.0);
                float b = hash(i.x + 1.0 + i.y * 57.0);
                float c = hash(i.x + (i.y + 1.0) * 57.0);
                float d = hash(i.x + 1.0 + (i.y + 1.0) * 57.0);

                float2 u = f * f * (3.0 - 2.0 * f);

                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float2 uv = IN.uv;
                uv.x += _Time.y * _ScrollSpeed;

                float n = noise(uv * 10.0 + _Time.y * _PulseSpeed);
                uv.y += (n - 0.5) * _Distortion;

                float pulse = (sin(_Time.y * _PulseSpeed) + 1) * 0.5;
                float energy = saturate((noise(uv * 20.0) + pulse) * 0.8) * _EmissionStrength;

                float fade = smoothstep(0.5, 0.5 - _WidthFade, abs(IN.uv.y - 0.5));

                half3 color = _BaseColor.rgb * energy;
                return half4(color * fade, fade);
            }
            ENDHLSL
        }
    }
}

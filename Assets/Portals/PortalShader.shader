Shader "Hidden/PortalShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex("Mask texture", 2D) = "white" {}
        _Cutout("Cutout", Range(0.0, 1.0)) = 0.5 
        _EdgeCutout("Cutout of the edge", Range(0.0, 1.0)) = 0.5 
        _EdgeTex("Edge texture", 2D) = "white"{}
    }
    SubShader
    { 
        Tags{ "Queue" = "Geometry" "IgnoreProjector" = "True" "RenderType" = "Opaque" }
        Lighting Off
        Cull Back
        ZWrite On
        ZTest Less

        Fog{ Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPos: TEXCOORD1;
            };
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }
            sampler2D _MainTex;
            sampler2D _MaskTex;
            sampler2D _EdgeTex;
            float _Cutout;
            float _EdgeCutout;

            fixed4 frag (v2f i) : SV_Target
            {
                i.screenPos /= i.screenPos.w;

                fixed4 l_MaskColor= tex2D(_MaskTex, i.uv); 
                
                if (l_MaskColor.a < _Cutout) clip(-1); 
                
                fixed4 col = tex2D(_MainTex, float2(i.screenPos.x, i.screenPos.y));

                fixed4 edge = tex2D(_EdgeTex, i.uv);

                if(edge.a >_EdgeCutout){
                    return edge;
                    }
                
                return col;
                /*
                i.screenPos /= i.screenPos.w;
                fixed4 mask = tex2D(_MaskTex, i.uv);
                fixed4 edge = tex2D(_EdgeTex, i.uv);

                // Portal area (inside mask)
                if (mask.a >= _Cutout)
                {
                    return tex2D(_MainTex, float2(i.screenPos.x, i.screenPos.y));
                }

                // Edge area (outside mask)
                if (edge.a > 0.08)
                {
                    return edge;
                }

                // Otherwise, nothing (fully clipped)
                clip(-1);
                return 0;
                */
            }
            ENDCG
        }
    }
    }
Shader "Hidden/MegaPortalShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Cullingtext ("Cooling Texture", 2D) = "white" {} //agregar una textura al material en el inspector
        _ClippingOffset ("Clipping Offset", Range(0.0, 0.5)) = 0.25 
    }
    SubShader
    {
        //Tags{ "Queue" = "Geometry " "IgnoreProjector" = "True" "RenderType" = "Opaque" }
        // No culling or depth
        Cull Off 
        ZWrite Off 
        ZTest Always

        Pass
        {
            CGPROGRAM //el programa del shader en si
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;    //+float2(1,1) <-- esto deveria hacerlo que haga un movimiento sinusoidal
                return o;
            }

            sampler2D _MainTex;     //se declara un sample

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);     //coger 1 textura, se samplea y se devuelve el color

                return col;
            }
            ENDCG //fin del shader
        }
    }
}

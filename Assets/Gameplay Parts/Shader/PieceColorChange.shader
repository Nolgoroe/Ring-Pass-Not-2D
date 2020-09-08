Shader "Custom/PieceColorChange"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _WhiteColor("WhiteColor", Color) = (1,1,1,1)
        _Cutoff("Cutoff", Range(0,1)) = 0.5
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // make fog work
                #pragma multi_compile_fog

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

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float4 _Color;
                float4 _WhiteColor;
                float _Cutoff;
                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    UNITY_TRANSFER_FOG(o,o.vertex);
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                // sample the texture
                    

                fixed4 col = tex2D(_MainTex, float2(i.uv.xy));
                if (col.a < _Cutoff) discard;
                //col = lerp(_WhiteColor, _Color, col.a);
                fixed4 FinalColor = lerp(_Color, _WhiteColor, col.r);
                    return FinalColor;
            }
            ENDCG
        }
        }}

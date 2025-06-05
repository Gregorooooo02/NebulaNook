Shader "Particles/Negative" {
    Properties {
        _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
        _MainTex ("Texture", 2D) = "white" {}
        _Glow ("Intensity", Range(0,10)) = 1 //Allows color saturation in HDR rendering.
    }
    SubShader {
        Tags { "Queue" = "Transparent" "PreviewType" = "Plane"}
        LOD 100
        Cull Off
        ZWrite Off
        BlendOp Add
        Blend OneMinusDstColor OneMinusSrcColor //Opacity depends on grey scale. Alpha value is irrelevant, hence "alpha source" in texture properties can be "none" for maximum performance.
        //AlphaToMask On //Required when using texture alpha channel for cropping.
  
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
      
            fixed4 _TintColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            half _Glow;
      
            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _TintColor * 2;
                o.color.rgb *=  _Glow;
                return o;
            }
      
            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.uv) * i.color;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Mobile/Particles/Alpha Blended"
}
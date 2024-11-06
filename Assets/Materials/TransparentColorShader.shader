Shader "Custom/TransparentColorShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}   // 스프라이트의 텍스처
        _TransparentColor ("Transparent Color", Color) = (1, 1, 1, 1) // 투명하게 만들 색상
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 200

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _TransparentColor;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // 텍스처에서 현재 픽셀 색상 읽기
                fixed4 texColor = tex2D(_MainTex, i.texcoord);
                
                // 특정 색상과 비교하여 투명화
                if (abs(texColor.r - _TransparentColor.r) < 0.7 &&
                    abs(texColor.g - _TransparentColor.g) < 0.7 &&
                    abs(texColor.b - _TransparentColor.b) < 0.7)
                {
                    texColor.a = 0; // 투명하게 설정
                }
                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

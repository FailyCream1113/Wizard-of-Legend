Shader "Custom/TransparentColorShader"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}   // ��������Ʈ�� �ؽ�ó
        _TransparentColor ("Transparent Color", Color) = (1, 1, 1, 1) // �����ϰ� ���� ����
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
                // �ؽ�ó���� ���� �ȼ� ���� �б�
                fixed4 texColor = tex2D(_MainTex, i.texcoord);
                
                // Ư�� ����� ���Ͽ� ����ȭ
                if (abs(texColor.r - _TransparentColor.r) < 0.7 &&
                    abs(texColor.g - _TransparentColor.g) < 0.7 &&
                    abs(texColor.b - _TransparentColor.b) < 0.7)
                {
                    texColor.a = 0; // �����ϰ� ����
                }
                return texColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

Shader "Custom/CartoonShader"
{
    Properties
    {
        _DiffuseColor("DiffuseColor", Color) = (1,1,0,1)
        _LightDirection("LightDirection", Vector) = (1,-1,-1,0)
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
            };

            float4 _DiffuseColor;
            float4 _LightDirection;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);  // 법선 벡터를 월드 공간으로 변환
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float ambientStrength = 0.2;
                float4 ambient = ambientStrength * float4(1.0, 1.0, 1.0, 1.0);

                float4 lightDir = normalize(_LightDirection);  // 조명 방향 정규화
                float lightIntensity = max(dot(i.normal, lightDir), 0);  // 조명 강도 계산

                float4 col = (_DiffuseColor * lightIntensity) + ambient;  // 최종 색상 계산

                return col;
            }
            ENDCG
        }
    }
}

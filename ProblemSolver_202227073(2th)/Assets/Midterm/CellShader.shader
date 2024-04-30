Shader "Unlit/YellowShader"
{
    Properties
    {
        _DiffuseColor("DiffuseColor", Color) = (1,1,0,1)
        _LightDirection("LightDirection", Vector) = (1,1,1,0)
        _SpecularColor("SpecularColor", Color) = (1,1,1,1)
        _Shininess("Shininess", Range(0.1, 100)) = 10
        _AmbientColor("AmbientColor", Color)=(1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }


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
                float3 viewDir : TEXCOORD0;
            };

            float4 _DiffuseColor;
            float4 _LightDirection;
            float4 _SpecularColor;
            float _Shininess;
            float4 _AmbientColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = v.normal;
                o.viewDir = normalize(_WorldSpaceCameraPos - v.vertex.xyz);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float4 ambient = _AmbientColor * 0.4f;//주변광색에 원하는 정도의 강도 곱하기


                float4 lightDir = normalize(_LightDirection); //빛의 방향벡터 정규화하기(벡터의 스칼라값이 1이 되게 만듬)
                float lightIntensity = max(dot(i.normal,lightDir),0);//빛의 방향벡터와 프래그먼트의 법선벡터를 내적하여 각도를 구하고 그것을 빛의 강도로 정함//그 각도가 90도~270도면 0으로 만듬
                float4 diffuse=_DiffuseColor * lightIntensity;//분산광은 물체색에 영향을 받으므로 물체의 색과 그 강도를 곱해서 구함


                float3 viewDir = normalize(i.viewDir);// 뷰방향벡터 정규화
                float3 halfwayDir = normalize(lightDir + viewDir);//뷰방향벡터와 빛방향벡터를 더해서 정규화하기


                float specularIntensity = pow(max(dot(i.normal, halfwayDir), 0.0), _Shininess);//프래그먼트의 법선벡터와 하프웨이벡터를 내적하고 밝기를 제곱하여 반사광의 강도를 정함
                float4 specular = _SpecularColor.rgba * specularIntensity;//반사광의 강도와 반사광의 컬러를 곱하여 반사광의 색을 정함

                

                float4 color = diffuse+specular+ambient;//분산광+반사광+주변광을 전부 더하여 최종색 결정
                



                float threshold = 0.1;//색의 임계값을 설정
                float4 banding=floor(color/threshold);// 최종 컬러를 임계값으로 나눠어서 색을 밴딩함
                float4 col=banding*threshold;//밴딩값 적용


                return col;
            }
            ENDCG
        }
    }
}
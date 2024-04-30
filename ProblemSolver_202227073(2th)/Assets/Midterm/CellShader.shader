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
				float4 ambient = _AmbientColor * 0.4f;//�ֺ������� ���ϴ� ������ ���� ���ϱ�


                float4 lightDir = normalize(_LightDirection); //���� ���⺤�� ����ȭ�ϱ�(������ ��Į���� 1�� �ǰ� ����)
                float lightIntensity = max(dot(i.normal,lightDir),0);//���� ���⺤�Ϳ� �����׸�Ʈ�� �������͸� �����Ͽ� ������ ���ϰ� �װ��� ���� ������ ����//�� ������ 90��~270���� 0���� ����
                float4 diffuse=_DiffuseColor * lightIntensity;//�л걤�� ��ü���� ������ �����Ƿ� ��ü�� ���� �� ������ ���ؼ� ����


                float3 viewDir = normalize(i.viewDir);// ����⺤�� ����ȭ
                float3 halfwayDir = normalize(lightDir + viewDir);//����⺤�Ϳ� �����⺤�͸� ���ؼ� ����ȭ�ϱ�


                float specularIntensity = pow(max(dot(i.normal, halfwayDir), 0.0), _Shininess);//�����׸�Ʈ�� �������Ϳ� �������̺��͸� �����ϰ� ��⸦ �����Ͽ� �ݻ籤�� ������ ����
                float4 specular = _SpecularColor.rgba * specularIntensity;//�ݻ籤�� ������ �ݻ籤�� �÷��� ���Ͽ� �ݻ籤�� ���� ����

                

                float4 color = diffuse+specular+ambient;//�л걤+�ݻ籤+�ֺ����� ���� ���Ͽ� ������ ����
                



                float threshold = 0.1;//���� �Ӱ谪�� ����
                float4 banding=floor(color/threshold);// ���� �÷��� �Ӱ谪���� ����� ���� �����
                float4 col=banding*threshold;//����� ����


                return col;
            }
            ENDCG
        }
    }
}
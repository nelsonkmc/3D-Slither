Shader "Unlit/PhongShader"
{
	Properties {
		_PointLightColor ("Point Light Color", Color) = (0, 0, 0)
		_PointLightPosition ("Point Light Position", Vector) = (0.0, 0.0, 0.0)
	}
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            uniform float3 _PointLightColor;
			uniform float3 _PointLightPosition;

			struct vertIn
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 color : COLOR0;
			};

            struct vertOut
			{
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float4 worldVertex : TEXCOORD0;
				float3 worldNormal : TEXCOORD1;
			};

            vertOut vert (vertIn v)
            {
            	float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
				float3 worldNormal = UnityObjectToWorldNormal(v.normal.xyz);

				vertOut o;
				o.worldVertex = worldVertex;
				o.worldNormal = worldNormal;
				o.color = v.color;

				// Transform vertex in world coordinates to camera coordinates
				o.vertex = UnityObjectToClipPos(v.vertex);

				return o;
            }
        
            fixed4 frag (vertOut v) : SV_Target
            {
            	float4 color;

            	// Calculate ambient RGB intensities
				float Ka = 1.3;
				float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

				// Calculate diffuse RBG reflections, we save the results of L.N because we will use it again
				// (when calculating the reflected ray in our specular component)
				float fAtt = 1;
				float Kd = 1;
				float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);

				// should we handle angle that is not between -90 degress to 90 degress
				float LdotN = max(0, dot(L, normalize(v.worldNormal.xyz)));
				float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);

				// Calculate specular reflections
				float Ks = 1;
				float specN = 5; // Values>>1 give tighter highlights
				float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
				float3 R = normalize(2 * LdotN * normalize(v.worldNormal.xyz) - L);
				float VdotR = max(0, dot(V, R));
				if (LdotN == 0.0f){
					VdotR = 0.0f;
				}
				// should we handle angle that is not between -90 degress to 90 degress
				float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(VdotR), specN);

                fixed4 c = 0;
                c.rgb = amb + dif + spe;
                c.a = v.color.a;

                return c;
            }
            ENDCG
        }
    }
}
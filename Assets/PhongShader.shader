// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// This shader is using Phong illumination model and Phone shading
// It is from the Lab5 code with a little modification including:
// add Blending from ShaderLab so that the water can be semi-transparent
// adjust Ks value since Terrain is not a glossy object in general.

Shader "Custom/PhongShader"
{
	Properties
	{
		_PointLightColor("Point Light Color", Color) = (0,0,0)
		_PointLightPosition("Point Light Position", Vector) = (0.0,0.0,0.0)
	}
	SubShader
	{
		Blend SrcAlpha OneMinusSrcAlpha

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
			float4 color : COLOR;
		};

		struct vertOut
		{
			float4 vertex : SV_POSITION;
			float4 color : COLOR;
			float4 worldVertex : TEXCOORD0;
			float3 worldNormal : TEXCOORD1;
		};

		// Implementation of the vertex shader
		vertOut vert(vertIn v)
		{
			vertOut o;

			// Convert Vertex position and corresponding normal into world coords
			float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
			float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

			// Transform vertex in world coordinates to camera coordinates, and pass colour
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.color = v.color;

			// Pass out the world vertex position and world normal to be interpolated
			// in the fragment shader (and utilised)
			o.worldVertex = worldVertex;
			o.worldNormal = worldNormal;

			return o;
		}

		// Implementation of the fragment shader
		fixed4 frag(vertOut v) : SV_Target
		{
			// Our interpolated normal might not be of length 1
			float3 interpNormal = normalize(v.worldNormal);

			// Calculate ambient RGB intensities
			float Ka = 1;
			float3 amb = v.color.rgb * UNITY_LIGHTMODEL_AMBIENT.rgb * Ka;

			// Calculate diffuse RBG reflections
			float fAtt = 1;
			float Kd = 1;
			float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
			float LdotN = dot(L, interpNormal);
			float3 dif = fAtt * _PointLightColor.rgb * Kd * v.color.rgb * saturate(LdotN);

			// Calculate specular reflections using Blinn-Phong approximation
			float Ks = 0.2;
			float specN = 25; // A higher specular power is needed when using Blinn-Phong
			float3 V = normalize(_WorldSpaceCameraPos - v.worldVertex.xyz);
			float3 H = normalize(V + L);
			float3 spe = fAtt * _PointLightColor.rgb * Ks * pow(saturate(dot(interpNormal, H)), specN);

			// Combine Phong illumination model components
			v.color.rgb = amb.rgb + dif.rgb + spe.rgb;

			return v.color;
		}
			ENDCG
		}
	}
}

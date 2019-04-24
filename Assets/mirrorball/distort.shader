Shader "Metropolia/Distort"
{
	Properties
	{
		_NoiseMap("Noise map", 2D) = "white" {}
		_DistortStrength("Distort Strength", Float) = 1
		_NoiseMask ("Mask", 2D) = "white" {}
	}

		SubShader
	{
		Tags{ "Queue" = "Transparent" }

		GrabPass
	{
	}

	Pass
	{
		CGPROGRAM

#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

		struct vertexData
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	struct fragmentData
	{
		float4 vertex : SV_POSITION;
		float2 uv : TEXCOORD0;
		float2 screenuv : TEXCOORD1;
	};

	sampler2D _NoiseMap;
	sampler2D _GrabTexture;
	sampler2D _NoiseMask;
	float _DistortStrength;

	fragmentData vert(vertexData v)
	{
		fragmentData o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		float4 grabPos = ComputeGrabScreenPos(o.vertex);
		o.screenuv = grabPos.xy / grabPos.w;
		return o;
	}

	fixed4 frag(fragmentData i) : SV_Target
	{
		float n = tex2D(_NoiseMap, i.uv);
		float m = tex2D(_NoiseMask, i.uv);
		float2 suv = i.screenuv + (2 * n - 1) * _DistortStrength * m;
		return tex2D(_GrabTexture, suv);

	}

		ENDCG
	}
	}
}
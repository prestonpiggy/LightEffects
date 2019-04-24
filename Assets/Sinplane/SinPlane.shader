Shader "Metropolia/SinPlane"
{
	Properties
	{
		
		_Scale ("Scale", Float) = 10.0
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }

		CGPROGRAM


		#pragma surface surf Standard fullforwardshadows vertex:vert
		#pragma target 3.0

		struct Input
		{
			float3 worldPos;
		};

		
		float _Scale;

		void vert(inout appdata_full v)
		{
			v.vertex.y += sin(v.texcoord.x * _Scale * _Time[1]) * cos(v.texcoord.y * _Scale)*0.1;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = fixed4(IN.worldPos.y, 0, 0, 0.1);
			o.Albedo = c;
			o.Alpha = 1.0;
		}

		ENDCG
	}	
}
Shader "Metropolia/OldFilm"
{
	Properties
	{
		_MainTex("Base", 2D) = "white" {}
	_FilmTex("Film", 2D) = "white" {}
	_FrameCount("Frame Count", Int) = 4
		_FrameSpeed("Frame Speed", Float) = 10
		_Amount("Amount", Float) = 1.0
	}

		SubShader
	{
		Pass
	{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag
#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
	uniform sampler2D _FilmTex;
	uniform int _FrameCount;
	uniform float _FrameSpeed;
	uniform float _Amount;

	float4 frag(v2f_img i) : COLOR
	{
		float4 c = tex2D(_MainTex, i.uv);
		float s = 1.0 / _FrameCount;
		float4 m = tex2D(_FilmTex,
			float2(
				i.uv.x * s + s * floor(_Time[1] * _FrameSpeed),
				i.uv.y
				));
		c *= saturate(m + (1.0 - _Amount));
		return c;
	}

		ENDCG
	}
	}
}
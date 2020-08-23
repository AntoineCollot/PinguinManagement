Shader "PostProcess/Frost" {
	Properties{
		_MainTex("Base (RGB)", 2D) = "white" {}
	_Cutoff("Cutoff", Range(0, 1)) = 0
	_FrostTexture("Frost Texture",2D)="white"{}
	_Distortion ("_Distortion", Float) = 0
	}
		SubShader{
		Pass{
		CGPROGRAM
#pragma vertex vert_img
#pragma fragment frag

#include "UnityCG.cginc"

		uniform sampler2D _MainTex;
		uniform float _Intensity;
		sampler2D _FrostTexture;
		half _Distortion;

	float4 frag(v2f_img i) : COLOR{
		float4 f = tex2D(_FrostTexture,i.uv);
		float intensity = _Intensity * f.a;
		float4 c = tex2D(_MainTex, i.uv + _Distortion * intensity * f.b);
		return c * (1-intensity) + f * intensity;
	}
		ENDCG
	}
	}
}
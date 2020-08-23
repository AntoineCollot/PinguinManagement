Shader "Toon/Ice" {
	Properties {
		_Color ("Main Color", Color) = (0.5,0.5,0.5,1)
		_Ramp ("Toon Ramp (RGB)", 2D) = "gray" {} 
		_BottomColor("Bottom Color", Color) = (0.23, 0, 0.95, 1)
		_RimBrightness("Rim Brightness", Range(3,4)) = 3
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma surface surf ToonRamp

sampler2D _Ramp;

// custom lighting function that uses a texture ramp based
// on angle between light direction and normal
#pragma lighting ToonRamp exclude_path:prepass
inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten)
{
	#ifndef USING_DIRECTIONAL_LIGHT
	lightDir = normalize(lightDir);
	#endif
	
	half d = dot (s.Normal, lightDir)*0.5 + 0.5;
	half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;
	
	half4 c;
	c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
	c.a = 0;
	return c;
}


float4 _Color;
fixed4 _BottomColor;
half _RimBrightness;

struct Input {
	float3 worldPos;
	float3 viewDir;
};

void surf (Input IN, inout SurfaceOutput o) {
	float3 localPos = saturate(IN.worldPos -  mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz);
	float softRim = 1 - saturate(dot(normalize(IN.viewDir), o.Normal));
	float hardRim = round(softRim);
	
	o.Emission = _Color * lerp(hardRim, softRim, localPos.y) * (_RimBrightness * localPos.y);
	
	float innerRime = 1.5 + saturate(dot(normalize(IN.viewDir), o.Normal));
	
	o.Albedo = _Color * pow(innerRime, 0.7) * lerp(_BottomColor, _Color, localPos.y);
}
ENDCG

	} 

	Fallback "Diffuse"
}

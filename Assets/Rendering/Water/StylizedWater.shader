Shader "Unlit/SimpleWater"
{
	Properties
	{
		_Tint("Tint", Color) = (1, 1, 1, .5) 
		_MainTex ("Main Texture", 2D) = "white" {}
		_NoiseTex("Extra Wave Noise", 2D) = "white" {}
		_Speed("Wave Speed", Range(0,1)) = 0.5
		_Amount("Wave Amount", Range(0,1)) = 0.5
		_Height("Wave Height", Range(0,1)) = 0.5
		_Foam("Foamline Thickness", Range(0,3)) = 0.5
 
 
 			_NoiseFreq ("Noise Frequency", Float) = 1
			_NoiseSpeedX ("Noise Speed X", Float) = 1
			_NoiseSpeedY ("Noise Speed Y", Float) = 1
			_NoiseSpeedZ ("Noise Speed Z", Float) = 1
	
		_LowerColorThreshold ("Lower Color Threshold", Range(0,1)) =0.3
		_UpperColorThreshold ("Upper Color Threshold", Range(0,1)) =0.7
		_LowerColor("Lower Color", Color) = (1,1,1,1)
		_MiddleColor("Middle Color", Color) = (1,1,1,1)
		_UpperColor("Upper Color", Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque"  "Queue" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
 
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
 
			#include "UnityCG.cginc"
					
			#include "noiseSimplex.cginc"
 
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
 
			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 scrPos : TEXCOORD1;
				float3 nsPos : TEXCOORD2;
			};
 
			float4 _Tint;
			uniform sampler2D _CameraDepthTexture; //Depth Texture
			sampler2D _MainTex, _NoiseTex;
			float4 _MainTex_ST;
			float _Speed, _Amount, _Height, _Foam; 
			
			half _NoiseFreq;
			half _NoiseSpeedX;
			half _NoiseSpeedY;
			half _NoiseSpeedZ;
 
			v2f vert (appdata v)
			{
				v2f o;
				float4 tex = tex2Dlod(_NoiseTex, float4(v.uv.xy, 0, 0));//extra noise tex
				v.vertex.y += sin(_Time.z * _Speed + (v.vertex.x * v.vertex.z * _Amount * tex)) * _Height;//movement
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.scrPos = ComputeScreenPos(o.vertex); // grab position on screen
				UNITY_TRANSFER_FOG(o,o.vertex);
 
 				o.nsPos = float3(o.vertex.x,o.vertex.y,0) * _NoiseFreq;
				o.nsPos.x += _Time.y * _NoiseSpeedX;
				o.nsPos.y += _Time.y * _NoiseSpeedY;
				o.nsPos.z += _Time.y * _NoiseSpeedZ;
 
				return o;
			}
 
 			fixed4 _UpperColor;
			fixed4 _MiddleColor;
			fixed4 _LowerColor;
			half _UpperColorThreshold;
			half _LowerColorThreshold;
 
			fixed4 frag (v2f i) : SV_Target
			{
				//Shapes
				float ns = snoise(i.nsPos) / 2 + 0.5f;
			
				fixed4 nsCol = _MiddleColor;
				//Take lower color if ns < lower threshold
				nsCol = step(_LowerColorThreshold,ns) * nsCol + step(ns,_LowerColorThreshold) * _LowerColor;
				//Take upper color if ns > upper threshold
				nsCol = step(ns,_UpperColorThreshold) * nsCol + step(_UpperColorThreshold,ns) * _UpperColor;
				
				// sample the texture
				half4 col = tex2D(_MainTex, i.uv) * _Tint;// texture times tint;
				half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos))); // depth
				half4 foamLine =1 - saturate(_Foam * (depth - i.scrPos.w));// foam line by comparing depth and screenposition
				col += foamLine * _Tint; // add the foam line and tint to the texture
				return col * nsCol;
			}
			ENDCG
		}
	}
}
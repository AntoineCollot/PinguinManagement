Shader "Custom/Ice"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Intensity ("Intensity", Range(0,10)) = 0.0
        _BaseIntensity ("Base Intensity", Range(0,10)) = 0.0
        _IceTintHeight ("Ice Tint Height", Range(0,10)) = 0.0
		_IceTint ("Ice Tint Color", Color) = (1,1,1,1)
		//_IceTint ("Ice Tint Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows finalcolor:mycolor

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
			float iceTintAmount;
        };


      fixed4 _IceTint;
	  fixed4 _Color;
	  half _IceTintHeight;
      void mycolor (Input IN, SurfaceOutputStandard o, inout fixed4 color)
      {
          color.rgb = lerp (color,_IceTint, saturate(1-IN.worldPos.y - _IceTintHeight));
      }

        half _Glossiness;
        half _Metallic;
        half _Intensity;
        half _BaseIntensity;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color * _Intensity + fixed4(_BaseIntensity,_BaseIntensity,_BaseIntensity,0);
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

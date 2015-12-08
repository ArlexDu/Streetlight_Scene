Shader "Happy/DoubleSides/Bumped Diffuse Lighted" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_LightMap ("Lightmap (RGB)", 2D) = "black" {}
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	
	_BColor ("Main Color", Color) = (1,1,1,1)
	_BMainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_BBumpMap ("Normalmap", 2D) = "bump" {}
	_BLightMap ("Lightmap (RGB)", 2D) = "black" {}
	_BCutoff ("Alpha cutoff", Range(0,1)) = 0.5
}

SubShader {
	Cull back
	LOD 300
	Tags {"IgnoreProjector"="True" "RenderType" = "TransparentCutout" }
   CGPROGRAM
   #pragma surface surf Lambert alphatest:_Cutoff
   struct Input {
     float2 uv_MainTex;
     float2 uv_BumpMap;
     float2 uv2_LightMap;
   };
   sampler2D _MainTex;
   sampler2D _BumpMap;
   sampler2D _LightMap;
   float4 _Color;
   void surf (Input IN, inout SurfaceOutput o)
     {
       half4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
       o.Albedo = c.rgb;
       half4 lm = tex2D (_LightMap, IN.uv2_LightMap);
       o.Emission = lm.rgb*o.Albedo.rgb;
       o.Alpha = c.a;
       o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
     }
   ENDCG  
    Cull front
    CGPROGRAM
    #pragma surface surf Lambert alphatest:_Cutoff
   struct Input {
     float2 uv_BMainTex;
     float2 uv_BBumpMap;
     float2 uv2_BLightMap;
   };
   sampler2D _BMainTex;
   sampler2D _BBumpMap;
   sampler2D _BLightMap;
   float4 _BColor;
   void surf (Input IN, inout SurfaceOutput o)
     {
       half4 c = tex2D (_BMainTex, IN.uv_BMainTex) * _BColor;
       o.Albedo = c.rgb;
       half4 lm = tex2D (_BLightMap, IN.uv2_BLightMap);
       o.Emission = lm.rgb*o.Albedo.rgb;
       o.Alpha = c.a;
       o.Normal = UnpackNormal(tex2D(_BBumpMap, IN.uv_BBumpMap));
     }
ENDCG
}
FallBack "Transparent/Cutout/Diffuse"
}

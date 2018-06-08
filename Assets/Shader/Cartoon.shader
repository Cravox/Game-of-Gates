// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "Trish/Cartoony" {
Properties {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _BumpMap ("Normalmap", 2D) = "bump" {}
	_CartoonRamp ("Toon Ramp (RGB)", 2D) = "gray" {} 
}

SubShader {
    Tags { "RenderType"="Opaque" }

CGPROGRAM
#pragma surface surf ToonRamp
#pragma lighting ToonRamp
sampler2D _CartoonRamp;

			inline fixed4 LightingToonRamp (SurfaceOutput surf, half3 lightDir, fixed3 viewDir, half atten)
			{
				fixed3 h = normalize (lightDir + viewDir);
				
				half NdotL = dot(surf.Normal, lightDir) * 0.5 + 0.5;
				half3 ramp = tex2D(_CartoonRamp, float2(NdotL, atten)).rgb;
				
				float nh = max (0, dot (surf.Normal, h));
				float spec = pow (nh, surf.Gloss * 128) * surf.Specular;
				
				half4 c;
				c.rgb = ((surf.Albedo * ramp * _LightColor0.rgb + _LightColor0.rgb * spec) * (atten * 2));
				c.a = surf.Alpha;
				return c;
			}

sampler2D _MainTex;
sampler2D _BumpMap;
float4 _Color;

struct Input {
    float2 uv_MainTex;
    float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput surfout) {
    fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
    surfout.Albedo = c.rgb;
	surfout.Alpha = c.a;
	surfout.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));	
}
ENDCG
}

FallBack "Legacy Shaders/Diffuse"
}

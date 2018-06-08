Shader "Custom/Disolve" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)

		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Normal("Normal Map", 2D) = "bump" {}

		_Emissive("Emissive Map", 2D) = "white" {}
		_EmissiveColor("Emissive Color", Color) = (1,1,1,1)

		_Disolve("Disolve Map", 2D) = "white" {}
		_DisolveAmount("Disolve Amount", Range(0,1)) = 1

		_BurnAmount("Burn width", Range(0,0.5)) = 0.25
		_BurnColor("Color", Color) = (1,0,0,1)
	}
		SubShader{
			Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Back

			//LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows alpha:fade

			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _Normal;
			sampler2D _Emissive;
			sampler2D _Disolve;

			fixed4 _Color;
			fixed4 _BurnColor;
			fixed4 _EmissiveColor;
			fixed _DisolveAmount;
			fixed _BurnAmount;

			struct Input {
				float2 uv_MainTex;
				float2 uv_Normal;
				float2 uv_Emissive;
				float2 uv_Disolve;
			};


			void surf(Input IN, inout SurfaceOutputStandard o) 
			{
				fixed4 m = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				fixed4 n = tex2D(_Normal, IN.uv_MainTex);

				fixed3 normal = UnpackNormal(tex2D(_Normal, IN.uv_Normal));
				normal.z /= 3;

				fixed4 e = tex2D(_Emissive, IN.uv_MainTex);
				fixed4 d = tex2D(_Disolve, IN.uv_MainTex);


				int burnArea = int(d.r - (_DisolveAmount + _BurnAmount) + 0.99);
				int disolve = int(d.r - (_DisolveAmount) + 0.99);

				half4 burn = lerp(_BurnColor, 0, burnArea);
				float4 null = (0, 0, 0, 0);

				o.Albedo = lerp(m.rgb, burn, disolve);
				o.Emission = lerp(e.rgb, burn, disolve) * _EmissiveColor;
				o.Normal = normal;

				if(_DisolveAmount == 0)
					o.Alpha = 0;
				else
					o.Alpha = lerp(1.0, 0.0, burnArea);
			}
			ENDCG
		}
			FallBack "Diffuse"
}

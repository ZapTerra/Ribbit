// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toon/BasicSoft" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_Color3 ("Main Color3", Color) = (.5,.5,.5,1)

		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { }
		_Color2("Main Color2", Color) = (.5,.5,.5,1)
		_RimColor("RimColor", Color) = (.5,.5,.5,1)
			_RimPower("RimPower",Float) = 1
			_Color2Power("Color2Power",Float) = 1
	}


	SubShader {
		Tags { "RenderType"="Transparent" }
		        Blend SrcAlpha OneMinusSrcAlpha

		Pass {
			Name "BASE"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			samplerCUBE _ToonShade;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _Color2;
						float4 _Color3;

			float _RimPower;
			float4 _RimColor;
			float _Color2Power;
			

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
			};
			
			struct v2f {
				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
				float3 cubenormal : TEXCOORD1;
				float3 wPos : TEXCOORD2;
				float3 wNormal : TEXCOORD3;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));
				o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;

				o.wNormal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half3 viewDirection =  normalize(i.wPos - _WorldSpaceCameraPos);

				fixed4 cube = texCUBE(_ToonShade, i.cubenormal);
				fixed4 col = lerp(_Color,_Color2,cube.r) * tex2D(_MainTex, i.texcoord);
				fixed4 c = fixed4(2.0f * cube.rgb * col.rgb, col.a);

				half rim = pow(1.0 - saturate(dot(-viewDirection, i.wNormal)), _RimPower); //same concept, but the value is based on how far you are from looking at the normal
				c += _RimColor * rim;
				//if (cube.r > 0.9) {
				//	col = _Color2 * tex2D(_MainTex, i.texcoord);
				//	c = fixed4(2.0f  * col.rgb, col.a);
				//}
				c = lerp(c,_Color3,_Color2Power);
				c.a = _Color2Power;
				return c;
			}
			ENDCG			
		}
	} 

	Fallback "VertexLit"
}

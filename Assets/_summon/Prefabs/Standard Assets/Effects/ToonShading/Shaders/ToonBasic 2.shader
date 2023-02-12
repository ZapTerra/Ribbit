// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toon/BasicGradient" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { }
		_Color2("Main Color2", Color) = (.5,.5,.5,1)
		
		_Color3("Gradient Color", Color) = (.5,.5,.5,1)
		_Color4("Gradient Color2", Color) = (.5,.5,.5,1)
		_Color5("Shadow Color", Color) = (0,0,0,1)
		_Color6("Multiply",Color) = (1,1,1,1)
		_RimColor("RimColor", Color) = (.5,.5,.5,1)
			_RimPower("RimPower",Float) = 1

	}


	SubShader {
		Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase"}

		Pass {
			Name "BASE"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
						#pragma multi_compile_fwdbase

			#include "AutoLight.cginc"
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			samplerCUBE _ToonShade;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _Color2;
			float4 _Color3;
			float4 _Color4;
			float _RimPower;
			float4 _RimColor;
			float4 _Color5;
			float4 _Color6;
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
								LIGHTING_COORDS(4,5)

			};

			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos (v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.cubenormal = mul (UNITY_MATRIX_MV, float4(v.normal,0));
				o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				                TRANSFER_VERTEX_TO_FRAGMENT(o);

				o.wNormal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				half3 viewDirection =  normalize(i.wPos - _WorldSpaceCameraPos);

				fixed4 cube = texCUBE(_ToonShade, i.cubenormal);
				fixed4 col = lerp(_Color,_Color2,cube.r) * tex2D(_MainTex, i.texcoord);
				fixed4 col2 = lerp(_Color3,_Color4,cube.r) * tex2D(_MainTex, i.texcoord);

				fixed4 c = fixed4(2.0f * cube.rgb * col.rgb, col.a);
				fixed4 c2 = fixed4(2.0f * cube.rgb * col2.rgb, col2.a);

				c = lerp(c,c2,i.texcoord.y);
				half rim = pow(1.0 - saturate(dot(-viewDirection, i.wNormal)), _RimPower); //same concept, but the value is based on how far you are from looking at the normal
				c += _RimColor * rim;
								float atten = LIGHT_ATTENUATION(i);

				c = lerp(c,_Color5,(1-atten)*_Color5.a);
				c = c*_Color6;
				//if (cube.r > 0.9) {
				//	col = _Color2 * tex2D(_MainTex, i.texcoord);
				//	c = fixed4(2.0f  * col.rgb, col.a);
				//}
				return c;
			}
			ENDCG			
		}
	} 

	Fallback "VertexLit"
}

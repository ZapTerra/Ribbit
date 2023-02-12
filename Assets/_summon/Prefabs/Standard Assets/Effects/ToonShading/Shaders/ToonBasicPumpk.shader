// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Toon/BasicSparkle" {
	Properties {
		_Color ("Main Color", Color) = (.5,.5,.5,1)
		_SparkleTex("SparkleTex",2D) = "white" {}
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ToonShade ("ToonShader Cubemap(RGB)", CUBE) = "" { }
		_Color2("Main Color2", Color) = (.5,.5,.5,1)
		_RimColor("RimColor", Color) = (.5,.5,.5,1)
				_SparkleColor("SparkleColor", Color) = (.5,.5,.5,1)

			_RimPower("RimPower",Float) = 1
			_Scale("Scale",Float)=1
						_Intensity("Intensity",Float)=1


	}


	SubShader {
		Tags { "RenderType"="Opaque" }

		Pass {
			Name "BASE"
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _SparkleTex;
			samplerCUBE _ToonShade;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _Color2;
			float _RimPower;
			float4 _RimColor;
			float _Scale;
			float _Intensity;
			float4 _SparkleColor;

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


				fixed3 sparklemap = tex2D(_SparkleTex, i.texcoord*_Scale); //sample the noise texture
				sparklemap -= half3(0.5,0.5,0.5); //change the noise texture into a random direction
				sparklemap = normalize(sparklemap); 
				half sparkle = pow(saturate((dot(-viewDirection, normalize(sparklemap + i.wNormal)))),_Intensity); //get a value based on how close you are to looking at the normal offset by a random direction 
				c += _SparkleColor*sparkle; //change the color based on this value
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

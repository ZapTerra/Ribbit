// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/SparkleShader2"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
				_Color("Color", Color) = (1,1,1,1)

		_SparkleTex("Sparkle Texture", 2D) = "white" {}
		_Scale("Scale", Float) = 1
		_Intensity("Intensity", Float) = 1
		_Frequency("Freq", Float) = 1

		_RimColor("RimColor", Color) = (1,1,1,1)
		_RimPower("RimPower", Float) = 1
		_Enter("Enter",Float) = 1
		

	}
	SubShader
	{
		Tags { "RenderType"="Overlay" "DisableBatching" = "True"}
		LOD 100
        
		//ZTest Off
		ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 wPos : TEXCOORD1;
				float3 wNormal : TEXCOORD2;
				float bWpos : TEXCOORD3;
			};

			sampler2D _MainTex;
			sampler2D _SparkleTex;
			half4 _RimColor;
			float _RimPower;
			float4 _SparkleTex_ST;
			float4 _Color;
			float _Scale;
			float _Intensity;
			float _Frequency;
			float _Enter;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.bWpos = mul(unity_ObjectToWorld, float4(0.0,0.0,0.0,1.0)).x;
				//o.bWpos = 
				o.bWpos -=_WorldSpaceCameraPos.x;

				//v.vertex*=max(5+(4+o.bWpos)-_Enter,1);
				v.vertex*=max(5+(4+o.bWpos)-_Time.y*4+5,1);

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _SparkleTex);

				o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.wNormal = normalize(mul(float4(v.normal, 0.0), unity_WorldToObject).xyz);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				half3 viewDirection = normalize(i.wPos - _WorldSpaceCameraPos);
				fixed3 sparklemap = tex2D(_SparkleTex, i.uv*_Scale+half2(i.bWpos*100,0));

				sparklemap.x -= 0.5;
				sparklemap.y -= 0.5;
				sparklemap.z -= 0.5;
				sparklemap = normalize(sparklemap);
				half sparkle = saturate((dot(-viewDirection, normalize(sparklemap+i.wNormal)) - _Frequency) * 1 / (1 - _Frequency))*_Intensity;
			

				// sample the texture
				fixed4 col = _Color;
				col += half4(sparkle, sparkle, sparkle,0);
				float val = abs(i.bWpos*14+i.wPos.x-_Time.z*6)%60+i.wPos.y/1.3;


				
				float4 rim = pow(1.0 - saturate(dot(-viewDirection, i.wNormal)), _RimPower);
				col = lerp(col,_RimColor,rim);
								if (val>1 && val<1.8) {
					col+=half4(0.2,0.2,0.2,0);
				}
				col.a = saturate(_Time.y*4-5-(2+(4+i.bWpos)));
				col*=col.a;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}

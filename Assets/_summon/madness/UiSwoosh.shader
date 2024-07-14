// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/UiSwoosh"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Color2 ("Color2", Color) = (1,1,1,1)
		_ClickColor("ClickColor",Color) = (1,1,1,1)
		_MousePos ("MousePos", Vector) = (0,0,0,0)
		    _PaintMap("PaintMap", 2D) = "white" {} // texture to paint on
			_TransColor("TransColor",Color) = (1,1,1,1)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float2 texcoord1 : TEXCOORD1;

			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 screenPos : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _Color;
			float4 _Color2;
			float4 _ClickColor;
			float4 _TransColor;

			    sampler2D _PaintMap;
				float4 _MousePos;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				float val = (1-tex2Dlod(_PaintMap,half4(o.screenPos.xy/o.screenPos.w,0,0)));
				float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
				float4 dir = float4(normalize(float3(o.screenPos.x-_MousePos.x/1024,o.screenPos.y-_MousePos.y/768,0)).x,0,-normalize(float3(o.screenPos.x-_MousePos.x/1024,o.screenPos.y-_MousePos.y/768,0)).y,0);
				v.vertex=v.vertex+dir*val*0.6*_MousePos.z;
				o.vertex = UnityObjectToClipPos(v.vertex);
				if (v.texcoord.y>0.6) {
					o.vertex.y =min(o.vertex.y,-0.8);
				}
				o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.uv1 = TRANSFORM_TEX(v.texcoord1, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				float draw = 0;
				fixed4 col = tex2D(_MainTex, half2((i.uv1.x)/4+floor(i.uv0.x*4+0.06)/4,i.uv0.y*8)) * _Color;
				fixed4 col2 = _Color2;
				float val = floor(i.uv0.x*4+0.06);
				if (i.uv0.x > 0.25*val && i.uv0.x<0.25*(val+1) && _MousePos.x<(257*(val+1))*(_ScreenParams.x/1024) && _MousePos.x>257*val*(_ScreenParams.y/768)) {
					col2 = lerp(half4(1,1,1,1),_ClickColor,1-(_MousePos.z+2)/2);
				}
				col = lerp(col,col2,col.a);
				if (i.uv0.y<0.5) {
					col = _TransColor;
				}
				// apply fog
				//half4 paint = (tex2D(_PaintMap, i.screenPos.xy/i.screenPos.w)); // painted on texture
				//col *= paint; // add paint to main;
				return col;

			}
			ENDCG
		}
	}
}

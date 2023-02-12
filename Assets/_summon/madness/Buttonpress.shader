Shader "Unlit/Buttonpress"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Iterate ("Iterate", float) = 0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		ZWrite Off
		ZTest Off
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Iterate;
			
			v2f vert (appdata v)
			{
				v2f o;
				float dist = distance(half2(0,0),v.vertex.xy);
				float val2 = max(0,_Iterate-dist);
				float val = -(pow(2.718,(-pow(val2*0.2-0.6,2))))+1.69768;
				//val = min(val,1.4);
				if (val2>10) {
					val-=(pow(val2-10,1)/10);
					val = max(val,1);
				}

				float val3 = v.vertex.x;
				v.vertex*=val;
				val = val/2;
				v.vertex.x = lerp(val3,v.vertex.x,0.5);
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex,i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}

Shader "Img/PixelateImageEffect"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Columns("Columns",float) = 64
		_Rows("Rows",float) = 64
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float _Rows;
			float _Columns;
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{

				float2 uv = i.uv;
				uv.x*=_Columns;
				uv.y*=_Rows;
				uv.x = floor(uv.x);
				uv.y = floor(uv.y);
				uv.x = uv.x / _Columns;
				uv.y = uv.y / _Rows;
				fixed4 col = tex2D(_MainTex, uv);



				if (col.r>0.5) {
					col = half4(1,1,1,1);
				} else if (col.r>0.25){
					//blue
					col = half4(.4,.5,.8,1);
				} else {
					//bg
					col = half4(.5,.4,.9,1);
				}
				// just invert the colors
				return col;
			}
			ENDCG
		}
	}
}

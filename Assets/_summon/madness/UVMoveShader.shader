Shader "Unlit/UVMoveShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Translate ("Translate",float)=0
		_Rotate ("Rotate",float)=0

	}
	SubShader
	{
		Tags { "RenderType"="Opaque" "DisableBatching" = "True"}
		LOD 100
		Cull Off
		ZWrite Off

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
			float _Translate;
			float _Rotate;
			
inline float3x3 xRotation3dRadians(float rad) {
    float s = sin(rad);
    float c = cos(rad);
    return float3x3(
        1, 0, 0,
        0, c, s,
        0, -s, c);
}
 
inline float3x3 yRotation3dRadians(float rad) {
    float s = sin(rad);
    float c = cos(rad);
    return float3x3(
        c, 0, -s,
        0, 1, 0,
        s, 0, c);
}
 
inline float3x3 zRotation3dRadians(float rad) {
    float s = sin(rad);
    float c = cos(rad);
    return float3x3(
        c, s, 0,
        -s, c, 0,
        0, 0, 1);
}


			v2f vert (appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				half testv = v.vertex.y;
				v.vertex = half4(mul(xRotation3dRadians(radians(_Time.y*15)), v.vertex),v.vertex.w);
				//v.vertex.z = max(0,v.vertex.z);
				if (o.uv.x<0.1) {
					v.vertex.y=testv;
				}

				o.vertex = UnityObjectToClipPos(v.vertex);

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}


			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = half4(0.4,0.4,0.4,1);
				if (_Translate>i.uv.x) {
					col = half4(1,1,1,1);
				}
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}

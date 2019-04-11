Shader "Unlit/GPUBindingDemo"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
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
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float3 local0 : POSITION;
				float3 local1 : NORMAL;
				float2 uv : TEXCOORD0;
				float2 bone0 : TEXCOORD1;
				float2 bone1 : TEXCOORD2;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			uniform matrix _Bones[100];
			
			v2f vert (appdata v)
			{
				v2f o;
				
				float4 pos = mul(_Bones[floor(v.bone0.x)], float4(v.local0, 1)) * v.bone0.y +
					mul(_Bones[floor(v.bone1.x)], float4(v.local1, 1)) * v.bone1.y;

				o.vertex = UnityObjectToClipPos(pos);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}

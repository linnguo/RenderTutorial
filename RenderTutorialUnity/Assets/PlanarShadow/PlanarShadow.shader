// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Unlit/PlanarShadow"
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
			ZWrite on
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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
		
		Pass
		{
			ZWrite Off

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert(appdata v)
			{
				v2f o;

				// 模型空间到世界空间
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				
				// HardCode方向光的方向
				float3 lightDir = normalize(float3(1, -1, 1));

				float3 shadowPos = worldPos - (worldPos.y / dot(lightDir, float3(0, 1, 0))) * lightDir;

				// 世界空间到相机空间
				float4 viewPos = mul(UNITY_MATRIX_V, float4(shadowPos, 1));

				// 相机空间到裁剪空间
				float4 clipPos = mul(UNITY_MATRIX_P, viewPos);

				o.vertex = clipPos;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = fixed4(0.5, 0.5, 0.5, 0.5);
				return col;
			}
			ENDCG
		}
	}
}

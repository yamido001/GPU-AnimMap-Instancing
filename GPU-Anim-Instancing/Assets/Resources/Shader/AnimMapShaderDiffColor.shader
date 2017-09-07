// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'

Shader "Custom/AnimMapShaderDiffColor"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_AnimMap ("AnimMap", 2D) ="white" {}
		_BlendColor ("_BlendColor", Color) = (1, 1, 1, 1)
		_AnimLen("Anim Length", Float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100
			Cull off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			//开启gpu instancing
			#pragma multi_compile_instancing


			#include "UnityCG.cginc"

			struct appdata
			{
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _AnimMap;
			float4 _AnimMap_TexelSize;//x == 1/width

			float _AnimLen;

			UNITY_INSTANCING_CBUFFER_START (MyProperties)
            UNITY_DEFINE_INSTANCED_PROP (float4, _BlendColor)
            UNITY_INSTANCING_CBUFFER_END
			
			v2f vert (appdata v, uint vid : SV_VertexID)
			{
				UNITY_SETUP_INSTANCE_ID(v);

				float f = _Time.y / _AnimLen;

				fmod(f, 1.0);

				float animMap_x = (vid + 0.5) * _AnimMap_TexelSize.x;
				float animMap_y = f;

				float4 pos = tex2Dlod(_AnimMap, float4(animMap_x, animMap_y, 0, 0));

				v2f o;
				UNITY_TRANSFER_INSTANCE_ID (v, o);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.vertex = UnityObjectToClipPos(pos);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID (i);
				fixed4 blendCol = UNITY_ACCESS_INSTANCED_PROP (_BlendColor);
				fixed4 col = tex2D(_MainTex, i.uv);
				return col * (1 - blendCol.a) + blendCol * blendCol.a;

//				fixed4 col = tex2D(_MainTex, i.uv);
//				return col;
			}
			ENDCG
		}
	}
}

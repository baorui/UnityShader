// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Shadow/ReceiveShadow"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			float4 _Color;

			struct v2f {
				float4 pos:SV_POSITION;
				float4 proj : TEXCOORD1;
				float2 depth : TEXCOORD2;
			};


			float4x4 _ProjectionMatrix;
			sampler2D _DepthTexture;

			v2f vert(appdata_full v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);

				//动态阴影
				o.depth = o.pos.zw;
				_ProjectionMatrix = mul(_ProjectionMatrix, unity_ObjectToWorld);
				o.proj = mul(_ProjectionMatrix, v.vertex);

				return o;
			}

			fixed4 frag(v2f v) : COLOR
			{
				float depth = v.depth.x / v.depth.y;
				fixed4 dcol = tex2Dproj(_DepthTexture, v.proj);
				float d = DecodeFloatRGBA(dcol);
				float shadowScale = 1;
				if (depth < d)
				{
					shadowScale = 0.55;
				}
				return _Color * shadowScale;
			}
			ENDCG
		}
	}
}
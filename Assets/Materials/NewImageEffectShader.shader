// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/NewImageEffectShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_LowColor ("LowThresholdColor", Color) = (0.1,0.1,0.1,1)
		_HiColor ("HiThresholdColor", Color) = (0.9,0.9,0.9,1)
	}
	SubShader
	{
		Tags{ "RenderType" = "transparent" }
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

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
			
			sampler2D _MainTex;
			float4 _LowColor;
			float4 _HiColor;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				// Assume a base transparency
				col.a = 1;
				if (col.r < _LowColor.r && col.g < _LowColor.g && col.b < _LowColor.b)
					col.a = 0;
				else if (col.r > _HiColor.r && col.g > _HiColor.g && col.b > _HiColor.b)
					col.a = 0;
				else
				{
					col.r = (col.r - _LowColor.r) / (_HiColor.r - _LowColor.r);
					col.g = (col.g - _LowColor.g) / (_HiColor.g - _LowColor.g);
					col.b = (col.b - _LowColor.b) / (_HiColor.b - _LowColor.b);
				}

				return col;
			}
			ENDCG
		}
	}
}

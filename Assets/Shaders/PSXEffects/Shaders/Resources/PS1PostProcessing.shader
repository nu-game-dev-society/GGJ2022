Shader "Hidden/PS1PostProcessing"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DitherTex("", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth writing
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ DITHER_SKY
			#pragma multi_compile _ DITHER_FAST DITHER_SLOW DITHER_TEX
			#pragma multi_compile _ SCANLINES_ON
			
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

	 
			#define DITHER_COLORS 256

			sampler2D _MainTex;
			sampler2D _DitherTex;
			sampler2D _CameraDepthTexture;
			float4 _DitherTex_TexelSize;
			float _ColorDepth;
			float _Scanlines;
			float _ScanlineIntensity;
			float _Dithering;
			float _DitherThreshold;
			float _DitherIntensity;
			float _DitherSky;
			float _SubtractFade;
			float _FavorRed;
			float _SLDirection;
			float _ResX;
			float _ResY;
			float _DitherType;

			float GetDitherOld(float2 pos, float factor) {
				float DITHER_THRESHOLDS[16] =
				{
					1.0 / 17.0,  9.0 / 17.0,  3.0 / 17.0, 11.0 / 17.0,
					13.0 / 17.0,  5.0 / 17.0, 15.0 / 17.0,  7.0 / 17.0,
					4.0 / 17.0, 12.0 / 17.0,  2.0 / 17.0, 10.0 / 17.0,
					16.0 / 17.0,  8.0 / 17.0, 14.0 / 17.0,  6.0 / 17.0
				};

				// Dynamic indexing isn't allowed in WebGL for some weird reason so here's this strange workaround
				uint i = (uint(pos.x) & 3) * 4 + uint(pos.y) & 3;
				#ifdef SHADER_API_GLES3
				for (int x = 0; x < 16; x++)
					if (x == i)
						return factor - DITHER_THRESHOLDS[x];
				return 0;
				#else
				return factor - DITHER_THRESHOLDS[i];
				#endif
			}

			float3 GetDither(float2 pos, float3 c, float intensity) {
				int DITHER_THRESHOLDS[16] =
				{
					-4, 0, -3, 1,
					2, -2, 3, -1,
					-3, 1, -4, 0,
					3, -1, 2, -2
				};
				int lut[DITHER_COLORS];
				

				c /= DITHER_COLORS;
				return c;
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float aspect = _ResY / _ResX;
				fixed4 col = tex2D(_MainTex, i.uv);
				#if !UNITY_COLORSPACE_GAMMA
				col.rgb = LinearToGammaSpace(col.rgb);
				#endif
				
				#ifdef UNITY_COLORSPACE_GAMMA
					half luma = LinearRgbToLuminance(GammaToLinearSpace(saturate(col.rgb)));
				#else
					half luma = LinearRgbToLuminance(col.rgb);
				#endif

				// Manipulate colors/saturate
				col.rgb -= (3 - col.rgb) * _SubtractFade;
				#if UNITY_COLORSPACE_GAMMA
					col.rgb -= _FavorRed * ((1 - col.rgb) * 0.25);
					col.r += _FavorRed * ((0.5 - col.rgb) * 0.1);
				#else
					col.rgb -= GammaToLinearSpace(_FavorRed * ((1 - col.rgb) * 0.25));
					col.r += GammaToLinearSpace(_FavorRed * ((0.5 - col.rgb) * 0.1));
				#endif

				// Calculate depth texture
				float depth = 1;
				#if !defined(DITHER_SKY)
					depth = 1 - floor(Linear01Depth(tex2D(_CameraDepthTexture, i.uv).r));
				#endif

				// Posterize
				col.rgb = saturate(floor(col.rgb * _ColorDepth) / _ColorDepth);

				#if !UNITY_COLORSPACE_GAMMA
					col.rgb = GammaToLinearSpace(col.rgb);
				#endif

				return col;
			}
			ENDCG
		}
	}
}

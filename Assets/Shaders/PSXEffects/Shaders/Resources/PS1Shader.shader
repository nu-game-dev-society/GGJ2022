Shader "PSXEffects/PS1Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DitherStrength ("Dither Strength", Range(0,1)) = 1
        _Color ("Color Tint", Color) = (1,1,1,1)
        _DrawDistance ("Draw Distance", Float) = 50
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _DitherStrength;
            float _DrawDistance;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 color : COLOR;
                float depthFade : TEXCOORD1;
            };

            // Simple single directional light
            float3 _LightDir = normalize(float3(0.3,0.7,0.5));
            float3 _LightColor = float3(1,1,1);

            v2f vert(appdata v)
            {
                v2f o;

                // Snap vertices to emulate PS1 fixed-point behavior
                v.vertex.xyz = round(v.vertex.xyz * 16.0) / 16.0;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                // Fake lighting: Lambert + vertex normal
                float NdotL = max(0, dot(normalize(v.normal), _LightDir));
                o.color = _LightColor * NdotL;

                // Simple draw distance fade
                float dist = length(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex).xyz);
                o.depthFade = saturate(1.0 - dist/_DrawDistance);

                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float4 texcol = tex2D(_MainTex, i.uv) * float4(i.color,1) * _Color;

                // Apply dithering
                int2 pix = int2(fmod(floor(i.pos.xy), 4));
                float ditherPattern = (pix.x + pix.y * 4) / 16.0;
                texcol.rgb = floor(texcol.rgb * 16.0 + ditherPattern * _DitherStrength) / 16.0;

                // Apply draw distance fade
                texcol.rgb *= i.depthFade;

                return texcol;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

Shader "Custom/JaggedLine"
{
    Properties
    {
        _Color ("Line Color", Color) = (1,1,1,1)
        _Speed ("Animation Speed", Range(0, 10)) = 1
        _WaveHeight ("Wave Height", Range(0, 1)) = 0.15
        _WaveFrequency ("Wave Frequency", Range(0, 20)) = 6
    }

    SubShader
    {
        Tags { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Speed;
            float _WaveHeight;
            float _WaveFrequency;
            fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float time = _Time.y * _Speed;
                float wave = sin(i.uv.x * _WaveFrequency + time) * _WaveHeight;
                
                float dist = abs(i.uv.y - 0.5 - wave);
                float horizontalFade = 1 - abs((i.uv.x - 0.5) * 2);
                float alpha = smoothstep(0.05, 0, dist) * horizontalFade;
                
                return fixed4(_Color.rgb, alpha * _Color.a);
            }
            ENDCG
        }
    }
}
Shader "UI/GlitchEffect"
{
    Properties
    {
        _GlitchIntensity ("Glitch Intensity", Range(0, 1)) = 0.1
        _VerticalLineFreq ("Vertical Line Frequency", Range(1, 100)) = 50
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _GlitchIntensity;
            float _VerticalLineFreq;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            float random(float2 st)
            {
                return frac(sin(dot(st.xy, float2(12.9898,78.233))) * 43758.5453123);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float time = _Time.y;
                
                float verticalNoise = random(float2(floor(i.uv.x * _VerticalLineFreq), floor(time * 10)));
                float lineIntensity = step(0.97 - _GlitchIntensity * 0.3, verticalNoise);
                float horizontalGlitch = step(0.99 - _GlitchIntensity * 0.3, random(float2(floor(time * 20), floor(i.uv.y * 20))));
                
                float alpha = (lineIntensity + horizontalGlitch) * _GlitchIntensity;
                float gray = random(float2(i.uv.x, time)) * 0.5 + 0.5;
                
                return fixed4(gray, gray, gray, alpha);
            }
            ENDCG
        }
    }
}
Shader "Custom/SepiaEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SepiaIntensity ("Sepia Intensity", Range(0, 1)) = 1
        _SepiaColor ("Sepia Tint", Vector) = (1.351, 1.203, 0.937, 1)
        _GrainIntensity ("Film Grain", Range(0, 1)) = 0.1
        _FlickerSpeed ("Flicker Speed", Range(0, 10)) = 2
        _FlickerIntensity ("Flicker Intensity", Range(0, 1)) = 0.05
    }
    
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        Cull Off ZWrite Off ZTest Always
        
        Pass
        {
            Name "Sepia"
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            
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
            float _SepiaIntensity;
            float4 _SepiaColor;
            float _GrainIntensity;
            float _FlickerSpeed;
            float _FlickerIntensity;
            
            float random(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453123);
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uvRandom = i.uv;
                uvRandom.y *= _Time.y;
                float grain = random(uvRandom) * _GrainIntensity;
                
                float flicker = sin(_Time.y * _FlickerSpeed) * _FlickerIntensity;
                
                float2 distortedUV = i.uv;
                distortedUV.y += sin(_Time.y * 2 + i.uv.x * 5) * 0.001;
                fixed4 col = tex2D(_MainTex, distortedUV);
                
                float3 sepiaColor = col.rgb * _SepiaColor.rgb;
                float3 finalColor = lerp(col.rgb, sepiaColor, _SepiaIntensity);
                
                finalColor += grain;
                finalColor *= (1 + flicker);
                
                return fixed4(finalColor, col.a);
            }
            ENDCG
        }
    }
}
Shader "UI/MindMapNodeNumber"
{
    Properties
    {
        _InversionAmount ("Inversion Amount", Range(0,1)) = 0
        _EdgeSmoothness ("Edge Smoothness", Range(0.001, 0.015)) = 0.02
        _WaveSpeed ("Wave Speed", Range(0, 5)) = 1
        _WaveAmount ("Wave Amount", Range(0, 0.1)) = 0.02
        _WaveFrequency("Wave Frequency", Range(1, 12)) = 6
        _PopupProgress ("Popup Progress", Range(0,1)) = 0
        _PopupBounciness ("Popup Bounciness", Range(0, 1)) = 0.2
        _ShapeMorph ("Shape Morph", Range(0, 1)) = 0
        
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
            };

            float _InversionAmount;
            float _EdgeSmoothness;
            float _WaveSpeed;
            float _WaveAmount;
            float _WaveFrequency;
            float _PopupProgress;
            float _PopupBounciness;
            float _ShapeMorph;
            float4 _ClipRect;

            float elasticOut(float x) {
                return sin(-13.0 * (x + 1.0) * UNITY_PI/2) * pow(2.0, -10.0 * x) + 1.0;
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;
                OUT.worldPosition = v.vertex;
                
                float2 centeredUV = v.texcoord - 0.5;
                float scale = lerp(0.2, 1.0, elasticOut(_PopupProgress));
                centeredUV *= scale;
                
                float rotation = (1.0 - _PopupProgress) * UNITY_PI;
                float2x2 rotMatrix = float2x2(cos(rotation), -sin(rotation), sin(rotation), cos(rotation));
                centeredUV = mul(rotMatrix, centeredUV);
                
                OUT.texcoord = centeredUV + 0.5;
                
                float4 popupPos = OUT.worldPosition;
                popupPos.xy = (popupPos.xy - float2(0.5, 0.5)) * scale + float2(0.5, 0.5);
                OUT.vertex = UnityObjectToClipPos(popupPos);
                
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float time = _Time.y * _WaveSpeed;
                float angle = atan2(IN.texcoord.y - center.y, IN.texcoord.x - center.x);
                if (angle < 0) angle += 2 * UNITY_PI;
                float wave = sin(angle * _WaveFrequency + time) * _WaveAmount * _PopupProgress;
                
                float baseRadius = 0.38;
                float rhomboidFactor = abs(cos(4 * angle)) * 0.08;
                float morphedRadius = lerp(baseRadius, baseRadius - rhomboidFactor, _ShapeMorph);
                float dist = distance(IN.texcoord, center) + wave;
                float radius = morphedRadius;
                float outlineWidth = 0.02;
                
                float outlineVisibility = smoothstep(0.07, 0.15, _PopupProgress);
                outlineWidth *= outlineVisibility;
                
                float outlineAlpha = smoothstep(radius + outlineWidth + _EdgeSmoothness, radius + outlineWidth - _EdgeSmoothness, dist) - 
                                   smoothstep(radius + _EdgeSmoothness, radius - _EdgeSmoothness, dist);
                float circleAlpha = smoothstep(radius + _EdgeSmoothness, radius - _EdgeSmoothness, dist);

                fixed4 color = fixed4(0, 0, 0, circleAlpha * _PopupProgress);
                color = fixed4(lerp(color.rgb, float3(1,1,1), outlineAlpha), max(color.a, outlineAlpha * _PopupProgress));
                color = fixed4(lerp(color.rgb, 1 - color.rgb, smoothstep(0.0, 1.0, _InversionAmount)), color.a);
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                
                return color;
            }
            ENDCG
        }
    }
}
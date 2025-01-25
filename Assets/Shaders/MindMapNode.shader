Shader "UI/CircleBackground"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _InversionAmount ("Inversion Amount", Range(0,1)) = 0
        _EdgeSmoothness ("Edge Smoothness", Range(0.001, 0.015)) = 0.02
        
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

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
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ClipRect;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
                
                float2 centeredUV = v.texcoord - 0.5;
                centeredUV *= 0.85;
                OUT.texcoord = centeredUV + 0.5;
                
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float dist = distance(IN.texcoord, center);
                
                fixed4 texColor = tex2D(_MainTex, IN.texcoord);
                fixed4 color;
                
                float radius = 0.38;
                float outlineWidth = 0.02;
                float outlineAlpha = smoothstep(radius + outlineWidth + _EdgeSmoothness, radius + outlineWidth - _EdgeSmoothness, dist) - 
                                     smoothstep(radius + _EdgeSmoothness, radius - _EdgeSmoothness, dist);
                float circleAlpha = smoothstep(radius + _EdgeSmoothness, radius - _EdgeSmoothness, dist);

                color = fixed4(0, 0, 0, circleAlpha);
                color.rgb = lerp(color.rgb, texColor.rgb, texColor.a * (1.0 - max(outlineAlpha, 1.0 - circleAlpha)));
                color = fixed4(lerp(color.rgb, float3(1,1,1), outlineAlpha),  max(color.a, outlineAlpha));
                color = fixed4(lerp(color, 1 - color.rgb, smoothstep(0.0, 1.0, _InversionAmount)), color.a);
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                return color;
            }
            ENDCG
        }
    }
}
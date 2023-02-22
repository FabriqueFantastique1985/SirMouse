// https://gamedev.stackexchange.com/questions/152241/how-can-i-make-delay-in-shader

Shader "Custom/Interactable_Shine"
{
    Properties 
    {
        _MainTex("MainTex",2D) = "white"{}
        _Color ("Color", Color) = (1,1,1,1)
        _LineColor ("LineColor", Color) = (1,1,0,1)
        _Speed ("Speed",Float) = 1
        _Thickness ("Thickness", Float) = 4.286548
        _LineSeparation ("LineSeparation", Range(0.1, 10)) = 1
        _LineRotation ("LineRotation", Range(-180, 180)) = 0
        _Numbers ("Numbers", Float ) = 1
        [MaterialToggle] _Enable ("Enable", Float ) = 1
        _Mask ("Mask", 2D) = "white" {}
    }
    SubShader 
    {
        Tags 
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        ZWrite Off Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Sprite"

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"

            struct VertexInput
            {
                float4 vertex   : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct VertexOutput
            {
                float4 vertex : SV_POSITION;
                half2 uv  : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            float _Speed;
            float _Numbers;


            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX (v.uv, _MainTex);
                return o;
            }

            float4 frag (VertexOutput i) : COLOR
            {
                float2 uv = i.uv.xy;
                float4 tex = tex2D(_MainTex, uv);

                return tex*_Color;
            }
        ENDCG
        }

        Tags 
        {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass 
        {
            Name "FlashEffect"
            Blend One One
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles n3ds wiiu 
            #pragma target 3.0
            uniform float4 _TimeEditor;
            uniform float _LineAlpha;
            uniform float _Speed;
            uniform float _Thickness;
            uniform float _LineSeparation;
            uniform float _LineRotation;
            uniform float4 _LineColor;
            uniform float _Numbers;
            uniform fixed _Enable;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            sampler2D _MainTex;

            struct VertexInput 
            {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };

            struct VertexOutput 
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
            };

            VertexOutput vert (VertexInput v) 
            {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos(v.vertex );
                return o;
            }

            float4 frag(VertexOutput i) : COLOR 
            {
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float node_9529_ang = (_LineRotation*0.01745333+0.0);
                float node_9529_spd = 1.0;
                float node_9529_cos = cos(node_9529_spd*node_9529_ang);
                float node_9529_sin = sin(node_9529_spd*node_9529_ang);
                float2 node_9529_piv = float2(0.5,0.5);
                float2 node_9529 = (mul(float2((i.uv0.r/_LineSeparation),(i.uv0.g/_LineSeparation))-node_9529_piv,float2x2( node_9529_cos, -node_9529_sin, node_9529_sin, node_9529_cos))+node_9529_piv);
                float4 node_5664 = _Time + _TimeEditor;
                float node_6322 = frac((_Numbers*(1.0 - (node_9529+(node_5664.g*_Speed))).g));
                float node_4443 = (pow(smoothstep( 0.5, 0.0, ((1.0 - node_6322)*node_6322) ),(35.0-_Thickness))*_LineColor.a);
                float3 emissive = lerp( ((node_4443*_LineColor.rgb)), 0.0, 1-_Enable )*_Mask_var.a;
                float3 finalColor = emissive;
                return fixed4(finalColor,1)*tex2D(_MainTex,i.uv0).a*_Mask_var.x;
            }
            ENDCG
        }
    }
Fallback "Sprites/Default"
}
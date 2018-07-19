//////////////////////////////////////////////////////////////
/// Shadero Sprite: Sprite Shader Editor - by VETASOFT 2018 //
/// Shader generate with Shadero 1.8.0                      //
/// http://u3d.as/V7t #AssetStore                           //
/// http://www.shadero.com #Docs                            //
//////////////////////////////////////////////////////////////

Shader "Shadero Previews/PreviewXATXQ4"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
_SourceNewTex_1("_SourceNewTex_1(RGB)", 2D) = "white" { }
ZoomUV_Zoom_1("ZoomUV_Zoom_1", Range(0.2, 4)) = 0.8623161
ZoomUV_PosX_1("ZoomUV_PosX_1", Range(-3, 3)) = 0.5
ZoomUV_PosY_1("ZoomUV_PosY_1", Range(-3, 3)) =0.5
_NewTex_1("NewTex_1(RGB)", 2D) = "white" { }
_SpriteFade("SpriteFade", Range(0, 1)) = 1.0

// required for UI.Mask
[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
[HideInInspector]_Stencil("Stencil ID", Float) = 0
[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
[HideInInspector]_ColorMask("Color Mask", Float) = 15

}

SubShader
{

Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True"}
ZWrite Off Blend SrcAlpha OneMinusSrcAlpha Cull Off

// required for UI.Mask
Stencil
{
Ref [_Stencil]
Comp [_StencilComp]
Pass [_StencilOp]
ReadMask [_StencilReadMask]
WriteMask [_StencilWriteMask]
}

Pass
{

CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma fragmentoption ARB_precision_hint_fastest
#include "UnityCG.cginc"

struct appdata_t{
float4 vertex   : POSITION;
float4 color    : COLOR;
float2 texcoord : TEXCOORD0;
};

struct v2f
{
float2 texcoord  : TEXCOORD0;
float4 vertex   : SV_POSITION;
float4 color    : COLOR;
};

sampler2D _MainTex;
float _SpriteFade;
sampler2D _SourceNewTex_1;
float ZoomUV_Zoom_1;
float ZoomUV_PosX_1;
float ZoomUV_PosY_1;
sampler2D _NewTex_1;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}


float4 TintRGBA(float4 txt, float4 color)
{
float3 tint = dot(txt.rgb, float3(.222, .707, .071));
tint.rgb *= color.rgb;
txt.rgb = lerp(txt.rgb,tint.rgb,color.a);
return txt;
}
float2 ZoomUV(float2 uv, float zoom, float posx, float posy)
{
float2 center = float2(posx, posy);
uv -= center;
uv = uv * zoom;
uv += center;
return uv;
}
float4 OperationBlend(float4 origin, float4 overlay, float blend)
{
float4 o = origin; 
o.a = overlay.a + origin.a * (1 - overlay.a);
o.rgb = (overlay.rgb * overlay.a + origin.rgb * origin.a * (1 - overlay.a)) / (o.a+0.0000001);
o.a = saturate(o.a);
o = lerp(origin, o, blend);
return o;
}
float3 Generate_Voronoi_hash3(float2 p, float seed)
{
float3 q = float3(dot(p, float2(127.1, 311.7)),
dot(p, float2(269.5, 183.3)),
dot(p, float2(419.2, 371.9)));
return frac(sin(q) * 43758.5453 * seed);
}
float4 Generate_Voronoi(float2 uv, float size,float seed, float black)
{
float2 p = floor(uv*size);
float2 f = frac(uv*size);
float k = 1.0 + 63.0*pow(1.0, 4.0);
float va = 0.0;
float wt = 0.0;
for (int j = -2; j <= 2; j++)
{
for (int i = -2; i <= 2; i++)
{
float2 g = float2(float(i), float(j));
float3 o = Generate_Voronoi_hash3(p + g, seed)*float3(1.0, 1.0, 1.0);
float2 r = g - f + o.xy;
float d = dot(r, r);
float ww = pow(1.0 - smoothstep(0.0, 1.414, sqrt(d)), k);
va += o.z*ww;
wt += ww;
}
}
float4 result = saturate(va / wt);
result.a = saturate(result.a + black);
return result;
}

float4 TurnAlphaToBlack(float4 txt,float fade)
{
float3 gs = lerp(txt.rgb,float3(0,0,0), 1-txt.a);
return lerp(txt,float4(gs, 1), fade);
}

float4 frag (v2f i) : COLOR
{
float2 ZoomUV_1 = ZoomUV(i.texcoord,ZoomUV_Zoom_1,ZoomUV_PosX_1,ZoomUV_PosY_1);
float4 SourceRGBA_2 = tex2D(_SourceNewTex_1, ZoomUV_1);
float4 NewTex_1 = tex2D(_NewTex_1, i.texcoord);
NewTex_1.a = SourceRGBA_2.a;
float4 FinalResult = NewTex_1;
FinalResult.rgb *= i.color.rgb;
FinalResult.a = FinalResult.a * _SpriteFade * i.color.a;
FinalResult = saturate(FinalResult);
return FinalResult;
}

ENDCG
}
}
Fallback "Sprites/Default"
}

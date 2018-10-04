//////////////////////////////////////////////////////////////
/// Shadero Sprite: Sprite Shader Editor - by VETASOFT 2018 //
/// Shader generate with Shadero 1.8.0                      //
/// http://u3d.as/V7t #AssetStore                           //
/// http://www.shadero.com #Docs                            //
//////////////////////////////////////////////////////////////

Shader "Shadero Customs/CorruptionEffect"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
TwistUV_TwistUV_Bend_1("TwistUV_TwistUV_Bend_1", Range(-1, 1)) = 0.3657227
TwistUV_TwistUV_PosX_1("TwistUV_TwistUV_PosX_1", Range(-1, 2)) = 0.5
TwistUV_TwistUV_PosY_1("TwistUV_TwistUV_PosY_1", Range(-1, 2)) = 0.5
TwistUV_TwistUV_Radius_1("TwistUV_TwistUV_Radius_1", Range(0, 1)) = 0.5
KaleidoscopeUV_PosX_1("KaleidoscopeUV_PosX_1",  Range(-2, 2)) = 0.1000004
KaleidoscopeUV_PosY_1("KaleidoscopeUV_PosY_1",  Range(-2, 2)) = -0.4057116
KaleidoscopeUV_Number_1("KaleidoscopeUV_Number_1", Range(0, 6)) = 2.751432
_CircleFade_PosX_1("_CircleFade_PosX_1", Range(-1, 2)) = 0.7441371
_CircleFade_PosY_1("_CircleFade_PosY_1", Range(-1, 2)) = 0.5
_CircleFade_Size_1("_CircleFade_Size_1", Range(-1, 1)) = 0.9684693
_CircleFade_Dist_1("_CircleFade_Dist_1", Range(0, 1)) = 0.2
TwistUV_TwistUV_Bend_2("TwistUV_TwistUV_Bend_2", Range(-1, 1)) = 0.2056756
TwistUV_TwistUV_PosX_2("TwistUV_TwistUV_PosX_2", Range(-1, 2)) = 0.5
TwistUV_TwistUV_PosY_2("TwistUV_TwistUV_PosY_2", Range(-1, 2)) = 0.5
TwistUV_TwistUV_Radius_2("TwistUV_TwistUV_Radius_2", Range(0, 1)) = 0.5
KaleidoscopeUV_PosX_2("KaleidoscopeUV_PosX_2",  Range(-2, 2)) = 0.03992745
KaleidoscopeUV_PosY_2("KaleidoscopeUV_PosY_2",  Range(-2, 2)) = -0.1
KaleidoscopeUV_Number_2("KaleidoscopeUV_Number_2", Range(0, 6)) = 0.8628566
_GenerateLightning_PosX_2("_GenerateLightning_PosX_2", Range(-2, 2)) = 0.5
_GenerateLightning_PosY_2("_GenerateLightning_PosY_2", Range(-2, 2)) = 0.5
_GenerateLightning_Size_2("_GenerateLightning_Size_2", Range( 1, 8)) = 4
_GenerateLightning_Number_2("_GenerateLightning_Number_2", Range(2, 16)) = 4
_GenerateLightning_Speed_2("_GenerateLightning_Speed_2", Range( 0, 8)) = 0.2685756
_CircleFade_PosX_2("_CircleFade_PosX_2", Range(-1, 2)) = 0.5
_CircleFade_PosY_2("_CircleFade_PosY_2", Range(-1, 2)) = 0.5
_CircleFade_Size_2("_CircleFade_Size_2", Range(-1, 1)) = 1
_CircleFade_Dist_2("_CircleFade_Dist_2", Range(0, 1)) = 0.2
_OperationBlend_Fade_1("_OperationBlend_Fade_1", Range(0, 1)) = 1
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

Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }
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
float TwistUV_TwistUV_Bend_1;
float TwistUV_TwistUV_PosX_1;
float TwistUV_TwistUV_PosY_1;
float TwistUV_TwistUV_Radius_1;
float KaleidoscopeUV_PosX_1;
float KaleidoscopeUV_PosY_1;
float KaleidoscopeUV_Number_1;
float _CircleFade_PosX_1;
float _CircleFade_PosY_1;
float _CircleFade_Size_1;
float _CircleFade_Dist_1;
float TwistUV_TwistUV_Bend_2;
float TwistUV_TwistUV_PosX_2;
float TwistUV_TwistUV_PosY_2;
float TwistUV_TwistUV_Radius_2;
float KaleidoscopeUV_PosX_2;
float KaleidoscopeUV_PosY_2;
float KaleidoscopeUV_Number_2;
float _GenerateLightning_PosX_2;
float _GenerateLightning_PosY_2;
float _GenerateLightning_Size_2;
float _GenerateLightning_Number_2;
float _GenerateLightning_Speed_2;
float _CircleFade_PosX_2;
float _CircleFade_PosY_2;
float _CircleFade_Size_2;
float _CircleFade_Dist_2;
float _OperationBlend_Fade_1;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}


float2 TwistUV(float2 uv, float value, float posx, float posy, float radius)
{
float2 center = float2(posx, posy);
float2 tc = uv - center;
float dist = length(tc);
if (dist < radius)
{
float percent = (radius - dist) / radius;
float theta = percent * percent * 16.0 * sin(value);
float s = sin(theta);
float c = cos(theta);
tc = float2(dot(tc, float2(c, -s)), dot(tc, float2(s, c)));
}
tc += center;
return tc;
}
float4 Circle_Fade(float4 txt, float2 uv, float posX, float posY, float Size, float Smooth)
{
float2 center = float2(posX, posY);
float dist = 1.0 - smoothstep(Size, Size + Smooth, length(center - uv));
txt.a *= dist;
return txt;
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
float Lightning_Hash(float2 p)
{
float3 p2 = float3(p.xy, 1.0);
return frac(sin(dot(p2, float3(37.1, 61.7, 12.4)))*3758.5453123);
}

float Lightning_noise(in float2 p)
{
float2 i = floor(p);
float2 f = frac(p);
f *= f * (1.5 - .5*f);
return lerp(lerp(Lightning_Hash(i + float2(0., 0.)), Lightning_Hash(i + float2(1., 0.)), f.x),
lerp(Lightning_Hash(i + float2(0., 1.)), Lightning_Hash(i + float2(1., 1.)), f.x),
f.y);
}

float Lightning_fbm(float2 p)
{
float v = 0.0;
v += Lightning_noise(p*1.0)*.5;
v += Lightning_noise(p*2.)*.25;
v += Lightning_noise(p*4.)*.125;
v += Lightning_noise(p*8.)*.0625;
return v;
}

float4 Generate_Lightning(float2 uv, float2 uvx, float posx, float posy, float size, float number, float speed, float black)
{
uv -= float2(posx, posy);
uv *= size;
uv -= float2(posx, posy);
float rot = (uv.x*uvx.x + uv.y*uvx.y);
float time = _Time * 20 * speed;
float4 r = float4(0, 0, 0, 0);
for (int i = 1; i < number; ++i)
{
float t = abs(.750 / ((rot + Lightning_fbm(uv + (time*5.75) / float(i)))*65.));
r += t *0.5;
}
r = saturate(r);
r.a = saturate(r.r + black);
return r;

}
float2 KaleidoscopeUV(float2 uv, float posx, float posy, float number)
{
uv = uv - float2(posx, posy);
float r = length(uv);
float a = abs(atan2(uv.y, uv.x));
float sides = number;
float tau = 3.1416;
a = fmod(a, tau / sides);
a = abs(a - tau / sides / 2.);
uv = r * float2(cos(a), sin(a));
return uv;
}
float4 frag (v2f i) : COLOR
{
float2 TwistUV_1 = TwistUV(i.texcoord,TwistUV_TwistUV_Bend_1,TwistUV_TwistUV_PosX_1,TwistUV_TwistUV_PosY_1,TwistUV_TwistUV_Radius_1);
float2 KaleidoscopeUV_1 = KaleidoscopeUV(TwistUV_1,KaleidoscopeUV_PosX_1,KaleidoscopeUV_PosY_1,KaleidoscopeUV_Number_1);
float4 _GenerateLightning_1 = Generate_Lightning(KaleidoscopeUV_1,float2(0,1),0.5,0.5,4,5.260559,0.2059431,0);
float4 _CircleFade_1 = Circle_Fade(_GenerateLightning_1,i.texcoord,_CircleFade_PosX_1,_CircleFade_PosY_1,_CircleFade_Size_1,_CircleFade_Dist_1);
float2 TwistUV_2 = TwistUV(i.texcoord,TwistUV_TwistUV_Bend_2,TwistUV_TwistUV_PosX_2,TwistUV_TwistUV_PosY_2,TwistUV_TwistUV_Radius_2);
float2 KaleidoscopeUV_2 = KaleidoscopeUV(TwistUV_2,KaleidoscopeUV_PosX_2,KaleidoscopeUV_PosY_2,KaleidoscopeUV_Number_2);
float4 _GenerateLightning_2 = Generate_Lightning(KaleidoscopeUV_2,float2(0,1),_GenerateLightning_PosX_2,_GenerateLightning_PosY_2,_GenerateLightning_Size_2,_GenerateLightning_Number_2,_GenerateLightning_Speed_2,0);
float4 _CircleFade_2 = Circle_Fade(_GenerateLightning_2,i.texcoord,_CircleFade_PosX_2,_CircleFade_PosY_2,_CircleFade_Size_2,_CircleFade_Dist_2);
float4 OperationBlend_1 = OperationBlend(_CircleFade_1, _CircleFade_2, _OperationBlend_Fade_1); 
float4 FinalResult = OperationBlend_1;
FinalResult.rgb *= i.color.rgb;
FinalResult.a = FinalResult.a * _SpriteFade * i.color.a;
return FinalResult;
}

ENDCG
}
}
Fallback "Sprites/Default"
}

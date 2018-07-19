//////////////////////////////////////////////////////////////
/// Shadero Sprite: Sprite Shader Editor - by VETASOFT 2018 //
/// Shader generate with Shadero 1.8.0                      //
/// http://u3d.as/V7t #AssetStore                           //
/// http://www.shadero.com #Docs                            //
//////////////////////////////////////////////////////////////

Shader "Shadero Previews/PreviewXATXQ1"
{
Properties
{
[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
_SourceNewTex_1("_SourceNewTex_1(RGB)", 2D) = "white" { }
_Generate_Fire_PosX_2("_Generate_Fire_PosX_2", Range(-1, 2)) = 0
_Generate_Fire_PosY_2("_Generate_Fire_PosY_2", Range(-1, 2)) = 0
_Generate_Fire_Precision_2("_Generate_Fire_Precision_2", Range(0, 1)) = 0.05
_Generate_Fire_Smooth_2("_Generate_Fire_Smooth_2", Range(0, 1)) = 0.5
_Generate_Fire_Speed_2("_Generate_Fire_Speed_2", Range(-2, 2)) = 1
_Displacement_Value_1("_Displacement_Value_1", Range(-0.3, 0.3)) = 0.05
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

Tags {"Queue" = "Transparent" "IgnoreProjector" = "true" "RenderType" = "Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" "DisableBatching" = "True"}
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
float3 worldPos : TEXCOORD2;
float4 color    : COLOR;
};

sampler2D _MainTex;
float _SpriteFade;
sampler2D _SourceNewTex_1;
float _Generate_Fire_PosX_2;
float _Generate_Fire_PosY_2;
float _Generate_Fire_Precision_2;
float _Generate_Fire_Smooth_2;
float _Generate_Fire_Speed_2;
float _Displacement_Value_1;

v2f vert(appdata_t IN)
{
v2f OUT;
OUT.vertex = UnityObjectToClipPos(IN.vertex);
OUT.worldPos = mul (unity_ObjectToWorld, IN.vertex);
OUT.texcoord = IN.texcoord;
OUT.color = IN.color;
return OUT;
}


float4 DisplacementUV(float2 uv,sampler2D source,float x, float y, float value)
{
return tex2D(source,lerp(uv,uv+float2(x,y),value));
}
float Generate_Fire_hash2D(float2 x)
{
return frac(sin(dot(x, float2(13.454, 7.405)))*12.3043);
}

float Generate_Fire_voronoi2D(float2 uv, float precision)
{
float2 fl = floor(uv);
float2 fr = frac(uv);
float res = 1.0;
for (int j = -1; j <= 1; j++)
{
for (int i = -1; i <= 1; i++)
{
float2 p = float2(i, j);
float h = Generate_Fire_hash2D(fl + p);
float2 vp = p - fr + h;
float d = dot(vp, vp);
res += 1.0 / pow(d, 8.0);
}
}
return pow(1.0 / res, precision);
}

float4 Generate_Fire(float2 uv, float posX, float posY, float precision, float smooth, float speed, float black)
{
uv += float2(posX, posY);
float t = _Time*60*speed;
float up0 = Generate_Fire_voronoi2D(uv * float2(6.0, 4.0) + float2(0, -t), precision);
float up1 = 0.5 + Generate_Fire_voronoi2D(uv * float2(6.0, 4.0) + float2(42, -t ) + 30.0, precision);
float finalMask = up0 * up1  + (1.0 - uv.y);
finalMask += (1.0 - uv.y)* 0.5;
finalMask *= 0.7 - abs(uv.x - 0.5);
float4 result = smoothstep(smooth, 0.95, finalMask);
result.a = saturate(result.a + black);
return result;
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
float2 AnimatedZoomUV(float2 uv, float zoom, float posx, float posy, float radius, float speed)
{
float2 center = float2(posx, posy);
uv -= center;
zoom -= radius * 0.1;
zoom += sin(_Time * speed * 20) * 0.1 * radius;
uv = uv * zoom;
uv += center;
return uv;
}
float4 ShadowLight(sampler2D source, float2 uv, float precision, float size, float4 color, float intensity, float posx, float posy,float fade)
{
int samples = precision;
int samples2 = samples *0.5;
float4 ret = float4(0, 0, 0, 0);
float count = 0;
for (int iy = -samples2; iy < samples2; iy++)
{
for (int ix = -samples2; ix < samples2; ix++)
{
float2 uv2 = float2(ix, iy);
uv2 /= samples;
uv2 *= size*0.1;
uv2 += float2(-posx,posy);
uv2 = saturate(uv+uv2);
ret += tex2D(source, uv2);
count++;
}
}
ret = lerp(float4(0, 0, 0, 0), ret / count, intensity);
ret.rgb = color.rgb;
float4 m = ret;
float4 b = tex2D(source, uv);
ret = lerp(ret, b, b.a);
ret = lerp(m,ret,fade);
return ret;
}
inline float RBFXmod2(float x,float modu)
{
return x - floor(x * (1.0 / modu)) * modu;
}

float3 RBFXrainbow2(float t)
{
t= RBFXmod2(t,1.0);
float tx = t * 8;
float r = clamp(tx - 4.0, 0.0, 1.0) + clamp(2.0 - tx, 0.0, 1.0);
float g = tx < 2.0 ? clamp(tx, 0.0, 1.0) : clamp(4.0 - tx, 0.0, 1.0);
float b = tx < 4.0 ? clamp(tx - 2.0, 0.0, 1.0) : clamp(6.0 - tx, 0.0, 1.0);
return float3(r, g, b);
}

float4 PlasmaLight(float4 txt, float2 uv, float _Fade, float speed, float bw)
{
float _TimeX=_Time.y * speed;
float a = 1.1 + _TimeX * 2.25;
float b = 0.5 + _TimeX * 1.77;
float c = 8.4 + _TimeX * 1.58;
float d = 610 + _TimeX * 2.03;
float x1 = 2.0 * uv.x;
float n = sin(a + x1) + sin(b - x1) + sin(c + 2.0 * uv.y) + sin(d + 5.0 * uv.y);
n = RBFXmod2(((5.0 + n) / 5.0), 1.0);
float4 nx=txt;
n += nx.r * 0.2 + nx.g * 0.4 + nx.b * 0.2;
float4 ret=float4(RBFXrainbow2(n),txt.a);
float g=ret.g;
ret= lerp(ret,g+ret,bw);
ret = lerp(txt,txt+ret,_Fade);
ret.a = txt.a;
return ret;
}
float4 HolographicParalax(float2 uv, sampler2D source, float value)
{
float rtx = -unity_ObjectToWorld[0][2];
rtx = rtx * 0.1;
float4 rgb = tex2D(source, uv + float2(rtx, 0));
float r = tex2D(source, uv + float2(rtx * (1-value), 0)).r;
float b = tex2D(source, uv + float2(rtx * (1+value), 0)).b;
return float4(r, rgb.g, b,rgb.a);
}
float4 frag (v2f i) : COLOR
{
float4 _Generate_Fire_2 = Generate_Fire(i.texcoord,_Generate_Fire_PosX_2,_Generate_Fire_PosY_2,_Generate_Fire_Precision_2,_Generate_Fire_Smooth_2,_Generate_Fire_Speed_2,0);
float4 _Displacement_1 = DisplacementUV(i.texcoord,_SourceNewTex_1,_Generate_Fire_2.r*_Generate_Fire_2.a,_Generate_Fire_2.g*_Generate_Fire_2.a,_Displacement_Value_1);
float4 FinalResult = _Displacement_1;
FinalResult.rgb *= i.color.rgb;
FinalResult.a = FinalResult.a * _SpriteFade * i.color.a;
return FinalResult;
}

ENDCG
}
}
Fallback "Sprites/Default"
}

// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shaders_VFX_Orbs"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_ToggleElementTexColours_Base0_Secondary1("ToggleElementTexColours_Base(0)_Secondary(1)", Range( 0 , 1)) = 0
		_ToggleElementFresColours_Base0_Secondary1("ToggleElementFresColours_Base(0)_Secondary(1)", Range( 0 , 1)) = 0
		_ToggleCatalystFresColours_Base0_Secondary1("ToggleCatalystFresColours_Base(0)_Secondary(1)", Range( 0 , 1)) = 0
		_ToggleCatalystTexColours_Base0_Secondary1("ToggleCatalystTexColours_Base(0)_Secondary(1)", Range( 0 , 1)) = 0
		_Toggle_Flashing("Toggle_Flashing", Range( 0 , 1)) = 0
		_ToggleElementCull("ToggleElementCull", Range( 0 , 1)) = 0.1294118
		_ToggleElementFade("ToggleElementFade", Range( -100 , 100)) = -0.5
		_ToggleCatalystCull("ToggleCatalystCull", Range( 0 , 1)) = 1
		_ToggleCatalystFade("ToggleCatalystFade", Range( -100 , 100)) = 0
		_ToggleOverallFade("ToggleOverallFade", Range( 0 , 100)) = 0
		_ToggleOverallCull("ToggleOverallCull", Range( -2 , 2)) = 0
		_Toggle_Element_Noise_Mask("Toggle_Element_Noise_Mask", Range( 0 , 1)) = 0
		_Toggle_Catalyst_Noise_Mask("Toggle_Catalyst_Noise_Mask", Range( 0 , 1)) = 0
		_Toggle_UseElementTexture_1_With_Noise_1("Toggle_UseElementTexture_1_With_Noise_1", Range( 0 , 1)) = 0
		_Toggle_UseElementTexture_2_With_Noise_2("Toggle_UseElementTexture_2_With_Noise_2", Range( 0 , 1)) = 0
		_Toggle_UseElementTex_3__4("Toggle_UseElementTex_3_&_4", Range( 0 , 1)) = 0
		_Toggle_UseCatalystTexture_1_With_Noise_1("Toggle_UseCatalystTexture_1_With_Noise_1", Range( 0 , 1)) = 0
		_Toggle_UseCatalystTexture_2_With_Noise_2("Toggle_UseCatalystTexture_2_With_Noise_2", Range( 0 , 1)) = 0
		_Toggle_UseCatalystTex_3__4("Toggle_UseCatalystTex_3_&_4", Range( 0 , 1)) = 0
		_Toggle_Element_Spec_On0_Off1("Toggle_Element_Spec_On(0)_Off(1)", Range( 0 , 1)) = 1
		_Toggle_Cataylst_Spec_On0_Off1("Toggle_Cataylst_Spec_On(0)_Off(1)", Range( 0 , 1)) = 1
		_Toggle_UseVertexNoise_Catalyst0_Element1("Toggle_UseVertexNoise_Catalyst(0)_Element(1)", Range( 0 , 1)) = 1
		_VertOffset_Intensity("VertOffset_Intensity", Float) = 0
		_NoiseScales_E1X_E2Y_C1Z_C2W("NoiseScales_E1(X)_E2(Y)_C1(Z)_C2(W)", Vector) = (1,1,1,1)
		[HDR]_BaseColour_Element("BaseColour_Element", Color) = (1,0,0,0)
		[HDR]_SecondaryColour_Element("SecondaryColour_Element", Color) = (1,0.8760238,0,0)
		_ElementTexture_Slot_1("ElementTexture_Slot_1", 2D) = "white" {}
		_ElementTexture_Slot_2("ElementTexture_Slot_2", 2D) = "white" {}
		_ElementTexture_Slot_3("ElementTexture_Slot_3", 2D) = "white" {}
		_ElementTexture_Slot_4("ElementTexture_Slot_4", 2D) = "white" {}
		_Element_Tex_Pan_Slot1_Slot2("Element_Tex_Pan_Slot1_Slot2", Vector) = (0,0,0,0)
		_Element_Tex_Pan_Slot3_Slot4("Element_Tex_Pan_Slot3_Slot4", Vector) = (0,0,0,0)
		_Element_Spec_StepValue("Element_Spec_StepValue", Float) = 0.1
		_Element_Specs_Intensity("Element_Specs_Intensity", Float) = 1
		_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW("Element_Specs_Remap_MinOld(X)_MaxOld(Y)_MinNew(Z)_MaxNew(W)", Vector) = (0,1,0,1)
		_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW("ElementNoise_Pan_Speed_Noise1(X/Y)_Noise2(Z/W)", Vector) = (0,0,0,0)
		_ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W("ElementVoronoi_Speed1(X)_Speed2(Y)_Scale1(Z)_Scale2(W)", Vector) = (0,0,0,0)
		[HDR]_BaseColour_Catalyst("BaseColour_Catalyst", Color) = (0,0.7648358,1,0)
		[HDR]_SecondaryColour_Catalyst("SecondaryColour_Catalyst", Color) = (1,0,0.7082286,0)
		_CatalystTexture_Slot_1("CatalystTexture_Slot_1", 2D) = "white" {}
		_CatalystTexture_Slot_2("CatalystTexture_Slot_2", 2D) = "white" {}
		_CatalystTexture_Slot_3("CatalystTexture_Slot_3", 2D) = "white" {}
		_CatalystTexture_Slot_4("CatalystTexture_Slot_4", 2D) = "white" {}
		_Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W("Catalyst_Flash_Slot1(X)_Slot2(Y)_Slot3(Z)_Slot4(W)", Vector) = (0,0,0,0)
		_Cataylst_Tex_Pan_Slot1XY_Slot2ZW("Cataylst_Tex_Pan_Slot1(X/Y)_Slot2(Z/W)", Vector) = (0,0,0,0)
		_Cataylst_Tex_Pan_Slot3XY_Slot4ZW("Cataylst_Tex_Pan_Slot3(X/Y)_Slot4(Z/W)", Vector) = (0,0,0,0)
		_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW("CatalystNoise_Pan_Speed_Noise1(X/Y)_Noise2(Z/W)", Vector) = (0,0,0,0)
		_CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W("CatalystVoronoi_Speed1(X)_Speed2(Y)_Scale1(Z)_Scale2(W)", Vector) = (0,0,0,0)
		_Element_Base_Noise_1_Remap("Element_Base_Noise_1_Remap", Vector) = (0,1,0,1)
		_Catalyst_Base_Noise_1_Remap("Catalyst_Base_Noise_1_Remap", Vector) = (0,1,0,1)
		_Element_Base_Noise_2_Remap("Element_Base_Noise_2_Remap", Vector) = (0,1,0,1)
		_Catalyst_Base_Noise_2_Remap("Catalyst_Base_Noise_2_Remap", Vector) = (0,1,0,1)
		_Element_Base_Remap("Element_Base_Remap", Vector) = (0,1,0,1)
		_Catalyst_Base_Remap("Catalyst_Base_Remap", Vector) = (0,1,0,1)
		_Catalyst_Spec_StepValue("Catalyst_Spec_StepValue", Float) = 0.1
		_Catalyst_Specs_Intensity("Catalyst_Specs_Intensity", Float) = 1
		_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW("Catalyst_Specs_Remap_MinOld(X)_MaxOld(Y)_MinNew(Z)_MaxNew(W)", Vector) = (0,1,0,1)
		_Fres_ScaleX_PowerY_Speeds("Fres_Scale(X)_Power(Y)_Speeds", Vector) = (1,1,0,0)
		_Fres_Scale_MinNew("Fres_Scale_MinNew", Float) = 1
		_Fres_Scale_MaxNew("Fres_Scale_MaxNew", Float) = 5
		_Fres_Power_MinNew("Fres_Power_MinNew", Float) = 1
		_Fres_Power_MaxNew("Fres_Power_MaxNew", Float) = 5
		_SoftEgdeMask_Texture("SoftEgdeMask_Texture", 2D) = "white" {}
		[ASEEnd]_SoftEdgeFade("SoftEdgeFade", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		
		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Transparent" "Queue"="Transparent" }
		
		Cull Back
		AlphaToMask Off
		
		HLSLINCLUDE
		#pragma target 2.0

		#pragma prefer_hlslcc gles
		#pragma exclude_renderers d3d11_9x 

		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70701

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_COLOR


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				#ifdef ASE_FOG
				float fogFactor : TEXCOORD2;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW;
			float4 _Cataylst_Tex_Pan_Slot1XY_Slot2ZW;
			float4 _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W;
			float4 _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW;
			float4 _Element_Tex_Pan_Slot3_Slot4;
			float4 _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W;
			float4 _Catalyst_Base_Noise_1_Remap;
			float4 _Catalyst_Base_Noise_2_Remap;
			float4 _Catalyst_Base_Remap;
			float4 _SecondaryColour_Catalyst;
			float4 _Element_Base_Remap;
			float4 _Element_Base_Noise_2_Remap;
			float4 _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W;
			float4 _SoftEgdeMask_Texture_ST;
			float4 _Cataylst_Tex_Pan_Slot3XY_Slot4ZW;
			float4 _NoiseScales_E1X_E2Y_C1Z_C2W;
			float4 _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW;
			float4 _Element_Base_Noise_1_Remap;
			float4 _BaseColour_Element;
			float4 _SecondaryColour_Element;
			float4 _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW;
			float4 _Element_Tex_Pan_Slot1_Slot2;
			float4 _BaseColour_Catalyst;
			float2 _Fres_ScaleX_PowerY_Speeds;
			float _Toggle_Catalyst_Noise_Mask;
			float _Toggle_UseCatalystTexture_2_With_Noise_2;
			float _ToggleCatalystFresColours_Base0_Secondary1;
			float _Toggle_UseCatalystTexture_1_With_Noise_1;
			float _Catalyst_Spec_StepValue;
			float _Catalyst_Specs_Intensity;
			float _Toggle_Flashing;
			float _Toggle_Cataylst_Spec_On0_Off1;
			float _Toggle_UseCatalystTex_3__4;
			float _ToggleCatalystTexColours_Base0_Secondary1;
			float _Toggle_UseElementTex_3__4;
			float _ToggleCatalystCull;
			float _Toggle_UseVertexNoise_Catalyst0_Element1;
			float _VertOffset_Intensity;
			float _ToggleElementCull;
			float _ToggleElementFade;
			float _ToggleElementTexColours_Base0_Secondary1;
			float _Toggle_UseElementTexture_1_With_Noise_1;
			float _Toggle_UseElementTexture_2_With_Noise_2;
			float _SoftEdgeFade;
			float _ToggleCatalystFade;
			float _Toggle_Element_Noise_Mask;
			float _Fres_Scale_MaxNew;
			float _Fres_Power_MinNew;
			float _Fres_Power_MaxNew;
			float _ToggleElementFresColours_Base0_Secondary1;
			float _ToggleOverallCull;
			float _Element_Spec_StepValue;
			float _Element_Specs_Intensity;
			float _Toggle_Element_Spec_On0_Off1;
			float _Fres_Scale_MinNew;
			float _ToggleOverallFade;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _ElementTexture_Slot_1;
			sampler2D _ElementTexture_Slot_2;
			sampler2D _SoftEgdeMask_Texture;
			sampler2D _ElementTexture_Slot_3;
			sampler2D _ElementTexture_Slot_4;
			sampler2D _CatalystTexture_Slot_1;
			sampler2D _CatalystTexture_Slot_2;
			sampler2D _CatalystTexture_Slot_3;
			sampler2D _CatalystTexture_Slot_4;


			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			
					float2 voronoihash305( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi305( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash305( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return (F2 + F1) * 0.5;
					}
			
					float2 voronoihash289( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi289( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash289( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
					float2 voronoihash313( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi313( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash313( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return (F2 + F1) * 0.5;
					}
			
					float2 voronoihash309( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi309( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash309( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
			
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 appendResult229 = (float2(_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 CatalystNoise1_Pan233 = appendResult229;
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float3 worldToObj216 = mul( GetWorldToObjectMatrix(), float4( ase_worldPos, 1 ) ).xyz;
				float2 panner224 = ( _TimeParameters.x * CatalystNoise1_Pan233 + worldToObj216.xy);
				float CatalystNoise_1_Scale237 = _NoiseScales_E1X_E2Y_C1Z_C2W.z;
				float simplePerlin3D226 = snoise( float3( panner224 ,  0.0 )*CatalystNoise_1_Scale237 );
				simplePerlin3D226 = simplePerlin3D226*0.5 + 0.5;
				float CatalystNoise_1227 = simplePerlin3D226;
				float2 appendResult208 = (float2(_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 ElementNoise1_Pan212 = appendResult208;
				float3 worldToObj46 = mul( GetWorldToObjectMatrix(), float4( ase_worldPos, 1 ) ).xyz;
				float2 panner52 = ( _TimeParameters.x * ElementNoise1_Pan212 + worldToObj46.xy);
				float ElementNoise_1_Scale235 = _NoiseScales_E1X_E2Y_C1Z_C2W.x;
				float simplePerlin3D59 = snoise( float3( panner52 ,  0.0 )*ElementNoise_1_Scale235 );
				simplePerlin3D59 = simplePerlin3D59*0.5 + 0.5;
				float ElementNoise_161 = simplePerlin3D59;
				float lerpResult468 = lerp( CatalystNoise_1227 , ElementNoise_161 , _Toggle_UseVertexNoise_Catalyst0_Element1);
				float3 VertexOffset10 = ( v.ase_normal * ( lerpResult468 * _VertOffset_Intensity ) );
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord4.xyz = ase_worldNormal;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;
				o.ase_texcoord4.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = VertexOffset10;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				#ifdef ASE_FOG
				o.fogFactor = ComputeFogFactor( positionCS.z );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag ( VertexOutput IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif
				float3 worldToObj49 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float temp_output_58_0 = ( worldToObj49.y * 0.88 );
				float smoothstepResult69 = smoothstep( _ToggleElementCull , _ToggleElementFade , temp_output_58_0);
				float Base_BottomCull72 = smoothstepResult69;
				float4 BaseColours_Elements19 = ( IN.ase_color * _BaseColour_Element );
				float4 SecondaryColours_Elements281 = ( IN.ase_color * _SecondaryColour_Element );
				float4 lerpResult329 = lerp( BaseColours_Elements19 , SecondaryColours_Elements281 , _ToggleElementTexColours_Base0_Secondary1);
				float4 Element_Base_Colour428 = ( Base_BottomCull72 * lerpResult329 );
				float2 appendResult208 = (float2(_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 ElementNoise1_Pan212 = appendResult208;
				float3 worldToObj46 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float2 panner52 = ( _TimeParameters.x * ElementNoise1_Pan212 + worldToObj46.xy);
				float ElementNoise_1_Scale235 = _NoiseScales_E1X_E2Y_C1Z_C2W.x;
				float simplePerlin3D59 = snoise( float3( panner52 ,  0.0 )*ElementNoise_1_Scale235 );
				simplePerlin3D59 = simplePerlin3D59*0.5 + 0.5;
				float ElementNoise_161 = simplePerlin3D59;
				float4 temp_cast_2 = (ElementNoise_161).xxxx;
				float2 appendResult167 = (float2(_Element_Tex_Pan_Slot1_Slot2.x , _Element_Tex_Pan_Slot1_Slot2.y));
				float2 ElementPan_Slot_1171 = appendResult167;
				float2 texCoord143 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner136 = ( 1.0 * _Time.y * ElementPan_Slot_1171 + texCoord143);
				float4 ElementTex_1207 = tex2D( _ElementTexture_Slot_1, panner136 );
				float4 lerpResult420 = lerp( temp_cast_2 , ( ElementTex_1207 * ElementNoise_161 ) , _Toggle_UseElementTexture_1_With_Noise_1);
				float4 temp_cast_3 = (_Element_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_4 = (_Element_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_5 = (_Element_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_6 = (_Element_Base_Noise_1_Remap.w).xxxx;
				float2 appendResult209 = (float2(_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.z , _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.w));
				float2 ElementNoise2_Pan213 = appendResult209;
				float2 panner163 = ( _TimeParameters.x * ElementNoise2_Pan213 + worldToObj46.xy);
				float ElementNoise_2_Scale236 = _NoiseScales_E1X_E2Y_C1Z_C2W.y;
				float simplePerlin3D165 = snoise( float3( panner163 ,  0.0 )*ElementNoise_2_Scale236 );
				simplePerlin3D165 = simplePerlin3D165*0.5 + 0.5;
				float ElementNoise_2166 = simplePerlin3D165;
				float4 temp_cast_9 = (ElementNoise_2166).xxxx;
				float2 appendResult168 = (float2(_Element_Tex_Pan_Slot1_Slot2.z , _Element_Tex_Pan_Slot1_Slot2.w));
				float2 ElementPan_Slot_2172 = appendResult168;
				float2 panner137 = ( 1.0 * _Time.y * ElementPan_Slot_2172 + texCoord143);
				float4 ElementTex_2206 = tex2D( _ElementTexture_Slot_2, panner137 );
				float4 lerpResult412 = lerp( temp_cast_9 , ( ElementTex_2206 * ElementNoise_2166 ) , _Toggle_UseElementTexture_2_With_Noise_2);
				float4 temp_cast_10 = (_Element_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_11 = (_Element_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_12 = (_Element_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_13 = (_Element_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_14 = (_Element_Base_Remap.x).xxxx;
				float4 temp_cast_15 = (_Element_Base_Remap.y).xxxx;
				float4 temp_cast_16 = (_Element_Base_Remap.z).xxxx;
				float4 temp_cast_17 = (_Element_Base_Remap.w).xxxx;
				float4 temp_output_403_0 = (temp_cast_16 + (( (temp_cast_5 + (lerpResult420 - temp_cast_3) * (temp_cast_6 - temp_cast_5) / (temp_cast_4 - temp_cast_3)) * (temp_cast_12 + (lerpResult412 - temp_cast_10) * (temp_cast_13 - temp_cast_12) / (temp_cast_11 - temp_cast_10)) ) - temp_cast_14) * (temp_cast_17 - temp_cast_16) / (temp_cast_15 - temp_cast_14));
				float4 temp_cast_18 = (1.0).xxxx;
				float4 temp_cast_19 = (_Element_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_20 = (_Element_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_21 = (_Element_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_22 = (_Element_Base_Noise_1_Remap.w).xxxx;
				float4 temp_cast_23 = (_Element_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_24 = (_Element_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_25 = (_Element_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_26 = (_Element_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_27 = (_Element_Base_Remap.x).xxxx;
				float4 temp_cast_28 = (_Element_Base_Remap.y).xxxx;
				float4 temp_cast_29 = (_Element_Base_Remap.z).xxxx;
				float4 temp_cast_30 = (_Element_Base_Remap.w).xxxx;
				float2 uv_SoftEgdeMask_Texture = IN.ase_texcoord3.xy * _SoftEgdeMask_Texture_ST.xy + _SoftEgdeMask_Texture_ST.zw;
				float SoftEdgeMask249 = ( tex2D( _SoftEgdeMask_Texture, uv_SoftEgdeMask_Texture ).r * _SoftEdgeFade );
				float4 lerpResult407 = lerp( temp_cast_18 , ( temp_output_403_0 * SoftEdgeMask249 ) , _Toggle_Element_Noise_Mask);
				float4 Element_Base424 = ( ( temp_output_403_0 * lerpResult407 ) * 1.0 );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord4.xyz;
				float fresnelNdotV53 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode53 = ( 0.0 + (_Fres_Scale_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.x ) ) ) - 0.0) * (_Fres_Scale_MaxNew - _Fres_Scale_MinNew) / (10.0 - 0.0)) * pow( 1.0 - fresnelNdotV53, (_Fres_Power_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.y ) ) ) - 0.0) * (_Fres_Power_MaxNew - _Fres_Power_MinNew) / (1.0 - 0.0)) ) );
				float Fresnel_Pulse67 = fresnelNode53;
				float Fresnel_BottomCull106 = ( Base_BottomCull72 * Fresnel_Pulse67 );
				float4 lerpResult354 = lerp( BaseColours_Elements19 , SecondaryColours_Elements281 , _ToggleElementFresColours_Base0_Secondary1);
				float4 Element_Fresnel_Colour429 = ( Fresnel_BottomCull106 * lerpResult354 );
				float ElementVoronoi_Scale_1301 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.z;
				float ElementVoronoi_Speed_1299 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.x;
				float time305 = ( _TimeParameters.x * ElementVoronoi_Speed_1299 );
				float2 voronoiSmoothId305 = 0;
				float2 texCoord284 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 coords305 = texCoord284 * ElementVoronoi_Scale_1301;
				float2 id305 = 0;
				float2 uv305 = 0;
				float fade305 = 0.5;
				float voroi305 = 0;
				float rest305 = 0;
				for( int it305 = 0; it305 <8; it305++ ){
				voroi305 += fade305 * voronoi305( coords305, time305, id305, uv305, 0,voronoiSmoothId305 );
				rest305 += fade305;
				coords305 *= 2;
				fade305 *= 0.5;
				}//Voronoi305
				voroi305 /= rest305;
				float ElementVoronoi_2328 = voroi305;
				float4 temp_cast_31 = (0.0).xxxx;
				float2 appendResult169 = (float2(_Element_Tex_Pan_Slot3_Slot4.x , _Element_Tex_Pan_Slot3_Slot4.y));
				float2 ElementPan_Slot_3173 = appendResult169;
				float2 panner138 = ( 1.0 * _Time.y * ElementPan_Slot_3173 + texCoord143);
				float4 ElementTex_3204 = tex2D( _ElementTexture_Slot_3, panner138 );
				float2 appendResult170 = (float2(_Element_Tex_Pan_Slot3_Slot4.z , _Element_Tex_Pan_Slot3_Slot4.w));
				float2 ElementPan_Slot_4174 = appendResult170;
				float2 panner139 = ( 1.0 * _Time.y * ElementPan_Slot_4174 + texCoord143);
				float4 ElementTex_4205 = tex2D( _ElementTexture_Slot_4, panner139 );
				float4 lerpResult457 = lerp( temp_cast_31 , ( ( ElementTex_3204 + ElementTex_4205 ) * (Element_Base_Colour428).rgba ) , _Toggle_UseElementTex_3__4);
				float temp_output_3_0_g25 = ( ElementNoise_2166 - ElementNoise_161 );
				float temp_output_261_0 = ( (_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z + (ElementNoise_161 - _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x) * (_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.w - _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z) / (_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.y - _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x)) * saturate( ( temp_output_3_0_g25 / fwidth( temp_output_3_0_g25 ) ) ) );
				float ElementVoronoi_Scale_2302 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.w;
				float ElementVoronoi_Speed_2300 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.y;
				float time289 = ( _TimeParameters.x * ElementVoronoi_Speed_2300 );
				float2 voronoiSmoothId289 = 0;
				float2 coords289 = texCoord284 * ElementVoronoi_Scale_2302;
				float2 id289 = 0;
				float2 uv289 = 0;
				float fade289 = 0.5;
				float voroi289 = 0;
				float rest289 = 0;
				for( int it289 = 0; it289 <2; it289++ ){
				voroi289 += fade289 * voronoi289( coords289, time289, id289, uv289, 0,voronoiSmoothId289 );
				rest289 += fade289;
				coords289 *= 2;
				fade289 *= 0.5;
				}//Voronoi289
				voroi289 /= rest289;
				float ElementVoronoi_1327 = voroi289;
				float temp_output_3_0_g24 = ( _Element_Spec_StepValue - ElementVoronoi_1327 );
				float lerpResult505 = lerp( 1.0 , 0.0 , _Toggle_Element_Spec_On0_Off1);
				float ElementBrightParts266 = ( ( ( ( temp_output_261_0 * ( temp_output_261_0 * ( saturate( ( temp_output_3_0_g24 / fwidth( temp_output_3_0_g24 ) ) ) * SoftEdgeMask249 ) ) ) * _Element_Specs_Intensity ) * Base_BottomCull72 ) * lerpResult505 );
				float smoothstepResult68 = smoothstep( _ToggleCatalystCull , _ToggleCatalystFade , ( 1.0 - temp_output_58_0 ));
				float Base_TopCull71 = smoothstepResult68;
				float4 BaseColours_Cataylst30 = ( IN.ase_color * _BaseColour_Catalyst );
				float4 SecondaryColours_Cataylst282 = ( IN.ase_color * _SecondaryColour_Catalyst );
				float4 lerpResult335 = lerp( BaseColours_Cataylst30 , SecondaryColours_Cataylst282 , _ToggleCatalystTexColours_Base0_Secondary1);
				float4 Catalyst_Base_Colour430 = ( Base_TopCull71 * lerpResult335 );
				float2 appendResult229 = (float2(_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 CatalystNoise1_Pan233 = appendResult229;
				float3 worldToObj216 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float2 panner224 = ( _TimeParameters.x * CatalystNoise1_Pan233 + worldToObj216.xy);
				float CatalystNoise_1_Scale237 = _NoiseScales_E1X_E2Y_C1Z_C2W.z;
				float simplePerlin3D226 = snoise( float3( panner224 ,  0.0 )*CatalystNoise_1_Scale237 );
				simplePerlin3D226 = simplePerlin3D226*0.5 + 0.5;
				float CatalystNoise_1227 = simplePerlin3D226;
				float4 temp_cast_34 = (CatalystNoise_1227).xxxx;
				float2 appendResult140 = (float2(_Cataylst_Tex_Pan_Slot1XY_Slot2ZW.x , _Cataylst_Tex_Pan_Slot1XY_Slot2ZW.y));
				float2 CatalystPan_Slot_1148 = appendResult140;
				float2 texCoord191 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner196 = ( 1.0 * _Time.y * CatalystPan_Slot_1148 + texCoord191);
				float lerpResult497 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.x ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnX125 = lerpResult497;
				float4 CatalystTex_1200 = ( tex2D( _CatalystTexture_Slot_1, panner196 ) * FlashingOnX125 );
				float4 lerpResult387 = lerp( temp_cast_34 , ( CatalystTex_1200 * CatalystNoise_1227 ) , _Toggle_UseCatalystTexture_1_With_Noise_1);
				float4 temp_cast_35 = (_Catalyst_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_36 = (_Catalyst_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_37 = (_Catalyst_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_38 = (_Catalyst_Base_Noise_1_Remap.w).xxxx;
				float2 appendResult231 = (float2(_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.z , _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.w));
				float2 CatalystNoise2_Pan232 = appendResult231;
				float2 panner222 = ( _TimeParameters.x * CatalystNoise2_Pan232 + worldToObj216.xy);
				float CatalystNoise_2_Scale238 = _NoiseScales_E1X_E2Y_C1Z_C2W.w;
				float simplePerlin3D225 = snoise( float3( panner222 ,  0.0 )*CatalystNoise_2_Scale238 );
				simplePerlin3D225 = simplePerlin3D225*0.5 + 0.5;
				float CatalystNoise_2228 = simplePerlin3D225;
				float4 temp_cast_41 = (CatalystNoise_2228).xxxx;
				float2 appendResult141 = (float2(_Cataylst_Tex_Pan_Slot1XY_Slot2ZW.z , _Cataylst_Tex_Pan_Slot1XY_Slot2ZW.w));
				float2 CatalystPan_Slot_2149 = appendResult141;
				float2 panner197 = ( 1.0 * _Time.y * CatalystPan_Slot_2149 + texCoord191);
				float lerpResult496 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.y ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnY124 = lerpResult496;
				float4 CatalystTex_2201 = ( tex2D( _CatalystTexture_Slot_2, panner197 ) * FlashingOnY124 );
				float4 lerpResult394 = lerp( temp_cast_41 , ( CatalystTex_2201 * CatalystNoise_2228 ) , _Toggle_UseCatalystTexture_2_With_Noise_2);
				float4 temp_cast_42 = (_Catalyst_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_43 = (_Catalyst_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_44 = (_Catalyst_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_45 = (_Catalyst_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_46 = (_Catalyst_Base_Remap.x).xxxx;
				float4 temp_cast_47 = (_Catalyst_Base_Remap.y).xxxx;
				float4 temp_cast_48 = (_Catalyst_Base_Remap.z).xxxx;
				float4 temp_cast_49 = (_Catalyst_Base_Remap.w).xxxx;
				float4 temp_output_371_0 = (temp_cast_48 + (( (temp_cast_37 + (lerpResult387 - temp_cast_35) * (temp_cast_38 - temp_cast_37) / (temp_cast_36 - temp_cast_35)) * (temp_cast_44 + (lerpResult394 - temp_cast_42) * (temp_cast_45 - temp_cast_44) / (temp_cast_43 - temp_cast_42)) ) - temp_cast_46) * (temp_cast_49 - temp_cast_48) / (temp_cast_47 - temp_cast_46));
				float4 temp_cast_50 = (1.0).xxxx;
				float4 temp_cast_51 = (_Catalyst_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_52 = (_Catalyst_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_53 = (_Catalyst_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_54 = (_Catalyst_Base_Noise_1_Remap.w).xxxx;
				float4 temp_cast_55 = (_Catalyst_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_56 = (_Catalyst_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_57 = (_Catalyst_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_58 = (_Catalyst_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_59 = (_Catalyst_Base_Remap.x).xxxx;
				float4 temp_cast_60 = (_Catalyst_Base_Remap.y).xxxx;
				float4 temp_cast_61 = (_Catalyst_Base_Remap.z).xxxx;
				float4 temp_cast_62 = (_Catalyst_Base_Remap.w).xxxx;
				float4 lerpResult378 = lerp( temp_cast_50 , ( temp_output_371_0 * SoftEdgeMask249 ) , _Toggle_Catalyst_Noise_Mask);
				float4 Catalyst_Base398 = ( ( temp_output_371_0 * lerpResult378 ) * 1.0 );
				float Fresnel_TopCull105 = ( Base_TopCull71 * Fresnel_Pulse67 );
				float4 lerpResult364 = lerp( BaseColours_Cataylst30 , SecondaryColours_Cataylst282 , _ToggleCatalystFresColours_Base0_Secondary1);
				float4 Catalyst_Fresnel_Colour431 = ( Fresnel_TopCull105 * lerpResult364 );
				float CatalystVoronoi_Scale_2296 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.w;
				float CatalystVoronoi_Speed_2294 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.y;
				float time313 = ( _TimeParameters.x * CatalystVoronoi_Speed_2294 );
				float2 voronoiSmoothId313 = 0;
				float2 texCoord315 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float2 coords313 = texCoord315 * CatalystVoronoi_Scale_2296;
				float2 id313 = 0;
				float2 uv313 = 0;
				float fade313 = 0.5;
				float voroi313 = 0;
				float rest313 = 0;
				for( int it313 = 0; it313 <8; it313++ ){
				voroi313 += fade313 * voronoi313( coords313, time313, id313, uv313, 0,voronoiSmoothId313 );
				rest313 += fade313;
				coords313 *= 2;
				fade313 *= 0.5;
				}//Voronoi313
				voroi313 /= rest313;
				float CatalystVoronoi_2326 = voroi313;
				float4 temp_cast_63 = (0.0).xxxx;
				float2 appendResult146 = (float2(_Cataylst_Tex_Pan_Slot3XY_Slot4ZW.x , _Cataylst_Tex_Pan_Slot3XY_Slot4ZW.y));
				float2 CatalystPan_Slot_3150 = appendResult146;
				float2 panner198 = ( 1.0 * _Time.y * CatalystPan_Slot_3150 + texCoord191);
				float lerpResult495 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.z ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnZ126 = lerpResult495;
				float4 CatalystTex_3202 = ( tex2D( _CatalystTexture_Slot_3, panner198 ) * FlashingOnZ126 );
				float2 appendResult147 = (float2(_Cataylst_Tex_Pan_Slot3XY_Slot4ZW.z , _Cataylst_Tex_Pan_Slot3XY_Slot4ZW.w));
				float2 CataylstPan_Slot_4151 = appendResult147;
				float2 panner199 = ( 1.0 * _Time.y * CataylstPan_Slot_4151 + texCoord191);
				float lerpResult493 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.w ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnW181 = lerpResult493;
				float4 CatalystTex_4203 = ( tex2D( _CatalystTexture_Slot_4, panner199 ) * FlashingOnW181 );
				float4 lerpResult452 = lerp( temp_cast_63 , ( ( CatalystTex_3202 + CatalystTex_4203 ) * (Catalyst_Base_Colour430).rgba ) , _Toggle_UseCatalystTex_3__4);
				float temp_output_3_0_g23 = ( CatalystNoise_2228 - CatalystNoise_1227 );
				float temp_output_338_0 = ( (_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z + (CatalystNoise_1227 - _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x) * (_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.w - _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z) / (_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.y - _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x)) * saturate( ( temp_output_3_0_g23 / fwidth( temp_output_3_0_g23 ) ) ) );
				float CatalystVoronoi_Scale_1295 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.z;
				float CatalystVoronoi_Speed_1293 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.x;
				float time309 = ( _TimeParameters.x * CatalystVoronoi_Speed_1293 );
				float2 voronoiSmoothId309 = 0;
				float2 coords309 = texCoord315 * CatalystVoronoi_Scale_1295;
				float2 id309 = 0;
				float2 uv309 = 0;
				float fade309 = 0.5;
				float voroi309 = 0;
				float rest309 = 0;
				for( int it309 = 0; it309 <2; it309++ ){
				voroi309 += fade309 * voronoi309( coords309, time309, id309, uv309, 0,voronoiSmoothId309 );
				rest309 += fade309;
				coords309 *= 2;
				fade309 *= 0.5;
				}//Voronoi309
				voroi309 /= rest309;
				float CatalystVoronoi_1325 = voroi309;
				float temp_output_3_0_g22 = ( _Catalyst_Spec_StepValue - CatalystVoronoi_1325 );
				float lerpResult509 = lerp( 1.0 , 0.0 , _Toggle_Cataylst_Spec_On0_Off1);
				float CatalystBrightParts348 = ( ( ( ( temp_output_338_0 * ( temp_output_338_0 * ( saturate( ( temp_output_3_0_g22 / fwidth( temp_output_3_0_g22 ) ) ) * SoftEdgeMask249 ) ) ) * _Catalyst_Specs_Intensity ) * Base_TopCull71 ) * lerpResult509 );
				float4 temp_output_445_0 = ( ( ( ( Element_Base_Colour428 * Element_Base424 ) + ( Element_Fresnel_Colour429 * ElementVoronoi_2328 ) ) + lerpResult457 + ( Element_Base_Colour428 * ElementBrightParts266 ) ) + ( ( ( Catalyst_Base_Colour430 * Catalyst_Base398 ) + ( Catalyst_Fresnel_Colour431 * CatalystVoronoi_2326 ) ) + lerpResult452 + ( Catalyst_Base_Colour430 * CatalystBrightParts348 ) ) );
				float4 Colour8 = temp_output_445_0;
				
				float grayscale462 = (temp_output_445_0.rgb.r + temp_output_445_0.rgb.g + temp_output_445_0.rgb.b) / 3;
				float VertexAlpha297 = IN.ase_color.a;
				float3 worldToObj92 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float smoothstepResult85 = smoothstep( _ToggleOverallCull , ( _ToggleOverallCull + _ToggleOverallFade ) , ( ElementNoise_161 + ( 1.0 - (0.0 + (worldToObj92.y - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) ) ));
				float DisolveAlpha93 = smoothstepResult85;
				float Alpha9 = ( ( (0.01 + (grayscale462 - 0.0) * (4.0 - 0.01) / (1.0 - 0.0)) * VertexAlpha297 ) * DisolveAlpha93 );
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = Colour8.rgb;
				float Alpha = Alpha9;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70701

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_COLOR


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW;
			float4 _Cataylst_Tex_Pan_Slot1XY_Slot2ZW;
			float4 _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W;
			float4 _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW;
			float4 _Element_Tex_Pan_Slot3_Slot4;
			float4 _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W;
			float4 _Catalyst_Base_Noise_1_Remap;
			float4 _Catalyst_Base_Noise_2_Remap;
			float4 _Catalyst_Base_Remap;
			float4 _SecondaryColour_Catalyst;
			float4 _Element_Base_Remap;
			float4 _Element_Base_Noise_2_Remap;
			float4 _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W;
			float4 _SoftEgdeMask_Texture_ST;
			float4 _Cataylst_Tex_Pan_Slot3XY_Slot4ZW;
			float4 _NoiseScales_E1X_E2Y_C1Z_C2W;
			float4 _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW;
			float4 _Element_Base_Noise_1_Remap;
			float4 _BaseColour_Element;
			float4 _SecondaryColour_Element;
			float4 _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW;
			float4 _Element_Tex_Pan_Slot1_Slot2;
			float4 _BaseColour_Catalyst;
			float2 _Fres_ScaleX_PowerY_Speeds;
			float _Toggle_Catalyst_Noise_Mask;
			float _Toggle_UseCatalystTexture_2_With_Noise_2;
			float _ToggleCatalystFresColours_Base0_Secondary1;
			float _Toggle_UseCatalystTexture_1_With_Noise_1;
			float _Catalyst_Spec_StepValue;
			float _Catalyst_Specs_Intensity;
			float _Toggle_Flashing;
			float _Toggle_Cataylst_Spec_On0_Off1;
			float _Toggle_UseCatalystTex_3__4;
			float _ToggleCatalystTexColours_Base0_Secondary1;
			float _Toggle_UseElementTex_3__4;
			float _ToggleCatalystCull;
			float _Toggle_UseVertexNoise_Catalyst0_Element1;
			float _VertOffset_Intensity;
			float _ToggleElementCull;
			float _ToggleElementFade;
			float _ToggleElementTexColours_Base0_Secondary1;
			float _Toggle_UseElementTexture_1_With_Noise_1;
			float _Toggle_UseElementTexture_2_With_Noise_2;
			float _SoftEdgeFade;
			float _ToggleCatalystFade;
			float _Toggle_Element_Noise_Mask;
			float _Fres_Scale_MaxNew;
			float _Fres_Power_MinNew;
			float _Fres_Power_MaxNew;
			float _ToggleElementFresColours_Base0_Secondary1;
			float _ToggleOverallCull;
			float _Element_Spec_StepValue;
			float _Element_Specs_Intensity;
			float _Toggle_Element_Spec_On0_Off1;
			float _Fres_Scale_MinNew;
			float _ToggleOverallFade;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _ElementTexture_Slot_1;
			sampler2D _ElementTexture_Slot_2;
			sampler2D _SoftEgdeMask_Texture;
			sampler2D _ElementTexture_Slot_3;
			sampler2D _ElementTexture_Slot_4;
			sampler2D _CatalystTexture_Slot_1;
			sampler2D _CatalystTexture_Slot_2;
			sampler2D _CatalystTexture_Slot_3;
			sampler2D _CatalystTexture_Slot_4;


			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			
					float2 voronoihash305( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi305( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash305( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return (F2 + F1) * 0.5;
					}
			
					float2 voronoihash289( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi289( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash289( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
					float2 voronoihash313( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi313( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash313( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return (F2 + F1) * 0.5;
					}
			
					float2 voronoihash309( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi309( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash309( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float2 appendResult229 = (float2(_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 CatalystNoise1_Pan233 = appendResult229;
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float3 worldToObj216 = mul( GetWorldToObjectMatrix(), float4( ase_worldPos, 1 ) ).xyz;
				float2 panner224 = ( _TimeParameters.x * CatalystNoise1_Pan233 + worldToObj216.xy);
				float CatalystNoise_1_Scale237 = _NoiseScales_E1X_E2Y_C1Z_C2W.z;
				float simplePerlin3D226 = snoise( float3( panner224 ,  0.0 )*CatalystNoise_1_Scale237 );
				simplePerlin3D226 = simplePerlin3D226*0.5 + 0.5;
				float CatalystNoise_1227 = simplePerlin3D226;
				float2 appendResult208 = (float2(_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 ElementNoise1_Pan212 = appendResult208;
				float3 worldToObj46 = mul( GetWorldToObjectMatrix(), float4( ase_worldPos, 1 ) ).xyz;
				float2 panner52 = ( _TimeParameters.x * ElementNoise1_Pan212 + worldToObj46.xy);
				float ElementNoise_1_Scale235 = _NoiseScales_E1X_E2Y_C1Z_C2W.x;
				float simplePerlin3D59 = snoise( float3( panner52 ,  0.0 )*ElementNoise_1_Scale235 );
				simplePerlin3D59 = simplePerlin3D59*0.5 + 0.5;
				float ElementNoise_161 = simplePerlin3D59;
				float lerpResult468 = lerp( CatalystNoise_1227 , ElementNoise_161 , _Toggle_UseVertexNoise_Catalyst0_Element1);
				float3 VertexOffset10 = ( v.ase_normal * ( lerpResult468 * _VertOffset_Intensity ) );
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				o.ase_texcoord3.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = VertexOffset10;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				float3 normalWS = TransformObjectToWorldDir( v.ase_normal );

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;

				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float3 worldToObj49 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float temp_output_58_0 = ( worldToObj49.y * 0.88 );
				float smoothstepResult69 = smoothstep( _ToggleElementCull , _ToggleElementFade , temp_output_58_0);
				float Base_BottomCull72 = smoothstepResult69;
				float4 BaseColours_Elements19 = ( IN.ase_color * _BaseColour_Element );
				float4 SecondaryColours_Elements281 = ( IN.ase_color * _SecondaryColour_Element );
				float4 lerpResult329 = lerp( BaseColours_Elements19 , SecondaryColours_Elements281 , _ToggleElementTexColours_Base0_Secondary1);
				float4 Element_Base_Colour428 = ( Base_BottomCull72 * lerpResult329 );
				float2 appendResult208 = (float2(_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 ElementNoise1_Pan212 = appendResult208;
				float3 worldToObj46 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float2 panner52 = ( _TimeParameters.x * ElementNoise1_Pan212 + worldToObj46.xy);
				float ElementNoise_1_Scale235 = _NoiseScales_E1X_E2Y_C1Z_C2W.x;
				float simplePerlin3D59 = snoise( float3( panner52 ,  0.0 )*ElementNoise_1_Scale235 );
				simplePerlin3D59 = simplePerlin3D59*0.5 + 0.5;
				float ElementNoise_161 = simplePerlin3D59;
				float4 temp_cast_2 = (ElementNoise_161).xxxx;
				float2 appendResult167 = (float2(_Element_Tex_Pan_Slot1_Slot2.x , _Element_Tex_Pan_Slot1_Slot2.y));
				float2 ElementPan_Slot_1171 = appendResult167;
				float2 texCoord143 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner136 = ( 1.0 * _Time.y * ElementPan_Slot_1171 + texCoord143);
				float4 ElementTex_1207 = tex2D( _ElementTexture_Slot_1, panner136 );
				float4 lerpResult420 = lerp( temp_cast_2 , ( ElementTex_1207 * ElementNoise_161 ) , _Toggle_UseElementTexture_1_With_Noise_1);
				float4 temp_cast_3 = (_Element_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_4 = (_Element_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_5 = (_Element_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_6 = (_Element_Base_Noise_1_Remap.w).xxxx;
				float2 appendResult209 = (float2(_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.z , _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.w));
				float2 ElementNoise2_Pan213 = appendResult209;
				float2 panner163 = ( _TimeParameters.x * ElementNoise2_Pan213 + worldToObj46.xy);
				float ElementNoise_2_Scale236 = _NoiseScales_E1X_E2Y_C1Z_C2W.y;
				float simplePerlin3D165 = snoise( float3( panner163 ,  0.0 )*ElementNoise_2_Scale236 );
				simplePerlin3D165 = simplePerlin3D165*0.5 + 0.5;
				float ElementNoise_2166 = simplePerlin3D165;
				float4 temp_cast_9 = (ElementNoise_2166).xxxx;
				float2 appendResult168 = (float2(_Element_Tex_Pan_Slot1_Slot2.z , _Element_Tex_Pan_Slot1_Slot2.w));
				float2 ElementPan_Slot_2172 = appendResult168;
				float2 panner137 = ( 1.0 * _Time.y * ElementPan_Slot_2172 + texCoord143);
				float4 ElementTex_2206 = tex2D( _ElementTexture_Slot_2, panner137 );
				float4 lerpResult412 = lerp( temp_cast_9 , ( ElementTex_2206 * ElementNoise_2166 ) , _Toggle_UseElementTexture_2_With_Noise_2);
				float4 temp_cast_10 = (_Element_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_11 = (_Element_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_12 = (_Element_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_13 = (_Element_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_14 = (_Element_Base_Remap.x).xxxx;
				float4 temp_cast_15 = (_Element_Base_Remap.y).xxxx;
				float4 temp_cast_16 = (_Element_Base_Remap.z).xxxx;
				float4 temp_cast_17 = (_Element_Base_Remap.w).xxxx;
				float4 temp_output_403_0 = (temp_cast_16 + (( (temp_cast_5 + (lerpResult420 - temp_cast_3) * (temp_cast_6 - temp_cast_5) / (temp_cast_4 - temp_cast_3)) * (temp_cast_12 + (lerpResult412 - temp_cast_10) * (temp_cast_13 - temp_cast_12) / (temp_cast_11 - temp_cast_10)) ) - temp_cast_14) * (temp_cast_17 - temp_cast_16) / (temp_cast_15 - temp_cast_14));
				float4 temp_cast_18 = (1.0).xxxx;
				float4 temp_cast_19 = (_Element_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_20 = (_Element_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_21 = (_Element_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_22 = (_Element_Base_Noise_1_Remap.w).xxxx;
				float4 temp_cast_23 = (_Element_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_24 = (_Element_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_25 = (_Element_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_26 = (_Element_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_27 = (_Element_Base_Remap.x).xxxx;
				float4 temp_cast_28 = (_Element_Base_Remap.y).xxxx;
				float4 temp_cast_29 = (_Element_Base_Remap.z).xxxx;
				float4 temp_cast_30 = (_Element_Base_Remap.w).xxxx;
				float2 uv_SoftEgdeMask_Texture = IN.ase_texcoord2.xy * _SoftEgdeMask_Texture_ST.xy + _SoftEgdeMask_Texture_ST.zw;
				float SoftEdgeMask249 = ( tex2D( _SoftEgdeMask_Texture, uv_SoftEgdeMask_Texture ).r * _SoftEdgeFade );
				float4 lerpResult407 = lerp( temp_cast_18 , ( temp_output_403_0 * SoftEdgeMask249 ) , _Toggle_Element_Noise_Mask);
				float4 Element_Base424 = ( ( temp_output_403_0 * lerpResult407 ) * 1.0 );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord3.xyz;
				float fresnelNdotV53 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode53 = ( 0.0 + (_Fres_Scale_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.x ) ) ) - 0.0) * (_Fres_Scale_MaxNew - _Fres_Scale_MinNew) / (10.0 - 0.0)) * pow( 1.0 - fresnelNdotV53, (_Fres_Power_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.y ) ) ) - 0.0) * (_Fres_Power_MaxNew - _Fres_Power_MinNew) / (1.0 - 0.0)) ) );
				float Fresnel_Pulse67 = fresnelNode53;
				float Fresnel_BottomCull106 = ( Base_BottomCull72 * Fresnel_Pulse67 );
				float4 lerpResult354 = lerp( BaseColours_Elements19 , SecondaryColours_Elements281 , _ToggleElementFresColours_Base0_Secondary1);
				float4 Element_Fresnel_Colour429 = ( Fresnel_BottomCull106 * lerpResult354 );
				float ElementVoronoi_Scale_1301 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.z;
				float ElementVoronoi_Speed_1299 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.x;
				float time305 = ( _TimeParameters.x * ElementVoronoi_Speed_1299 );
				float2 voronoiSmoothId305 = 0;
				float2 texCoord284 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 coords305 = texCoord284 * ElementVoronoi_Scale_1301;
				float2 id305 = 0;
				float2 uv305 = 0;
				float fade305 = 0.5;
				float voroi305 = 0;
				float rest305 = 0;
				for( int it305 = 0; it305 <8; it305++ ){
				voroi305 += fade305 * voronoi305( coords305, time305, id305, uv305, 0,voronoiSmoothId305 );
				rest305 += fade305;
				coords305 *= 2;
				fade305 *= 0.5;
				}//Voronoi305
				voroi305 /= rest305;
				float ElementVoronoi_2328 = voroi305;
				float4 temp_cast_31 = (0.0).xxxx;
				float2 appendResult169 = (float2(_Element_Tex_Pan_Slot3_Slot4.x , _Element_Tex_Pan_Slot3_Slot4.y));
				float2 ElementPan_Slot_3173 = appendResult169;
				float2 panner138 = ( 1.0 * _Time.y * ElementPan_Slot_3173 + texCoord143);
				float4 ElementTex_3204 = tex2D( _ElementTexture_Slot_3, panner138 );
				float2 appendResult170 = (float2(_Element_Tex_Pan_Slot3_Slot4.z , _Element_Tex_Pan_Slot3_Slot4.w));
				float2 ElementPan_Slot_4174 = appendResult170;
				float2 panner139 = ( 1.0 * _Time.y * ElementPan_Slot_4174 + texCoord143);
				float4 ElementTex_4205 = tex2D( _ElementTexture_Slot_4, panner139 );
				float4 lerpResult457 = lerp( temp_cast_31 , ( ( ElementTex_3204 + ElementTex_4205 ) * (Element_Base_Colour428).rgba ) , _Toggle_UseElementTex_3__4);
				float temp_output_3_0_g25 = ( ElementNoise_2166 - ElementNoise_161 );
				float temp_output_261_0 = ( (_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z + (ElementNoise_161 - _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x) * (_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.w - _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z) / (_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.y - _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x)) * saturate( ( temp_output_3_0_g25 / fwidth( temp_output_3_0_g25 ) ) ) );
				float ElementVoronoi_Scale_2302 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.w;
				float ElementVoronoi_Speed_2300 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.y;
				float time289 = ( _TimeParameters.x * ElementVoronoi_Speed_2300 );
				float2 voronoiSmoothId289 = 0;
				float2 coords289 = texCoord284 * ElementVoronoi_Scale_2302;
				float2 id289 = 0;
				float2 uv289 = 0;
				float fade289 = 0.5;
				float voroi289 = 0;
				float rest289 = 0;
				for( int it289 = 0; it289 <2; it289++ ){
				voroi289 += fade289 * voronoi289( coords289, time289, id289, uv289, 0,voronoiSmoothId289 );
				rest289 += fade289;
				coords289 *= 2;
				fade289 *= 0.5;
				}//Voronoi289
				voroi289 /= rest289;
				float ElementVoronoi_1327 = voroi289;
				float temp_output_3_0_g24 = ( _Element_Spec_StepValue - ElementVoronoi_1327 );
				float lerpResult505 = lerp( 1.0 , 0.0 , _Toggle_Element_Spec_On0_Off1);
				float ElementBrightParts266 = ( ( ( ( temp_output_261_0 * ( temp_output_261_0 * ( saturate( ( temp_output_3_0_g24 / fwidth( temp_output_3_0_g24 ) ) ) * SoftEdgeMask249 ) ) ) * _Element_Specs_Intensity ) * Base_BottomCull72 ) * lerpResult505 );
				float smoothstepResult68 = smoothstep( _ToggleCatalystCull , _ToggleCatalystFade , ( 1.0 - temp_output_58_0 ));
				float Base_TopCull71 = smoothstepResult68;
				float4 BaseColours_Cataylst30 = ( IN.ase_color * _BaseColour_Catalyst );
				float4 SecondaryColours_Cataylst282 = ( IN.ase_color * _SecondaryColour_Catalyst );
				float4 lerpResult335 = lerp( BaseColours_Cataylst30 , SecondaryColours_Cataylst282 , _ToggleCatalystTexColours_Base0_Secondary1);
				float4 Catalyst_Base_Colour430 = ( Base_TopCull71 * lerpResult335 );
				float2 appendResult229 = (float2(_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 CatalystNoise1_Pan233 = appendResult229;
				float3 worldToObj216 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float2 panner224 = ( _TimeParameters.x * CatalystNoise1_Pan233 + worldToObj216.xy);
				float CatalystNoise_1_Scale237 = _NoiseScales_E1X_E2Y_C1Z_C2W.z;
				float simplePerlin3D226 = snoise( float3( panner224 ,  0.0 )*CatalystNoise_1_Scale237 );
				simplePerlin3D226 = simplePerlin3D226*0.5 + 0.5;
				float CatalystNoise_1227 = simplePerlin3D226;
				float4 temp_cast_34 = (CatalystNoise_1227).xxxx;
				float2 appendResult140 = (float2(_Cataylst_Tex_Pan_Slot1XY_Slot2ZW.x , _Cataylst_Tex_Pan_Slot1XY_Slot2ZW.y));
				float2 CatalystPan_Slot_1148 = appendResult140;
				float2 texCoord191 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner196 = ( 1.0 * _Time.y * CatalystPan_Slot_1148 + texCoord191);
				float lerpResult497 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.x ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnX125 = lerpResult497;
				float4 CatalystTex_1200 = ( tex2D( _CatalystTexture_Slot_1, panner196 ) * FlashingOnX125 );
				float4 lerpResult387 = lerp( temp_cast_34 , ( CatalystTex_1200 * CatalystNoise_1227 ) , _Toggle_UseCatalystTexture_1_With_Noise_1);
				float4 temp_cast_35 = (_Catalyst_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_36 = (_Catalyst_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_37 = (_Catalyst_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_38 = (_Catalyst_Base_Noise_1_Remap.w).xxxx;
				float2 appendResult231 = (float2(_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.z , _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.w));
				float2 CatalystNoise2_Pan232 = appendResult231;
				float2 panner222 = ( _TimeParameters.x * CatalystNoise2_Pan232 + worldToObj216.xy);
				float CatalystNoise_2_Scale238 = _NoiseScales_E1X_E2Y_C1Z_C2W.w;
				float simplePerlin3D225 = snoise( float3( panner222 ,  0.0 )*CatalystNoise_2_Scale238 );
				simplePerlin3D225 = simplePerlin3D225*0.5 + 0.5;
				float CatalystNoise_2228 = simplePerlin3D225;
				float4 temp_cast_41 = (CatalystNoise_2228).xxxx;
				float2 appendResult141 = (float2(_Cataylst_Tex_Pan_Slot1XY_Slot2ZW.z , _Cataylst_Tex_Pan_Slot1XY_Slot2ZW.w));
				float2 CatalystPan_Slot_2149 = appendResult141;
				float2 panner197 = ( 1.0 * _Time.y * CatalystPan_Slot_2149 + texCoord191);
				float lerpResult496 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.y ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnY124 = lerpResult496;
				float4 CatalystTex_2201 = ( tex2D( _CatalystTexture_Slot_2, panner197 ) * FlashingOnY124 );
				float4 lerpResult394 = lerp( temp_cast_41 , ( CatalystTex_2201 * CatalystNoise_2228 ) , _Toggle_UseCatalystTexture_2_With_Noise_2);
				float4 temp_cast_42 = (_Catalyst_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_43 = (_Catalyst_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_44 = (_Catalyst_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_45 = (_Catalyst_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_46 = (_Catalyst_Base_Remap.x).xxxx;
				float4 temp_cast_47 = (_Catalyst_Base_Remap.y).xxxx;
				float4 temp_cast_48 = (_Catalyst_Base_Remap.z).xxxx;
				float4 temp_cast_49 = (_Catalyst_Base_Remap.w).xxxx;
				float4 temp_output_371_0 = (temp_cast_48 + (( (temp_cast_37 + (lerpResult387 - temp_cast_35) * (temp_cast_38 - temp_cast_37) / (temp_cast_36 - temp_cast_35)) * (temp_cast_44 + (lerpResult394 - temp_cast_42) * (temp_cast_45 - temp_cast_44) / (temp_cast_43 - temp_cast_42)) ) - temp_cast_46) * (temp_cast_49 - temp_cast_48) / (temp_cast_47 - temp_cast_46));
				float4 temp_cast_50 = (1.0).xxxx;
				float4 temp_cast_51 = (_Catalyst_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_52 = (_Catalyst_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_53 = (_Catalyst_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_54 = (_Catalyst_Base_Noise_1_Remap.w).xxxx;
				float4 temp_cast_55 = (_Catalyst_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_56 = (_Catalyst_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_57 = (_Catalyst_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_58 = (_Catalyst_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_59 = (_Catalyst_Base_Remap.x).xxxx;
				float4 temp_cast_60 = (_Catalyst_Base_Remap.y).xxxx;
				float4 temp_cast_61 = (_Catalyst_Base_Remap.z).xxxx;
				float4 temp_cast_62 = (_Catalyst_Base_Remap.w).xxxx;
				float4 lerpResult378 = lerp( temp_cast_50 , ( temp_output_371_0 * SoftEdgeMask249 ) , _Toggle_Catalyst_Noise_Mask);
				float4 Catalyst_Base398 = ( ( temp_output_371_0 * lerpResult378 ) * 1.0 );
				float Fresnel_TopCull105 = ( Base_TopCull71 * Fresnel_Pulse67 );
				float4 lerpResult364 = lerp( BaseColours_Cataylst30 , SecondaryColours_Cataylst282 , _ToggleCatalystFresColours_Base0_Secondary1);
				float4 Catalyst_Fresnel_Colour431 = ( Fresnel_TopCull105 * lerpResult364 );
				float CatalystVoronoi_Scale_2296 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.w;
				float CatalystVoronoi_Speed_2294 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.y;
				float time313 = ( _TimeParameters.x * CatalystVoronoi_Speed_2294 );
				float2 voronoiSmoothId313 = 0;
				float2 texCoord315 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 coords313 = texCoord315 * CatalystVoronoi_Scale_2296;
				float2 id313 = 0;
				float2 uv313 = 0;
				float fade313 = 0.5;
				float voroi313 = 0;
				float rest313 = 0;
				for( int it313 = 0; it313 <8; it313++ ){
				voroi313 += fade313 * voronoi313( coords313, time313, id313, uv313, 0,voronoiSmoothId313 );
				rest313 += fade313;
				coords313 *= 2;
				fade313 *= 0.5;
				}//Voronoi313
				voroi313 /= rest313;
				float CatalystVoronoi_2326 = voroi313;
				float4 temp_cast_63 = (0.0).xxxx;
				float2 appendResult146 = (float2(_Cataylst_Tex_Pan_Slot3XY_Slot4ZW.x , _Cataylst_Tex_Pan_Slot3XY_Slot4ZW.y));
				float2 CatalystPan_Slot_3150 = appendResult146;
				float2 panner198 = ( 1.0 * _Time.y * CatalystPan_Slot_3150 + texCoord191);
				float lerpResult495 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.z ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnZ126 = lerpResult495;
				float4 CatalystTex_3202 = ( tex2D( _CatalystTexture_Slot_3, panner198 ) * FlashingOnZ126 );
				float2 appendResult147 = (float2(_Cataylst_Tex_Pan_Slot3XY_Slot4ZW.z , _Cataylst_Tex_Pan_Slot3XY_Slot4ZW.w));
				float2 CataylstPan_Slot_4151 = appendResult147;
				float2 panner199 = ( 1.0 * _Time.y * CataylstPan_Slot_4151 + texCoord191);
				float lerpResult493 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.w ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnW181 = lerpResult493;
				float4 CatalystTex_4203 = ( tex2D( _CatalystTexture_Slot_4, panner199 ) * FlashingOnW181 );
				float4 lerpResult452 = lerp( temp_cast_63 , ( ( CatalystTex_3202 + CatalystTex_4203 ) * (Catalyst_Base_Colour430).rgba ) , _Toggle_UseCatalystTex_3__4);
				float temp_output_3_0_g23 = ( CatalystNoise_2228 - CatalystNoise_1227 );
				float temp_output_338_0 = ( (_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z + (CatalystNoise_1227 - _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x) * (_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.w - _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z) / (_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.y - _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x)) * saturate( ( temp_output_3_0_g23 / fwidth( temp_output_3_0_g23 ) ) ) );
				float CatalystVoronoi_Scale_1295 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.z;
				float CatalystVoronoi_Speed_1293 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.x;
				float time309 = ( _TimeParameters.x * CatalystVoronoi_Speed_1293 );
				float2 voronoiSmoothId309 = 0;
				float2 coords309 = texCoord315 * CatalystVoronoi_Scale_1295;
				float2 id309 = 0;
				float2 uv309 = 0;
				float fade309 = 0.5;
				float voroi309 = 0;
				float rest309 = 0;
				for( int it309 = 0; it309 <2; it309++ ){
				voroi309 += fade309 * voronoi309( coords309, time309, id309, uv309, 0,voronoiSmoothId309 );
				rest309 += fade309;
				coords309 *= 2;
				fade309 *= 0.5;
				}//Voronoi309
				voroi309 /= rest309;
				float CatalystVoronoi_1325 = voroi309;
				float temp_output_3_0_g22 = ( _Catalyst_Spec_StepValue - CatalystVoronoi_1325 );
				float lerpResult509 = lerp( 1.0 , 0.0 , _Toggle_Cataylst_Spec_On0_Off1);
				float CatalystBrightParts348 = ( ( ( ( temp_output_338_0 * ( temp_output_338_0 * ( saturate( ( temp_output_3_0_g22 / fwidth( temp_output_3_0_g22 ) ) ) * SoftEdgeMask249 ) ) ) * _Catalyst_Specs_Intensity ) * Base_TopCull71 ) * lerpResult509 );
				float4 temp_output_445_0 = ( ( ( ( Element_Base_Colour428 * Element_Base424 ) + ( Element_Fresnel_Colour429 * ElementVoronoi_2328 ) ) + lerpResult457 + ( Element_Base_Colour428 * ElementBrightParts266 ) ) + ( ( ( Catalyst_Base_Colour430 * Catalyst_Base398 ) + ( Catalyst_Fresnel_Colour431 * CatalystVoronoi_2326 ) ) + lerpResult452 + ( Catalyst_Base_Colour430 * CatalystBrightParts348 ) ) );
				float grayscale462 = (temp_output_445_0.rgb.r + temp_output_445_0.rgb.g + temp_output_445_0.rgb.b) / 3;
				float VertexAlpha297 = IN.ase_color.a;
				float3 worldToObj92 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float smoothstepResult85 = smoothstep( _ToggleOverallCull , ( _ToggleOverallCull + _ToggleOverallFade ) , ( ElementNoise_161 + ( 1.0 - (0.0 + (worldToObj92.y - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) ) ));
				float DisolveAlpha93 = smoothstepResult85;
				float Alpha9 = ( ( (0.01 + (grayscale462 - 0.0) * (4.0 - 0.01) / (1.0 - 0.0)) * VertexAlpha297 ) * DisolveAlpha93 );
				
				float Alpha = Alpha9;
				float AlphaClipThreshold = 0.5;
				float AlphaClipThresholdShadow = 0.5;

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define ASE_SRP_VERSION 70701

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_COLOR


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_color : COLOR;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW;
			float4 _Cataylst_Tex_Pan_Slot1XY_Slot2ZW;
			float4 _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W;
			float4 _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW;
			float4 _Element_Tex_Pan_Slot3_Slot4;
			float4 _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W;
			float4 _Catalyst_Base_Noise_1_Remap;
			float4 _Catalyst_Base_Noise_2_Remap;
			float4 _Catalyst_Base_Remap;
			float4 _SecondaryColour_Catalyst;
			float4 _Element_Base_Remap;
			float4 _Element_Base_Noise_2_Remap;
			float4 _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W;
			float4 _SoftEgdeMask_Texture_ST;
			float4 _Cataylst_Tex_Pan_Slot3XY_Slot4ZW;
			float4 _NoiseScales_E1X_E2Y_C1Z_C2W;
			float4 _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW;
			float4 _Element_Base_Noise_1_Remap;
			float4 _BaseColour_Element;
			float4 _SecondaryColour_Element;
			float4 _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW;
			float4 _Element_Tex_Pan_Slot1_Slot2;
			float4 _BaseColour_Catalyst;
			float2 _Fres_ScaleX_PowerY_Speeds;
			float _Toggle_Catalyst_Noise_Mask;
			float _Toggle_UseCatalystTexture_2_With_Noise_2;
			float _ToggleCatalystFresColours_Base0_Secondary1;
			float _Toggle_UseCatalystTexture_1_With_Noise_1;
			float _Catalyst_Spec_StepValue;
			float _Catalyst_Specs_Intensity;
			float _Toggle_Flashing;
			float _Toggle_Cataylst_Spec_On0_Off1;
			float _Toggle_UseCatalystTex_3__4;
			float _ToggleCatalystTexColours_Base0_Secondary1;
			float _Toggle_UseElementTex_3__4;
			float _ToggleCatalystCull;
			float _Toggle_UseVertexNoise_Catalyst0_Element1;
			float _VertOffset_Intensity;
			float _ToggleElementCull;
			float _ToggleElementFade;
			float _ToggleElementTexColours_Base0_Secondary1;
			float _Toggle_UseElementTexture_1_With_Noise_1;
			float _Toggle_UseElementTexture_2_With_Noise_2;
			float _SoftEdgeFade;
			float _ToggleCatalystFade;
			float _Toggle_Element_Noise_Mask;
			float _Fres_Scale_MaxNew;
			float _Fres_Power_MinNew;
			float _Fres_Power_MaxNew;
			float _ToggleElementFresColours_Base0_Secondary1;
			float _ToggleOverallCull;
			float _Element_Spec_StepValue;
			float _Element_Specs_Intensity;
			float _Toggle_Element_Spec_On0_Off1;
			float _Fres_Scale_MinNew;
			float _ToggleOverallFade;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _ElementTexture_Slot_1;
			sampler2D _ElementTexture_Slot_2;
			sampler2D _SoftEgdeMask_Texture;
			sampler2D _ElementTexture_Slot_3;
			sampler2D _ElementTexture_Slot_4;
			sampler2D _CatalystTexture_Slot_1;
			sampler2D _CatalystTexture_Slot_2;
			sampler2D _CatalystTexture_Slot_3;
			sampler2D _CatalystTexture_Slot_4;


			float3 mod3D289( float3 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 mod3D289( float4 x ) { return x - floor( x / 289.0 ) * 289.0; }
			float4 permute( float4 x ) { return mod3D289( ( x * 34.0 + 1.0 ) * x ); }
			float4 taylorInvSqrt( float4 r ) { return 1.79284291400159 - r * 0.85373472095314; }
			float snoise( float3 v )
			{
				const float2 C = float2( 1.0 / 6.0, 1.0 / 3.0 );
				float3 i = floor( v + dot( v, C.yyy ) );
				float3 x0 = v - i + dot( i, C.xxx );
				float3 g = step( x0.yzx, x0.xyz );
				float3 l = 1.0 - g;
				float3 i1 = min( g.xyz, l.zxy );
				float3 i2 = max( g.xyz, l.zxy );
				float3 x1 = x0 - i1 + C.xxx;
				float3 x2 = x0 - i2 + C.yyy;
				float3 x3 = x0 - 0.5;
				i = mod3D289( i);
				float4 p = permute( permute( permute( i.z + float4( 0.0, i1.z, i2.z, 1.0 ) ) + i.y + float4( 0.0, i1.y, i2.y, 1.0 ) ) + i.x + float4( 0.0, i1.x, i2.x, 1.0 ) );
				float4 j = p - 49.0 * floor( p / 49.0 );  // mod(p,7*7)
				float4 x_ = floor( j / 7.0 );
				float4 y_ = floor( j - 7.0 * x_ );  // mod(j,N)
				float4 x = ( x_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 y = ( y_ * 2.0 + 0.5 ) / 7.0 - 1.0;
				float4 h = 1.0 - abs( x ) - abs( y );
				float4 b0 = float4( x.xy, y.xy );
				float4 b1 = float4( x.zw, y.zw );
				float4 s0 = floor( b0 ) * 2.0 + 1.0;
				float4 s1 = floor( b1 ) * 2.0 + 1.0;
				float4 sh = -step( h, 0.0 );
				float4 a0 = b0.xzyw + s0.xzyw * sh.xxyy;
				float4 a1 = b1.xzyw + s1.xzyw * sh.zzww;
				float3 g0 = float3( a0.xy, h.x );
				float3 g1 = float3( a0.zw, h.y );
				float3 g2 = float3( a1.xy, h.z );
				float3 g3 = float3( a1.zw, h.w );
				float4 norm = taylorInvSqrt( float4( dot( g0, g0 ), dot( g1, g1 ), dot( g2, g2 ), dot( g3, g3 ) ) );
				g0 *= norm.x;
				g1 *= norm.y;
				g2 *= norm.z;
				g3 *= norm.w;
				float4 m = max( 0.6 - float4( dot( x0, x0 ), dot( x1, x1 ), dot( x2, x2 ), dot( x3, x3 ) ), 0.0 );
				m = m* m;
				m = m* m;
				float4 px = float4( dot( x0, g0 ), dot( x1, g1 ), dot( x2, g2 ), dot( x3, g3 ) );
				return 42.0 * dot( m, px);
			}
			
					float2 voronoihash305( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi305( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash305( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return (F2 + F1) * 0.5;
					}
			
					float2 voronoihash289( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi289( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash289( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			
					float2 voronoihash313( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi313( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash313( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return (F2 + F1) * 0.5;
					}
			
					float2 voronoihash309( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi309( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -1; j <= 1; j++ )
						{
							for ( int i = -1; i <= 1; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash309( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.5 * dot( r, r );
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F1;
					}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 appendResult229 = (float2(_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 CatalystNoise1_Pan233 = appendResult229;
				float3 ase_worldPos = mul(GetObjectToWorldMatrix(), v.vertex).xyz;
				float3 worldToObj216 = mul( GetWorldToObjectMatrix(), float4( ase_worldPos, 1 ) ).xyz;
				float2 panner224 = ( _TimeParameters.x * CatalystNoise1_Pan233 + worldToObj216.xy);
				float CatalystNoise_1_Scale237 = _NoiseScales_E1X_E2Y_C1Z_C2W.z;
				float simplePerlin3D226 = snoise( float3( panner224 ,  0.0 )*CatalystNoise_1_Scale237 );
				simplePerlin3D226 = simplePerlin3D226*0.5 + 0.5;
				float CatalystNoise_1227 = simplePerlin3D226;
				float2 appendResult208 = (float2(_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 ElementNoise1_Pan212 = appendResult208;
				float3 worldToObj46 = mul( GetWorldToObjectMatrix(), float4( ase_worldPos, 1 ) ).xyz;
				float2 panner52 = ( _TimeParameters.x * ElementNoise1_Pan212 + worldToObj46.xy);
				float ElementNoise_1_Scale235 = _NoiseScales_E1X_E2Y_C1Z_C2W.x;
				float simplePerlin3D59 = snoise( float3( panner52 ,  0.0 )*ElementNoise_1_Scale235 );
				simplePerlin3D59 = simplePerlin3D59*0.5 + 0.5;
				float ElementNoise_161 = simplePerlin3D59;
				float lerpResult468 = lerp( CatalystNoise_1227 , ElementNoise_161 , _Toggle_UseVertexNoise_Catalyst0_Element1);
				float3 VertexOffset10 = ( v.ase_normal * ( lerpResult468 * _VertOffset_Intensity ) );
				
				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				o.ase_texcoord3.w = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = VertexOffset10;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = TransformWorldToHClip( positionWS );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_color = v.ase_color;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float3 worldToObj49 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float temp_output_58_0 = ( worldToObj49.y * 0.88 );
				float smoothstepResult69 = smoothstep( _ToggleElementCull , _ToggleElementFade , temp_output_58_0);
				float Base_BottomCull72 = smoothstepResult69;
				float4 BaseColours_Elements19 = ( IN.ase_color * _BaseColour_Element );
				float4 SecondaryColours_Elements281 = ( IN.ase_color * _SecondaryColour_Element );
				float4 lerpResult329 = lerp( BaseColours_Elements19 , SecondaryColours_Elements281 , _ToggleElementTexColours_Base0_Secondary1);
				float4 Element_Base_Colour428 = ( Base_BottomCull72 * lerpResult329 );
				float2 appendResult208 = (float2(_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 ElementNoise1_Pan212 = appendResult208;
				float3 worldToObj46 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float2 panner52 = ( _TimeParameters.x * ElementNoise1_Pan212 + worldToObj46.xy);
				float ElementNoise_1_Scale235 = _NoiseScales_E1X_E2Y_C1Z_C2W.x;
				float simplePerlin3D59 = snoise( float3( panner52 ,  0.0 )*ElementNoise_1_Scale235 );
				simplePerlin3D59 = simplePerlin3D59*0.5 + 0.5;
				float ElementNoise_161 = simplePerlin3D59;
				float4 temp_cast_2 = (ElementNoise_161).xxxx;
				float2 appendResult167 = (float2(_Element_Tex_Pan_Slot1_Slot2.x , _Element_Tex_Pan_Slot1_Slot2.y));
				float2 ElementPan_Slot_1171 = appendResult167;
				float2 texCoord143 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner136 = ( 1.0 * _Time.y * ElementPan_Slot_1171 + texCoord143);
				float4 ElementTex_1207 = tex2D( _ElementTexture_Slot_1, panner136 );
				float4 lerpResult420 = lerp( temp_cast_2 , ( ElementTex_1207 * ElementNoise_161 ) , _Toggle_UseElementTexture_1_With_Noise_1);
				float4 temp_cast_3 = (_Element_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_4 = (_Element_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_5 = (_Element_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_6 = (_Element_Base_Noise_1_Remap.w).xxxx;
				float2 appendResult209 = (float2(_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.z , _ElementNoise_Pan_Speed_Noise1XY_Noise2ZW.w));
				float2 ElementNoise2_Pan213 = appendResult209;
				float2 panner163 = ( _TimeParameters.x * ElementNoise2_Pan213 + worldToObj46.xy);
				float ElementNoise_2_Scale236 = _NoiseScales_E1X_E2Y_C1Z_C2W.y;
				float simplePerlin3D165 = snoise( float3( panner163 ,  0.0 )*ElementNoise_2_Scale236 );
				simplePerlin3D165 = simplePerlin3D165*0.5 + 0.5;
				float ElementNoise_2166 = simplePerlin3D165;
				float4 temp_cast_9 = (ElementNoise_2166).xxxx;
				float2 appendResult168 = (float2(_Element_Tex_Pan_Slot1_Slot2.z , _Element_Tex_Pan_Slot1_Slot2.w));
				float2 ElementPan_Slot_2172 = appendResult168;
				float2 panner137 = ( 1.0 * _Time.y * ElementPan_Slot_2172 + texCoord143);
				float4 ElementTex_2206 = tex2D( _ElementTexture_Slot_2, panner137 );
				float4 lerpResult412 = lerp( temp_cast_9 , ( ElementTex_2206 * ElementNoise_2166 ) , _Toggle_UseElementTexture_2_With_Noise_2);
				float4 temp_cast_10 = (_Element_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_11 = (_Element_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_12 = (_Element_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_13 = (_Element_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_14 = (_Element_Base_Remap.x).xxxx;
				float4 temp_cast_15 = (_Element_Base_Remap.y).xxxx;
				float4 temp_cast_16 = (_Element_Base_Remap.z).xxxx;
				float4 temp_cast_17 = (_Element_Base_Remap.w).xxxx;
				float4 temp_output_403_0 = (temp_cast_16 + (( (temp_cast_5 + (lerpResult420 - temp_cast_3) * (temp_cast_6 - temp_cast_5) / (temp_cast_4 - temp_cast_3)) * (temp_cast_12 + (lerpResult412 - temp_cast_10) * (temp_cast_13 - temp_cast_12) / (temp_cast_11 - temp_cast_10)) ) - temp_cast_14) * (temp_cast_17 - temp_cast_16) / (temp_cast_15 - temp_cast_14));
				float4 temp_cast_18 = (1.0).xxxx;
				float4 temp_cast_19 = (_Element_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_20 = (_Element_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_21 = (_Element_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_22 = (_Element_Base_Noise_1_Remap.w).xxxx;
				float4 temp_cast_23 = (_Element_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_24 = (_Element_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_25 = (_Element_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_26 = (_Element_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_27 = (_Element_Base_Remap.x).xxxx;
				float4 temp_cast_28 = (_Element_Base_Remap.y).xxxx;
				float4 temp_cast_29 = (_Element_Base_Remap.z).xxxx;
				float4 temp_cast_30 = (_Element_Base_Remap.w).xxxx;
				float2 uv_SoftEgdeMask_Texture = IN.ase_texcoord2.xy * _SoftEgdeMask_Texture_ST.xy + _SoftEgdeMask_Texture_ST.zw;
				float SoftEdgeMask249 = ( tex2D( _SoftEgdeMask_Texture, uv_SoftEgdeMask_Texture ).r * _SoftEdgeFade );
				float4 lerpResult407 = lerp( temp_cast_18 , ( temp_output_403_0 * SoftEdgeMask249 ) , _Toggle_Element_Noise_Mask);
				float4 Element_Base424 = ( ( temp_output_403_0 * lerpResult407 ) * 1.0 );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord3.xyz;
				float fresnelNdotV53 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode53 = ( 0.0 + (_Fres_Scale_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.x ) ) ) - 0.0) * (_Fres_Scale_MaxNew - _Fres_Scale_MinNew) / (10.0 - 0.0)) * pow( 1.0 - fresnelNdotV53, (_Fres_Power_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.y ) ) ) - 0.0) * (_Fres_Power_MaxNew - _Fres_Power_MinNew) / (1.0 - 0.0)) ) );
				float Fresnel_Pulse67 = fresnelNode53;
				float Fresnel_BottomCull106 = ( Base_BottomCull72 * Fresnel_Pulse67 );
				float4 lerpResult354 = lerp( BaseColours_Elements19 , SecondaryColours_Elements281 , _ToggleElementFresColours_Base0_Secondary1);
				float4 Element_Fresnel_Colour429 = ( Fresnel_BottomCull106 * lerpResult354 );
				float ElementVoronoi_Scale_1301 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.z;
				float ElementVoronoi_Speed_1299 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.x;
				float time305 = ( _TimeParameters.x * ElementVoronoi_Speed_1299 );
				float2 voronoiSmoothId305 = 0;
				float2 texCoord284 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 coords305 = texCoord284 * ElementVoronoi_Scale_1301;
				float2 id305 = 0;
				float2 uv305 = 0;
				float fade305 = 0.5;
				float voroi305 = 0;
				float rest305 = 0;
				for( int it305 = 0; it305 <8; it305++ ){
				voroi305 += fade305 * voronoi305( coords305, time305, id305, uv305, 0,voronoiSmoothId305 );
				rest305 += fade305;
				coords305 *= 2;
				fade305 *= 0.5;
				}//Voronoi305
				voroi305 /= rest305;
				float ElementVoronoi_2328 = voroi305;
				float4 temp_cast_31 = (0.0).xxxx;
				float2 appendResult169 = (float2(_Element_Tex_Pan_Slot3_Slot4.x , _Element_Tex_Pan_Slot3_Slot4.y));
				float2 ElementPan_Slot_3173 = appendResult169;
				float2 panner138 = ( 1.0 * _Time.y * ElementPan_Slot_3173 + texCoord143);
				float4 ElementTex_3204 = tex2D( _ElementTexture_Slot_3, panner138 );
				float2 appendResult170 = (float2(_Element_Tex_Pan_Slot3_Slot4.z , _Element_Tex_Pan_Slot3_Slot4.w));
				float2 ElementPan_Slot_4174 = appendResult170;
				float2 panner139 = ( 1.0 * _Time.y * ElementPan_Slot_4174 + texCoord143);
				float4 ElementTex_4205 = tex2D( _ElementTexture_Slot_4, panner139 );
				float4 lerpResult457 = lerp( temp_cast_31 , ( ( ElementTex_3204 + ElementTex_4205 ) * (Element_Base_Colour428).rgba ) , _Toggle_UseElementTex_3__4);
				float temp_output_3_0_g25 = ( ElementNoise_2166 - ElementNoise_161 );
				float temp_output_261_0 = ( (_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z + (ElementNoise_161 - _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x) * (_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.w - _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z) / (_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.y - _Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x)) * saturate( ( temp_output_3_0_g25 / fwidth( temp_output_3_0_g25 ) ) ) );
				float ElementVoronoi_Scale_2302 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.w;
				float ElementVoronoi_Speed_2300 = _ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.y;
				float time289 = ( _TimeParameters.x * ElementVoronoi_Speed_2300 );
				float2 voronoiSmoothId289 = 0;
				float2 coords289 = texCoord284 * ElementVoronoi_Scale_2302;
				float2 id289 = 0;
				float2 uv289 = 0;
				float fade289 = 0.5;
				float voroi289 = 0;
				float rest289 = 0;
				for( int it289 = 0; it289 <2; it289++ ){
				voroi289 += fade289 * voronoi289( coords289, time289, id289, uv289, 0,voronoiSmoothId289 );
				rest289 += fade289;
				coords289 *= 2;
				fade289 *= 0.5;
				}//Voronoi289
				voroi289 /= rest289;
				float ElementVoronoi_1327 = voroi289;
				float temp_output_3_0_g24 = ( _Element_Spec_StepValue - ElementVoronoi_1327 );
				float lerpResult505 = lerp( 1.0 , 0.0 , _Toggle_Element_Spec_On0_Off1);
				float ElementBrightParts266 = ( ( ( ( temp_output_261_0 * ( temp_output_261_0 * ( saturate( ( temp_output_3_0_g24 / fwidth( temp_output_3_0_g24 ) ) ) * SoftEdgeMask249 ) ) ) * _Element_Specs_Intensity ) * Base_BottomCull72 ) * lerpResult505 );
				float smoothstepResult68 = smoothstep( _ToggleCatalystCull , _ToggleCatalystFade , ( 1.0 - temp_output_58_0 ));
				float Base_TopCull71 = smoothstepResult68;
				float4 BaseColours_Cataylst30 = ( IN.ase_color * _BaseColour_Catalyst );
				float4 SecondaryColours_Cataylst282 = ( IN.ase_color * _SecondaryColour_Catalyst );
				float4 lerpResult335 = lerp( BaseColours_Cataylst30 , SecondaryColours_Cataylst282 , _ToggleCatalystTexColours_Base0_Secondary1);
				float4 Catalyst_Base_Colour430 = ( Base_TopCull71 * lerpResult335 );
				float2 appendResult229 = (float2(_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.x , _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.y));
				float2 CatalystNoise1_Pan233 = appendResult229;
				float3 worldToObj216 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float2 panner224 = ( _TimeParameters.x * CatalystNoise1_Pan233 + worldToObj216.xy);
				float CatalystNoise_1_Scale237 = _NoiseScales_E1X_E2Y_C1Z_C2W.z;
				float simplePerlin3D226 = snoise( float3( panner224 ,  0.0 )*CatalystNoise_1_Scale237 );
				simplePerlin3D226 = simplePerlin3D226*0.5 + 0.5;
				float CatalystNoise_1227 = simplePerlin3D226;
				float4 temp_cast_34 = (CatalystNoise_1227).xxxx;
				float2 appendResult140 = (float2(_Cataylst_Tex_Pan_Slot1XY_Slot2ZW.x , _Cataylst_Tex_Pan_Slot1XY_Slot2ZW.y));
				float2 CatalystPan_Slot_1148 = appendResult140;
				float2 texCoord191 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner196 = ( 1.0 * _Time.y * CatalystPan_Slot_1148 + texCoord191);
				float lerpResult497 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.x ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnX125 = lerpResult497;
				float4 CatalystTex_1200 = ( tex2D( _CatalystTexture_Slot_1, panner196 ) * FlashingOnX125 );
				float4 lerpResult387 = lerp( temp_cast_34 , ( CatalystTex_1200 * CatalystNoise_1227 ) , _Toggle_UseCatalystTexture_1_With_Noise_1);
				float4 temp_cast_35 = (_Catalyst_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_36 = (_Catalyst_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_37 = (_Catalyst_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_38 = (_Catalyst_Base_Noise_1_Remap.w).xxxx;
				float2 appendResult231 = (float2(_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.z , _CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW.w));
				float2 CatalystNoise2_Pan232 = appendResult231;
				float2 panner222 = ( _TimeParameters.x * CatalystNoise2_Pan232 + worldToObj216.xy);
				float CatalystNoise_2_Scale238 = _NoiseScales_E1X_E2Y_C1Z_C2W.w;
				float simplePerlin3D225 = snoise( float3( panner222 ,  0.0 )*CatalystNoise_2_Scale238 );
				simplePerlin3D225 = simplePerlin3D225*0.5 + 0.5;
				float CatalystNoise_2228 = simplePerlin3D225;
				float4 temp_cast_41 = (CatalystNoise_2228).xxxx;
				float2 appendResult141 = (float2(_Cataylst_Tex_Pan_Slot1XY_Slot2ZW.z , _Cataylst_Tex_Pan_Slot1XY_Slot2ZW.w));
				float2 CatalystPan_Slot_2149 = appendResult141;
				float2 panner197 = ( 1.0 * _Time.y * CatalystPan_Slot_2149 + texCoord191);
				float lerpResult496 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.y ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnY124 = lerpResult496;
				float4 CatalystTex_2201 = ( tex2D( _CatalystTexture_Slot_2, panner197 ) * FlashingOnY124 );
				float4 lerpResult394 = lerp( temp_cast_41 , ( CatalystTex_2201 * CatalystNoise_2228 ) , _Toggle_UseCatalystTexture_2_With_Noise_2);
				float4 temp_cast_42 = (_Catalyst_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_43 = (_Catalyst_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_44 = (_Catalyst_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_45 = (_Catalyst_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_46 = (_Catalyst_Base_Remap.x).xxxx;
				float4 temp_cast_47 = (_Catalyst_Base_Remap.y).xxxx;
				float4 temp_cast_48 = (_Catalyst_Base_Remap.z).xxxx;
				float4 temp_cast_49 = (_Catalyst_Base_Remap.w).xxxx;
				float4 temp_output_371_0 = (temp_cast_48 + (( (temp_cast_37 + (lerpResult387 - temp_cast_35) * (temp_cast_38 - temp_cast_37) / (temp_cast_36 - temp_cast_35)) * (temp_cast_44 + (lerpResult394 - temp_cast_42) * (temp_cast_45 - temp_cast_44) / (temp_cast_43 - temp_cast_42)) ) - temp_cast_46) * (temp_cast_49 - temp_cast_48) / (temp_cast_47 - temp_cast_46));
				float4 temp_cast_50 = (1.0).xxxx;
				float4 temp_cast_51 = (_Catalyst_Base_Noise_1_Remap.x).xxxx;
				float4 temp_cast_52 = (_Catalyst_Base_Noise_1_Remap.y).xxxx;
				float4 temp_cast_53 = (_Catalyst_Base_Noise_1_Remap.z).xxxx;
				float4 temp_cast_54 = (_Catalyst_Base_Noise_1_Remap.w).xxxx;
				float4 temp_cast_55 = (_Catalyst_Base_Noise_2_Remap.x).xxxx;
				float4 temp_cast_56 = (_Catalyst_Base_Noise_2_Remap.y).xxxx;
				float4 temp_cast_57 = (_Catalyst_Base_Noise_2_Remap.z).xxxx;
				float4 temp_cast_58 = (_Catalyst_Base_Noise_2_Remap.w).xxxx;
				float4 temp_cast_59 = (_Catalyst_Base_Remap.x).xxxx;
				float4 temp_cast_60 = (_Catalyst_Base_Remap.y).xxxx;
				float4 temp_cast_61 = (_Catalyst_Base_Remap.z).xxxx;
				float4 temp_cast_62 = (_Catalyst_Base_Remap.w).xxxx;
				float4 lerpResult378 = lerp( temp_cast_50 , ( temp_output_371_0 * SoftEdgeMask249 ) , _Toggle_Catalyst_Noise_Mask);
				float4 Catalyst_Base398 = ( ( temp_output_371_0 * lerpResult378 ) * 1.0 );
				float Fresnel_TopCull105 = ( Base_TopCull71 * Fresnel_Pulse67 );
				float4 lerpResult364 = lerp( BaseColours_Cataylst30 , SecondaryColours_Cataylst282 , _ToggleCatalystFresColours_Base0_Secondary1);
				float4 Catalyst_Fresnel_Colour431 = ( Fresnel_TopCull105 * lerpResult364 );
				float CatalystVoronoi_Scale_2296 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.w;
				float CatalystVoronoi_Speed_2294 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.y;
				float time313 = ( _TimeParameters.x * CatalystVoronoi_Speed_2294 );
				float2 voronoiSmoothId313 = 0;
				float2 texCoord315 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float2 coords313 = texCoord315 * CatalystVoronoi_Scale_2296;
				float2 id313 = 0;
				float2 uv313 = 0;
				float fade313 = 0.5;
				float voroi313 = 0;
				float rest313 = 0;
				for( int it313 = 0; it313 <8; it313++ ){
				voroi313 += fade313 * voronoi313( coords313, time313, id313, uv313, 0,voronoiSmoothId313 );
				rest313 += fade313;
				coords313 *= 2;
				fade313 *= 0.5;
				}//Voronoi313
				voroi313 /= rest313;
				float CatalystVoronoi_2326 = voroi313;
				float4 temp_cast_63 = (0.0).xxxx;
				float2 appendResult146 = (float2(_Cataylst_Tex_Pan_Slot3XY_Slot4ZW.x , _Cataylst_Tex_Pan_Slot3XY_Slot4ZW.y));
				float2 CatalystPan_Slot_3150 = appendResult146;
				float2 panner198 = ( 1.0 * _Time.y * CatalystPan_Slot_3150 + texCoord191);
				float lerpResult495 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.z ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnZ126 = lerpResult495;
				float4 CatalystTex_3202 = ( tex2D( _CatalystTexture_Slot_3, panner198 ) * FlashingOnZ126 );
				float2 appendResult147 = (float2(_Cataylst_Tex_Pan_Slot3XY_Slot4ZW.z , _Cataylst_Tex_Pan_Slot3XY_Slot4ZW.w));
				float2 CataylstPan_Slot_4151 = appendResult147;
				float2 panner199 = ( 1.0 * _Time.y * CataylstPan_Slot_4151 + texCoord191);
				float lerpResult493 = lerp( abs( sin( ( _TimeParameters.x * _Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W.w ) ) ) , 1.0 , _Toggle_Flashing);
				float FlashingOnW181 = lerpResult493;
				float4 CatalystTex_4203 = ( tex2D( _CatalystTexture_Slot_4, panner199 ) * FlashingOnW181 );
				float4 lerpResult452 = lerp( temp_cast_63 , ( ( CatalystTex_3202 + CatalystTex_4203 ) * (Catalyst_Base_Colour430).rgba ) , _Toggle_UseCatalystTex_3__4);
				float temp_output_3_0_g23 = ( CatalystNoise_2228 - CatalystNoise_1227 );
				float temp_output_338_0 = ( (_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z + (CatalystNoise_1227 - _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x) * (_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.w - _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.z) / (_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.y - _Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW.x)) * saturate( ( temp_output_3_0_g23 / fwidth( temp_output_3_0_g23 ) ) ) );
				float CatalystVoronoi_Scale_1295 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.z;
				float CatalystVoronoi_Speed_1293 = _CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W.x;
				float time309 = ( _TimeParameters.x * CatalystVoronoi_Speed_1293 );
				float2 voronoiSmoothId309 = 0;
				float2 coords309 = texCoord315 * CatalystVoronoi_Scale_1295;
				float2 id309 = 0;
				float2 uv309 = 0;
				float fade309 = 0.5;
				float voroi309 = 0;
				float rest309 = 0;
				for( int it309 = 0; it309 <2; it309++ ){
				voroi309 += fade309 * voronoi309( coords309, time309, id309, uv309, 0,voronoiSmoothId309 );
				rest309 += fade309;
				coords309 *= 2;
				fade309 *= 0.5;
				}//Voronoi309
				voroi309 /= rest309;
				float CatalystVoronoi_1325 = voroi309;
				float temp_output_3_0_g22 = ( _Catalyst_Spec_StepValue - CatalystVoronoi_1325 );
				float lerpResult509 = lerp( 1.0 , 0.0 , _Toggle_Cataylst_Spec_On0_Off1);
				float CatalystBrightParts348 = ( ( ( ( temp_output_338_0 * ( temp_output_338_0 * ( saturate( ( temp_output_3_0_g22 / fwidth( temp_output_3_0_g22 ) ) ) * SoftEdgeMask249 ) ) ) * _Catalyst_Specs_Intensity ) * Base_TopCull71 ) * lerpResult509 );
				float4 temp_output_445_0 = ( ( ( ( Element_Base_Colour428 * Element_Base424 ) + ( Element_Fresnel_Colour429 * ElementVoronoi_2328 ) ) + lerpResult457 + ( Element_Base_Colour428 * ElementBrightParts266 ) ) + ( ( ( Catalyst_Base_Colour430 * Catalyst_Base398 ) + ( Catalyst_Fresnel_Colour431 * CatalystVoronoi_2326 ) ) + lerpResult452 + ( Catalyst_Base_Colour430 * CatalystBrightParts348 ) ) );
				float grayscale462 = (temp_output_445_0.rgb.r + temp_output_445_0.rgb.g + temp_output_445_0.rgb.b) / 3;
				float VertexAlpha297 = IN.ase_color.a;
				float3 worldToObj92 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float smoothstepResult85 = smoothstep( _ToggleOverallCull , ( _ToggleOverallCull + _ToggleOverallFade ) , ( ElementNoise_161 + ( 1.0 - (0.0 + (worldToObj92.y - -1.0) * (1.0 - 0.0) / (1.0 - -1.0)) ) ));
				float DisolveAlpha93 = smoothstepResult85;
				float Alpha9 = ( ( (0.01 + (grayscale462 - 0.0) * (4.0 - 0.01) / (1.0 - 0.0)) * VertexAlpha297 ) * DisolveAlpha93 );
				
				float Alpha = Alpha9;
				float AlphaClipThreshold = 0.5;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				return 0;
			}
			ENDHLSL
		}

	
	}
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18935
0;0;1920;1019;6388.927;1284.696;4.586908;False;False
Node;AmplifyShaderEditor.CommentaryNode;488;-6151.794,-1151.512;Inherit;False;2080.678;4418.773;Values;7;487;486;101;251;107;283;108;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;486;-6101.693,1860.944;Inherit;False;1752.833;807.6781;Catalyst Values;44;177;114;144;140;115;116;141;230;148;229;149;120;119;231;233;121;122;232;124;125;145;178;146;147;117;179;118;150;151;180;123;292;126;181;294;296;295;293;493;495;496;497;498;499;;0.01530123,0,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;177;-6068.514,1974.41;Inherit;False;Property;_Catalyst_Flash_Slot1X_Slot2Y_Slot3Z_Slot4W;Catalyst_Flash_Slot1(X)_Slot2(Y)_Slot3(Z)_Slot4(W);43;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;114;-5973.378,1904.062;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;144;-5040.981,1935.557;Inherit;False;Property;_Cataylst_Tex_Pan_Slot1XY_Slot2ZW;Cataylst_Tex_Pan_Slot1(X/Y)_Slot2(Z/W);44;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;487;-6101.794,2690.105;Inherit;False;1980.678;577.156;Element Values;25;211;175;234;208;209;168;167;237;238;171;213;172;212;235;236;176;170;169;174;173;298;299;301;302;300;;1,0,0,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;-5762.158,1898.383;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-5757.552,2006.909;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;119;-5637.303,2001.662;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;491;-3977.38,-1152.372;Inherit;False;3332.784;4411.765;Textures Noises Voronois;2;489;490;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinOpNode;120;-5634.466,1901.58;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;230;-5032.607,2308.831;Inherit;False;Property;_CatalystNoise_Pan_Speed_Noise1XY_Noise2ZW;CatalystNoise_Pan_Speed_Noise1(X/Y)_Noise2(Z/W);46;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;140;-4735.153,1924.089;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;141;-4734.185,2016.808;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;211;-6051.794,2740.105;Inherit;False;Property;_ElementNoise_Pan_Speed_Noise1XY_Noise2ZW;ElementNoise_Pan_Speed_Noise1(X/Y)_Noise2(Z/W);35;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.6,0.5,0.1,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;175;-5384.047,2756.616;Inherit;False;Property;_Element_Tex_Pan_Slot1_Slot2;Element_Tex_Pan_Slot1_Slot2;30;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;149;-4598.86,2012.61;Inherit;False;CatalystPan_Slot_2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;209;-5738.941,2847.738;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;168;-5077.25,2837.867;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;148;-4604.86,1923.61;Inherit;False;CatalystPan_Slot_1;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;208;-5741.812,2754.455;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;489;-3860.62,1075.861;Inherit;False;3059.113;2154.532;Elemental Orb;110;38;214;185;46;215;47;143;152;136;241;163;52;137;242;129;165;128;59;207;61;166;206;18;330;188;189;331;329;97;139;138;304;356;359;319;357;131;320;306;130;98;354;284;358;204;428;205;305;355;429;328;288;327;285;317;318;289;422;416;421;415;418;413;411;423;419;414;412;417;426;420;399;400;401;402;405;403;404;425;406;407;410;408;409;424;263;253;261;264;262;268;265;256;260;257;267;266;252;254;258;259;255;500;502;503;504;505;506;507;508;;1,0,0,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;498;-6000.615,2157.684;Inherit;False;Constant;_Float2;Float 2;60;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;229;-4722.624,2323.182;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;490;-3918.38,-1102.372;Inherit;False;3223.784;2105.534;Catalyst Orb;118;194;193;191;218;220;219;217;196;197;216;154;156;222;224;239;132;133;240;153;226;225;155;227;200;201;228;192;195;198;199;184;182;158;134;332;362;333;361;363;334;324;312;323;183;157;314;96;315;364;365;335;313;202;360;95;203;431;326;430;308;310;321;309;325;322;390;395;396;388;389;392;391;393;397;374;394;373;387;372;368;369;376;370;380;371;382;381;379;378;384;377;383;398;351;342;346;338;341;348;349;345;352;340;350;343;347;337;339;344;336;501;514;512;511;510;515;509;513;;0,0.09729433,1,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode;231;-4719.754,2416.464;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;499;-6072.151,2254.225;Inherit;False;Property;_Toggle_Flashing;Toggle_Flashing;4;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;121;-5526.855,1998.861;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;122;-5521.744,1901.083;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;167;-5078.219,2745.147;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;218;-2512.037,-812.4902;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector4Node;234;-5973.397,2975.468;Inherit;False;Property;_NoiseScales_E1X_E2Y_C1Z_C2W;NoiseScales_E1(X)_E2(Y)_C1(Z)_C2(W);23;0;Create;True;0;0;0;False;0;False;1,1,1,1;3.38,1.3,1,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;194;-3868.38,-794.3016;Inherit;False;148;CatalystPan_Slot_1;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;191;-3866.599,-912.6354;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;38;-2682.461,1418.935;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;172;-4941.926,2833.668;Inherit;False;ElementPan_Slot_2;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;232;-4593.562,2413.782;Inherit;False;CatalystNoise2_Pan;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;497;-5393.257,1893.72;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;213;-5612.749,2845.055;Inherit;False;ElementNoise2_Pan;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;171;-4947.926,2744.668;Inherit;False;ElementPan_Slot_1;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;193;-3866.344,-720.533;Inherit;False;149;CatalystPan_Slot_2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;212;-5609.749,2751.055;Inherit;False;ElementNoise1_Pan;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;233;-4590.562,2319.781;Inherit;False;CatalystNoise1_Pan;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.LerpOp;496;-5393.256,2019.592;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;298;-4722.359,2817.946;Inherit;False;Property;_ElementVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W;ElementVoronoi_Speed1(X)_Speed2(Y)_Scale1(Z)_Scale2(W);36;0;Create;True;0;0;0;False;0;False;0,0,0,0;10,10,10,10;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TransformPositionNode;46;-2517.761,1415.841;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;152;-3770.823,1354.131;Inherit;False;171;ElementPan_Slot_1;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;125;-5248.849,1910.944;Inherit;False;FlashingOnX;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;219;-2360.714,-969.0469;Inherit;False;233;CatalystNoise1_Pan;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;47;-2483.646,1344.779;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;197;-3609.549,-780.0181;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;237;-5616.544,3076.261;Inherit;False;CatalystNoise_1_Scale;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;220;-2370.714,-664.0463;Inherit;False;232;CatalystNoise2_Pan;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;217;-2313.222,-886.6467;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;216;-2347.337,-815.5845;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;214;-2531.138,1262.38;Inherit;False;212;ElementNoise1_Pan;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;238;-5615.544,3151.261;Inherit;False;CatalystNoise_2_Scale;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;185;-3768.787,1427.9;Inherit;False;172;ElementPan_Slot_2;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;196;-3612.318,-901.2596;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;124;-5250.535,2011.117;Inherit;False;FlashingOnY;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;143;-3769.044,1235.798;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;215;-2541.138,1567.378;Inherit;False;213;ElementNoise2_Pan;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode;101;-6098.799,160.4544;Inherit;False;1701.797;556.6018;FresnelPulse;17;32;33;34;35;36;37;39;40;42;43;44;45;50;51;53;60;67;;0,0.9486632,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;292;-5613.511,2486.143;Inherit;False;Property;_CatalystVoronoi_Speed1X_Speed2Y_Scale1Z_Scale2W;CatalystVoronoi_Speed1(X)_Speed2(Y)_Scale1(Z)_Scale2(W);47;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;235;-5620.544,2928.26;Inherit;False;ElementNoise_1_Scale;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;236;-5619.544,3002.26;Inherit;False;ElementNoise_2_Scale;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;133;-3351.984,-858.467;Inherit;True;Property;_CatalystTexture_Slot_2;CatalystTexture_Slot_2;40;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;241;-2323.229,1371.443;Inherit;False;235;ElementNoise_1_Scale;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;33;-6048.799,339.5982;Inherit;False;Property;_Fres_ScaleX_PowerY_Speeds;Fres_Scale(X)_Power(Y)_Speeds;57;0;Create;True;0;0;0;False;0;False;1,1;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.GetLocalVarNode;154;-3072.159,-984.1595;Inherit;False;125;FlashingOnX;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;136;-3514.762,1247.174;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;242;-2326.229,1572.443;Inherit;False;236;ElementNoise_2_Scale;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;132;-3354.24,-1052.372;Inherit;True;Property;_CatalystTexture_Slot_1;CatalystTexture_Slot_1;39;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;156;-3071.159,-791.6592;Inherit;False;124;FlashingOnY;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;163;-2277.167,1448.039;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;239;-2153.298,-863.2126;Inherit;False;237;CatalystNoise_1_Scale;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;32;-5925.493,273.0352;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;300;-4405.116,2843.555;Inherit;False;ElementVoronoi_Speed_2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;251;-6093.658,-138.0261;Inherit;False;1345.618;281.5611;Soft Edge;6;246;247;248;249;250;245;;0,0.9071302,1,1;0;0
Node;AmplifyShaderEditor.PannerNode;137;-3511.992,1368.416;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;224;-2108.944,-986.3965;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;222;-2106.743,-783.3862;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;240;-2152.298,-661.2123;Inherit;False;238;CatalystNoise_2_Scale;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;293;-5281.38,2456.084;Inherit;False;CatalystVoronoi_Speed_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;52;-2279.368,1245.03;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;226;-1932.027,-877.0767;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;107;-6099.27,730.6426;Inherit;False;1674.781;609.0057;Culling Bot/Top;18;65;66;69;68;70;72;58;64;62;63;55;49;41;71;73;74;106;105;;0,0.9806142,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;129;-3264.09,1318.481;Inherit;True;Property;_ElementTexture_Slot_2;ElementTexture_Slot_2;27;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;-2894.159,-1045.159;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;155;-2893.159,-852.6592;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;288;-1598.119,1274.894;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;317;-1679.546,1349.351;Inherit;False;300;ElementVoronoi_Speed_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-5727.007,444.9124;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;128;-3269.506,1125.861;Inherit;True;Property;_ElementTexture_Slot_1;ElementTexture_Slot_1;26;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;302;-4405.116,2993.555;Inherit;False;ElementVoronoi_Scale_2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;165;-2100.107,1449.357;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;295;-4986.643,2510.762;Inherit;False;CatalystVoronoi_Scale_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;308;-1488.758,-902.5408;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;225;-1929.684,-782.0674;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;321;-1519.978,-816.6312;Inherit;False;293;CatalystVoronoi_Speed_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;245;-6043.658,-88.02609;Inherit;True;Property;_SoftEgdeMask_Texture;SoftEgdeMask_Texture;62;0;Create;True;0;0;0;False;0;False;9a7be70759d48ff48a8ef359791bfa99;9a7be70759d48ff48a8ef359791bfa99;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.NoiseGeneratorNode;59;-2102.45,1354.348;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;34;-5723.832,210.4544;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;61;-1899.964,1275.418;Inherit;True;ElementNoise_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;36;-5592.313,450.1094;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;228;-1730.339,-738.9974;Inherit;True;CatalystNoise_2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;284;-1457.33,1156.11;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;166;-1900.76,1492.427;Inherit;True;ElementNoise_2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;310;-1274.328,-922.905;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;207;-2881.422,1321.761;Inherit;False;ElementTex_1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;318;-1681.546,1423.352;Inherit;False;302;ElementVoronoi_Scale_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;206;-2881.422,1397.306;Inherit;False;ElementTex_2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;315;-1490.067,-1036.726;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;285;-1430.69,1275.531;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;322;-1515.794,-741.3196;Inherit;False;295;CatalystVoronoi_Scale_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;246;-5749.005,-86.46499;Inherit;True;Property;_MAT_Masking_SquareMaskSprite;MAT_Masking_SquareMaskSprite;4;0;Create;True;0;0;0;False;0;False;-1;9a7be70759d48ff48a8ef359791bfa99;9a7be70759d48ff48a8ef359791bfa99;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;250;-5463.53,13.54116;Inherit;False;Property;_SoftEdgeFade;SoftEdgeFade;63;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;37;-5589.139,215.6515;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;201;-2711.157,-802.1088;Inherit;False;CatalystTex_2;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;227;-1729.541,-956.0068;Inherit;True;CatalystNoise_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;200;-2711.157,-877.6545;Inherit;False;CatalystTex_1;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;41;-6049.27,981.9191;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.VoronoiNode;289;-1234.117,1158.24;Inherit;True;0;0;1;0;2;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.GetLocalVarNode;422;-3666.585,2112.567;Inherit;False;61;ElementNoise_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;45;-5474.665,454.8384;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;390;-3523.744,-188.9572;Inherit;False;200;CatalystTex_1;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.VoronoiNode;309;-1134.755,-970.196;Inherit;False;0;0;1;0;2;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.AbsOpNode;39;-5471.488,220.3804;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-5598.977,601.0563;Inherit;False;Property;_Fres_Power_MaxNew;Fres_Power_MaxNew;61;0;Create;True;0;0;0;False;0;False;5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;395;-3517.321,199.5732;Inherit;False;201;CatalystTex_2;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;415;-3654.161,2430.037;Inherit;False;206;ElementTex_2;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TransformPositionNode;49;-5879.264,977.298;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;247;-5312.552,-70.46188;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;396;-3523.321,270.6341;Inherit;False;228;CatalystNoise_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;388;-3529.744,-117.8967;Inherit;False;227;CatalystNoise_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-5595.802,293.5982;Inherit;False;Property;_Fres_Scale_MinNew;Fres_Scale_MinNew;58;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;421;-3660.585,2041.507;Inherit;False;207;ElementTex_1;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;416;-3660.161,2501.097;Inherit;False;166;ElementNoise_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-5596.802,366.5972;Inherit;False;Property;_Fres_Scale_MaxNew;Fres_Scale_MaxNew;59;0;Create;True;0;0;0;False;0;False;5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-5598.977,528.0563;Inherit;False;Property;_Fres_Power_MinNew;Fres_Power_MinNew;60;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;392;-3340.771,146.6691;Inherit;False;228;CatalystNoise_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;50;-5343.665,456.8384;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;393;-3308.32,223.5732;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;389;-3314.744,-164.9572;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;413;-3707.884,2573.284;Inherit;False;Property;_Toggle_UseElementTexture_2_With_Noise_2;Toggle_UseElementTexture_2_With_Noise_2;14;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;419;-3451.585,2065.507;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;397;-3562.487,344.533;Inherit;False;Property;_Toggle_UseCatalystTexture_2_With_Noise_2;Toggle_UseCatalystTexture_2_With_Noise_2;17;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;391;-3568.91,-43.99763;Inherit;False;Property;_Toggle_UseCatalystTexture_1_With_Noise_1;Toggle_UseCatalystTexture_1_With_Noise_1;16;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;411;-3445.16,2454.037;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;51;-5356.488,225.3814;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;10;False;3;FLOAT;1;False;4;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;374;-3347.195,-241.8614;Inherit;False;227;CatalystNoise_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;325;-942.5965,-969.3793;Inherit;True;CatalystVoronoi_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;327;-1049.507,1156.479;Inherit;True;ElementVoronoi_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;414;-3477.611,2377.133;Inherit;False;166;ElementNoise_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;423;-3705.751,2186.466;Inherit;False;Property;_Toggle_UseElementTexture_1_With_Noise_1;Toggle_UseElementTexture_1_With_Noise_1;13;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;248;-5171.506,-75.33303;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;418;-3484.035,1988.602;Inherit;False;61;ElementNoise_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;55;-5673.013,977.7631;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;117;-5753.552,2101.909;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;268;-3810.62,2661.135;Inherit;False;Property;_Element_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW;Element_Specs_Remap_MinOld(X)_MaxOld(Y)_MinNew(Z)_MaxNew(W);34;0;Create;True;0;0;0;False;0;False;0,1,0,1;0,1,0,2;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;53;-5155.928,271.9544;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;394;-3122.32,185.5732;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;249;-4944.04,-92.65023;Inherit;True;SoftEdgeMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;336;-3572.911,762.4692;Inherit;False;325;CatalystVoronoi_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;-5750.049,2210.286;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;254;-3721.543,2836.782;Inherit;False;61;ElementNoise_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;412;-3259.161,2416.037;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;255;-3754.069,3068.223;Inherit;False;Property;_Element_Spec_StepValue;Element_Spec_StepValue;32;0;Create;True;0;0;0;False;0;False;0.1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;372;-3131.463,13.88832;Inherit;False;Property;_Catalyst_Base_Noise_1_Remap;Catalyst_Base_Noise_1_Remap;49;0;Create;True;0;0;0;False;0;False;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;420;-3265.585,2027.507;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;351;-3562.508,684.8801;Inherit;False;228;CatalystNoise_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;283;-6087.036,-1101.512;Inherit;False;841.77;946.473;Colours;14;25;29;280;278;27;26;277;279;30;282;19;281;28;297;;0,0.9903989,1,1;0;0
Node;AmplifyShaderEditor.Vector4Node;145;-5041.917,2107.1;Inherit;False;Property;_Cataylst_Tex_Pan_Slot3XY_Slot4ZW;Cataylst_Tex_Pan_Slot3(X/Y)_Slot4(Z/W);45;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;349;-3597.54,840.9919;Inherit;False;Property;_Catalyst_Spec_StepValue;Catalyst_Spec_StepValue;54;0;Create;True;0;0;0;False;0;False;0.1;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;417;-3268.303,2244.352;Inherit;False;Property;_Element_Base_Noise_1_Remap;Element_Base_Noise_1_Remap;48;0;Create;True;0;0;0;False;0;False;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;387;-3128.744,-202.9572;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector4Node;341;-3654.09,433.9042;Inherit;False;Property;_Catalyst_Specs_Remap_MinOldX_MaxOldY_MinNewZ_MaxNewW;Catalyst_Specs_Remap_MinOld(X)_MaxOld(Y)_MinNew(Z)_MaxNew(W);56;0;Create;True;0;0;0;False;0;False;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;426;-3267.433,2627.49;Inherit;False;Property;_Element_Base_Noise_2_Remap;Element_Base_Noise_2_Remap;50;0;Create;True;0;0;0;False;0;False;0,1,0,1;0,1,0.3,1.5;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector4Node;373;-3130.592,397.0275;Inherit;False;Property;_Catalyst_Base_Noise_2_Remap;Catalyst_Base_Noise_2_Remap;51;0;Create;True;0;0;0;False;0;False;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;253;-3723.438,2993.7;Inherit;False;327;ElementVoronoi_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;352;-3565.014,609.5511;Inherit;False;227;CatalystNoise_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;252;-3719.037,2912.112;Inherit;False;166;ElementNoise_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-5554.23,1005.346;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.88;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;350;-3332.441,887.1616;Inherit;False;249;SoftEdgeMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;368;-2647.105,-69.30244;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;147;-4728.365,2205.788;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;256;-3471.972,2743.849;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;176;-5384.983,2928.158;Inherit;False;Property;_Element_Tex_Pan_Slot3_Slot4;Element_Tex_Pan_Slot3_Slot4;31;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;28;-6000.381,-881.1287;Inherit;False;Property;_BaseColour_Catalyst;BaseColour_Catalyst;37;1;[HDR];Create;True;0;0;0;False;0;False;0,0.7648358,1,0;0,0.7648358,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;400;-2782.946,2330.161;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.SinOpNode;118;-5634.526,2096.662;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;65;-5404.319,911.3575;Inherit;False;Property;_ToggleCatalystCull;ToggleCatalystCull;7;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;179;-5627.356,2208.705;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;257;-3482.798,2918.377;Inherit;False;Step Antialiasing;-1;;25;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;258;-3488.97,3114.393;Inherit;False;249;SoftEdgeMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;279;-6037.036,-537.4227;Inherit;False;Property;_SecondaryColour_Element;SecondaryColour_Element;25;1;[HDR];Create;True;0;0;0;False;0;False;1,0.8760238,0,0;1.968058,3.622642,0.0512637,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;280;-6030.899,-367.0388;Inherit;False;Property;_SecondaryColour_Catalyst;SecondaryColour_Catalyst;38;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0.7082286,0;1,0,0.7082286,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;347;-3315.443,516.6183;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.2;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;29;-5947.957,-708.3451;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;62;-5643.479,1127.17;Inherit;False;Property;_ToggleElementCull;ToggleElementCull;5;0;Create;True;0;0;0;False;0;False;0.1294118;0.654;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;63;-5653.405,1211.751;Inherit;False;Property;_ToggleElementFade;ToggleElementFade;6;0;Create;True;0;0;0;False;0;False;-0.5;-0.5;-100;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;25;-6006.518,-1051.512;Inherit;False;Property;_BaseColour_Element;BaseColour_Element;24;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;0.6002468,3.953349,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;369;-2646.105,99.69753;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;64;-5263.096,794.3367;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;60;-4935.354,272.9073;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;345;-3326.268,691.1461;Inherit;False;Step Antialiasing;-1;;23;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0.6;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-5403.319,984.3571;Inherit;False;Property;_ToggleCatalystFade;ToggleCatalystFade;8;0;Create;True;0;0;0;False;0;False;0;-100;-100;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;399;-2783.946,2161.161;Inherit;False;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;259;-3482.693,3020.721;Inherit;False;Step Antialiasing;-1;;24;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;146;-4730.333,2114.069;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;346;-3326.164,793.4901;Inherit;False;Step Antialiasing;-1;;22;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;-5699.208,-908.7106;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;261;-3255.977,2803.233;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;278;-5687.968,-443.2307;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;260;-3273.765,2940.227;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;277;-5687.968,-540.2307;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.DynamicAppendNode;170;-5071.43,3026.846;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.AbsOpNode;123;-5525.077,2093.749;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;150;-4596.86,2107.611;Inherit;False;CatalystPan_Slot_3;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;402;-2594.74,2345.423;Inherit;False;Property;_Element_Base_Remap;Element_Base_Remap;52;0;Create;True;0;0;0;False;0;False;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;68;-5092.897,783.3606;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;-0.25;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;376;-2457.9,114.9592;Inherit;False;Property;_Catalyst_Base_Remap;Catalyst_Base_Remap;53;0;Create;True;0;0;0;False;0;False;0,1,0,1;0,1,0,1;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;401;-2586.945,2239.161;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-5698.065,-812.3672;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SmoothstepOpNode;69;-5362.266,1085.648;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0.5;False;2;FLOAT;0.05;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;338;-3086.364,574.1621;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;180;-5514.243,2207.015;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;370;-2447.105,18.69765;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;344;-3104.152,711.1561;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;151;-4593.86,2196.611;Inherit;False;CataylstPan_Slot_4;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;169;-5073.398,2935.127;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;67;-4747.002,265.0273;Inherit;True;Fresnel_Pulse;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;493;-5392.032,2312.885;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;403;-2343.85,2250.62;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;30;-5474.999,-746.7861;Inherit;True;BaseColours_Cataylst;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;70;-5071.842,1004.651;Inherit;False;67;Fresnel_Pulse;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;262;-3117.885,2887.98;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;195;-3865.344,-575.533;Inherit;False;151;CataylstPan_Slot_4;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;281;-5508.266,-556.1945;Inherit;True;SecondaryColours_Elements;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;282;-5504.266,-361.1946;Inherit;True;SecondaryColours_Cataylst;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;495;-5393.255,2139.354;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;173;-4939.926,2928.668;Inherit;False;ElementPan_Slot_3;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;72;-5122.908,1087.071;Inherit;False;Base_BottomCull;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;380;-2654.239,350.2192;Inherit;False;249;SoftEdgeMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;174;-4936.926,3017.668;Inherit;False;ElementPan_Slot_4;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;192;-3865.344,-647.533;Inherit;False;150;CatalystPan_Slot_3;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TFHCRemapNode;371;-2207.01,20.15626;Inherit;True;5;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,1;False;3;COLOR;0,0,0,0;False;4;COLOR;1,1,1,1;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;405;-2791.079,2580.683;Inherit;False;249;SoftEdgeMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;19;-5476.177,-934.8839;Inherit;True;BaseColours_Elements;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;337;-2948.271,658.9091;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;71;-4874.541,780.6426;Inherit;False;Base_TopCull;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;181;-5246.032,2219.493;Inherit;False;FlashingOnW;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;425;-2800.079,2655.683;Inherit;False;Property;_Toggle_Element_Noise_Mask;Toggle_Element_Noise_Mask;11;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;299;-4406.116,2769.555;Inherit;False;ElementVoronoi_Speed_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;404;-2791.079,2508.683;Inherit;False;Constant;_Float0;Float 0;48;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;188;-3767.787,1500.9;Inherit;False;173;ElementPan_Slot_3;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-4828.276,967.9007;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;294;-5280.38,2530.084;Inherit;False;CatalystVoronoi_Speed_2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;332;-2708.654,-480.8755;Inherit;False;282;SecondaryColours_Cataylst;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;198;-3609.549,-659.0179;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;126;-5249.535,2111.116;Inherit;False;FlashingOnZ;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;331;-2881.319,1819.333;Inherit;False;Property;_ToggleElementTexColours_Base0_Secondary1;ToggleElementTexColours_Base(0)_Secondary(1);0;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;189;-3767.787,1572.9;Inherit;False;174;ElementPan_Slot_4;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-4829.25,866.0046;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;199;-3607.549,-543.0175;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;382;-2440.219,310.1237;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;381;-2663.239,425.2192;Inherit;False;Property;_Toggle_Catalyst_Noise_Mask;Toggle_Catalyst_Noise_Mask;12;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;406;-2577.059,2540.586;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;334;-2688.503,-554.3632;Inherit;False;30;BaseColours_Cataylst;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;342;-2819.405,667.087;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;18;-2847.989,1672.044;Inherit;False;19;BaseColours_Elements;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;330;-2868.139,1745.531;Inherit;False;281;SecondaryColours_Elements;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;333;-2728.362,-407.0745;Inherit;False;Property;_ToggleCatalystTexColours_Base0_Secondary1;ToggleCatalystTexColours_Base(0)_Secondary(1);3;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;263;-2898.018,2896.159;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;379;-2654.239,278.2194;Inherit;False;Constant;_ConstantWhite;Constant White;48;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;264;-2793.124,2802.685;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;105;-4660.489,786.9731;Inherit;True;Fresnel_TopCull;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;319;-1692.325,1583.891;Inherit;False;299;ElementVoronoi_Speed_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;378;-2275.731,287.2194;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;312;-1482.921,-663.0534;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;184;-3066.319,-398.1915;Inherit;False;181;FlashingOnW;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;501;-2747.658,837.9945;Inherit;False;71;Base_TopCull;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;97;-2585.087,1653.689;Inherit;False;72;Base_BottomCull;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;407;-2414.078,2517.683;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;356;-2867.329,1900.665;Inherit;False;19;BaseColours_Elements;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;134;-3348.256,-663.2839;Inherit;True;Property;_CatalystTexture_Slot_3;CatalystTexture_Slot_3;41;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;182;-3348.428,-471.1035;Inherit;True;Property;_CatalystTexture_Slot_4;CatalystTexture_Slot_4;42;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;139;-3509.992,1605.416;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;343;-2623.512,573.6141;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;362;-2711.166,-251.2735;Inherit;False;282;SecondaryColours_Cataylst;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;96;-2351.034,-570.9345;Inherit;False;71;Base_TopCull;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;158;-3059.159,-598.6589;Inherit;False;126;FlashingOnZ;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;359;-2900.659,2047.954;Inherit;False;Property;_ToggleElementFresColours_Base0_Secondary1;ToggleElementFresColours_Base(0)_Secondary(1);1;0;Create;True;0;0;0;False;0;False;0;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;340;-2814.414,743.6582;Inherit;False;Property;_Catalyst_Specs_Intensity;Catalyst_Specs_Intensity;55;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;301;-4407.116,2919.555;Inherit;False;ElementVoronoi_Scale_1;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;106;-4692.489,1022.972;Inherit;True;Fresnel_BottomCull;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;363;-2724.345,-177.4725;Inherit;False;Property;_ToggleCatalystFresColours_Base0_Secondary1;ToggleCatalystFresColours_Base(0)_Secondary(1);2;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;357;-2887.479,1974.153;Inherit;False;281;SecondaryColours_Elements;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;329;-2564.316,1730.531;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;296;-4984.643,2584.762;Inherit;False;CatalystVoronoi_Scale_2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;324;-1523.443,-575.4456;Inherit;False;294;CatalystVoronoi_Speed_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;267;-2912.027,2968.728;Inherit;False;Property;_Element_Specs_Intensity;Element_Specs_Intensity;33;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;138;-3511.992,1489.416;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;502;-2904.8,3053.307;Inherit;False;72;Base_BottomCull;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;335;-2332.654,-497.8755;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;361;-2691.015,-324.7616;Inherit;False;30;BaseColours_Cataylst;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;304;-1611.145,1499.304;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;500;-2635.8,3022.307;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;365;-2445.153,-343.1156;Inherit;False;105;Fresnel_TopCull;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;512;-2349.17,887.5609;Inherit;False;Property;_Toggle_Cataylst_Spec_On0_Off1;Toggle_Cataylst_Spec_On(0)_Off(1);20;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;98;-2302.137,1654.722;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;-2888.319,-459.1915;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;506;-2447.7,3019.382;Inherit;False;Constant;_Float3;Float 3;61;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;507;-2322.7,3018.382;Inherit;False;Constant;_Float4;Float 4;61;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;364;-2425.163,-266.2735;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-2881.159,-659.6592;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;508;-2498.7,3117.382;Inherit;False;Property;_Toggle_Element_Spec_On0_Off1;Toggle_Element_Spec_On(0)_Off(1);19;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;95;-2125.285,-569.0056;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;511;-2283.17,788.5609;Inherit;False;Constant;_Float6;Float 6;61;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;130;-3257.326,1513.896;Inherit;True;Property;_ElementTexture_Slot_3;ElementTexture_Slot_3;28;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;320;-1693.325,1660.891;Inherit;False;301;ElementVoronoi_Scale_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;314;-1275.491,-669.4174;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;354;-2555.121,1962.51;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;358;-2591.428,1882.311;Inherit;False;106;Fresnel_BottomCull;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;131;-3250.562,1710.056;Inherit;True;Property;_ElementTexture_Slot_4;ElementTexture_Slot_4;29;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;510;-2158.17,787.5609;Inherit;False;Constant;_Float5;Float 5;61;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;377;-1913.085,18.1277;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;408;-2049.924,2248.592;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;514;-2498.097,800.0707;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;339;-2478.975,575.8062;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;384;-1924.525,228.8013;Inherit;False;Constant;_Catalyst_Base_Intensity;Catalyst_Base_Intensity;49;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;265;-2648.588,2804.878;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;306;-1443.715,1494.94;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;323;-1522.443,-500.4455;Inherit;False;296;CatalystVoronoi_Scale_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;410;-2061.364,2459.265;Inherit;False;Constant;_Element_Base_Intensity;Element_Base_Intensity;49;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;505;-2162.7,2991.382;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;430;-1896.012,-468.3505;Inherit;False;Catalyst_Base_Colour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;428;-2063.404,1742.773;Inherit;False;Element_Base_Colour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;203;-2708.46,-652.3655;Inherit;False;CatalystTex_4;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;409;-1788.363,2255.265;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;202;-2711.157,-727.9122;Inherit;False;CatalystTex_3;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;509;-1998.171,760.5609;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;360;-2128.161,-353.0825;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;503;-2399.8,2804.307;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;108;-6097.442,1355.823;Inherit;False;1687.638;473.1558;Disolve;12;78;79;81;82;83;84;85;87;89;92;93;94;;0,0.9444442,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;492;-623.1014,-1152.216;Inherit;False;2944.291;1369.31;Combine;59;4;3;2;481;450;451;447;448;479;484;482;456;454;438;427;442;435;439;443;436;432;459;483;434;453;485;455;440;441;458;433;457;444;437;452;449;446;445;470;471;462;469;473;464;465;468;475;467;472;463;474;466;10;9;8;517;518;520;519;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;204;-2881.422,1471.503;Inherit;False;ElementTex_3;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;383;-1651.524,24.80116;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;355;-2303.013,1869.917;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;205;-2878.725,1547.049;Inherit;False;ElementTex_4;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;515;-2235.271,573.4857;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;313;-1115.681,-709.685;Inherit;False;0;0;1;3;8;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.VoronoiNode;305;-1234.442,1411.75;Inherit;True;0;0;1;3;8;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.GetLocalVarNode;481;-573.1014,-626.2665;Inherit;False;428;Element_Base_Colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;431;-1893.295,-381.3644;Inherit;False;Catalyst_Fresnel_Colour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;398;-1439.051,28.50233;Inherit;False;Catalyst_Base;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;479;-548.1064,29.66208;Inherit;False;430;Catalyst_Base_Colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;504;-2058.7,2809.382;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;84;-6047.442,1413.763;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;513;-1894.171,578.5609;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;326;-936.5965,-713.3793;Inherit;True;CatalystVoronoi_2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;429;-2060.687,1829.759;Inherit;False;Element_Fresnel_Colour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;448;-567.1994,-702.0188;Inherit;False;205;ElementTex_4;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;447;-567.0922,-778.1253;Inherit;False;204;ElementTex_3;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;424;-1575.89,2258.966;Inherit;False;Element_Base;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;328;-1045.507,1406.479;Inherit;True;ElementVoronoi_2;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;450;-503.9629,-128.0503;Inherit;False;202;CatalystTex_3;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;451;-503.9629,-51.68936;Inherit;False;203;CatalystTex_4;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;348;-1723.021,576.2455;Inherit;False;CatalystBrightParts;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;443;-41.61502,-406.104;Inherit;False;398;Catalyst_Base;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;435;-86.71613,-951.424;Inherit;False;429;Element_Fresnel_Colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;266;-1809.904,2804.698;Inherit;False;ElementBrightParts;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;442;-60.69902,-480.8331;Inherit;False;430;Catalyst_Base_Colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.TransformPositionNode;92;-5871.605,1405.823;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;456;-305.5284,-788.2123;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;427;-84.87213,-1102.216;Inherit;False;428;Element_Base_Colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;484;-297.6265,86.92074;Inherit;False;True;True;True;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;482;-304.2334,-573.7567;Inherit;False;True;True;True;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;439;-42.866,-255.9771;Inherit;False;326;CatalystVoronoi_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;432;-65.78813,-1027.486;Inherit;False;424;Element_Base;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;436;-67.03911,-877.3599;Inherit;False;328;ElementVoronoi_2;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;454;-299.7281,-131.4741;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;438;-62.54301,-330.042;Inherit;False;431;Catalyst_Fresnel_Colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;483;-75.99761,-787.6455;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;434;181.9431,-934.2826;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;433;185.5151,-1027.231;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.BreakToComponentsNode;78;-5660.145,1415.146;Inherit;True;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;440;206.1162,-312.9001;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;441;209.6882,-405.849;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;485;-54.62651,-114.0792;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;517;166.3047,-116.8505;Inherit;False;348;CatalystBrightParts;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;459;179.3163,-839.2915;Inherit;False;Constant;_Float1;Float 1;57;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;458;-67.63579,101.0947;Inherit;False;Property;_Toggle_UseCatalystTex_3__4;Toggle_UseCatalystTex_3_&_4;18;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;455;-75.23467,-568.1408;Inherit;False;Property;_Toggle_UseElementTex_3__4;Toggle_UseElementTex_3_&_4;15;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;453;-53.575,-186.0341;Inherit;False;Constant;_ConstantBlack;Constant Black;57;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;519;153.344,-662.0097;Inherit;False;266;ElementBrightParts;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;444;373.3691,-384.329;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;520;432.8441,-658.1099;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;437;336.2612,-998.7109;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;457;400.2124,-769.9551;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TFHCRemapNode;79;-5446.859,1435.497;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;-1;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;452;378.9253,-287.3951;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;518;392.5048,-159.7505;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-5568.47,1712.979;Inherit;False;Property;_ToggleOverallFade;ToggleOverallFade;9;0;Create;True;0;0;0;False;0;False;0;0;0;100;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;87;-5262.997,1524.726;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;449;687.7386,-639.0197;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;446;685.6805,-870.5376;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-5570.469,1632.979;Inherit;False;Property;_ToggleOverallCull;ToggleOverallCull;10;0;Create;True;0;0;0;False;0;False;0;0;-2;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;94;-5280.769,1430.38;Inherit;False;61;ElementNoise_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;445;921.9968,-750.8469;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-5074.3,1649.559;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;89;-5079.517,1483.365;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;85;-4889.248,1488.066;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1.63;False;2;FLOAT;1.54;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;471;538.1865,-66.0861;Inherit;False;Property;_Toggle_UseVertexNoise_Catalyst0_Element1;Toggle_UseVertexNoise_Catalyst(0)_Element(1);21;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;469;631.4346,-213.8071;Inherit;False;227;CatalystNoise_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;297;-5716.331,-659.4886;Inherit;False;VertexAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;470;630.4346,-140.8071;Inherit;False;61;ElementNoise_1;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCGrayscale;462;1146.668,-629.8766;Inherit;True;2;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;93;-4633.805,1494.475;Inherit;False;DisolveAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;473;629.8297,13.35383;Inherit;False;Property;_VertOffset_Intensity;VertOffset_Intensity;22;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;465;1343.198,-579.5294;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0.01;False;4;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;464;1570.772,-445.0849;Inherit;False;297;VertexAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;468;853.4355,-159.8071;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;463;1785.772,-574.0847;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;472;1015.831,-157.6461;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;475;885.8296,-326.646;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;467;1775.444,-479.3043;Inherit;False;93;DisolveAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;466;1968.444,-575.3043;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;474;1163.83,-196.6461;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;9;2097.189,-580.1801;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;10;1292.347,-198.7201;Inherit;False;VertexOffset;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;8;1172.811,-753.3176;Inherit;False;Colour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;504.8409,1582.586;Inherit;False;9;Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;5;503.8409,1504.585;Inherit;False;8;Colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;7;488.8409,1654.585;Inherit;False;10;VertexOffset;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;898.598,1551.434;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;Shaders_VFX_Orbs;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;1;637937978201807975;  Blend;0;0;Two Sided;1;0;Cast Shadows;1;0;  Use Shadow Threshold;0;0;Receive Shadows;1;0;GPU Instancing;1;0;LOD CrossFade;0;0;Built-in Fog;0;0;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Vertex Position,InvertActionOnDeselection;1;0;0;5;False;True;True;True;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;898.598,1551.434;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;116;0;114;0
WireConnection;116;1;177;1
WireConnection;115;0;114;0
WireConnection;115;1;177;2
WireConnection;119;0;115;0
WireConnection;120;0;116;0
WireConnection;140;0;144;1
WireConnection;140;1;144;2
WireConnection;141;0;144;3
WireConnection;141;1;144;4
WireConnection;149;0;141;0
WireConnection;209;0;211;3
WireConnection;209;1;211;4
WireConnection;168;0;175;3
WireConnection;168;1;175;4
WireConnection;148;0;140;0
WireConnection;208;0;211;1
WireConnection;208;1;211;2
WireConnection;229;0;230;1
WireConnection;229;1;230;2
WireConnection;231;0;230;3
WireConnection;231;1;230;4
WireConnection;121;0;119;0
WireConnection;122;0;120;0
WireConnection;167;0;175;1
WireConnection;167;1;175;2
WireConnection;172;0;168;0
WireConnection;232;0;231;0
WireConnection;497;0;122;0
WireConnection;497;1;498;0
WireConnection;497;2;499;0
WireConnection;213;0;209;0
WireConnection;171;0;167;0
WireConnection;212;0;208;0
WireConnection;233;0;229;0
WireConnection;496;0;121;0
WireConnection;496;1;498;0
WireConnection;496;2;499;0
WireConnection;46;0;38;0
WireConnection;125;0;497;0
WireConnection;197;0;191;0
WireConnection;197;2;193;0
WireConnection;237;0;234;3
WireConnection;216;0;218;0
WireConnection;238;0;234;4
WireConnection;196;0;191;0
WireConnection;196;2;194;0
WireConnection;124;0;496;0
WireConnection;235;0;234;1
WireConnection;236;0;234;2
WireConnection;133;1;197;0
WireConnection;136;0;143;0
WireConnection;136;2;152;0
WireConnection;132;1;196;0
WireConnection;163;0;46;0
WireConnection;163;2;215;0
WireConnection;163;1;47;0
WireConnection;300;0;298;2
WireConnection;137;0;143;0
WireConnection;137;2;185;0
WireConnection;224;0;216;0
WireConnection;224;2;219;0
WireConnection;224;1;217;0
WireConnection;222;0;216;0
WireConnection;222;2;220;0
WireConnection;222;1;217;0
WireConnection;293;0;292;1
WireConnection;52;0;46;0
WireConnection;52;2;214;0
WireConnection;52;1;47;0
WireConnection;226;0;224;0
WireConnection;226;1;239;0
WireConnection;129;1;137;0
WireConnection;153;0;132;0
WireConnection;153;1;154;0
WireConnection;155;0;133;0
WireConnection;155;1;156;0
WireConnection;35;0;32;0
WireConnection;35;1;33;2
WireConnection;128;1;136;0
WireConnection;302;0;298;4
WireConnection;165;0;163;0
WireConnection;165;1;242;0
WireConnection;295;0;292;3
WireConnection;225;0;222;0
WireConnection;225;1;240;0
WireConnection;59;0;52;0
WireConnection;59;1;241;0
WireConnection;34;0;32;0
WireConnection;34;1;33;1
WireConnection;61;0;59;0
WireConnection;36;0;35;0
WireConnection;228;0;225;0
WireConnection;166;0;165;0
WireConnection;310;0;308;0
WireConnection;310;1;321;0
WireConnection;207;0;128;0
WireConnection;206;0;129;0
WireConnection;285;0;288;0
WireConnection;285;1;317;0
WireConnection;246;0;245;0
WireConnection;37;0;34;0
WireConnection;201;0;155;0
WireConnection;227;0;226;0
WireConnection;200;0;153;0
WireConnection;289;0;284;0
WireConnection;289;1;285;0
WireConnection;289;2;318;0
WireConnection;45;0;36;0
WireConnection;309;0;315;0
WireConnection;309;1;310;0
WireConnection;309;2;322;0
WireConnection;39;0;37;0
WireConnection;49;0;41;0
WireConnection;247;0;246;1
WireConnection;247;1;250;0
WireConnection;50;0;45;0
WireConnection;50;3;42;0
WireConnection;50;4;40;0
WireConnection;393;0;395;0
WireConnection;393;1;396;0
WireConnection;389;0;390;0
WireConnection;389;1;388;0
WireConnection;419;0;421;0
WireConnection;419;1;422;0
WireConnection;411;0;415;0
WireConnection;411;1;416;0
WireConnection;51;0;39;0
WireConnection;51;3;44;0
WireConnection;51;4;43;0
WireConnection;325;0;309;0
WireConnection;327;0;289;0
WireConnection;248;0;247;0
WireConnection;55;0;49;0
WireConnection;117;0;114;0
WireConnection;117;1;177;3
WireConnection;53;2;51;0
WireConnection;53;3;50;0
WireConnection;394;0;392;0
WireConnection;394;1;393;0
WireConnection;394;2;397;0
WireConnection;249;0;248;0
WireConnection;178;0;114;0
WireConnection;178;1;177;4
WireConnection;412;0;414;0
WireConnection;412;1;411;0
WireConnection;412;2;413;0
WireConnection;420;0;418;0
WireConnection;420;1;419;0
WireConnection;420;2;423;0
WireConnection;387;0;374;0
WireConnection;387;1;389;0
WireConnection;387;2;391;0
WireConnection;58;0;55;1
WireConnection;368;0;387;0
WireConnection;368;1;372;1
WireConnection;368;2;372;2
WireConnection;368;3;372;3
WireConnection;368;4;372;4
WireConnection;147;0;145;3
WireConnection;147;1;145;4
WireConnection;256;0;254;0
WireConnection;256;1;268;1
WireConnection;256;2;268;2
WireConnection;256;3;268;3
WireConnection;256;4;268;4
WireConnection;400;0;412;0
WireConnection;400;1;426;1
WireConnection;400;2;426;2
WireConnection;400;3;426;3
WireConnection;400;4;426;4
WireConnection;118;0;117;0
WireConnection;179;0;178;0
WireConnection;257;1;254;0
WireConnection;257;2;252;0
WireConnection;347;0;352;0
WireConnection;347;1;341;1
WireConnection;347;2;341;2
WireConnection;347;3;341;3
WireConnection;347;4;341;4
WireConnection;369;0;394;0
WireConnection;369;1;373;1
WireConnection;369;2;373;2
WireConnection;369;3;373;3
WireConnection;369;4;373;4
WireConnection;64;0;58;0
WireConnection;60;0;53;0
WireConnection;345;1;352;0
WireConnection;345;2;351;0
WireConnection;399;0;420;0
WireConnection;399;1;417;1
WireConnection;399;2;417;2
WireConnection;399;3;417;3
WireConnection;399;4;417;4
WireConnection;259;1;253;0
WireConnection;259;2;255;0
WireConnection;146;0;145;1
WireConnection;146;1;145;2
WireConnection;346;1;336;0
WireConnection;346;2;349;0
WireConnection;26;0;29;0
WireConnection;26;1;25;0
WireConnection;261;0;256;0
WireConnection;261;1;257;0
WireConnection;278;0;29;0
WireConnection;278;1;280;0
WireConnection;260;0;259;0
WireConnection;260;1;258;0
WireConnection;277;0;29;0
WireConnection;277;1;279;0
WireConnection;170;0;176;3
WireConnection;170;1;176;4
WireConnection;123;0;118;0
WireConnection;150;0;146;0
WireConnection;68;0;64;0
WireConnection;68;1;65;0
WireConnection;68;2;66;0
WireConnection;401;0;399;0
WireConnection;401;1;400;0
WireConnection;27;0;29;0
WireConnection;27;1;28;0
WireConnection;69;0;58;0
WireConnection;69;1;62;0
WireConnection;69;2;63;0
WireConnection;338;0;347;0
WireConnection;338;1;345;0
WireConnection;180;0;179;0
WireConnection;370;0;368;0
WireConnection;370;1;369;0
WireConnection;344;0;346;0
WireConnection;344;1;350;0
WireConnection;151;0;147;0
WireConnection;169;0;176;1
WireConnection;169;1;176;2
WireConnection;67;0;60;0
WireConnection;493;0;180;0
WireConnection;493;1;498;0
WireConnection;493;2;499;0
WireConnection;403;0;401;0
WireConnection;403;1;402;1
WireConnection;403;2;402;2
WireConnection;403;3;402;3
WireConnection;403;4;402;4
WireConnection;30;0;27;0
WireConnection;262;0;261;0
WireConnection;262;1;260;0
WireConnection;281;0;277;0
WireConnection;282;0;278;0
WireConnection;495;0;123;0
WireConnection;495;1;498;0
WireConnection;495;2;499;0
WireConnection;173;0;169;0
WireConnection;72;0;69;0
WireConnection;174;0;170;0
WireConnection;371;0;370;0
WireConnection;371;1;376;1
WireConnection;371;2;376;2
WireConnection;371;3;376;3
WireConnection;371;4;376;4
WireConnection;19;0;26;0
WireConnection;337;0;338;0
WireConnection;337;1;344;0
WireConnection;71;0;68;0
WireConnection;181;0;493;0
WireConnection;299;0;298;1
WireConnection;74;0;72;0
WireConnection;74;1;70;0
WireConnection;294;0;292;2
WireConnection;198;0;191;0
WireConnection;198;2;192;0
WireConnection;126;0;495;0
WireConnection;73;0;71;0
WireConnection;73;1;70;0
WireConnection;199;0;191;0
WireConnection;199;2;195;0
WireConnection;382;0;371;0
WireConnection;382;1;380;0
WireConnection;406;0;403;0
WireConnection;406;1;405;0
WireConnection;342;0;337;0
WireConnection;263;0;262;0
WireConnection;264;0;261;0
WireConnection;264;1;263;0
WireConnection;105;0;73;0
WireConnection;378;0;379;0
WireConnection;378;1;382;0
WireConnection;378;2;381;0
WireConnection;407;0;404;0
WireConnection;407;1;406;0
WireConnection;407;2;425;0
WireConnection;134;1;198;0
WireConnection;182;1;199;0
WireConnection;139;0;143;0
WireConnection;139;2;189;0
WireConnection;343;0;338;0
WireConnection;343;1;342;0
WireConnection;301;0;298;3
WireConnection;106;0;74;0
WireConnection;329;0;18;0
WireConnection;329;1;330;0
WireConnection;329;2;331;0
WireConnection;296;0;292;4
WireConnection;138;0;143;0
WireConnection;138;2;188;0
WireConnection;335;0;334;0
WireConnection;335;1;332;0
WireConnection;335;2;333;0
WireConnection;500;0;502;0
WireConnection;98;0;97;0
WireConnection;98;1;329;0
WireConnection;183;0;182;0
WireConnection;183;1;184;0
WireConnection;364;0;361;0
WireConnection;364;1;362;0
WireConnection;364;2;363;0
WireConnection;157;0;134;0
WireConnection;157;1;158;0
WireConnection;95;0;96;0
WireConnection;95;1;335;0
WireConnection;130;1;138;0
WireConnection;314;0;312;0
WireConnection;314;1;324;0
WireConnection;354;0;356;0
WireConnection;354;1;357;0
WireConnection;354;2;359;0
WireConnection;131;1;139;0
WireConnection;377;0;371;0
WireConnection;377;1;378;0
WireConnection;408;0;403;0
WireConnection;408;1;407;0
WireConnection;514;0;501;0
WireConnection;339;0;343;0
WireConnection;339;1;340;0
WireConnection;265;0;264;0
WireConnection;265;1;267;0
WireConnection;306;0;304;0
WireConnection;306;1;319;0
WireConnection;505;0;506;0
WireConnection;505;1;507;0
WireConnection;505;2;508;0
WireConnection;430;0;95;0
WireConnection;428;0;98;0
WireConnection;203;0;183;0
WireConnection;409;0;408;0
WireConnection;409;1;410;0
WireConnection;202;0;157;0
WireConnection;509;0;511;0
WireConnection;509;1;510;0
WireConnection;509;2;512;0
WireConnection;360;0;365;0
WireConnection;360;1;364;0
WireConnection;503;0;265;0
WireConnection;503;1;500;0
WireConnection;204;0;130;0
WireConnection;383;0;377;0
WireConnection;383;1;384;0
WireConnection;355;0;358;0
WireConnection;355;1;354;0
WireConnection;205;0;131;0
WireConnection;515;0;339;0
WireConnection;515;1;514;0
WireConnection;313;0;315;0
WireConnection;313;1;314;0
WireConnection;313;2;323;0
WireConnection;305;0;284;0
WireConnection;305;1;306;0
WireConnection;305;2;320;0
WireConnection;431;0;360;0
WireConnection;398;0;383;0
WireConnection;504;0;503;0
WireConnection;504;1;505;0
WireConnection;513;0;515;0
WireConnection;513;1;509;0
WireConnection;326;0;313;0
WireConnection;429;0;355;0
WireConnection;424;0;409;0
WireConnection;328;0;305;0
WireConnection;348;0;513;0
WireConnection;266;0;504;0
WireConnection;92;0;84;0
WireConnection;456;0;447;0
WireConnection;456;1;448;0
WireConnection;484;0;479;0
WireConnection;482;0;481;0
WireConnection;454;0;450;0
WireConnection;454;1;451;0
WireConnection;483;0;456;0
WireConnection;483;1;482;0
WireConnection;434;0;435;0
WireConnection;434;1;436;0
WireConnection;433;0;427;0
WireConnection;433;1;432;0
WireConnection;78;0;92;0
WireConnection;440;0;438;0
WireConnection;440;1;439;0
WireConnection;441;0;442;0
WireConnection;441;1;443;0
WireConnection;485;0;454;0
WireConnection;485;1;484;0
WireConnection;444;0;441;0
WireConnection;444;1;440;0
WireConnection;520;0;427;0
WireConnection;520;1;519;0
WireConnection;437;0;433;0
WireConnection;437;1;434;0
WireConnection;457;0;459;0
WireConnection;457;1;483;0
WireConnection;457;2;455;0
WireConnection;79;0;78;1
WireConnection;452;0;453;0
WireConnection;452;1;485;0
WireConnection;452;2;458;0
WireConnection;518;0;442;0
WireConnection;518;1;517;0
WireConnection;87;0;79;0
WireConnection;449;0;444;0
WireConnection;449;1;452;0
WireConnection;449;2;518;0
WireConnection;446;0;437;0
WireConnection;446;1;457;0
WireConnection;446;2;520;0
WireConnection;445;0;446;0
WireConnection;445;1;449;0
WireConnection;82;0;81;0
WireConnection;82;1;83;0
WireConnection;89;0;94;0
WireConnection;89;1;87;0
WireConnection;85;0;89;0
WireConnection;85;1;81;0
WireConnection;85;2;82;0
WireConnection;297;0;29;4
WireConnection;462;0;445;0
WireConnection;93;0;85;0
WireConnection;465;0;462;0
WireConnection;468;0;469;0
WireConnection;468;1;470;0
WireConnection;468;2;471;0
WireConnection;463;0;465;0
WireConnection;463;1;464;0
WireConnection;472;0;468;0
WireConnection;472;1;473;0
WireConnection;466;0;463;0
WireConnection;466;1;467;0
WireConnection;474;0;475;0
WireConnection;474;1;472;0
WireConnection;9;0;466;0
WireConnection;10;0;474;0
WireConnection;8;0;445;0
WireConnection;1;2;5;0
WireConnection;1;3;6;0
WireConnection;1;5;7;0
ASEEND*/
//CHKSM=708687481EC9E45972ACBDD0ED2FBCA88899E723
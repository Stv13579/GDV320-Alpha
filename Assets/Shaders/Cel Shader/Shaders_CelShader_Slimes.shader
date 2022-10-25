// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shaders_CelShader_Slimes"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_IsUsingEyes("IsUsingEyes", Range( 0 , 1)) = 0
		_Unique_Eye_On("Unique_Eye_On", Range( 0 , 1)) = 0
		_IsBeingDamaged("IsBeingDamaged", Range( 0 , 1)) = 0
		_IsBeingStunned("IsBeingStunned", Range( 0 , 1)) = 0
		_BlinkSpeed("BlinkSpeed", Float) = 1
		_BlinkingEmissiveRate("BlinkingEmissiveRate", Float) = 0.3
		_Toggle_EnemyHPEmissive("Toggle_EnemyHPEmissive", Range( 0 , 1)) = 1
		_Diffuse_Colour("Diffuse_Colour", Color) = (1,1,1,0)
		[HDR]_Damage_Colour("Damage_Colour", Color) = (2,0,0,0)
		_Spec_Smoothness("Spec_Smoothness", Range( 0 , 1)) = 0.954
		[HDR]_Spec_Colour("Spec_Colour", Color) = (1,1,1,0)
		_FresnelScale("FresnelScale", Float) = 0.65
		_Highlight_Strenght("Highlight_Strenght", Float) = 1
		_Rim_Strenght("Rim_Strenght", Float) = 1
		_SubSurface_Stenght("SubSurface_Stenght", Range( 0 , 10)) = 10
		_SSS_Fres_Scale("SSS_Fres_Scale", Float) = 11
		_SSS_Fres_Power("SSS_Fres_Power", Float) = 3
		_Rim_Alpha("Rim_Alpha", Range( 0 , 1)) = 1
		_SubSurf_Alpha("SubSurf_Alpha", Range( 0 , 1)) = 1
		_Specular_Alpha("Specular_Alpha", Range( 0 , 1)) = 1
		_Outline_Colour("Outline_Colour", Color) = (0,0,0,0)
		_Outline_Width("Outline_Width", Range( 0 , 0.1)) = 0.01
		_Outline_Distance("Outline_Distance", Float) = 20
		_Outline_Alpha("Outline_Alpha", Range( 0 , 1)) = 1
		_Overall_Smoothness("Overall_Smoothness", Range( 0 , 1)) = 0
		_DiffuseBody("Diffuse Body", 2D) = "white" {}
		_DiffuseNormalEyes("Diffuse Normal Eyes", 2D) = "white" {}
		_DiffuseBlinkEyes("Diffuse Blink Eyes", 2D) = "white" {}
		_DiffuseHurtEyes("Diffuse Hurt Eyes", 2D) = "white" {}
		_DiffuseStunEyes("Diffuse Stun Eyes", 2D) = "white" {}
		_DiffuseDeathEyes("Diffuse Death Eyes", 2D) = "white" {}
		_EmissiveBody("Emissive Body", 2D) = "white" {}
		_EmmisiveNormalEyes("Emmisive Normal Eyes", 2D) = "white" {}
		_EmmisiveBlinkEyes("Emmisive Blink Eyes", 2D) = "white" {}
		_EmmisiveHurtEyes("Emmisive Hurt Eyes", 2D) = "white" {}
		_EmmisiveStunEyes("Emmisive Stun Eyes", 2D) = "white" {}
		_EmmisiveDeathEyes("Emmisive Death Eyes", 2D) = "white" {}
		_DisolveEdgeThickness("DisolveEdgeThickness", Float) = 0.01
		[HDR]_DisolveColour("DisolveColour", Color) = (0,0,0,0)
		_DisolveNoiseScale("DisolveNoiseScale", Float) = 3
		[ASEEnd]_DisolveAlphaClipping("DisolveAlphaClipping", Range( -1 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
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

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry" }
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
			Name "ExtraPrePass"
			
			
			Blend One Zero
			Cull Front
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#define _EMISSION
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70701

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_EXTRA_PREPASS

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
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
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _DiffuseBlinkEyes_ST;
			float4 _EmmisiveNormalEyes_ST;
			float4 _DiffuseStunEyes_ST;
			float4 _DiffuseDeathEyes_ST;
			float4 _EmmisiveBlinkEyes_ST;
			float4 _DiffuseNormalEyes_ST;
			float4 _EmmisiveDeathEyes_ST;
			float4 _DiffuseHurtEyes_ST;
			float4 _Damage_Colour;
			float4 _EmmisiveStunEyes_ST;
			float4 _EmmisiveHurtEyes_ST;
			float4 _Outline_Colour;
			float4 _DisolveColour;
			float4 _Spec_Colour;
			float4 _DiffuseBody_ST;
			float4 _Diffuse_Colour;
			float4 _EmissiveBody_ST;
			float _DisolveEdgeThickness;
			float _Outline_Width;
			float _Toggle_EnemyHPEmissive;
			float _Rim_Alpha;
			float _Spec_Smoothness;
			float _BlinkingEmissiveRate;
			float _Rim_Strenght;
			float _SSS_Fres_Scale;
			float _FresnelScale;
			float _SubSurf_Alpha;
			float _SSS_Fres_Power;
			float _Specular_Alpha;
			float _SubSurface_Stenght;
			float _IsUsingEyes;
			float _Unique_Eye_On;
			float _IsBeingStunned;
			float _BlinkSpeed;
			float _IsBeingDamaged;
			float _DisolveAlphaClipping;
			float _DisolveNoiseScale;
			float _Outline_Alpha;
			float _Outline_Distance;
			float _Highlight_Strenght;
			float _Overall_Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _DiffuseBody;


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
			

			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float2 uv_DiffuseBody = v.ase_texcoord.xy * _DiffuseBody_ST.xy + _DiffuseBody_ST.zw;
				float4 tex2DNode206 = tex2Dlod( _DiffuseBody, float4( uv_DiffuseBody, 0, 0.0) );
				float Texture_Alpha101 = tex2DNode206.a;
				float4 unityObjectToClipPos104 = TransformWorldToHClip(TransformObjectToWorld(v.vertex.xyz));
				float3 Outline96 = ( v.ase_normal * _Outline_Width * Texture_Alpha101 * max( unityObjectToClipPos104.w , _Outline_Distance ) );
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = ( Outline96 * _Outline_Alpha );
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
				float3 worldToObj319 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float simplePerlin3D317 = snoise( worldToObj319*_DisolveNoiseScale );
				simplePerlin3D317 = simplePerlin3D317*0.5 + 0.5;
				float Alpha322 = simplePerlin3D317;
				
				float AlphaClipping323 = _DisolveAlphaClipping;
				
				float3 Color = _Outline_Colour.rgb;
				float Alpha = Alpha322;
				float AlphaClipThreshold = AlphaClipping323;

				#ifdef _ALPHATEST_ON
					clip( Alpha - AlphaClipThreshold );
				#endif

				#ifdef ASE_FOG
					Color = MixFog( Color, IN.fogFactor );
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				return half4( Color, Alpha );
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" }
			
			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#define _EMISSION
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70701

			
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#define ASE_NEEDS_VERT_TEXTURE_COORDINATES1
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord : TEXCOORD0;
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _DiffuseBlinkEyes_ST;
			float4 _EmmisiveNormalEyes_ST;
			float4 _DiffuseStunEyes_ST;
			float4 _DiffuseDeathEyes_ST;
			float4 _EmmisiveBlinkEyes_ST;
			float4 _DiffuseNormalEyes_ST;
			float4 _EmmisiveDeathEyes_ST;
			float4 _DiffuseHurtEyes_ST;
			float4 _Damage_Colour;
			float4 _EmmisiveStunEyes_ST;
			float4 _EmmisiveHurtEyes_ST;
			float4 _Outline_Colour;
			float4 _DisolveColour;
			float4 _Spec_Colour;
			float4 _DiffuseBody_ST;
			float4 _Diffuse_Colour;
			float4 _EmissiveBody_ST;
			float _DisolveEdgeThickness;
			float _Outline_Width;
			float _Toggle_EnemyHPEmissive;
			float _Rim_Alpha;
			float _Spec_Smoothness;
			float _BlinkingEmissiveRate;
			float _Rim_Strenght;
			float _SSS_Fres_Scale;
			float _FresnelScale;
			float _SubSurf_Alpha;
			float _SSS_Fres_Power;
			float _Specular_Alpha;
			float _SubSurface_Stenght;
			float _IsUsingEyes;
			float _Unique_Eye_On;
			float _IsBeingStunned;
			float _BlinkSpeed;
			float _IsBeingDamaged;
			float _DisolveAlphaClipping;
			float _DisolveNoiseScale;
			float _Outline_Alpha;
			float _Outline_Distance;
			float _Highlight_Strenght;
			float _Overall_Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _DiffuseBody;
			sampler2D _DiffuseNormalEyes;
			sampler2D _DiffuseBlinkEyes;
			sampler2D _DiffuseDeathEyes;
			sampler2D _DiffuseStunEyes;
			sampler2D _DiffuseHurtEyes;
			sampler2D _EmissiveBody;
			sampler2D _EmmisiveNormalEyes;
			sampler2D _EmmisiveBlinkEyes;
			sampler2D _EmmisiveDeathEyes;
			sampler2D _EmmisiveStunEyes;
			sampler2D _EmmisiveHurtEyes;


			float3 ASEIndirectDiffuse( float2 uvStaticLightmap, float3 normalWS )
			{
			#ifdef LIGHTMAP_ON
				return SampleLightmap( uvStaticLightmap, normalWS );
			#else
				return SampleSH(normalWS);
			#endif
			}
			
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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord7.xy = v.texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord7.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				
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
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				
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
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				
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

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag ( VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				float dotResult4 = dot( WorldNormal , _MainLightPosition.xyz );
				float ase_lightAtten = 0;
				Light ase_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_mainLight.distanceAttenuation * ase_mainLight.shadowAttenuation;
				float temp_output_7_0 = min( saturate( dotResult4 ) , ase_lightAtten );
				float temp_output_3_0_g13 = ( temp_output_7_0 - 0.1 );
				float temp_output_9_0 = saturate( ( temp_output_3_0_g13 / fwidth( temp_output_3_0_g13 ) ) );
				float3 bakedGI13 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, WorldNormal);
				MixRealtimeAndBakedGI(ase_mainLight, WorldNormal, bakedGI13, half4(0,0,0,0));
				float2 uv_DiffuseBody = IN.ase_texcoord7.xy * _DiffuseBody_ST.xy + _DiffuseBody_ST.zw;
				float4 tex2DNode206 = tex2D( _DiffuseBody, uv_DiffuseBody );
				float DamageValue154 = _IsBeingDamaged;
				float4 lerpResult155 = lerp( _Diffuse_Colour , _Damage_Colour , DamageValue154);
				float4 ColourChange304 = lerpResult155;
				float4 temp_output_308_0 = ( tex2DNode206 * ColourChange304 );
				float2 uv_DiffuseNormalEyes = IN.ase_texcoord7.xy * _DiffuseNormalEyes_ST.xy + _DiffuseNormalEyes_ST.zw;
				float4 tex2DNode207 = tex2D( _DiffuseNormalEyes, uv_DiffuseNormalEyes );
				float2 uv_DiffuseBlinkEyes = IN.ase_texcoord7.xy * _DiffuseBlinkEyes_ST.xy + _DiffuseBlinkEyes_ST.zw;
				float4 tex2DNode208 = tex2D( _DiffuseBlinkEyes, uv_DiffuseBlinkEyes );
				float temp_output_3_0_g12 = ( 0.2 - abs( sin( ( _TimeParameters.x * _BlinkSpeed ) ) ) );
				float BlinkingSystem148 = saturate( ( temp_output_3_0_g12 / fwidth( temp_output_3_0_g12 ) ) );
				float4 lerpResult236 = lerp( tex2DNode207 , tex2DNode208 , BlinkingSystem148);
				float2 uv_DiffuseDeathEyes = IN.ase_texcoord7.xy * _DiffuseDeathEyes_ST.xy + _DiffuseDeathEyes_ST.zw;
				float4 tex2DNode211 = tex2D( _DiffuseDeathEyes, uv_DiffuseDeathEyes );
				float2 uv_DiffuseStunEyes = IN.ase_texcoord7.xy * _DiffuseStunEyes_ST.xy + _DiffuseStunEyes_ST.zw;
				float4 tex2DNode210 = tex2D( _DiffuseStunEyes, uv_DiffuseStunEyes );
				float StunValue249 = _IsBeingStunned;
				float4 lerpResult250 = lerp( tex2DNode211 , tex2DNode210 , StunValue249);
				float2 uv_DiffuseHurtEyes = IN.ase_texcoord7.xy * _DiffuseHurtEyes_ST.xy + _DiffuseHurtEyes_ST.zw;
				float4 tex2DNode209 = tex2D( _DiffuseHurtEyes, uv_DiffuseHurtEyes );
				float4 lerpResult243 = lerp( lerpResult250 , tex2DNode209 , DamageValue154);
				float UniqueEyesToggle257 = _Unique_Eye_On;
				float4 lerpResult245 = lerp( lerpResult236 , lerpResult243 , UniqueEyesToggle257);
				float EyesOnOffSwitch277 = _IsUsingEyes;
				float4 lerpResult275 = lerp( lerpResult245 , float4( 0,0,0,0 ) , EyesOnOffSwitch277);
				float NormalEyesAlpha295 = tex2DNode207.a;
				float BlinkEyesAlpha296 = tex2DNode208.a;
				float lerpResult285 = lerp( NormalEyesAlpha295 , BlinkEyesAlpha296 , BlinkingSystem148);
				float DeathEyesAlpha312 = tex2DNode211.a;
				float StunEyesAlpha313 = tex2DNode210.a;
				float lerpResult289 = lerp( DeathEyesAlpha312 , StunEyesAlpha313 , StunValue249);
				float HurtEyeAlpha311 = tex2DNode209.a;
				float lerpResult286 = lerp( lerpResult289 , HurtEyeAlpha311 , DamageValue154);
				float lerpResult291 = lerp( lerpResult285 , lerpResult286 , UniqueEyesToggle257);
				float lerpResult293 = lerp( lerpResult291 , 0.0 , EyesOnOffSwitch277);
				float4 lerpResult284 = lerp( temp_output_308_0 , lerpResult275 , lerpResult293);
				float4 VDiffuse125 = lerpResult284;
				float4 Diffuse19 = ( float4( ( temp_output_9_0 + ( ( 1.0 - temp_output_9_0 ) * bakedGI13 ) ) , 0.0 ) * ( VDiffuse125 * ColourChange304 ) );
				float NormalDotLight64 = temp_output_7_0;
				float NormalDotLightStep39 = temp_output_9_0;
				float lerpResult71 = lerp( NormalDotLight64 , pow( ( 1.0 - NormalDotLight64 ) , _SubSurface_Stenght ) , NormalDotLightStep39);
				float4 VSubSurf127 = temp_output_308_0;
				float fresnelNdotV83 = dot( WorldNormal, WorldViewDirection );
				float fresnelNode83 = ( 0.0 + _SSS_Fres_Scale * pow( 1.0 - fresnelNdotV83, _SSS_Fres_Power ) );
				float4 SubsurfaceScattering80 = ( ( lerpResult71 * VSubSurf127 ) + ( fresnelNode83 * VSubSurf127 ) );
				float fresnelNdotV42 = dot( WorldNormal, WorldViewDirection );
				float fresnelNode42 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV42, 5.0 ) );
				float temp_output_3_0_g14 = ( fresnelNode42 - 0.1 );
				float temp_output_43_0 = saturate( ( temp_output_3_0_g14 / fwidth( temp_output_3_0_g14 ) ) );
				float3 bakedGI51 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, WorldNormal);
				MixRealtimeAndBakedGI(ase_mainLight, WorldNormal, bakedGI51, half4(0,0,0,0));
				float4 Highlight53 = ( ( NormalDotLightStep39 * temp_output_43_0 * _MainLightColor * _Highlight_Strenght ) + float4( ( ( 1.0 - NormalDotLightStep39 ) * temp_output_43_0 * bakedGI51 * _Rim_Strenght ) , 0.0 ) );
				float4 clampResult158 = clamp( ( Diffuse19 + ( SubsurfaceScattering80 * _SubSurf_Alpha ) + ( Highlight53 * _Rim_Alpha ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				
				float3 worldToObj319 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float simplePerlin3D317 = snoise( worldToObj319*_DisolveNoiseScale );
				simplePerlin3D317 = simplePerlin3D317*0.5 + 0.5;
				float4 DisolveEmissive331 = ( _DisolveColour * step( simplePerlin3D317 , ( _DisolveEdgeThickness + _DisolveAlphaClipping ) ) );
				float2 uv_EmissiveBody = IN.ase_texcoord7.xy * _EmissiveBody_ST.xy + _EmissiveBody_ST.zw;
				float4 tex2DNode235 = tex2D( _EmissiveBody, uv_EmissiveBody );
				float BreathingRate187 = abs( sin( ( _TimeParameters.x * _BlinkingEmissiveRate ) ) );
				float4 lerpResult255 = lerp( tex2DNode235 , ( tex2DNode235 * 3.0 ) , BreathingRate187);
				float HPFloat202 = _Toggle_EnemyHPEmissive;
				float2 uv_EmmisiveNormalEyes = IN.ase_texcoord7.xy * _EmmisiveNormalEyes_ST.xy + _EmmisiveNormalEyes_ST.zw;
				float2 uv_EmmisiveBlinkEyes = IN.ase_texcoord7.xy * _EmmisiveBlinkEyes_ST.xy + _EmmisiveBlinkEyes_ST.zw;
				float4 lerpResult259 = lerp( tex2D( _EmmisiveNormalEyes, uv_EmmisiveNormalEyes ) , tex2D( _EmmisiveBlinkEyes, uv_EmmisiveBlinkEyes ) , BlinkingSystem148);
				float2 uv_EmmisiveDeathEyes = IN.ase_texcoord7.xy * _EmmisiveDeathEyes_ST.xy + _EmmisiveDeathEyes_ST.zw;
				float2 uv_EmmisiveStunEyes = IN.ase_texcoord7.xy * _EmmisiveStunEyes_ST.xy + _EmmisiveStunEyes_ST.zw;
				float4 lerpResult269 = lerp( tex2D( _EmmisiveDeathEyes, uv_EmmisiveDeathEyes ) , tex2D( _EmmisiveStunEyes, uv_EmmisiveStunEyes ) , StunValue249);
				float2 uv_EmmisiveHurtEyes = IN.ase_texcoord7.xy * _EmmisiveHurtEyes_ST.xy + _EmmisiveHurtEyes_ST.zw;
				float4 lerpResult263 = lerp( lerpResult269 , tex2D( _EmmisiveHurtEyes, uv_EmmisiveHurtEyes ) , DamageValue154);
				float4 lerpResult271 = lerp( lerpResult259 , lerpResult263 , UniqueEyesToggle257);
				float4 lerpResult279 = lerp( lerpResult271 , float4( 0,0,0,0 ) , EyesOnOffSwitch277);
				float4 lerpResult310 = lerp( ( lerpResult255 * HPFloat202 ) , lerpResult279 , lerpResult293);
				float4 VEmissive126 = ( lerpResult310 * ColourChange304 );
				
				float3 normalizeResult23 = normalize( ( WorldViewDirection + _MainLightPosition.xyz ) );
				float dotResult24 = dot( normalizeResult23 , WorldNormal );
				float temp_output_3_0_g15 = ( pow( dotResult24 , exp2( ( ( _Spec_Smoothness * 10.0 ) + 1.0 ) ) ) - 0.1 );
				float4 Specular35 = ( saturate( ( temp_output_3_0_g15 / fwidth( temp_output_3_0_g15 ) ) ) * _Spec_Colour * _Spec_Smoothness );
				
				float Alpha322 = simplePerlin3D317;
				
				float AlphaClipping323 = _DisolveAlphaClipping;
				
				float3 Albedo = clampResult158.rgb;
				float3 Normal = float3(0, 0, 1);
				float3 Emission = ( DisolveEmissive331 + VEmissive126 ).rgb;
				float3 Specular = ( Specular35 * _Specular_Alpha ).rgb;
				float Metallic = 0;
				float Smoothness = _Overall_Smoothness;
				float Occlusion = 1;
				float Alpha = Alpha322;
				float AlphaClipThreshold = AlphaClipping323;
				float AlphaClipThresholdShadow = 0.5;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, float4( WorldNormal, 0 ) ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos.xy ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return color;
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
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#define _EMISSION
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70701

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
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
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _DiffuseBlinkEyes_ST;
			float4 _EmmisiveNormalEyes_ST;
			float4 _DiffuseStunEyes_ST;
			float4 _DiffuseDeathEyes_ST;
			float4 _EmmisiveBlinkEyes_ST;
			float4 _DiffuseNormalEyes_ST;
			float4 _EmmisiveDeathEyes_ST;
			float4 _DiffuseHurtEyes_ST;
			float4 _Damage_Colour;
			float4 _EmmisiveStunEyes_ST;
			float4 _EmmisiveHurtEyes_ST;
			float4 _Outline_Colour;
			float4 _DisolveColour;
			float4 _Spec_Colour;
			float4 _DiffuseBody_ST;
			float4 _Diffuse_Colour;
			float4 _EmissiveBody_ST;
			float _DisolveEdgeThickness;
			float _Outline_Width;
			float _Toggle_EnemyHPEmissive;
			float _Rim_Alpha;
			float _Spec_Smoothness;
			float _BlinkingEmissiveRate;
			float _Rim_Strenght;
			float _SSS_Fres_Scale;
			float _FresnelScale;
			float _SubSurf_Alpha;
			float _SSS_Fres_Power;
			float _Specular_Alpha;
			float _SubSurface_Stenght;
			float _IsUsingEyes;
			float _Unique_Eye_On;
			float _IsBeingStunned;
			float _BlinkSpeed;
			float _IsBeingDamaged;
			float _DisolveAlphaClipping;
			float _DisolveNoiseScale;
			float _Outline_Alpha;
			float _Outline_Distance;
			float _Highlight_Strenght;
			float _Overall_Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

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
			

			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
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
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

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

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
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

				float3 worldToObj319 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float simplePerlin3D317 = snoise( worldToObj319*_DisolveNoiseScale );
				simplePerlin3D317 = simplePerlin3D317*0.5 + 0.5;
				float Alpha322 = simplePerlin3D317;
				
				float AlphaClipping323 = _DisolveAlphaClipping;
				
				float Alpha = Alpha322;
				float AlphaClipThreshold = AlphaClipping323;
				float AlphaClipThresholdShadow = 0.5;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

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
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
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
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#define _EMISSION
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70701

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				
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
				
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _DiffuseBlinkEyes_ST;
			float4 _EmmisiveNormalEyes_ST;
			float4 _DiffuseStunEyes_ST;
			float4 _DiffuseDeathEyes_ST;
			float4 _EmmisiveBlinkEyes_ST;
			float4 _DiffuseNormalEyes_ST;
			float4 _EmmisiveDeathEyes_ST;
			float4 _DiffuseHurtEyes_ST;
			float4 _Damage_Colour;
			float4 _EmmisiveStunEyes_ST;
			float4 _EmmisiveHurtEyes_ST;
			float4 _Outline_Colour;
			float4 _DisolveColour;
			float4 _Spec_Colour;
			float4 _DiffuseBody_ST;
			float4 _Diffuse_Colour;
			float4 _EmissiveBody_ST;
			float _DisolveEdgeThickness;
			float _Outline_Width;
			float _Toggle_EnemyHPEmissive;
			float _Rim_Alpha;
			float _Spec_Smoothness;
			float _BlinkingEmissiveRate;
			float _Rim_Strenght;
			float _SSS_Fres_Scale;
			float _FresnelScale;
			float _SubSurf_Alpha;
			float _SSS_Fres_Power;
			float _Specular_Alpha;
			float _SubSurface_Stenght;
			float _IsUsingEyes;
			float _Unique_Eye_On;
			float _IsBeingStunned;
			float _BlinkSpeed;
			float _IsBeingDamaged;
			float _DisolveAlphaClipping;
			float _DisolveNoiseScale;
			float _Outline_Alpha;
			float _Outline_Distance;
			float _Highlight_Strenght;
			float _Overall_Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
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
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				
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

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif
			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
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

				float3 worldToObj319 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float simplePerlin3D317 = snoise( worldToObj319*_DisolveNoiseScale );
				simplePerlin3D317 = simplePerlin3D317*0.5 + 0.5;
				float Alpha322 = simplePerlin3D317;
				
				float AlphaClipping323 = _DisolveAlphaClipping;
				
				float Alpha = Alpha322;
				float AlphaClipThreshold = AlphaClipping323;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
				outputDepth = DepthValue;
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#define _EMISSION
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70701

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#define ASE_NEEDS_VERT_TEXTURE_COORDINATES1
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
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
				float4 ase_texcoord2 : TEXCOORD2;
				float4 lightmapUVOrVertexSH : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _DiffuseBlinkEyes_ST;
			float4 _EmmisiveNormalEyes_ST;
			float4 _DiffuseStunEyes_ST;
			float4 _DiffuseDeathEyes_ST;
			float4 _EmmisiveBlinkEyes_ST;
			float4 _DiffuseNormalEyes_ST;
			float4 _EmmisiveDeathEyes_ST;
			float4 _DiffuseHurtEyes_ST;
			float4 _Damage_Colour;
			float4 _EmmisiveStunEyes_ST;
			float4 _EmmisiveHurtEyes_ST;
			float4 _Outline_Colour;
			float4 _DisolveColour;
			float4 _Spec_Colour;
			float4 _DiffuseBody_ST;
			float4 _Diffuse_Colour;
			float4 _EmissiveBody_ST;
			float _DisolveEdgeThickness;
			float _Outline_Width;
			float _Toggle_EnemyHPEmissive;
			float _Rim_Alpha;
			float _Spec_Smoothness;
			float _BlinkingEmissiveRate;
			float _Rim_Strenght;
			float _SSS_Fres_Scale;
			float _FresnelScale;
			float _SubSurf_Alpha;
			float _SSS_Fres_Power;
			float _Specular_Alpha;
			float _SubSurface_Stenght;
			float _IsUsingEyes;
			float _Unique_Eye_On;
			float _IsBeingStunned;
			float _BlinkSpeed;
			float _IsBeingDamaged;
			float _DisolveAlphaClipping;
			float _DisolveNoiseScale;
			float _Outline_Alpha;
			float _Outline_Distance;
			float _Highlight_Strenght;
			float _Overall_Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _DiffuseBody;
			sampler2D _DiffuseNormalEyes;
			sampler2D _DiffuseBlinkEyes;
			sampler2D _DiffuseDeathEyes;
			sampler2D _DiffuseStunEyes;
			sampler2D _DiffuseHurtEyes;
			sampler2D _EmissiveBody;
			sampler2D _EmmisiveNormalEyes;
			sampler2D _EmmisiveBlinkEyes;
			sampler2D _EmmisiveDeathEyes;
			sampler2D _EmmisiveStunEyes;
			sampler2D _EmmisiveHurtEyes;


			float3 ASEIndirectDiffuse( float2 uvStaticLightmap, float3 normalWS )
			{
			#ifdef LIGHTMAP_ON
				return SampleLightmap( uvStaticLightmap, normalWS );
			#else
				return SampleSH(normalWS);
			#endif
			}
			
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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( ase_worldNormal, o.lightmapUVOrVertexSH.xyz );
				
				o.ase_texcoord4.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				o.ase_texcoord4.zw = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
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

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
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
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
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
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
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
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
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

				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float dotResult4 = dot( ase_worldNormal , _MainLightPosition.xyz );
				float ase_lightAtten = 0;
				Light ase_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_mainLight.distanceAttenuation * ase_mainLight.shadowAttenuation;
				float temp_output_7_0 = min( saturate( dotResult4 ) , ase_lightAtten );
				float temp_output_3_0_g13 = ( temp_output_7_0 - 0.1 );
				float temp_output_9_0 = saturate( ( temp_output_3_0_g13 / fwidth( temp_output_3_0_g13 ) ) );
				float3 bakedGI13 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, ase_worldNormal);
				MixRealtimeAndBakedGI(ase_mainLight, ase_worldNormal, bakedGI13, half4(0,0,0,0));
				float2 uv_DiffuseBody = IN.ase_texcoord4.xy * _DiffuseBody_ST.xy + _DiffuseBody_ST.zw;
				float4 tex2DNode206 = tex2D( _DiffuseBody, uv_DiffuseBody );
				float DamageValue154 = _IsBeingDamaged;
				float4 lerpResult155 = lerp( _Diffuse_Colour , _Damage_Colour , DamageValue154);
				float4 ColourChange304 = lerpResult155;
				float4 temp_output_308_0 = ( tex2DNode206 * ColourChange304 );
				float2 uv_DiffuseNormalEyes = IN.ase_texcoord4.xy * _DiffuseNormalEyes_ST.xy + _DiffuseNormalEyes_ST.zw;
				float4 tex2DNode207 = tex2D( _DiffuseNormalEyes, uv_DiffuseNormalEyes );
				float2 uv_DiffuseBlinkEyes = IN.ase_texcoord4.xy * _DiffuseBlinkEyes_ST.xy + _DiffuseBlinkEyes_ST.zw;
				float4 tex2DNode208 = tex2D( _DiffuseBlinkEyes, uv_DiffuseBlinkEyes );
				float temp_output_3_0_g12 = ( 0.2 - abs( sin( ( _TimeParameters.x * _BlinkSpeed ) ) ) );
				float BlinkingSystem148 = saturate( ( temp_output_3_0_g12 / fwidth( temp_output_3_0_g12 ) ) );
				float4 lerpResult236 = lerp( tex2DNode207 , tex2DNode208 , BlinkingSystem148);
				float2 uv_DiffuseDeathEyes = IN.ase_texcoord4.xy * _DiffuseDeathEyes_ST.xy + _DiffuseDeathEyes_ST.zw;
				float4 tex2DNode211 = tex2D( _DiffuseDeathEyes, uv_DiffuseDeathEyes );
				float2 uv_DiffuseStunEyes = IN.ase_texcoord4.xy * _DiffuseStunEyes_ST.xy + _DiffuseStunEyes_ST.zw;
				float4 tex2DNode210 = tex2D( _DiffuseStunEyes, uv_DiffuseStunEyes );
				float StunValue249 = _IsBeingStunned;
				float4 lerpResult250 = lerp( tex2DNode211 , tex2DNode210 , StunValue249);
				float2 uv_DiffuseHurtEyes = IN.ase_texcoord4.xy * _DiffuseHurtEyes_ST.xy + _DiffuseHurtEyes_ST.zw;
				float4 tex2DNode209 = tex2D( _DiffuseHurtEyes, uv_DiffuseHurtEyes );
				float4 lerpResult243 = lerp( lerpResult250 , tex2DNode209 , DamageValue154);
				float UniqueEyesToggle257 = _Unique_Eye_On;
				float4 lerpResult245 = lerp( lerpResult236 , lerpResult243 , UniqueEyesToggle257);
				float EyesOnOffSwitch277 = _IsUsingEyes;
				float4 lerpResult275 = lerp( lerpResult245 , float4( 0,0,0,0 ) , EyesOnOffSwitch277);
				float NormalEyesAlpha295 = tex2DNode207.a;
				float BlinkEyesAlpha296 = tex2DNode208.a;
				float lerpResult285 = lerp( NormalEyesAlpha295 , BlinkEyesAlpha296 , BlinkingSystem148);
				float DeathEyesAlpha312 = tex2DNode211.a;
				float StunEyesAlpha313 = tex2DNode210.a;
				float lerpResult289 = lerp( DeathEyesAlpha312 , StunEyesAlpha313 , StunValue249);
				float HurtEyeAlpha311 = tex2DNode209.a;
				float lerpResult286 = lerp( lerpResult289 , HurtEyeAlpha311 , DamageValue154);
				float lerpResult291 = lerp( lerpResult285 , lerpResult286 , UniqueEyesToggle257);
				float lerpResult293 = lerp( lerpResult291 , 0.0 , EyesOnOffSwitch277);
				float4 lerpResult284 = lerp( temp_output_308_0 , lerpResult275 , lerpResult293);
				float4 VDiffuse125 = lerpResult284;
				float4 Diffuse19 = ( float4( ( temp_output_9_0 + ( ( 1.0 - temp_output_9_0 ) * bakedGI13 ) ) , 0.0 ) * ( VDiffuse125 * ColourChange304 ) );
				float NormalDotLight64 = temp_output_7_0;
				float NormalDotLightStep39 = temp_output_9_0;
				float lerpResult71 = lerp( NormalDotLight64 , pow( ( 1.0 - NormalDotLight64 ) , _SubSurface_Stenght ) , NormalDotLightStep39);
				float4 VSubSurf127 = temp_output_308_0;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float fresnelNdotV83 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode83 = ( 0.0 + _SSS_Fres_Scale * pow( 1.0 - fresnelNdotV83, _SSS_Fres_Power ) );
				float4 SubsurfaceScattering80 = ( ( lerpResult71 * VSubSurf127 ) + ( fresnelNode83 * VSubSurf127 ) );
				float fresnelNdotV42 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode42 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV42, 5.0 ) );
				float temp_output_3_0_g14 = ( fresnelNode42 - 0.1 );
				float temp_output_43_0 = saturate( ( temp_output_3_0_g14 / fwidth( temp_output_3_0_g14 ) ) );
				float3 bakedGI51 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, ase_worldNormal);
				MixRealtimeAndBakedGI(ase_mainLight, ase_worldNormal, bakedGI51, half4(0,0,0,0));
				float4 Highlight53 = ( ( NormalDotLightStep39 * temp_output_43_0 * _MainLightColor * _Highlight_Strenght ) + float4( ( ( 1.0 - NormalDotLightStep39 ) * temp_output_43_0 * bakedGI51 * _Rim_Strenght ) , 0.0 ) );
				float4 clampResult158 = clamp( ( Diffuse19 + ( SubsurfaceScattering80 * _SubSurf_Alpha ) + ( Highlight53 * _Rim_Alpha ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				
				float3 worldToObj319 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float simplePerlin3D317 = snoise( worldToObj319*_DisolveNoiseScale );
				simplePerlin3D317 = simplePerlin3D317*0.5 + 0.5;
				float4 DisolveEmissive331 = ( _DisolveColour * step( simplePerlin3D317 , ( _DisolveEdgeThickness + _DisolveAlphaClipping ) ) );
				float2 uv_EmissiveBody = IN.ase_texcoord4.xy * _EmissiveBody_ST.xy + _EmissiveBody_ST.zw;
				float4 tex2DNode235 = tex2D( _EmissiveBody, uv_EmissiveBody );
				float BreathingRate187 = abs( sin( ( _TimeParameters.x * _BlinkingEmissiveRate ) ) );
				float4 lerpResult255 = lerp( tex2DNode235 , ( tex2DNode235 * 3.0 ) , BreathingRate187);
				float HPFloat202 = _Toggle_EnemyHPEmissive;
				float2 uv_EmmisiveNormalEyes = IN.ase_texcoord4.xy * _EmmisiveNormalEyes_ST.xy + _EmmisiveNormalEyes_ST.zw;
				float2 uv_EmmisiveBlinkEyes = IN.ase_texcoord4.xy * _EmmisiveBlinkEyes_ST.xy + _EmmisiveBlinkEyes_ST.zw;
				float4 lerpResult259 = lerp( tex2D( _EmmisiveNormalEyes, uv_EmmisiveNormalEyes ) , tex2D( _EmmisiveBlinkEyes, uv_EmmisiveBlinkEyes ) , BlinkingSystem148);
				float2 uv_EmmisiveDeathEyes = IN.ase_texcoord4.xy * _EmmisiveDeathEyes_ST.xy + _EmmisiveDeathEyes_ST.zw;
				float2 uv_EmmisiveStunEyes = IN.ase_texcoord4.xy * _EmmisiveStunEyes_ST.xy + _EmmisiveStunEyes_ST.zw;
				float4 lerpResult269 = lerp( tex2D( _EmmisiveDeathEyes, uv_EmmisiveDeathEyes ) , tex2D( _EmmisiveStunEyes, uv_EmmisiveStunEyes ) , StunValue249);
				float2 uv_EmmisiveHurtEyes = IN.ase_texcoord4.xy * _EmmisiveHurtEyes_ST.xy + _EmmisiveHurtEyes_ST.zw;
				float4 lerpResult263 = lerp( lerpResult269 , tex2D( _EmmisiveHurtEyes, uv_EmmisiveHurtEyes ) , DamageValue154);
				float4 lerpResult271 = lerp( lerpResult259 , lerpResult263 , UniqueEyesToggle257);
				float4 lerpResult279 = lerp( lerpResult271 , float4( 0,0,0,0 ) , EyesOnOffSwitch277);
				float4 lerpResult310 = lerp( ( lerpResult255 * HPFloat202 ) , lerpResult279 , lerpResult293);
				float4 VEmissive126 = ( lerpResult310 * ColourChange304 );
				
				float Alpha322 = simplePerlin3D317;
				
				float AlphaClipping323 = _DisolveAlphaClipping;
				
				
				float3 Albedo = clampResult158.rgb;
				float3 Emission = ( DisolveEmissive331 + VEmissive126 ).rgb;
				float Alpha = Alpha322;
				float AlphaClipThreshold = AlphaClipping323;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _SPECULAR_SETUP 1
			#define _EMISSION
			#define _ALPHATEST_ON 1
			#define ASE_SRP_VERSION 70701

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_2D

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
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
				float4 ase_texcoord2 : TEXCOORD2;
				float4 lightmapUVOrVertexSH : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _DiffuseBlinkEyes_ST;
			float4 _EmmisiveNormalEyes_ST;
			float4 _DiffuseStunEyes_ST;
			float4 _DiffuseDeathEyes_ST;
			float4 _EmmisiveBlinkEyes_ST;
			float4 _DiffuseNormalEyes_ST;
			float4 _EmmisiveDeathEyes_ST;
			float4 _DiffuseHurtEyes_ST;
			float4 _Damage_Colour;
			float4 _EmmisiveStunEyes_ST;
			float4 _EmmisiveHurtEyes_ST;
			float4 _Outline_Colour;
			float4 _DisolveColour;
			float4 _Spec_Colour;
			float4 _DiffuseBody_ST;
			float4 _Diffuse_Colour;
			float4 _EmissiveBody_ST;
			float _DisolveEdgeThickness;
			float _Outline_Width;
			float _Toggle_EnemyHPEmissive;
			float _Rim_Alpha;
			float _Spec_Smoothness;
			float _BlinkingEmissiveRate;
			float _Rim_Strenght;
			float _SSS_Fres_Scale;
			float _FresnelScale;
			float _SubSurf_Alpha;
			float _SSS_Fres_Power;
			float _Specular_Alpha;
			float _SubSurface_Stenght;
			float _IsUsingEyes;
			float _Unique_Eye_On;
			float _IsBeingStunned;
			float _BlinkSpeed;
			float _IsBeingDamaged;
			float _DisolveAlphaClipping;
			float _DisolveNoiseScale;
			float _Outline_Alpha;
			float _Outline_Distance;
			float _Highlight_Strenght;
			float _Overall_Smoothness;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _DiffuseBody;
			sampler2D _DiffuseNormalEyes;
			sampler2D _DiffuseBlinkEyes;
			sampler2D _DiffuseDeathEyes;
			sampler2D _DiffuseStunEyes;
			sampler2D _DiffuseHurtEyes;


			float3 ASEIndirectDiffuse( float2 uvStaticLightmap, float3 normalWS )
			{
			#ifdef LIGHTMAP_ON
				return SampleLightmap( uvStaticLightmap, normalWS );
			#else
				return SampleSH(normalWS);
			#endif
			}
			
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
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( ase_worldNormal, o.lightmapUVOrVertexSH.xyz );
				
				o.ase_texcoord4.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				o.ase_texcoord4.zw = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
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

				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
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
				o.texcoord1 = v.texcoord1;
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
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
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

				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float dotResult4 = dot( ase_worldNormal , _MainLightPosition.xyz );
				float ase_lightAtten = 0;
				Light ase_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_mainLight.distanceAttenuation * ase_mainLight.shadowAttenuation;
				float temp_output_7_0 = min( saturate( dotResult4 ) , ase_lightAtten );
				float temp_output_3_0_g13 = ( temp_output_7_0 - 0.1 );
				float temp_output_9_0 = saturate( ( temp_output_3_0_g13 / fwidth( temp_output_3_0_g13 ) ) );
				float3 bakedGI13 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, ase_worldNormal);
				MixRealtimeAndBakedGI(ase_mainLight, ase_worldNormal, bakedGI13, half4(0,0,0,0));
				float2 uv_DiffuseBody = IN.ase_texcoord4.xy * _DiffuseBody_ST.xy + _DiffuseBody_ST.zw;
				float4 tex2DNode206 = tex2D( _DiffuseBody, uv_DiffuseBody );
				float DamageValue154 = _IsBeingDamaged;
				float4 lerpResult155 = lerp( _Diffuse_Colour , _Damage_Colour , DamageValue154);
				float4 ColourChange304 = lerpResult155;
				float4 temp_output_308_0 = ( tex2DNode206 * ColourChange304 );
				float2 uv_DiffuseNormalEyes = IN.ase_texcoord4.xy * _DiffuseNormalEyes_ST.xy + _DiffuseNormalEyes_ST.zw;
				float4 tex2DNode207 = tex2D( _DiffuseNormalEyes, uv_DiffuseNormalEyes );
				float2 uv_DiffuseBlinkEyes = IN.ase_texcoord4.xy * _DiffuseBlinkEyes_ST.xy + _DiffuseBlinkEyes_ST.zw;
				float4 tex2DNode208 = tex2D( _DiffuseBlinkEyes, uv_DiffuseBlinkEyes );
				float temp_output_3_0_g12 = ( 0.2 - abs( sin( ( _TimeParameters.x * _BlinkSpeed ) ) ) );
				float BlinkingSystem148 = saturate( ( temp_output_3_0_g12 / fwidth( temp_output_3_0_g12 ) ) );
				float4 lerpResult236 = lerp( tex2DNode207 , tex2DNode208 , BlinkingSystem148);
				float2 uv_DiffuseDeathEyes = IN.ase_texcoord4.xy * _DiffuseDeathEyes_ST.xy + _DiffuseDeathEyes_ST.zw;
				float4 tex2DNode211 = tex2D( _DiffuseDeathEyes, uv_DiffuseDeathEyes );
				float2 uv_DiffuseStunEyes = IN.ase_texcoord4.xy * _DiffuseStunEyes_ST.xy + _DiffuseStunEyes_ST.zw;
				float4 tex2DNode210 = tex2D( _DiffuseStunEyes, uv_DiffuseStunEyes );
				float StunValue249 = _IsBeingStunned;
				float4 lerpResult250 = lerp( tex2DNode211 , tex2DNode210 , StunValue249);
				float2 uv_DiffuseHurtEyes = IN.ase_texcoord4.xy * _DiffuseHurtEyes_ST.xy + _DiffuseHurtEyes_ST.zw;
				float4 tex2DNode209 = tex2D( _DiffuseHurtEyes, uv_DiffuseHurtEyes );
				float4 lerpResult243 = lerp( lerpResult250 , tex2DNode209 , DamageValue154);
				float UniqueEyesToggle257 = _Unique_Eye_On;
				float4 lerpResult245 = lerp( lerpResult236 , lerpResult243 , UniqueEyesToggle257);
				float EyesOnOffSwitch277 = _IsUsingEyes;
				float4 lerpResult275 = lerp( lerpResult245 , float4( 0,0,0,0 ) , EyesOnOffSwitch277);
				float NormalEyesAlpha295 = tex2DNode207.a;
				float BlinkEyesAlpha296 = tex2DNode208.a;
				float lerpResult285 = lerp( NormalEyesAlpha295 , BlinkEyesAlpha296 , BlinkingSystem148);
				float DeathEyesAlpha312 = tex2DNode211.a;
				float StunEyesAlpha313 = tex2DNode210.a;
				float lerpResult289 = lerp( DeathEyesAlpha312 , StunEyesAlpha313 , StunValue249);
				float HurtEyeAlpha311 = tex2DNode209.a;
				float lerpResult286 = lerp( lerpResult289 , HurtEyeAlpha311 , DamageValue154);
				float lerpResult291 = lerp( lerpResult285 , lerpResult286 , UniqueEyesToggle257);
				float lerpResult293 = lerp( lerpResult291 , 0.0 , EyesOnOffSwitch277);
				float4 lerpResult284 = lerp( temp_output_308_0 , lerpResult275 , lerpResult293);
				float4 VDiffuse125 = lerpResult284;
				float4 Diffuse19 = ( float4( ( temp_output_9_0 + ( ( 1.0 - temp_output_9_0 ) * bakedGI13 ) ) , 0.0 ) * ( VDiffuse125 * ColourChange304 ) );
				float NormalDotLight64 = temp_output_7_0;
				float NormalDotLightStep39 = temp_output_9_0;
				float lerpResult71 = lerp( NormalDotLight64 , pow( ( 1.0 - NormalDotLight64 ) , _SubSurface_Stenght ) , NormalDotLightStep39);
				float4 VSubSurf127 = temp_output_308_0;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float fresnelNdotV83 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode83 = ( 0.0 + _SSS_Fres_Scale * pow( 1.0 - fresnelNdotV83, _SSS_Fres_Power ) );
				float4 SubsurfaceScattering80 = ( ( lerpResult71 * VSubSurf127 ) + ( fresnelNode83 * VSubSurf127 ) );
				float fresnelNdotV42 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode42 = ( 0.0 + _FresnelScale * pow( 1.0 - fresnelNdotV42, 5.0 ) );
				float temp_output_3_0_g14 = ( fresnelNode42 - 0.1 );
				float temp_output_43_0 = saturate( ( temp_output_3_0_g14 / fwidth( temp_output_3_0_g14 ) ) );
				float3 bakedGI51 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, ase_worldNormal);
				MixRealtimeAndBakedGI(ase_mainLight, ase_worldNormal, bakedGI51, half4(0,0,0,0));
				float4 Highlight53 = ( ( NormalDotLightStep39 * temp_output_43_0 * _MainLightColor * _Highlight_Strenght ) + float4( ( ( 1.0 - NormalDotLightStep39 ) * temp_output_43_0 * bakedGI51 * _Rim_Strenght ) , 0.0 ) );
				float4 clampResult158 = clamp( ( Diffuse19 + ( SubsurfaceScattering80 * _SubSurf_Alpha ) + ( Highlight53 * _Rim_Alpha ) ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				
				float3 worldToObj319 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float simplePerlin3D317 = snoise( worldToObj319*_DisolveNoiseScale );
				simplePerlin3D317 = simplePerlin3D317*0.5 + 0.5;
				float Alpha322 = simplePerlin3D317;
				
				float AlphaClipping323 = _DisolveAlphaClipping;
				
				
				float3 Albedo = clampResult158.rgb;
				float Alpha = Alpha322;
				float AlphaClipThreshold = AlphaClipping323;

				half4 color = half4( Albedo, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}
		
	}
	
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback "Hidden/InternalErrorShader"
	
}
/*ASEBEGIN
Version=18935
373;195;1469;750;-643.6934;2494.619;4.143716;True;False
Node;AmplifyShaderEditor.CommentaryNode;282;4001.49,-1121.904;Inherit;False;967.292;752.7191;Important Floats;28;143;140;139;138;144;137;148;147;154;248;249;257;178;179;177;175;176;187;201;202;276;277;246;18;156;157;155;304;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;143;4052.541,-851.7119;Inherit;False;Property;_BlinkSpeed;BlinkSpeed;4;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;140;4047.541,-923.7119;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;4211.541,-923.7119;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;61;-1124.051,-219.8217;Inherit;False;2300.392;951.9884;Diffuse;18;5;16;11;14;19;12;2;4;8;6;7;9;39;10;13;64;128;305;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SinOpNode;138;4331.542,-922.7119;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;281;-3043.046,-1101.253;Inherit;False;1866.152;2187.612;Texture;68;127;259;262;267;268;269;254;253;256;263;270;255;271;273;274;261;266;264;265;260;235;126;280;279;236;237;243;242;252;250;258;207;208;209;211;210;206;101;125;245;275;278;284;285;295;296;286;287;288;290;291;292;293;294;298;299;289;303;307;308;309;310;311;312;313;314;315;316;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;248;4020.89,-698.471;Inherit;False;Property;_IsBeingStunned;IsBeingStunned;3;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;5;-1101.051,0.727196;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;210;-2983.265,856.3591;Inherit;True;Property;_DiffuseStunEyes;Diffuse Stun Eyes;29;0;Create;True;0;0;0;False;0;False;-1;None;7d13efda9d609c84d954e01d98cc4348;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;211;-2985.947,664.4965;Inherit;True;Property;_DiffuseDeathEyes;Diffuse Death Eyes;30;0;Create;True;0;0;0;False;0;False;-1;None;ac71aa672243e754f98c682d2de9a3ad;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldNormalVector;2;-1104.051,-169.8217;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;147;4021.943,-772.0212;Inherit;False;Property;_IsBeingDamaged;IsBeingDamaged;2;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;144;4441.542,-921.7119;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;312;-2628.191,921.6622;Inherit;False;DeathEyesAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;154;4291.438,-770.9921;Inherit;False;DamageValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;313;-2636.762,1002.211;Inherit;False;StunEyesAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;4;-879.8677,-112.0473;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;137;4550.542,-919.7119;Inherit;False;Step Antialiasing;-1;;12;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0;False;2;FLOAT;0.2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;209;-2990.223,477.9585;Inherit;True;Property;_DiffuseHurtEyes;Diffuse Hurt Eyes;28;0;Create;True;0;0;0;False;0;False;-1;None;9770a08588e96ef4eba7f3616f6cbdf7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;207;-2987.99,98.25856;Inherit;True;Property;_DiffuseNormalEyes;Diffuse Normal Eyes;26;0;Create;True;0;0;0;False;0;False;-1;e7a302b879925ae478b7574410a5d1ec;afa162dcdfdb03a4da6959da3d20e85a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;249;4294.892,-696.471;Inherit;False;StunValue;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;208;-2988.753,284.7669;Inherit;True;Property;_DiffuseBlinkEyes;Diffuse Blink Eyes;27;0;Create;True;0;0;0;False;0;False;-1;d183cf8255ce4924da915f0bd345d5a0;918d5f00f085c874588e0801ecea9b5d;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;311;-2662.501,440.8586;Inherit;False;HurtEyeAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;157;4536.834,-460.4355;Inherit;False;154;DamageValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;252;-2672.83,623.7863;Inherit;False;249;StunValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;156;4519.834,-629.4355;Inherit;False;Property;_Damage_Colour;Damage_Colour;8;1;[HDR];Create;True;0;0;0;False;0;False;2,0,0,0;2,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;288;-2185.674,1006.235;Inherit;False;249;StunValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;315;-2246.472,920.9689;Inherit;False;313;StunEyesAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;296;-2630.68,42.93304;Inherit;False;BlinkEyesAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;314;-2247.104,849.4277;Inherit;False;312;DeathEyesAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;148;4737.833,-923.5699;Inherit;True;BlinkingSystem;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;295;-2635.68,-30.06696;Inherit;False;NormalEyesAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightAttenuation;8;-672.5497,129.6805;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;246;4025.381,-616.2026;Inherit;False;Property;_Unique_Eye_On;Unique_Eye_On;1;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;6;-657.123,-91.53743;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;18;4516.429,-797.8314;Inherit;False;Property;_Diffuse_Colour;Diffuse_Colour;7;0;Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;287;-2033.785,854.2781;Inherit;False;154;DamageValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;257;4299.813,-613.5839;Inherit;False;UniqueEyesToggle;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;242;-2674.942,549.8295;Inherit;False;154;DamageValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;289;-1993.373,930.036;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;299;-2132.167,550.6719;Inherit;False;296;BlinkEyesAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMinOpNode;7;-464.8889,-27.71422;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;237;-2664.739,366.8023;Inherit;False;148;BlinkingSystem;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;298;-2143.167,474.6719;Inherit;False;295;NormalEyesAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;316;-2033.217,778.3597;Inherit;False;311;HurtEyeAlpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;155;4757.835,-688.4355;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;292;-2137.583,623.2508;Inherit;False;148;BlinkingSystem;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;250;-2470.53,706.5871;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;276;4016.432,-448.8831;Inherit;False;Property;_IsUsingEyes;IsUsingEyes;0;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;286;-1815.294,778.1713;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;258;-2410.653,342.6964;Inherit;False;257;UniqueEyesToggle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;304;4762.993,-525.4874;Inherit;False;ColourChange;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;277;4318.164,-449.5017;Inherit;False;EyesOnOffSwitch;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;64;-212.2124,-123.1253;Inherit;False;NormalDotLight;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;236;-2422.749,200.8039;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FunctionNode;9;-231.9439,-27.46984;Inherit;True;Step Antialiasing;-1;;13;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0.1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;285;-1896.592,521.2523;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;243;-2341.451,457.7227;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;290;-1884.496,663.1448;Inherit;False;257;UniqueEyesToggle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;82;1281.448,-1119.073;Inherit;False;1583.045;703.9894;Subsurface Scattering;15;67;73;63;75;71;78;72;77;83;84;85;87;86;80;130;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;294;-1621.4,718.9637;Inherit;False;277;EyesOnOffSwitch;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;206;-2280.506,29.06958;Inherit;True;Property;_DiffuseBody;Diffuse Body;25;0;Create;True;0;0;0;False;0;False;-1;6ffef316e0380a142befe3e8ec830859;737664e74f5869642a4a8f16632f03c3;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;278;-2147.556,398.5151;Inherit;False;277;EyesOnOffSwitch;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;39;69.8935,-148.9549;Inherit;False;NormalDotLightStep;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;291;-1581.057,574.5021;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;1287.548,-1077.073;Inherit;True;64;NormalDotLight;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;245;-2107.213,254.0536;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;309;-2181.765,-46.98209;Inherit;False;304;ColourChange;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;60;-1124.003,-1115.409;Inherit;False;2373.498;866.2963;RimLight;14;49;52;51;50;41;47;53;46;40;43;44;42;48;88;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;44;-1074.003,-778.2938;Inherit;False;Property;_FresnelScale;FresnelScale;11;0;Create;True;0;0;0;False;0;False;0.65;0.65;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;308;-1887.296,27.01804;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;72;1486.836,-1043.452;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;63;1296.988,-713.9822;Inherit;True;39;NormalDotLightStep;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;293;-1404.285,616.5604;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;1312.917,-815.4886;Inherit;False;Property;_SubSurface_Stenght;SubSurface_Stenght;14;0;Create;True;0;0;0;False;0;False;10;1.06;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;275;-1930.441,296.1119;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.FresnelNode;42;-840.702,-846.4988;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;127;-1648.48,-6.763184;Inherit;False;VSubSurf;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;284;-1660.984,91.68369;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;87;1767.572,-557.6547;Inherit;False;Property;_SSS_Fres_Power;SSS_Fres_Power;16;0;Create;True;0;0;0;False;0;False;3;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;77;1717.612,-1014.888;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;2.9;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;1768.772,-639.2548;Inherit;False;Property;_SSS_Fres_Scale;SSS_Fres_Scale;15;0;Create;True;0;0;0;False;0;False;11;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;75;1533.175,-715.4902;Inherit;True;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;239.9768,-708.1265;Inherit;False;39;NormalDotLightStep;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;83;1997.256,-635.6816;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;88;26.64403,-860.5748;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;49;458.9368,-698.9484;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;10;112.8599,16.32068;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;71;1954.169,-1082.762;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;13;14.4552,90.5533;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;125;-1424.895,-61.2883;Inherit;False;VDiffuse;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;130;2077.736,-761.3249;Inherit;False;127;VSubSurf;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;40;131.2214,-1065.409;Inherit;False;39;NormalDotLightStep;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;43;-556.8851,-842.6078;Inherit;True;Step Antialiasing;-1;;14;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0.1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;12.11992,-739.3398;Inherit;False;Property;_Highlight_Strenght;Highlight_Strenght;12;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;51;345.5215,-440.9233;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;52;371.1898,-365.1126;Inherit;False;Property;_Rim_Strenght;Rim_Strenght;13;0;Create;True;0;0;0;False;0;False;1;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;41;379.2762,-990.0381;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;50;587.4279,-524.5681;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;128;271.5554,143.9174;Inherit;False;125;VDiffuse;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;85;2446.848,-787.1091;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;282.7659,46.99821;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GetLocalVarNode;305;261.5187,221.5107;Inherit;False;304;ColourChange;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;2368.399,-1044.966;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;1,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;334;4996.883,-1115.488;Inherit;False;1363.234;577.4836;Disolve;13;318;319;320;330;321;317;328;327;326;322;323;329;331;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;84;2657.662,-887.275;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;12;448.3843,-84.96317;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;577.4207,102.3984;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;47;786.0803,-804.0235;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;2649.531,-1080.121;Inherit;False;SubsurfaceScattering;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;732.0095,40.81031;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;53;1025.495,-739.4885;Inherit;False;Highlight;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldPosInputsNode;318;5046.883,-1007.594;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;320;5232.883,-858.5939;Inherit;False;Property;_DisolveNoiseScale;DisolveNoiseScale;39;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TransformPositionNode;319;5226.883,-1005.594;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;90;1488.286,432.7668;Inherit;False;Property;_Rim_Alpha;Rim_Alpha;17;0;Create;True;0;0;0;False;0;False;1;0.12;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;1506.839,159.9613;Inherit;False;80;SubsurfaceScattering;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;92;1480.302,237.0498;Inherit;False;Property;_SubSurf_Alpha;SubSurf_Alpha;18;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;54;1539.743,352.5099;Inherit;False;53;Highlight;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;19;952.3403,53.97856;Inherit;False;Diffuse;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;321;5191.251,-708.4136;Inherit;False;Property;_DisolveAlphaClipping;DisolveAlphaClipping;40;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;37;1815.005,33.16698;Inherit;False;19;Diffuse;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;1808.216,116.059;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;1807.103,333.5249;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;317;5468.45,-882.9615;Inherit;False;Simplex3D;True;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;62;-1118.999,763.0065;Inherit;False;2119.078;531.3668;Specular;15;32;33;31;35;27;21;20;28;22;25;23;24;30;26;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;322;5750.518,-732.0042;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;323;5749.518,-654.0042;Inherit;False;AlphaClipping;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;120;2877.943,-1117.302;Inherit;False;1102.771;677.2579;Outline;9;105;98;100;102;103;99;96;104;106;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;38;2133.016,109.9608;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;270;-2341.978,-614.939;Inherit;False;257;UniqueEyesToggle;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;129;2485.755,168.4831;Inherit;False;126;VEmissive;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;253;-2401.723,-937.5306;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;101;-1944.986,148.5067;Inherit;False;Texture_Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;263;-2306.078,-523.2253;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;254;-2552.723,-871.5309;Inherit;False;Constant;_Float1;Float 1;37;0;Create;True;0;0;0;False;0;False;3;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;268;-2604.155,-333.8494;Inherit;False;249;StunValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-153.0653,1118.19;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;23;-615.3117,909.3231;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;333;2855.365,132.3525;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;324;2889.808,376.8923;Inherit;False;322;Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;177;4271.874,-1058.067;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;273;-2028.437,-957.1351;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;264;-2987.147,-487.6784;Inherit;True;Property;_EmmisiveHurtEyes;Emmisive Hurt Eyes;34;0;Create;True;0;0;0;False;0;False;-1;None;db069222c8bb4394387182ad326710c8;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;97;2250.836,-48.97765;Inherit;False;96;Outline;1;0;OBJECT;;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PosVertexDataNode;103;2927.943,-713.0078;Inherit;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;325;2869.808,455.8923;Inherit;False;323;AlphaClipping;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;632.0363,1010.091;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;326;5739.256,-942.8109;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;327;5559.256,-755.8111;Inherit;False;2;2;0;FLOAT;0.01;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;328;5465.893,-1065.488;Inherit;False;Property;_DisolveColour;DisolveColour;38;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;330;5233.417,-783.8001;Inherit;False;Property;_DisolveEdgeThickness;DisolveEdgeThickness;37;0;Create;True;0;0;0;False;0;False;0.01;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;158;2352.079,107.354;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;280;-2058.323,-625.4319;Inherit;False;277;EyesOnOffSwitch;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;20;-1048.765,813.0065;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMaxOpNode;105;3419.816,-701.0328;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;261;-2983.573,-672.5381;Inherit;True;Property;_EmmisiveBlinkEyes;Emmisive Blink Eyes;33;0;Create;True;0;0;0;False;0;False;-1;966836a50a117fc4f80e11a8622ba893;ecab41688e1a7a04f8fd125062e9ef6e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;271;-2051.02,-748.5348;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;102;3166.758,-805.4868;Inherit;False;101;Texture_Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;119;2408.422,459.1085;Inherit;False;Property;_Overall_Smoothness;Overall_Smoothness;24;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;303;-1423.699,-949.7871;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Exp2OpNode;30;127.7069,1121.615;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;25;-653.5859,1030.876;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-21.2392,1121.615;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;310;-1675.934,-1020.996;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;267;-2606.267,-407.8063;Inherit;False;154;DamageValue;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;175;4089.873,-1085.068;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;235;-2975.775,-1051.253;Inherit;True;Property;_EmissiveBody;Emissive Body;31;0;Create;True;0;0;0;False;0;False;-1;ab596d584a854af44a37dcb4f1bd5150;6437fa09f09cf6349a95e33197acc569;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;96;3756.715,-957.1078;Inherit;False;Outline;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NormalVertexDataNode;98;3207.096,-1067.302;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;107;2414.387,-166.2668;Inherit;False;Property;_Outline_Colour;Outline_Colour;20;0;Create;True;0;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;202;4326.682,-531.5013;Inherit;False;HPFloat;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;266;-2992.472,-296.3396;Inherit;True;Property;_EmmisiveDeathEyes;Emmisive Death Eyes;36;0;Create;True;0;0;0;False;0;False;-1;None;545a9129ef3dd6644a76aabe2761cd53;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;3558.096,-947.3018;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;279;-1871.524,-852.032;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;259;-2354.074,-756.8322;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;187;4671.28,-1044.509;Inherit;False;BreathingRate;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;115;2145.729,25.2052;Inherit;False;Property;_Outline_Alpha;Outline_Alpha;23;0;Create;True;0;0;0;False;0;False;1;0.085;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;26;280.0777,986.3643;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;-428.7013,1109.63;Inherit;False;Property;_Spec_Smoothness;Spec_Smoothness;9;0;Create;True;0;0;0;False;0;False;0.954;0.88;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;126;-1361.56,-1053.249;Inherit;False;VEmissive;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;116;2431.729,4.2052;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;100;3144.31,-906.8978;Inherit;False;Property;_Outline_Width;Outline_Width;21;0;Create;True;0;0;0;False;0;False;0.01;0.005;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;274;-2209.437,-895.1351;Inherit;False;202;HPFloat;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;256;-2408.262,-844.9054;Inherit;False;187;BreathingRate;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;3172.968,-556.0438;Inherit;False;Property;_Outline_Distance;Outline_Distance;22;0;Create;True;0;0;0;False;0;False;20;16.51;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;255;-2225.499,-1016.942;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;176;4051.873,-1003.068;Inherit;False;Property;_BlinkingEmissiveRate;BlinkingEmissiveRate;5;0;Create;True;0;0;0;False;0;False;0.3;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;178;4408.875,-1028.068;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;269;-2401.855,-251.0495;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;329;5979.729,-1005.582;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;21;-1068.999,967.5313;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.GetLocalVarNode;36;2140.802,322.8531;Inherit;False;35;Specular;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;201;4023.368,-528.6713;Inherit;False;Property;_Toggle_EnemyHPEmissive;Toggle_EnemyHPEmissive;6;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;342.5237,1090.529;Inherit;False;Property;_Spec_Colour;Spec_Colour;10;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;262;-2576.065,-612.834;Inherit;False;148;BlinkingSystem;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;-772.8182,887.0667;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode;265;-2989.792,-104.4778;Inherit;True;Property;_EmmisiveStunEyes;Emmisive Stun Eyes;35;0;Create;True;0;0;0;False;0;False;-1;None;9a7997ea205d4bd4cae9f3633c5e9db7;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;33;434.8592,966.9973;Inherit;False;Step Antialiasing;-1;;15;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0.1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;260;-2978.715,-863.9757;Inherit;True;Property;_EmmisiveNormalEyes;Emmisive Normal Eyes;32;0;Create;True;0;0;0;False;0;False;-1;9e1f19420ee119348a4bf46372947886;2c88ebabf39fc7b4083ecb4c73bdeabe;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;332;2589.365,65.35248;Inherit;False;331;DisolveEmissive;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;331;6128.117,-1007.213;Inherit;False;DisolveEmissive;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;307;-1633.795,-807.3677;Inherit;False;304;ColourChange;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;2445.212,245.0531;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;35;828.1292,1043.453;Inherit;False;Specular;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.UnityObjToClipPosHlpNode;104;3160.945,-727.3138;Inherit;False;1;0;FLOAT3;0,0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;94;2089.219,414.8907;Inherit;False;Property;_Specular_Alpha;Specular_Alpha;19;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;179;4551.973,-1036.768;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;24;-426.9891,979.5163;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;113;2655.199,105.8385;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;111;2655.199,105.8385;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;112;2655.199,105.8385;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;109;3175.527,-82.58971;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;114;2655.199,105.8385;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;110;3175.865,90.7217;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;Shaders_CelShader_Slimes;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;18;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;38;Workflow;0;637888563752726232;Surface;0;0;  Refraction Model;0;0;  Blend;0;0;Two Sided;1;0;Fragment Normal Space,InvertActionOnDeselection;0;0;Transmission;0;0;  Transmission Shadow;0.5,False,-1;0;Translucency;0;0;  Translucency Strength;1,False,-1;0;  Normal Distortion;0.5,False,-1;0;  Scattering;2,False,-1;0;  Direct;0.9,False,-1;0;  Ambient;0.1,False,-1;0;  Shadow;0.5,False,-1;0;Cast Shadows;1;0;  Use Shadow Threshold;0;0;Receive Shadows;1;0;GPU Instancing;1;0;LOD CrossFade;1;0;Built-in Fog;1;0;_FinalColorxAlpha;0;0;Meta Pass;1;0;Override Baked GI;0;0;Extra Pre Pass;1;0;DOTS Instancing;0;638005518337626709;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Write Depth;0;0;  Early Z;0;0;Vertex Position,InvertActionOnDeselection;1;0;0;6;True;True;True;True;True;True;False;;False;0
WireConnection;139;0;140;0
WireConnection;139;1;143;0
WireConnection;138;0;139;0
WireConnection;144;0;138;0
WireConnection;312;0;211;4
WireConnection;154;0;147;0
WireConnection;313;0;210;4
WireConnection;4;0;2;0
WireConnection;4;1;5;0
WireConnection;137;1;144;0
WireConnection;249;0;248;0
WireConnection;311;0;209;4
WireConnection;296;0;208;4
WireConnection;148;0;137;0
WireConnection;295;0;207;4
WireConnection;6;0;4;0
WireConnection;257;0;246;0
WireConnection;289;0;314;0
WireConnection;289;1;315;0
WireConnection;289;2;288;0
WireConnection;7;0;6;0
WireConnection;7;1;8;0
WireConnection;155;0;18;0
WireConnection;155;1;156;0
WireConnection;155;2;157;0
WireConnection;250;0;211;0
WireConnection;250;1;210;0
WireConnection;250;2;252;0
WireConnection;286;0;289;0
WireConnection;286;1;316;0
WireConnection;286;2;287;0
WireConnection;304;0;155;0
WireConnection;277;0;276;0
WireConnection;64;0;7;0
WireConnection;236;0;207;0
WireConnection;236;1;208;0
WireConnection;236;2;237;0
WireConnection;9;2;7;0
WireConnection;285;0;298;0
WireConnection;285;1;299;0
WireConnection;285;2;292;0
WireConnection;243;0;250;0
WireConnection;243;1;209;0
WireConnection;243;2;242;0
WireConnection;39;0;9;0
WireConnection;291;0;285;0
WireConnection;291;1;286;0
WireConnection;291;2;290;0
WireConnection;245;0;236;0
WireConnection;245;1;243;0
WireConnection;245;2;258;0
WireConnection;308;0;206;0
WireConnection;308;1;309;0
WireConnection;72;0;67;0
WireConnection;293;0;291;0
WireConnection;293;2;294;0
WireConnection;275;0;245;0
WireConnection;275;2;278;0
WireConnection;42;2;44;0
WireConnection;127;0;308;0
WireConnection;284;0;308;0
WireConnection;284;1;275;0
WireConnection;284;2;293;0
WireConnection;77;0;72;0
WireConnection;77;1;78;0
WireConnection;75;0;63;0
WireConnection;83;2;86;0
WireConnection;83;3;87;0
WireConnection;49;0;48;0
WireConnection;10;0;9;0
WireConnection;71;0;67;0
WireConnection;71;1;77;0
WireConnection;71;2;75;0
WireConnection;125;0;284;0
WireConnection;43;2;42;0
WireConnection;41;0;40;0
WireConnection;41;1;43;0
WireConnection;41;2;88;0
WireConnection;41;3;46;0
WireConnection;50;0;49;0
WireConnection;50;1;43;0
WireConnection;50;2;51;0
WireConnection;50;3;52;0
WireConnection;85;0;83;0
WireConnection;85;1;130;0
WireConnection;11;0;10;0
WireConnection;11;1;13;0
WireConnection;73;0;71;0
WireConnection;73;1;130;0
WireConnection;84;0;73;0
WireConnection;84;1;85;0
WireConnection;12;0;9;0
WireConnection;12;1;11;0
WireConnection;16;0;128;0
WireConnection;16;1;305;0
WireConnection;47;0;41;0
WireConnection;47;1;50;0
WireConnection;80;0;84;0
WireConnection;14;0;12;0
WireConnection;14;1;16;0
WireConnection;53;0;47;0
WireConnection;319;0;318;0
WireConnection;19;0;14;0
WireConnection;91;0;81;0
WireConnection;91;1;92;0
WireConnection;89;0;54;0
WireConnection;89;1;90;0
WireConnection;317;0;319;0
WireConnection;317;1;320;0
WireConnection;322;0;317;0
WireConnection;323;0;321;0
WireConnection;38;0;37;0
WireConnection;38;1;91;0
WireConnection;38;2;89;0
WireConnection;253;0;235;0
WireConnection;253;1;254;0
WireConnection;101;0;206;4
WireConnection;263;0;269;0
WireConnection;263;1;264;0
WireConnection;263;2;267;0
WireConnection;28;0;27;0
WireConnection;23;0;22;0
WireConnection;333;0;332;0
WireConnection;333;1;129;0
WireConnection;177;0;175;0
WireConnection;177;1;176;0
WireConnection;273;0;255;0
WireConnection;273;1;274;0
WireConnection;31;0;33;0
WireConnection;31;1;32;0
WireConnection;31;2;27;0
WireConnection;326;0;317;0
WireConnection;326;1;327;0
WireConnection;327;0;330;0
WireConnection;327;1;321;0
WireConnection;158;0;38;0
WireConnection;105;0;104;4
WireConnection;105;1;106;0
WireConnection;271;0;259;0
WireConnection;271;1;263;0
WireConnection;271;2;270;0
WireConnection;303;0;310;0
WireConnection;303;1;307;0
WireConnection;30;0;29;0
WireConnection;29;0;28;0
WireConnection;310;0;273;0
WireConnection;310;1;279;0
WireConnection;310;2;293;0
WireConnection;96;0;99;0
WireConnection;202;0;201;0
WireConnection;99;0;98;0
WireConnection;99;1;100;0
WireConnection;99;2;102;0
WireConnection;99;3;105;0
WireConnection;279;0;271;0
WireConnection;279;2;280;0
WireConnection;259;0;260;0
WireConnection;259;1;261;0
WireConnection;259;2;262;0
WireConnection;187;0;179;0
WireConnection;26;0;24;0
WireConnection;26;1;30;0
WireConnection;126;0;303;0
WireConnection;116;0;97;0
WireConnection;116;1;115;0
WireConnection;255;0;235;0
WireConnection;255;1;253;0
WireConnection;255;2;256;0
WireConnection;178;0;177;0
WireConnection;269;0;266;0
WireConnection;269;1;265;0
WireConnection;269;2;268;0
WireConnection;329;0;328;0
WireConnection;329;1;326;0
WireConnection;22;0;20;0
WireConnection;22;1;21;0
WireConnection;33;2;26;0
WireConnection;331;0;329;0
WireConnection;93;0;36;0
WireConnection;93;1;94;0
WireConnection;35;0;31;0
WireConnection;104;0;103;0
WireConnection;179;0;178;0
WireConnection;24;0;23;0
WireConnection;24;1;25;0
WireConnection;109;0;107;0
WireConnection;109;1;324;0
WireConnection;109;2;325;0
WireConnection;109;3;116;0
WireConnection;110;0;158;0
WireConnection;110;2;333;0
WireConnection;110;9;93;0
WireConnection;110;4;119;0
WireConnection;110;6;324;0
WireConnection;110;7;325;0
ASEEND*/
//CHKSM=4C942507EFB347BBDAADDC57BEBEA12B6BA3D512
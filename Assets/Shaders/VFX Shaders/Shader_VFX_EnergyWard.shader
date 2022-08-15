// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader_VFX_EnergyWard"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_ShieldDamage("ShieldDamage", Range( 0 , 1)) = 0
		_VignetteSmoothness("VignetteSmoothness", Float) = 0.73
		_Panner_1_Speed("Panner_1_Speed", Vector) = (-1,0,0,0)
		_Voronoi_2_Speed("Voronoi_2_Speed", Float) = 2
		_Panner_2_Speed("Panner_2_Speed", Vector) = (-1,0,0,0)
		_Voronoi_2_Scale("Voronoi_2_Scale", Float) = 3
		_VignettePowerExp("VignettePowerExp", Float) = 8.29
		_Voronoi_Power_Exp("Voronoi_Power_Exp", Range( 0 , 2)) = 0.7647059
		_Texture0("Texture 0", 2D) = "white" {}
		_Texture1("Texture 1", 2D) = "white" {}
		_Fres_Scale_MinNew("Fres_Scale_MinNew", Float) = 1
		_Fres_Power_MaxNew("Fres_Power_MaxNew", Float) = 5
		_Fres_Scale_MaxNew("Fres_Scale_MaxNew", Float) = 2
		_Fres_Power_MinNew("Fres_Power_MinNew", Float) = 3
		_Fres_ScaleX_PowerY_Speeds("Fres_Scale(X)_Power(Y)_Speeds", Vector) = (1,1,0,0)
		_Voronoi_Speed("Voronoi_Speed", Float) = 0
		_Voronoi_Scale("Voronoi_Scale", Float) = 0
		[HDR]_Colour_1("Colour_1", Color) = (0,0,0,0)
		[HDR]_Colour_Impact_1("Colour_Impact_1", Color) = (0,0,0,0)
		[HDR]_Colour_2("Colour_2", Color) = (0,0,0,0)
		[HDR]_Colour_Impact_2("Colour_Impact_2", Color) = (0,0,0,0)
		_ToggleSoftMask("ToggleSoftMask", Range( 0 , 1)) = 0
		_SoftEgdeMask_Texture("SoftEgdeMask_Texture", 2D) = "white" {}
		_SoftEdgeFade("SoftEdgeFade", Float) = 1
		[ASEEnd]_StopSplash("StopSplash", Range( 0 , 1)) = 0
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
			#define _RECEIVE_SHADOWS_OFF 1
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

			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
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
				float4 ase_texcoord5 : TEXCOORD5;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Colour_1;
			float4 _Colour_2;
			float4 _SoftEgdeMask_Texture_ST;
			float4 _Colour_Impact_1;
			float4 _Colour_Impact_2;
			float2 _Panner_1_Speed;
			float2 _Panner_2_Speed;
			float2 _Fres_ScaleX_PowerY_Speeds;
			float _Voronoi_2_Speed;
			float _Voronoi_2_Scale;
			float _ToggleSoftMask;
			float _SoftEdgeFade;
			float _VignettePowerExp;
			float _VignetteSmoothness;
			float _ShieldDamage;
			float _Voronoi_Speed;
			float _Voronoi_Scale;
			float _Fres_Power_MaxNew;
			float _Fres_Power_MinNew;
			float _Fres_Scale_MaxNew;
			float _Fres_Scale_MinNew;
			float _Voronoi_Power_Exp;
			float _StopSplash;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Texture0;
			sampler2D _Texture1;
			sampler2D _SoftEgdeMask_Texture;


					float2 voronoihash51( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi51( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash51( n + g );
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
			
					float2 voronoihash141( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi141( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash141( n + g );
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
			
					float2 voronoihash93( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi93( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash93( n + g );
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

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord4.xyz = ase_worldNormal;
				
				o.ase_color = v.ase_color;
				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				o.ase_texcoord5 = v.ase_texcoord1;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;
				o.ase_texcoord4.w = 0;
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
				float4 ase_texcoord1 : TEXCOORD1;

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
				o.ase_texcoord1 = v.ase_texcoord1;
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
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
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
				float2 CenteredUV15_g1 = ( IN.ase_texcoord3.xy - float2( 0.5,0.5 ) );
				float2 break17_g1 = CenteredUV15_g1;
				float2 appendResult23_g1 = (float2(( length( CenteredUV15_g1 ) * 1.0 * 2.0 ) , ( atan2( break17_g1.x , break17_g1.y ) * ( 1.0 / TWO_PI ) * 1.0 )));
				float2 temp_output_6_0 = appendResult23_g1;
				float2 panner7 = ( 1.0 * _Time.y * _Panner_1_Speed + temp_output_6_0);
				float2 panner25 = ( 1.0 * _Time.y * _Panner_2_Speed + temp_output_6_0);
				float4 Texture46 = ( tex2D( _Texture0, panner7 ) + tex2D( _Texture1, panner25 ) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord4.xyz;
				float fresnelNdotV43 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode43 = ( 0.0 + (_Fres_Scale_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.x ) ) ) - 0.0) * (_Fres_Scale_MaxNew - _Fres_Scale_MinNew) / (1.0 - 0.0)) * pow( 1.0 - fresnelNdotV43, (_Fres_Power_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.y ) ) ) - 0.0) * (_Fres_Power_MaxNew - _Fres_Power_MinNew) / (1.0 - 0.0)) ) );
				float time51 = ( _Voronoi_Speed * _TimeParameters.x );
				float2 voronoiSmoothId51 = 0;
				float2 coords51 = IN.ase_texcoord3.xy * _Voronoi_Scale;
				float2 id51 = 0;
				float2 uv51 = 0;
				float voroi51 = voronoi51( coords51, time51, id51, uv51, 0, voronoiSmoothId51 );
				float fresnelNdotV140 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode140 = ( 0.0 + (20.0 + (abs( sin( ( _TimeParameters.x * 10.0 ) ) ) - 0.0) * (10.0 - 20.0) / (1.0 - 0.0)) * pow( 1.0 - fresnelNdotV140, (1.0 + (abs( sin( ( _TimeParameters.x * 4.0 ) ) ) - 0.0) * (0.5 - 1.0) / (1.0 - 0.0)) ) );
				float time141 = ( 4.0 * _TimeParameters.x );
				float2 voronoiSmoothId141 = 0;
				float2 coords141 = IN.ase_texcoord3.xy * 8.15;
				float2 id141 = 0;
				float2 uv141 = 0;
				float fade141 = 0.5;
				float voroi141 = 0;
				float rest141 = 0;
				for( int it141 = 0; it141 <3; it141++ ){
				voroi141 += fade141 * voronoi141( coords141, time141, id141, uv141, 0,voronoiSmoothId141 );
				rest141 += fade141;
				coords141 *= 2;
				fade141 *= 0.5;
				}//Voronoi141
				voroi141 /= rest141;
				float ShieldDamage119 = _ShieldDamage;
				float lerpResult144 = lerp( ( fresnelNode43 * voroi51 ) , ( fresnelNode140 * voroi141 ) , ShieldDamage119);
				float Fresnel45 = lerpResult144;
				float4 lerpResult106 = lerp( ( ( ( _Colour_1 * IN.ase_color ) * Texture46 ) + ( ( IN.ase_color * _Colour_2 ) * Fresnel45 ) ) , ( ( ( _Colour_Impact_1 * IN.ase_color ) * Texture46 ) + ( ( _Colour_Impact_2 * IN.ase_color ) * Fresnel45 ) ) , ShieldDamage119);
				float4 clampResult68 = clamp( lerpResult106 , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				float4 Colour82 = clampResult68;
				
				float2 CenteredUV15_g2 = ( IN.ase_texcoord3.xy - float2( 0.5,0.5 ) );
				float2 break17_g2 = CenteredUV15_g2;
				float2 appendResult23_g2 = (float2(( length( CenteredUV15_g2 ) * 1.0 * 2.0 ) , ( atan2( break17_g2.x , break17_g2.y ) * ( 1.0 / TWO_PI ) * 1.0 )));
				float lerpResult117 = lerp( _VignetteSmoothness , 1.3 , ShieldDamage119);
				float PolarMask47 = pow( ( appendResult23_g2.x * lerpResult117 ) , _VignettePowerExp );
				float4 temp_output_11_0 = ( ( Texture46 + Fresnel45 ) * PolarMask47 );
				float2 uv_SoftEgdeMask_Texture = IN.ase_texcoord3.xy * _SoftEgdeMask_Texture_ST.xy + _SoftEgdeMask_Texture_ST.zw;
				float SoftEdgeMask77 = ( tex2D( _SoftEgdeMask_Texture, uv_SoftEgdeMask_Texture ).r * _SoftEdgeFade );
				float4 lerpResult70 = lerp( temp_output_11_0 , ( temp_output_11_0 * SoftEdgeMask77 ) , _ToggleSoftMask);
				float4 Pulse96 = lerpResult70;
				float time93 = ( _Voronoi_2_Speed * _TimeParameters.x );
				float2 voronoiSmoothId93 = 0;
				float2 coords93 = IN.ase_texcoord3.xy * _Voronoi_2_Scale;
				float2 id93 = 0;
				float2 uv93 = 0;
				float voroi93 = voronoi93( coords93, time93, id93, uv93, 0, voronoiSmoothId93 );
				float4 appendResult92 = (float4(IN.ase_texcoord5.x , IN.ase_texcoord5.y , _Voronoi_Power_Exp , _Voronoi_Power_Exp));
				float Fire_Disolve95 = pow( voroi93 , appendResult92.x );
				float4 lerpResult84 = lerp( lerpResult70 , ( Pulse96 * Fire_Disolve95 ) , _StopSplash);
				float VertexColour103 = IN.ase_color.a;
				float4 clampResult69 = clamp( ( lerpResult84 * VertexColour103 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				float4 Alpha80 = clampResult69;
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = Colour82.rgb;
				float Alpha = Alpha80.r;
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
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define _RECEIVE_SHADOWS_OFF 1
			#define ASE_SRP_VERSION 70701

			
			#pragma vertex vert
			#pragma fragment frag

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#define ASE_NEEDS_VERT_NORMAL


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;
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
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Colour_1;
			float4 _Colour_2;
			float4 _SoftEgdeMask_Texture_ST;
			float4 _Colour_Impact_1;
			float4 _Colour_Impact_2;
			float2 _Panner_1_Speed;
			float2 _Panner_2_Speed;
			float2 _Fres_ScaleX_PowerY_Speeds;
			float _Voronoi_2_Speed;
			float _Voronoi_2_Scale;
			float _ToggleSoftMask;
			float _SoftEdgeFade;
			float _VignettePowerExp;
			float _VignetteSmoothness;
			float _ShieldDamage;
			float _Voronoi_Speed;
			float _Voronoi_Scale;
			float _Fres_Power_MaxNew;
			float _Fres_Power_MinNew;
			float _Fres_Scale_MaxNew;
			float _Fres_Scale_MinNew;
			float _Voronoi_Power_Exp;
			float _StopSplash;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			sampler2D _Texture0;
			sampler2D _Texture1;
			sampler2D _SoftEgdeMask_Texture;


					float2 voronoihash51( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi51( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash51( n + g );
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
			
					float2 voronoihash141( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi141( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash141( n + g );
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
			
					float2 voronoihash93( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi93( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash93( n + g );
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

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				
				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				o.ase_texcoord4 = v.ase_texcoord1;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				o.ase_texcoord3.w = 0;
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
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;

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
				o.ase_texcoord1 = v.ase_texcoord1;
				o.ase_color = v.ase_color;
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
				o.ase_texcoord1 = patch[0].ase_texcoord1 * bary.x + patch[1].ase_texcoord1 * bary.y + patch[2].ase_texcoord1 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
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

				float2 CenteredUV15_g1 = ( IN.ase_texcoord2.xy - float2( 0.5,0.5 ) );
				float2 break17_g1 = CenteredUV15_g1;
				float2 appendResult23_g1 = (float2(( length( CenteredUV15_g1 ) * 1.0 * 2.0 ) , ( atan2( break17_g1.x , break17_g1.y ) * ( 1.0 / TWO_PI ) * 1.0 )));
				float2 temp_output_6_0 = appendResult23_g1;
				float2 panner7 = ( 1.0 * _Time.y * _Panner_1_Speed + temp_output_6_0);
				float2 panner25 = ( 1.0 * _Time.y * _Panner_2_Speed + temp_output_6_0);
				float4 Texture46 = ( tex2D( _Texture0, panner7 ) + tex2D( _Texture1, panner25 ) );
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = IN.ase_texcoord3.xyz;
				float fresnelNdotV43 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode43 = ( 0.0 + (_Fres_Scale_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.x ) ) ) - 0.0) * (_Fres_Scale_MaxNew - _Fres_Scale_MinNew) / (1.0 - 0.0)) * pow( 1.0 - fresnelNdotV43, (_Fres_Power_MinNew + (abs( sin( ( _TimeParameters.x * _Fres_ScaleX_PowerY_Speeds.y ) ) ) - 0.0) * (_Fres_Power_MaxNew - _Fres_Power_MinNew) / (1.0 - 0.0)) ) );
				float time51 = ( _Voronoi_Speed * _TimeParameters.x );
				float2 voronoiSmoothId51 = 0;
				float2 coords51 = IN.ase_texcoord2.xy * _Voronoi_Scale;
				float2 id51 = 0;
				float2 uv51 = 0;
				float voroi51 = voronoi51( coords51, time51, id51, uv51, 0, voronoiSmoothId51 );
				float fresnelNdotV140 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode140 = ( 0.0 + (20.0 + (abs( sin( ( _TimeParameters.x * 10.0 ) ) ) - 0.0) * (10.0 - 20.0) / (1.0 - 0.0)) * pow( 1.0 - fresnelNdotV140, (1.0 + (abs( sin( ( _TimeParameters.x * 4.0 ) ) ) - 0.0) * (0.5 - 1.0) / (1.0 - 0.0)) ) );
				float time141 = ( 4.0 * _TimeParameters.x );
				float2 voronoiSmoothId141 = 0;
				float2 coords141 = IN.ase_texcoord2.xy * 8.15;
				float2 id141 = 0;
				float2 uv141 = 0;
				float fade141 = 0.5;
				float voroi141 = 0;
				float rest141 = 0;
				for( int it141 = 0; it141 <3; it141++ ){
				voroi141 += fade141 * voronoi141( coords141, time141, id141, uv141, 0,voronoiSmoothId141 );
				rest141 += fade141;
				coords141 *= 2;
				fade141 *= 0.5;
				}//Voronoi141
				voroi141 /= rest141;
				float ShieldDamage119 = _ShieldDamage;
				float lerpResult144 = lerp( ( fresnelNode43 * voroi51 ) , ( fresnelNode140 * voroi141 ) , ShieldDamage119);
				float Fresnel45 = lerpResult144;
				float2 CenteredUV15_g2 = ( IN.ase_texcoord2.xy - float2( 0.5,0.5 ) );
				float2 break17_g2 = CenteredUV15_g2;
				float2 appendResult23_g2 = (float2(( length( CenteredUV15_g2 ) * 1.0 * 2.0 ) , ( atan2( break17_g2.x , break17_g2.y ) * ( 1.0 / TWO_PI ) * 1.0 )));
				float lerpResult117 = lerp( _VignetteSmoothness , 1.3 , ShieldDamage119);
				float PolarMask47 = pow( ( appendResult23_g2.x * lerpResult117 ) , _VignettePowerExp );
				float4 temp_output_11_0 = ( ( Texture46 + Fresnel45 ) * PolarMask47 );
				float2 uv_SoftEgdeMask_Texture = IN.ase_texcoord2.xy * _SoftEgdeMask_Texture_ST.xy + _SoftEgdeMask_Texture_ST.zw;
				float SoftEdgeMask77 = ( tex2D( _SoftEgdeMask_Texture, uv_SoftEgdeMask_Texture ).r * _SoftEdgeFade );
				float4 lerpResult70 = lerp( temp_output_11_0 , ( temp_output_11_0 * SoftEdgeMask77 ) , _ToggleSoftMask);
				float4 Pulse96 = lerpResult70;
				float time93 = ( _Voronoi_2_Speed * _TimeParameters.x );
				float2 voronoiSmoothId93 = 0;
				float2 coords93 = IN.ase_texcoord2.xy * _Voronoi_2_Scale;
				float2 id93 = 0;
				float2 uv93 = 0;
				float voroi93 = voronoi93( coords93, time93, id93, uv93, 0, voronoiSmoothId93 );
				float4 appendResult92 = (float4(IN.ase_texcoord4.x , IN.ase_texcoord4.y , _Voronoi_Power_Exp , _Voronoi_Power_Exp));
				float Fire_Disolve95 = pow( voroi93 , appendResult92.x );
				float4 lerpResult84 = lerp( lerpResult70 , ( Pulse96 * Fire_Disolve95 ) , _StopSplash);
				float VertexColour103 = IN.ase_color.a;
				float4 clampResult69 = clamp( ( lerpResult84 * VertexColour103 ) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
				float4 Alpha80 = clampResult69;
				
				float Alpha = Alpha80.r;
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
598;297;1244;693;4312.619;660.473;1.655106;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;29;-3435.538,-480.7477;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;30;-3574.844,-409.1846;Inherit;False;Property;_Fres_ScaleX_PowerY_Speeds;Fres_Scale(X)_Power(Y)_Speeds;14;0;Create;True;0;0;0;False;0;False;1,1;5,2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;122;-3683.584,37.05772;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;-3237.052,-308.8709;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;125;-3485.922,-72.52287;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;124;-3489.098,161.9346;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;4;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-3233.876,-543.3283;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;34;-3099.184,-538.1313;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;33;-3102.359,-303.6739;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;126;-3351.23,-67.32584;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;127;-3354.405,167.1316;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-3109.023,-152.727;Inherit;False;Property;_Fres_Power_MaxNew;Fres_Power_MaxNew;11;0;Create;True;0;0;0;False;0;False;5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;35;-3106.847,-387.1846;Inherit;False;Property;_Fres_Scale_MaxNew;Fres_Scale_MaxNew;12;0;Create;True;0;0;0;False;0;False;2;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;-2648.052,-248.2727;Inherit;False;Property;_Voronoi_Speed;Voronoi_Speed;15;0;Create;True;0;0;0;False;0;False;0;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;55;-2651.052,-165.2727;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;37;-2981.533,-533.4023;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-3109.023,-225.727;Inherit;False;Property;_Fres_Power_MinNew;Fres_Power_MinNew;13;0;Create;True;0;0;0;False;0;False;3;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;107;-1388.029,-1409.434;Inherit;False;Property;_ShieldDamage;ShieldDamage;0;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;38;-2984.71,-298.945;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;134;-2903.098,305.5327;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;128;-3236.756,171.8604;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-3105.847,-460.1846;Inherit;False;Property;_Fres_Scale_MinNew;Fres_Scale_MinNew;10;0;Create;True;0;0;0;False;0;False;1;20;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;132;-3233.579,-62.59684;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;24;-1697.22,1279.274;Inherit;False;Property;_Panner_2_Speed;Panner_2_Speed;4;0;Create;True;0;0;0;False;0;False;-1,0;-3,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TFHCRemapNode;136;-3118.579,-57.59654;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;20;False;4;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;119;-1108.848,-1381.546;Inherit;False;ShieldDamage;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;56;-2433.052,-119.2727;Inherit;False;Property;_Voronoi_Scale;Voronoi_Scale;16;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;138;-3105.756,173.8605;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;0.5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;6;-1866.715,820.415;Inherit;True;Polar Coordinates;-1;;1;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;139;-2695.098,236.5327;Inherit;False;2;2;0;FLOAT;4;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;8;-1495.249,886.8975;Inherit;False;Property;_Panner_1_Speed;Panner_1_Speed;2;0;Create;True;0;0;0;False;0;False;-1,0;-3,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-2443.052,-234.2727;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;42;-2853.71,-296.945;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;41;-2866.533,-528.402;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;1;False;4;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;43;-2665.975,-481.8287;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;140;-2918.021,-11.02327;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;7;-1277.062,816.373;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;23;-1483.446,1031.396;Inherit;True;Property;_Texture1;Texture 1;9;0;Create;True;0;0;0;False;0;False;None;5512be1e0e083f24eb6f50956c6066f6;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.PannerNode;25;-1468.408,1223.106;Inherit;True;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VoronoiNode;51;-2230.189,-315.9625;Inherit;True;0;0;1;3;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.VoronoiNode;141;-2482.235,154.843;Inherit;True;0;0;1;3;3;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;8.15;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.TexturePropertyNode;10;-1324.24,629.0244;Inherit;True;Property;_Texture0;Texture 0;8;0;Create;True;0;0;0;False;0;False;None;41bc13a0b2b7ee84196f8f26ec56af2d;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.FunctionNode;15;-1821.585,-620.1916;Inherit;True;Polar Coordinates;-1;;2;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-1649.675,-934.4273;Inherit;False;Property;_VignetteSmoothness;VignetteSmoothness;1;0;Create;True;0;0;0;False;0;False;0.73;1.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-1633.104,-848.2462;Inherit;False;Constant;_VignetteDamaged;VignetteDamaged;1;0;Create;True;0;0;0;False;0;False;1.3;1.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;121;-1544.261,-732.4949;Inherit;False;119;ShieldDamage;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;145;-1963.441,98.94403;Inherit;False;119;ShieldDamage;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;72;-1626.569,325.0364;Inherit;True;Property;_SoftEgdeMask_Texture;SoftEgdeMask_Texture;22;0;Create;True;0;0;0;False;0;False;9a7be70759d48ff48a8ef359791bfa99;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SamplerNode;22;-970.1536,1027.096;Inherit;True;Property;_TextureSample1;Texture Sample 1;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;57;-2059.119,-373.5156;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;13;-1515.775,-625.6415;Inherit;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.LerpOp;117;-1278.23,-898.9365;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-976.9404,835.7244;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;142;-2311.166,97.28982;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-1087.127,-525.6415;Inherit;False;Property;_VignettePowerExp;VignettePowerExp;6;0;Create;True;0;0;0;False;0;False;8.29;2.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;73;-1331.915,326.5975;Inherit;True;Property;_MAT_Masking_SquareMaskSprite;MAT_Masking_SquareMaskSprite;4;0;Create;True;0;0;0;False;0;False;-1;9a7be70759d48ff48a8ef359791bfa99;9a7be70759d48ff48a8ef359791bfa99;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;74;-1046.44,426.6036;Inherit;False;Property;_SoftEdgeFade;SoftEdgeFade;23;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-648.438,937.5864;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1086.774,-627.6415;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;144;-1756.841,-146.6049;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;17;-841.7741,-626.6415;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;45;-1545.287,-146.4301;Inherit;False;Fresnel;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-895.4625,342.6006;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;-501.3502,927.2736;Inherit;False;Texture;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;49;-394.6367,-439.203;Inherit;False;46;Texture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.ComponentMaskNode;76;-754.4166,337.7294;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;47;-567.9539,-643.2861;Inherit;False;PolarMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;50;-393.434,-348.8025;Inherit;False;45;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;77;-526.9508,320.4122;Inherit;True;SoftEdgeMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;87;-186.6449,389.3138;Inherit;False;Property;_Voronoi_2_Speed;Voronoi_2_Speed;3;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;44;-150.6662,-435.4533;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;38.82647,-226.0153;Inherit;False;47;PolarMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;86;-183.7828,463.3595;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;88;38.43232,503.3287;Inherit;False;Property;_Voronoi_2_Scale;Voronoi_2_Scale;5;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-165.1297,715.7442;Inherit;False;Property;_Voronoi_Power_Exp;Voronoi_Power_Exp;7;0;Create;True;0;0;0;False;0;False;0.7647059;0.7647059;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;58.21724,376.3595;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;11;343.6104,-429.1602;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;89;-215.5457,530.1995;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;78;-27.31748,-102.7786;Inherit;False;77;SoftEdgeMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;79;200.7507,-78.93762;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;71;398.7513,-203.0369;Inherit;False;Property;_ToggleSoftMask;ToggleSoftMask;21;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;92;375.4543,608.1995;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.VoronoiNode;93;232.9402,372.8059;Inherit;True;0;0;1;0;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.LerpOp;70;763.6132,-442.3887;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;94;426.7613,388.304;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT4;2,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;96;1050.519,-636.6443;Inherit;False;Pulse;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;95;679.6103,389.7767;Inherit;False;Fire_Disolve;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;100;-506.7147,-1086.694;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;97;536.8405,-119.7887;Inherit;False;96;Pulse;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;98;517.776,-42.47214;Inherit;False;95;Fire_Disolve;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;716.8926,-111.3156;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;103;-184.4197,-1041.12;Inherit;False;VertexColour;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;699.1601,96.38354;Inherit;False;Property;_StopSplash;StopSplash;24;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;84;1108.932,-451.8374;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;105;1156.444,-235.8633;Inherit;False;103;VertexColour;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;1383.444,-330.8633;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;69;1582.911,-549.3517;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;80;1813.811,-562.0363;Inherit;False;Alpha;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-109.3433,-1365.576;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;108;-787.0825,-1643.365;Inherit;False;Property;_Colour_Impact_1;Colour_Impact_1;18;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;2.297397,1.35141,1.297354,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;241.5421,-1351.233;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;112;59.82907,-1466.742;Inherit;False;46;Texture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;66;62.77214,-1015.739;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;-163.1534,-937.9074;Inherit;False;46;Texture;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;106;567.6835,-1034.129;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;114;261.0656,-1536.056;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-118.4197,-1187.12;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;58;-148.547,-654.8287;Inherit;False;45;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;68;858.0084,-1029.467;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;1,1,1,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;110;-94.4627,-1545.795;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;113;31.48378,-1261.757;Inherit;False;45;Fresnel;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;83;1685.69,-737.29;Inherit;False;82;Colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;65;75.15129,-751.6511;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;82;1049.69,-1067.29;Inherit;False;Colour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;63;-797.6452,-1160.676;Inherit;False;Property;_Colour_1;Colour_1;17;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;2.867696,1.611372,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;102;-226.4197,-835.1204;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;109;-793.9988,-1347.369;Inherit;False;Property;_Colour_Impact_2;Colour_Impact_2;20;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;1,0.5731823,0.5235849,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;64;-794.6413,-985.3781;Inherit;False;Property;_Colour_2;Colour_2;19;1;[HDR];Create;True;0;0;0;False;0;False;0,0,0,0;0.9003702,0.3984397,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;60;234.5467,-896.4031;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;81;1686.719,-656.2571;Inherit;False;80;Alpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;116;434.1738,-1435.835;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;120;310.9136,-788.291;Inherit;False;119;ShieldDamage;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;2303.243,-822.0376;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;Shader_VFX_EnergyWard;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;1;637948977415539814;  Blend;0;0;Two Sided;1;637949279730780235;Cast Shadows;0;637948977428227121;  Use Shadow Threshold;0;0;Receive Shadows;0;637948977434428785;GPU Instancing;1;0;LOD CrossFade;0;0;Built-in Fog;0;0;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Vertex Position,InvertActionOnDeselection;1;0;0;5;False;True;False;True;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;32;0;29;0
WireConnection;32;1;30;2
WireConnection;125;0;122;0
WireConnection;124;0;122;0
WireConnection;31;0;29;0
WireConnection;31;1;30;1
WireConnection;34;0;31;0
WireConnection;33;0;32;0
WireConnection;126;0;125;0
WireConnection;127;0;124;0
WireConnection;37;0;34;0
WireConnection;38;0;33;0
WireConnection;128;0;127;0
WireConnection;132;0;126;0
WireConnection;136;0;132;0
WireConnection;119;0;107;0
WireConnection;138;0;128;0
WireConnection;139;1;134;0
WireConnection;53;0;54;0
WireConnection;53;1;55;0
WireConnection;42;0;38;0
WireConnection;42;3;36;0
WireConnection;42;4;40;0
WireConnection;41;0;37;0
WireConnection;41;3;39;0
WireConnection;41;4;35;0
WireConnection;43;2;41;0
WireConnection;43;3;42;0
WireConnection;140;2;136;0
WireConnection;140;3;138;0
WireConnection;7;0;6;0
WireConnection;7;2;8;0
WireConnection;25;0;6;0
WireConnection;25;2;24;0
WireConnection;51;1;53;0
WireConnection;51;2;56;0
WireConnection;141;1;139;0
WireConnection;22;0;23;0
WireConnection;22;1;25;0
WireConnection;57;0;43;0
WireConnection;57;1;51;0
WireConnection;13;0;15;0
WireConnection;117;0;16;0
WireConnection;117;1;118;0
WireConnection;117;2;121;0
WireConnection;9;0;10;0
WireConnection;9;1;7;0
WireConnection;142;0;140;0
WireConnection;142;1;141;0
WireConnection;73;0;72;0
WireConnection;26;0;9;0
WireConnection;26;1;22;0
WireConnection;14;0;13;0
WireConnection;14;1;117;0
WireConnection;144;0;57;0
WireConnection;144;1;142;0
WireConnection;144;2;145;0
WireConnection;17;0;14;0
WireConnection;17;1;12;0
WireConnection;45;0;144;0
WireConnection;75;0;73;1
WireConnection;75;1;74;0
WireConnection;46;0;26;0
WireConnection;76;0;75;0
WireConnection;47;0;17;0
WireConnection;77;0;76;0
WireConnection;44;0;49;0
WireConnection;44;1;50;0
WireConnection;91;0;87;0
WireConnection;91;1;86;0
WireConnection;11;0;44;0
WireConnection;11;1;48;0
WireConnection;79;0;11;0
WireConnection;79;1;78;0
WireConnection;92;0;89;1
WireConnection;92;1;89;2
WireConnection;92;2;90;0
WireConnection;92;3;90;0
WireConnection;93;1;91;0
WireConnection;93;2;88;0
WireConnection;70;0;11;0
WireConnection;70;1;79;0
WireConnection;70;2;71;0
WireConnection;94;0;93;0
WireConnection;94;1;92;0
WireConnection;96;0;70;0
WireConnection;95;0;94;0
WireConnection;99;0;97;0
WireConnection;99;1;98;0
WireConnection;103;0;100;4
WireConnection;84;0;70;0
WireConnection;84;1;99;0
WireConnection;84;2;85;0
WireConnection;104;0;84;0
WireConnection;104;1;105;0
WireConnection;69;0;104;0
WireConnection;80;0;69;0
WireConnection;111;0;109;0
WireConnection;111;1;100;0
WireConnection;115;0;111;0
WireConnection;115;1;113;0
WireConnection;66;0;101;0
WireConnection;66;1;59;0
WireConnection;106;0;60;0
WireConnection;106;1;116;0
WireConnection;106;2;120;0
WireConnection;114;0;110;0
WireConnection;114;1;112;0
WireConnection;101;0;63;0
WireConnection;101;1;100;0
WireConnection;68;0;106;0
WireConnection;110;0;108;0
WireConnection;110;1;100;0
WireConnection;65;0;102;0
WireConnection;65;1;58;0
WireConnection;82;0;68;0
WireConnection;102;0;100;0
WireConnection;102;1;64;0
WireConnection;60;0;66;0
WireConnection;60;1;65;0
WireConnection;116;0;114;0
WireConnection;116;1;115;0
WireConnection;1;2;83;0
WireConnection;1;3;81;0
ASEEND*/
//CHKSM=421A269C4A0FB3BB28630D29C7632B46BD1B2214
// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Shader_VFX_FullScreen"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin]_Toggle_EffectIntensity("Toggle_EffectIntensity", Range( 0 , 10)) = 0
		_Toggle_Selection12("Toggle_Selection 1 & 2", Range( 0 , 1)) = 0
		_Toggle_Array12("Toggle_Array 1 & 2 ", Range( 0 , 1)) = 0
		_Toggle_Array34("Toggle_Array 3 & 4 ", Range( 0 , 1)) = 0
		_Toggle_BetweenVoidBlinkLifesteal("Toggle_Between VoidBlink & Lifesteal", Range( 0 , 1)) = 1
		_Toggle_BetweenOnFireInToxic("Toggle_Between OnFire & InToxic", Range( 0 , 1)) = 0
		_Toggle_BetweenDamagedSlowed("Toggle_Between Damaged & Slowed", Range( 0 , 1)) = 0
		[HDR]_BaseColour("BaseColour", Color) = (1,0,0,0)
		_VignetteSmoothness("VignetteSmoothness", Float) = 0.64
		_VignettePowerExp("VignettePowerExp", Float) = 5.09
		_SmoothStepCull("SmoothStepCull", Float) = 1
		_SmoothStepFade("SmoothStepFade", Range( -2 , 0.99)) = 0
		_TexNoiseScale("TexNoiseScale", Float) = 0
		_Texture0("Texture 0", 2D) = "white" {}
		[ASEEnd]_UseFullscreenMaskOrRectangleMask("Use FullscreenMask Or RectangleMask", Range( 0 , 1)) = 0

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
				float4 ase_texcoord3 : TEXCOORD3;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColour;
			float _VignetteSmoothness;
			float _VignettePowerExp;
			float _UseFullscreenMaskOrRectangleMask;
			float _TexNoiseScale;
			float _Toggle_BetweenVoidBlinkLifesteal;
			float _Toggle_BetweenOnFireInToxic;
			float _Toggle_Array12;
			float _Toggle_BetweenDamagedSlowed;
			float _Toggle_Array34;
			float _Toggle_Selection12;
			float _Toggle_EffectIntensity;
			float _SmoothStepCull;
			float _SmoothStepFade;
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


					float2 voronoihash17( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi17( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash17( n + g );
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
			
					float2 voronoihash62( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi62( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash62( n + g );
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
						
			F1 = 8.0;
			for ( int j = -2; j <= 2; j++ )
			{
			for ( int i = -2; i <= 2; i++ )
			{
			float2 g = mg + float2( i, j );
			float2 o = voronoihash62( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
			float d = dot( 0.5 * ( r + mr ), normalize( r - mr ) );
			F1 = min( F1, d );
			}
			}
			return F1;
					}
			
					float2 voronoihash80( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi80( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash80( n + g );
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
						return F2 - F1;
					}
			
					float2 voronoihash115( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi115( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -2; j <= 2; j++ )
						{
							for ( int i = -2; i <= 2; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash115( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.707 * sqrt(dot( r, r ));
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
			
					float2 voronoihash123( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi123( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -2; j <= 2; j++ )
						{
							for ( int i = -2; i <= 2; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash123( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.707 * sqrt(dot( r, r ));
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
			
					float2 voronoihash176( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi176( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash176( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.707 * sqrt(dot( r, r ));
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F2;
					}
			
			
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord3.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord3.zw = 0;
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
				float2 CenteredUV15_g1 = ( IN.ase_texcoord3.xy - float2( 0.5,0.5 ) );
				float2 break17_g1 = CenteredUV15_g1;
				float2 appendResult23_g1 = (float2(( length( CenteredUV15_g1 ) * 1.0 * 2.0 ) , ( atan2( break17_g1.x , break17_g1.y ) * ( 1.0 / TWO_PI ) * 1.0 )));
				float clampResult53 = clamp( pow( ( appendResult23_g1.x * _VignetteSmoothness ) , _VignettePowerExp ) , 0.0 , 1.0 );
				float FullScreenMask18 = clampResult53;
				float2 texCoord148 = IN.ase_texcoord3.xy * float2( 0.9,0.9 ) + float2( 0.1,0.1 );
				float2 texCoord151 = IN.ase_texcoord3.xy * float2( 0.9,0.9 ) + float2( 0,0 );
				float RectangleMask136 = ( ( 1.0 - ( tex2D( _Texture0, texCoord148 ).r + tex2D( _Texture0, texCoord151 ).r ) ) * FullScreenMask18 );
				float lerpResult153 = lerp( FullScreenMask18 , RectangleMask136 , _UseFullscreenMaskOrRectangleMask);
				float time17 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId17 = 0;
				float2 appendResult102 = (float2(WorldPosition.x , WorldPosition.y));
				float2 temp_output_104_0 = ( appendResult102 * _TexNoiseScale * 0.001 );
				float2 coords17 = temp_output_104_0 * 4.0;
				float2 id17 = 0;
				float2 uv17 = 0;
				float fade17 = 0.5;
				float voroi17 = 0;
				float rest17 = 0;
				for( int it17 = 0; it17 <4; it17++ ){
				voroi17 += fade17 * voronoi17( coords17, time17, id17, uv17, 0,voronoiSmoothId17 );
				rest17 += fade17;
				coords17 *= 2;
				fade17 *= 0.5;
				}//Voronoi17
				voroi17 /= rest17;
				float VFX_VoidBlink42 = ( lerpResult153 * pow( voroi17 , 1.6 ) );
				float time62 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId62 = 0;
				float2 coords62 = temp_output_104_0 * 4.0;
				float2 id62 = 0;
				float2 uv62 = 0;
				float fade62 = 0.5;
				float voroi62 = 0;
				float rest62 = 0;
				for( int it62 = 0; it62 <4; it62++ ){
				voroi62 += fade62 * voronoi62( coords62, time62, id62, uv62, 0,voronoiSmoothId62 );
				rest62 += fade62;
				coords62 *= 2;
				fade62 *= 0.5;
				}//Voronoi62
				voroi62 /= rest62;
				float VFX_Lifesteal65 = ( lerpResult153 * pow( voroi62 , 1.6 ) );
				float lerpResult32 = lerp( VFX_VoidBlink42 , VFX_Lifesteal65 , _Toggle_BetweenVoidBlinkLifesteal);
				float time80 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId80 = 0;
				float2 coords80 = temp_output_104_0 * 4.0;
				float2 id80 = 0;
				float2 uv80 = 0;
				float fade80 = 0.5;
				float voroi80 = 0;
				float rest80 = 0;
				for( int it80 = 0; it80 <5; it80++ ){
				voroi80 += fade80 * voronoi80( coords80, time80, id80, uv80, 0,voronoiSmoothId80 );
				rest80 += fade80;
				coords80 *= 2;
				fade80 *= 0.5;
				}//Voronoi80
				voroi80 /= rest80;
				float VFX_OnFire88 = ( lerpResult153 * pow( voroi80 , -0.87 ) );
				float time115 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId115 = 0;
				float2 coords115 = temp_output_104_0 * 4.0;
				float2 id115 = 0;
				float2 uv115 = 0;
				float fade115 = 0.5;
				float voroi115 = 0;
				float rest115 = 0;
				for( int it115 = 0; it115 <5; it115++ ){
				voroi115 += fade115 * voronoi115( coords115, time115, id115, uv115, 0,voronoiSmoothId115 );
				rest115 += fade115;
				coords115 *= 2;
				fade115 *= 0.5;
				}//Voronoi115
				voroi115 /= rest115;
				float VFX_InToxic114 = ( FullScreenMask18 * pow( voroi115 , -0.87 ) );
				float lerpResult33 = lerp( VFX_OnFire88 , VFX_InToxic114 , _Toggle_BetweenOnFireInToxic);
				float lerpResult35 = lerp( lerpResult32 , lerpResult33 , _Toggle_Array12);
				float time123 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId123 = 0;
				float2 coords123 = temp_output_104_0 * 4.0;
				float2 id123 = 0;
				float2 uv123 = 0;
				float fade123 = 0.5;
				float voroi123 = 0;
				float rest123 = 0;
				for( int it123 = 0; it123 <5; it123++ ){
				voroi123 += fade123 * voronoi123( coords123, time123, id123, uv123, 0,voronoiSmoothId123 );
				rest123 += fade123;
				coords123 *= 2;
				fade123 *= 0.5;
				}//Voronoi123
				voroi123 /= rest123;
				float temp_output_126_0 = pow( voroi123 , -0.87 );
				float lerpResult164 = lerp( temp_output_126_0 , ( temp_output_126_0 * 0.7 ) , ( temp_output_126_0 * abs( sin( ( 2.5 * _TimeParameters.x ) ) ) ));
				float VFX_NearDeath128 = ( lerpResult153 * lerpResult164 );
				float time176 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId176 = 0;
				float2 coords176 = temp_output_104_0 * 4.0;
				float2 id176 = 0;
				float2 uv176 = 0;
				float voroi176 = voronoi176( coords176, time176, id176, uv176, 0, voronoiSmoothId176 );
				float VFX_Slowed184 = ( lerpResult153 * pow( voroi176 , -0.87 ) );
				float lerpResult34 = lerp( VFX_NearDeath128 , VFX_Slowed184 , _Toggle_BetweenDamagedSlowed);
				float lerpResult36 = lerp( 0.0 , 0.0 , 0.0);
				float lerpResult37 = lerp( lerpResult34 , lerpResult36 , _Toggle_Array34);
				float lerpResult38 = lerp( lerpResult35 , lerpResult37 , _Toggle_Selection12);
				float clampResult96 = clamp( (0.0 + (lerpResult38 - 0.0) * (3.0 - 0.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
				float FullScreenEffect29 = clampResult96;
				
				float2 texCoord91 = IN.ase_texcoord3.xy * float2( 1,1 ) + float2( 0,0 );
				float smoothstepResult48 = smoothstep( _SmoothStepCull , _SmoothStepFade , texCoord91.y);
				float clampResult54 = clamp( smoothstepResult48 , 0.0 , 1.0 );
				float VerticalMasking55 = clampResult54;
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = ( FullScreenEffect29 * _BaseColour ).rgb;
				float Alpha = ( _Toggle_EffectIntensity * FullScreenEffect29 * VerticalMasking55 );
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
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseColour;
			float _VignetteSmoothness;
			float _VignettePowerExp;
			float _UseFullscreenMaskOrRectangleMask;
			float _TexNoiseScale;
			float _Toggle_BetweenVoidBlinkLifesteal;
			float _Toggle_BetweenOnFireInToxic;
			float _Toggle_Array12;
			float _Toggle_BetweenDamagedSlowed;
			float _Toggle_Array34;
			float _Toggle_Selection12;
			float _Toggle_EffectIntensity;
			float _SmoothStepCull;
			float _SmoothStepFade;
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


					float2 voronoihash17( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi17( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash17( n + g );
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
			
					float2 voronoihash62( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi62( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash62( n + g );
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
						
			F1 = 8.0;
			for ( int j = -2; j <= 2; j++ )
			{
			for ( int i = -2; i <= 2; i++ )
			{
			float2 g = mg + float2( i, j );
			float2 o = voronoihash62( n + g );
					o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
			float d = dot( 0.5 * ( r + mr ), normalize( r - mr ) );
			F1 = min( F1, d );
			}
			}
			return F1;
					}
			
					float2 voronoihash80( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi80( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash80( n + g );
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
						return F2 - F1;
					}
			
					float2 voronoihash115( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi115( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -2; j <= 2; j++ )
						{
							for ( int i = -2; i <= 2; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash115( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.707 * sqrt(dot( r, r ));
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
			
					float2 voronoihash123( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi123( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
					{
						float2 n = floor( v );
						float2 f = frac( v );
						float F1 = 8.0;
						float F2 = 8.0; float2 mg = 0;
						for ( int j = -2; j <= 2; j++ )
						{
							for ( int i = -2; i <= 2; i++ )
						 	{
						 		float2 g = float2( i, j );
						 		float2 o = voronoihash123( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.707 * sqrt(dot( r, r ));
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
			
					float2 voronoihash176( float2 p )
					{
						
						p = float2( dot( p, float2( 127.1, 311.7 ) ), dot( p, float2( 269.5, 183.3 ) ) );
						return frac( sin( p ) *43758.5453);
					}
			
					float voronoi176( float2 v, float time, inout float2 id, inout float2 mr, float smoothness, inout float2 smoothId )
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
						 		float2 o = voronoihash176( n + g );
								o = ( sin( time + o * 6.2831 ) * 0.5 + 0.5 ); float2 r = f - g - o;
								float d = 0.707 * sqrt(dot( r, r ));
						 		if( d<F1 ) {
						 			F2 = F1;
						 			F1 = d; mg = g; mr = r; id = o;
						 		} else if( d<F2 ) {
						 			F2 = d;
						
						 		}
						 	}
						}
						return F2;
					}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
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
				float clampResult53 = clamp( pow( ( appendResult23_g1.x * _VignetteSmoothness ) , _VignettePowerExp ) , 0.0 , 1.0 );
				float FullScreenMask18 = clampResult53;
				float2 texCoord148 = IN.ase_texcoord2.xy * float2( 0.9,0.9 ) + float2( 0.1,0.1 );
				float2 texCoord151 = IN.ase_texcoord2.xy * float2( 0.9,0.9 ) + float2( 0,0 );
				float RectangleMask136 = ( ( 1.0 - ( tex2D( _Texture0, texCoord148 ).r + tex2D( _Texture0, texCoord151 ).r ) ) * FullScreenMask18 );
				float lerpResult153 = lerp( FullScreenMask18 , RectangleMask136 , _UseFullscreenMaskOrRectangleMask);
				float time17 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId17 = 0;
				float2 appendResult102 = (float2(WorldPosition.x , WorldPosition.y));
				float2 temp_output_104_0 = ( appendResult102 * _TexNoiseScale * 0.001 );
				float2 coords17 = temp_output_104_0 * 4.0;
				float2 id17 = 0;
				float2 uv17 = 0;
				float fade17 = 0.5;
				float voroi17 = 0;
				float rest17 = 0;
				for( int it17 = 0; it17 <4; it17++ ){
				voroi17 += fade17 * voronoi17( coords17, time17, id17, uv17, 0,voronoiSmoothId17 );
				rest17 += fade17;
				coords17 *= 2;
				fade17 *= 0.5;
				}//Voronoi17
				voroi17 /= rest17;
				float VFX_VoidBlink42 = ( lerpResult153 * pow( voroi17 , 1.6 ) );
				float time62 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId62 = 0;
				float2 coords62 = temp_output_104_0 * 4.0;
				float2 id62 = 0;
				float2 uv62 = 0;
				float fade62 = 0.5;
				float voroi62 = 0;
				float rest62 = 0;
				for( int it62 = 0; it62 <4; it62++ ){
				voroi62 += fade62 * voronoi62( coords62, time62, id62, uv62, 0,voronoiSmoothId62 );
				rest62 += fade62;
				coords62 *= 2;
				fade62 *= 0.5;
				}//Voronoi62
				voroi62 /= rest62;
				float VFX_Lifesteal65 = ( lerpResult153 * pow( voroi62 , 1.6 ) );
				float lerpResult32 = lerp( VFX_VoidBlink42 , VFX_Lifesteal65 , _Toggle_BetweenVoidBlinkLifesteal);
				float time80 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId80 = 0;
				float2 coords80 = temp_output_104_0 * 4.0;
				float2 id80 = 0;
				float2 uv80 = 0;
				float fade80 = 0.5;
				float voroi80 = 0;
				float rest80 = 0;
				for( int it80 = 0; it80 <5; it80++ ){
				voroi80 += fade80 * voronoi80( coords80, time80, id80, uv80, 0,voronoiSmoothId80 );
				rest80 += fade80;
				coords80 *= 2;
				fade80 *= 0.5;
				}//Voronoi80
				voroi80 /= rest80;
				float VFX_OnFire88 = ( lerpResult153 * pow( voroi80 , -0.87 ) );
				float time115 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId115 = 0;
				float2 coords115 = temp_output_104_0 * 4.0;
				float2 id115 = 0;
				float2 uv115 = 0;
				float fade115 = 0.5;
				float voroi115 = 0;
				float rest115 = 0;
				for( int it115 = 0; it115 <5; it115++ ){
				voroi115 += fade115 * voronoi115( coords115, time115, id115, uv115, 0,voronoiSmoothId115 );
				rest115 += fade115;
				coords115 *= 2;
				fade115 *= 0.5;
				}//Voronoi115
				voroi115 /= rest115;
				float VFX_InToxic114 = ( FullScreenMask18 * pow( voroi115 , -0.87 ) );
				float lerpResult33 = lerp( VFX_OnFire88 , VFX_InToxic114 , _Toggle_BetweenOnFireInToxic);
				float lerpResult35 = lerp( lerpResult32 , lerpResult33 , _Toggle_Array12);
				float time123 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId123 = 0;
				float2 coords123 = temp_output_104_0 * 4.0;
				float2 id123 = 0;
				float2 uv123 = 0;
				float fade123 = 0.5;
				float voroi123 = 0;
				float rest123 = 0;
				for( int it123 = 0; it123 <5; it123++ ){
				voroi123 += fade123 * voronoi123( coords123, time123, id123, uv123, 0,voronoiSmoothId123 );
				rest123 += fade123;
				coords123 *= 2;
				fade123 *= 0.5;
				}//Voronoi123
				voroi123 /= rest123;
				float temp_output_126_0 = pow( voroi123 , -0.87 );
				float lerpResult164 = lerp( temp_output_126_0 , ( temp_output_126_0 * 0.7 ) , ( temp_output_126_0 * abs( sin( ( 2.5 * _TimeParameters.x ) ) ) ));
				float VFX_NearDeath128 = ( lerpResult153 * lerpResult164 );
				float time176 = ( _TimeParameters.x * 1.0 );
				float2 voronoiSmoothId176 = 0;
				float2 coords176 = temp_output_104_0 * 4.0;
				float2 id176 = 0;
				float2 uv176 = 0;
				float voroi176 = voronoi176( coords176, time176, id176, uv176, 0, voronoiSmoothId176 );
				float VFX_Slowed184 = ( lerpResult153 * pow( voroi176 , -0.87 ) );
				float lerpResult34 = lerp( VFX_NearDeath128 , VFX_Slowed184 , _Toggle_BetweenDamagedSlowed);
				float lerpResult36 = lerp( 0.0 , 0.0 , 0.0);
				float lerpResult37 = lerp( lerpResult34 , lerpResult36 , _Toggle_Array34);
				float lerpResult38 = lerp( lerpResult35 , lerpResult37 , _Toggle_Selection12);
				float clampResult96 = clamp( (0.0 + (lerpResult38 - 0.0) * (3.0 - 0.0) / (1.0 - 0.0)) , 0.0 , 1.0 );
				float FullScreenEffect29 = clampResult96;
				float2 texCoord91 = IN.ase_texcoord2.xy * float2( 1,1 ) + float2( 0,0 );
				float smoothstepResult48 = smoothstep( _SmoothStepCull , _SmoothStepFade , texCoord91.y);
				float clampResult54 = clamp( smoothstepResult48 , 0.0 , 1.0 );
				float VerticalMasking55 = clampResult54;
				
				float Alpha = ( _Toggle_EffectIntensity * FullScreenEffect29 * VerticalMasking55 );
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
7;18;1848;1001;3830.253;-1499.059;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;44;-364.7432,-1146.571;Inherit;False;1504.389;311.4498;Mask Polar;8;10;6;8;5;9;7;18;53;;1,1,1,1;0;0
Node;AmplifyShaderEditor.FunctionNode;5;-314.7432,-1089.121;Inherit;True;Polar Coordinates;-1;;1;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.BreakToComponentsNode;6;-8.933198,-1094.571;Inherit;True;FLOAT2;1;0;FLOAT2;0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;9;183.0666,-1030.571;Inherit;False;Property;_VignetteSmoothness;VignetteSmoothness;9;0;Create;True;0;0;0;False;0;False;0.64;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;192.7145,-950.5708;Inherit;False;Property;_VignettePowerExp;VignettePowerExp;10;0;Create;True;0;0;0;False;0;False;5.09;8.23;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;144;-1301.63,-1855.867;Inherit;True;Property;_Texture0;Texture 0;14;0;Create;True;0;0;0;False;0;False;9a7be70759d48ff48a8ef359791bfa99;9a7be70759d48ff48a8ef359791bfa99;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;148;-1225.416,-1972.989;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.9,0.9;False;1;FLOAT2;0.1,0.1;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;394.0672,-1098.571;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;151;-1221.778,-1671.731;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;0.9,0.9;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;150;-943.2928,-1777.31;Inherit;True;Property;_TextureSample1;Texture Sample 1;14;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;142;-941.9304,-1962.568;Inherit;True;Property;_TextureSample0;Texture Sample 0;14;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PowerNode;7;545.0674,-1095.571;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;152;-577.3544,-1770.216;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;101;-4012.692,239.7537;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;53;785.0547,-1093.411;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;141;-365.0071,-1735.975;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;102;-3788.834,209.1985;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;120;-3209.142,1381.938;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;119;-3253.142,1458.937;Inherit;False;Constant;_Voronoi_NearDeath_Speed;Voronoi_NearDeath_Speed;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;18;934.6462,-1104.606;Inherit;False;FullScreenMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;105;-3802.631,384.7384;Inherit;False;Constant;_UIScale;UIScale;12;0;Create;True;0;0;0;False;0;False;0.001;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;161;-3033.842,1764.762;Inherit;False;Constant;_NearDeath_BlinkSpeed;NearDeath_BlinkSpeed;16;0;Create;True;0;0;0;False;0;False;2.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;103;-3815.834,310.1985;Inherit;False;Property;_TexNoiseScale;TexNoiseScale;13;0;Create;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;162;-2990.842,1845.762;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;122;-2992.142,1410.938;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;140;-212.7812,-1770.347;Inherit;False;18;FullScreenMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;137;-211.2812,-1694.647;Inherit;False;True;True;True;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;121;-3142.142,1535.938;Inherit;False;Constant;_Voronoi_NearDeath_Scale;Voronoi_NearDeath_Scale;9;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;160;-2786.842,1807.762;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;104;-3592.834,248.1985;Inherit;False;3;3;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VoronoiNode;123;-2784.037,1365.736;Inherit;True;1;1;1;3;5;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleTimeNode;22;-3183.051,-42.21822;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;87;-3185.932,660.0381;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;138;-2.081268,-1759.847;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-3227.051,39.78169;Inherit;False;Constant;_Voronoi_Blink_Speed;Voronoi_Blink_Speed;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;170;-3490.384,2056.65;Inherit;False;Constant;_Voronoi_Slowed_Speed;Voronoi_Slowed_Speed;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;168;-3446.384,1979.65;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;124;-2955.543,1660.238;Inherit;False;Constant;_Voronoi_NearDeath_Power;Voronoi_NearDeath_Power;10;0;Create;True;0;0;0;False;0;False;-0.87;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;59;-3184.968,302.5302;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;159;-2642.842,1807.762;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;60;-3252.968,376.5301;Inherit;False;Constant;_Voronoi_Lifesteal_Speed;Voronoi_Lifesteal_Speed;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;81;-3253.932,734.038;Inherit;False;Constant;_Voronoi_OnFire_Speed;Voronoi_OnFire_Speed;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;107;-3204.646,1019.354;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-3248.646,1096.353;Inherit;False;Constant;_Voronoi_InToxic_Speed;Voronoi_InToxic_Speed;8;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-3142.932,811.0381;Inherit;False;Constant;_Voronoi_OnFire_Scale;Voronoi_OnFire_Scale;9;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;-2990.051,-16.2182;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;109;-3137.646,1173.354;Inherit;False;Constant;_Voronoi_InToxic_Scale;Voronoi_InToxic_Scale;9;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;173;-3379.384,2133.651;Inherit;False;Constant;_Voronoi_Slowed_Scale;Voronoi_Slowed_Scale;9;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;172;-3229.384,2008.65;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;61;-3141.968,453.5303;Inherit;False;Constant;_Voronoi_Lifesteal_Scale;Voronoi_Lifesteal_Scale;9;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-3016.051,94.7818;Inherit;False;Constant;_Voronoi_Blink_Scale;Voronoi_Blink_Scale;9;0;Create;True;0;0;0;False;0;False;4;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;126;-2545.142,1440.938;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;108;-2987.646,1048.354;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;136;209.8107,-1766.403;Inherit;False;RectangleMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-2991.968,328.5302;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;-2992.932,686.0381;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;165;-2544.396,1536.652;Inherit;False;Constant;_Float0;Float 0;16;0;Create;True;0;0;0;False;0;False;0.7;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode;163;-2519.842,1729.762;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;80;-2784.827,640.8372;Inherit;True;0;0;1;2;5;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;175;-3192.785,2257.951;Inherit;False;Constant;_Voronoi_Slowed_Power;Voronoi_Slowed_Power;10;0;Create;True;0;0;0;False;0;False;-0.87;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;154;-2718.031,-295.4246;Inherit;False;136;RectangleMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;-2801.052,203.7816;Inherit;False;Constant;_Voronoi_Blink_Power;Voronoi_Blink_Power;10;0;Create;True;0;0;0;False;0;False;1.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;176;-3021.279,1963.448;Inherit;True;0;1;1;1;1;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.RangedFloatNode;67;-2813.969,549.5302;Inherit;False;Constant;_Voronoi_Lifesteal_Power;Voronoi_Lifesteal_Power;10;0;Create;True;0;0;0;False;0;False;1.6;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;86;-2814.933,907.0381;Inherit;False;Constant;_Voronoi_OnFire_Power;Voronoi_OnFire_Power;10;0;Create;True;0;0;0;False;0;False;-0.87;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VoronoiNode;115;-2779.541,1003.152;Inherit;True;1;1;1;3;5;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.VoronoiNode;17;-2781.946,-61.41919;Inherit;True;0;0;1;3;4;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.VoronoiNode;62;-2783.863,283.3292;Inherit;True;0;0;1;4;4;False;1;False;False;False;4;0;FLOAT2;0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;3;FLOAT;0;FLOAT2;1;FLOAT2;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;166;-2333.396,1499.652;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;157;-2335.363,1590.58;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;155;-2765.712,-211.6918;Inherit;False;Property;_UseFullscreenMaskOrRectangleMask;Use FullscreenMask Or RectangleMask;15;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;110;-2809.647,1269.354;Inherit;False;Constant;_Voronoi_InToxic_Power;Voronoi_InToxic_Power;10;0;Create;True;0;0;0;False;0;False;-0.87;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;-2715.229,-372.9498;Inherit;False;18;FullScreenMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;83;-2545.932,716.0381;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;153;-2414.501,-367.5278;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;112;-2540.646,1078.354;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;179;-2782.384,2038.651;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;25;-2543.051,13.78183;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;164;-2194.514,1437.713;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;111;-2534.893,1007.946;Inherit;False;18;FullScreenMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;63;-2544.968,358.5302;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;113;-1947.746,1042.531;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-1952.242,1405.115;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;183;-2189.484,2002.828;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1956.652,321.1797;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;-1950.151,-22.04075;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-1953.032,680.2155;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;114;-1798.784,1036.24;Inherit;False;VFX_InToxic;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-1818.134,-26.74554;Inherit;False;VFX_VoidBlink;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;88;-1804.07,673.9246;Inherit;False;VFX_OnFire;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;65;-1820.05,318.003;Inherit;False;VFX_Lifesteal;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;128;-1803.281,1398.824;Inherit;False;VFX_NearDeath;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;184;-2006.079,2000.386;Inherit;False;VFX_Slowed;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;43;-802.7615,2.585205;Inherit;False;42;VFX_VoidBlink;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;90;-856.8464,403.5975;Inherit;False;Property;_Toggle_BetweenOnFireInToxic;Toggle_Between OnFire & InToxic;5;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;156;-827.4709,488.8147;Inherit;False;128;VFX_NearDeath;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;89;-787.8464,236.5975;Inherit;False;88;VFX_OnFire;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;68;-799.6083,75.16736;Inherit;False;65;VFX_Lifesteal;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;118;-891.0886,661.0371;Inherit;False;Property;_Toggle_BetweenDamagedSlowed;Toggle_Between Damaged & Slowed;6;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;116;-795.064,323.0868;Inherit;False;114;VFX_InToxic;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-841.4088,153.7796;Inherit;False;Property;_Toggle_BetweenVoidBlinkLifesteal;Toggle_Between VoidBlink & Lifesteal;4;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;185;-824.3804,573.795;Inherit;False;184;VFX_Slowed;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;33;-492.1341,253.8452;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;36;-556.7663,685.08;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-294.4664,639.4801;Inherit;False;Property;_Toggle_Array12;Toggle_Array 1 & 2 ;2;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;32;-495.2672,73.98885;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;34;-561.434,493.1454;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-293.4665,711.68;Inherit;False;Property;_Toggle_Array34;Toggle_Array 3 & 4 ;3;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;35;83.1336,265.58;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;41;-294.3777,785.1262;Inherit;False;Property;_Toggle_Selection12;Toggle_Selection 1 & 2;1;0;Create;True;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;91;-1486.18,-1212.43;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;37;80.03365,381.8802;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1492.372,-1085.184;Inherit;False;Property;_SmoothStepCull;SmoothStepCull;11;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-1536.862,-1007.272;Inherit;False;Property;_SmoothStepFade;SmoothStepFade;12;0;Create;True;0;0;0;False;0;False;0;0.99;-2;0.99;0;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;48;-1230.022,-1200.879;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;38;296.8555,300.6165;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;54;-979.8604,-1189.616;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;95;575.1719,257.113;Inherit;True;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;96;882.6111,154.8283;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;55;-841.4315,-1195.159;Inherit;False;VerticalMasking;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;57;1097.127,242.8529;Inherit;False;55;VerticalMasking;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;14;959.7652,-142.0639;Inherit;False;Property;_Toggle_EffectIntensity;Toggle_EffectIntensity;0;0;Create;True;0;0;0;False;0;False;0;10;0;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;29;1069.461,90.32334;Inherit;False;FullScreenEffect;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;16;1183.828,-1125.278;Inherit;False;370;280;Do Not Touch, Will Break Shader;1;11;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;173.6003,-255.8938;Inherit;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;31;-85.17972,-300.527;Inherit;False;Property;_BaseColour;BaseColour;7;1;[HDR];Create;True;0;0;0;False;0;False;1,0,0,0;3.3603,0,7.377211,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;1233.828,-1075.279;Inherit;True;Property;_MainTex;MainTex;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;19;-83.7912,-396.7627;Inherit;False;29;FullScreenEffect;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;458.3141,34.80114;Inherit;False;Constant;_Alpha;Alpha;4;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;1377.734,-132.2702;Inherit;True;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;4;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;3;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;1884.845,-257.8333;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;Shader_VFX_FullScreen;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;5;False;-1;10;False;-1;1;1;False;-1;10;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;2;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;1;637946662316295493;  Blend;0;0;Two Sided;1;0;Cast Shadows;0;637946662322371015;  Use Shadow Threshold;0;0;Receive Shadows;0;637946662330354255;GPU Instancing;1;0;LOD CrossFade;0;0;Built-in Fog;0;0;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;0;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Vertex Position,InvertActionOnDeselection;1;0;0;5;False;True;False;True;False;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2;0,0;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;6;0;5;0
WireConnection;8;0;6;0
WireConnection;8;1;9;0
WireConnection;150;0;144;0
WireConnection;150;1;151;0
WireConnection;142;0;144;0
WireConnection;142;1;148;0
WireConnection;7;0;8;0
WireConnection;7;1;10;0
WireConnection;152;0;142;1
WireConnection;152;1;150;1
WireConnection;53;0;7;0
WireConnection;141;0;152;0
WireConnection;102;0;101;1
WireConnection;102;1;101;2
WireConnection;18;0;53;0
WireConnection;122;0;120;0
WireConnection;122;1;119;0
WireConnection;137;0;141;0
WireConnection;160;0;161;0
WireConnection;160;1;162;0
WireConnection;104;0;102;0
WireConnection;104;1;103;0
WireConnection;104;2;105;0
WireConnection;123;0;104;0
WireConnection;123;1;122;0
WireConnection;123;2;121;0
WireConnection;138;0;137;0
WireConnection;138;1;140;0
WireConnection;159;0;160;0
WireConnection;23;0;22;0
WireConnection;23;1;20;0
WireConnection;172;0;168;0
WireConnection;172;1;170;0
WireConnection;126;0;123;0
WireConnection;126;1;124;0
WireConnection;108;0;107;0
WireConnection;108;1;106;0
WireConnection;136;0;138;0
WireConnection;58;0;59;0
WireConnection;58;1;60;0
WireConnection;82;0;87;0
WireConnection;82;1;81;0
WireConnection;163;0;159;0
WireConnection;80;0;104;0
WireConnection;80;1;82;0
WireConnection;80;2;79;0
WireConnection;176;0;104;0
WireConnection;176;1;172;0
WireConnection;176;2;173;0
WireConnection;115;0;104;0
WireConnection;115;1;108;0
WireConnection;115;2;109;0
WireConnection;17;0;104;0
WireConnection;17;1;23;0
WireConnection;17;2;24;0
WireConnection;62;0;104;0
WireConnection;62;1;58;0
WireConnection;62;2;61;0
WireConnection;166;0;126;0
WireConnection;166;1;165;0
WireConnection;157;0;126;0
WireConnection;157;1;163;0
WireConnection;83;0;80;0
WireConnection;83;1;86;0
WireConnection;153;0;28;0
WireConnection;153;1;154;0
WireConnection;153;2;155;0
WireConnection;112;0;115;0
WireConnection;112;1;110;0
WireConnection;179;0;176;0
WireConnection;179;1;175;0
WireConnection;25;0;17;0
WireConnection;25;1;26;0
WireConnection;164;0;126;0
WireConnection;164;1;166;0
WireConnection;164;2;157;0
WireConnection;63;0;62;0
WireConnection;63;1;67;0
WireConnection;113;0;111;0
WireConnection;113;1;112;0
WireConnection;127;0;153;0
WireConnection;127;1;164;0
WireConnection;183;0;153;0
WireConnection;183;1;179;0
WireConnection;64;0;153;0
WireConnection;64;1;63;0
WireConnection;27;0;153;0
WireConnection;27;1;25;0
WireConnection;84;0;153;0
WireConnection;84;1;83;0
WireConnection;114;0;113;0
WireConnection;42;0;27;0
WireConnection;88;0;84;0
WireConnection;65;0;64;0
WireConnection;128;0;127;0
WireConnection;184;0;183;0
WireConnection;33;0;89;0
WireConnection;33;1;116;0
WireConnection;33;2;90;0
WireConnection;32;0;43;0
WireConnection;32;1;68;0
WireConnection;32;2;45;0
WireConnection;34;0;156;0
WireConnection;34;1;185;0
WireConnection;34;2;118;0
WireConnection;35;0;32;0
WireConnection;35;1;33;0
WireConnection;35;2;40;0
WireConnection;37;0;34;0
WireConnection;37;1;36;0
WireConnection;37;2;39;0
WireConnection;48;0;91;2
WireConnection;48;1;49;0
WireConnection;48;2;52;0
WireConnection;38;0;35;0
WireConnection;38;1;37;0
WireConnection;38;2;41;0
WireConnection;54;0;48;0
WireConnection;95;0;38;0
WireConnection;96;0;95;0
WireConnection;55;0;54;0
WireConnection;29;0;96;0
WireConnection;30;0;19;0
WireConnection;30;1;31;0
WireConnection;100;0;14;0
WireConnection;100;1;29;0
WireConnection;100;2;57;0
WireConnection;1;2;30;0
WireConnection;1;3;100;0
ASEEND*/
//CHKSM=8C209233324B03B7A8E80227982E40CDA2FF14A4
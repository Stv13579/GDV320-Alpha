// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Water"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin][HDR]_Diffuse_Colour_Bottom("Diffuse_Colour_Bottom", Color) = (1,1,1,0)
		_Spec_Smoothness("Spec_Smoothness", Range( 0 , 1)) = 0.954
		[HDR]_Spec_Colour("Spec_Colour", Color) = (1,1,1,0)
		[HDR]_Diffuse_Colour_Top("Diffuse_Colour_Top", Color) = (1,1,1,0)
		_RimDistance("RimDistance", Float) = 0.65
		_Highlight_Strenght("Highlight_Strenght", Float) = 1
		_Rim_Strenght("Rim_Strenght", Float) = 1
		_Float1("Float 1", Float) = 0.002
		_Float0("Float 0", Float) = 0.002
		_WaterLevel("WaterLevel", Range( -10 , 10)) = 0
		_WaterWaveSpeed("WaterWaveSpeed", Float) = 0
		_RotateWater_X("RotateWater_X", Vector) = (0.003,-0.003,0,0)
		_RotateWater_Z("RotateWater_Z", Vector) = (0.003,-0.003,0,0)
		[HDR]_Diffuse_Colour_Backface("Diffuse_Colour_Backface", Color) = (0.5176471,0.3215686,0.8588236,1)
		_ColourGradientSmoothStep("ColourGradientSmoothStep", Vector) = (0,1,0,0)
		[ASEEnd]_Float3("Float 3", Float) = 10

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
		
		Cull Off
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
			
			Blend One Zero, One Zero
			ZWrite On
			ZTest LEqual
			Offset 0 , 0
			ColorMask RGBA
			

			HLSLPROGRAM
			
			#pragma multi_compile_instancing
			#define _RECEIVE_SHADOWS_OFF 1
			#define _ALPHATEST_ON 1
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
			#define ASE_NEEDS_FRAG_SHADOWCOORDS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
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
				float4 lightmapUVOrVertexSH : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Diffuse_Colour_Top;
			float4 _Diffuse_Colour_Bottom;
			float4 _Diffuse_Colour_Backface;
			float4 _Spec_Colour;
			float2 _ColourGradientSmoothStep;
			float2 _RotateWater_X;
			float2 _RotateWater_Z;
			float _WaterLevel;
			float _RimDistance;
			float _Highlight_Strenght;
			float _Rim_Strenght;
			float _Spec_Smoothness;
			float _Float3;
			float _WaterWaveSpeed;
			float _Float0;
			float _Float1;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			float3 ASEIndirectDiffuse( float2 uvStaticLightmap, float3 normalWS )
			{
			#ifdef LIGHTMAP_ON
				return SampleLightmap( uvStaticLightmap, normalWS );
			#else
				return SampleSH(normalWS);
			#endif
			}
			
			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
			}
			
			
			VertexOutput VertexFunction ( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( ase_worldNormal, o.lightmapUVOrVertexSH.xyz );
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
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

			half4 frag ( VertexOutput IN , FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC ) : SV_Target
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
				float3 ase_worldNormal = IN.ase_texcoord3.xyz;
				float dotResult169 = dot( ase_worldNormal , _MainLightPosition.xyz );
				float ase_lightAtten = 0;
				Light ase_mainLight = GetMainLight( ShadowCoords );
				ase_lightAtten = ase_mainLight.distanceAttenuation * ase_mainLight.shadowAttenuation;
				float temp_output_3_0_g17 = ( min( saturate( dotResult169 ) , ase_lightAtten ) - 0.2 );
				float temp_output_174_0 = saturate( ( temp_output_3_0_g17 / fwidth( temp_output_3_0_g17 ) ) );
				float3 bakedGI173 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, ase_worldNormal);
				MixRealtimeAndBakedGI(ase_mainLight, ase_worldNormal, bakedGI173, half4(0,0,0,0));
				float Fill316 = _WaterLevel;
				float3 worldToObj361 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float Y_WorldPos270 = worldToObj361.y;
				float smoothstepResult312 = smoothstep( ( _ColourGradientSmoothStep.x - Fill316 ) , ( _ColourGradientSmoothStep.y - Fill316 ) , ( 1.0 - Y_WorldPos270 ));
				float4 lerpResult266 = lerp( _Diffuse_Colour_Top , _Diffuse_Colour_Bottom , smoothstepResult312);
				float4 DiffuseColour267 = lerpResult266;
				float NormalDotLightStep254 = temp_output_174_0;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = normalize(ase_worldViewDir);
				float fresnelNdotV243 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode243 = ( 0.0 + _RimDistance * pow( 1.0 - fresnelNdotV243, 5.0 ) );
				float temp_output_3_0_g19 = ( fresnelNode243 - 0.1 );
				float temp_output_245_0 = saturate( ( temp_output_3_0_g19 / fwidth( temp_output_3_0_g19 ) ) );
				float3 bakedGI249 = ASEIndirectDiffuse( IN.lightmapUVOrVertexSH.xy, ase_worldNormal);
				MixRealtimeAndBakedGI(ase_mainLight, ase_worldNormal, bakedGI249, half4(0,0,0,0));
				float4 Backface279 = _Diffuse_Colour_Backface;
				float4 switchResult163 = (((ase_vface>0)?(( ( float4( ( temp_output_174_0 + ( ( 1.0 - temp_output_174_0 ) * ( bakedGI173 * float3( 1,1,1 ) ) ) ) , 0.0 ) * _MainLightColor * DiffuseColour267 ) + ( ( NormalDotLightStep254 * temp_output_245_0 * _Highlight_Strenght * _MainLightColor ) + float4( ( ( 1.0 - NormalDotLightStep254 ) * temp_output_245_0 * bakedGI249 * _Rim_Strenght ) , 0.0 ) ) )):(Backface279)));
				float4 Diffuse260 = switchResult163;
				float3 normalizeResult331 = normalize( ( ase_worldViewDir + _MainLightPosition.xyz ) );
				float dotResult333 = dot( normalizeResult331 , ase_worldNormal );
				float temp_output_3_0_g18 = ( pow( dotResult333 , exp2( ( ( _Spec_Smoothness * 10.0 ) + 1.0 ) ) ) - 0.1 );
				float4 Specular325 = ( saturate( ( temp_output_3_0_g18 / fwidth( temp_output_3_0_g18 ) ) ) * _Spec_Colour * _Spec_Smoothness );
				float4 Colour258 = ( Diffuse260 + Specular325 );
				
				float temp_output_206_0 = sin( ( _WaterWaveSpeed * _TimeParameters.x ) );
				float lerpResult199 = lerp( _RotateWater_X.x , _RotateWater_X.y , temp_output_206_0);
				float3 worldToObj136 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float3 temp_cast_3 = (worldToObj136.x).xxx;
				float3 rotatedValue187 = RotateAroundAxis( float3( 1,0,0 ), worldToObj136, temp_cast_3, 90.0 );
				float3 temp_cast_4 = (worldToObj136.z).xxx;
				float3 rotatedValue189 = RotateAroundAxis( float3( 0,0,1 ), worldToObj136, temp_cast_4, 90.0 );
				float lerpResult208 = lerp( _RotateWater_Z.x , _RotateWater_Z.y , temp_output_206_0);
				float temp_output_146_0 = ( sin( _TimeParameters.x * 0.5 ) * _Float0 );
				float smoothstepResult138 = smoothstep( _WaterLevel , _Float3 , ( ( ( ( lerpResult199 * rotatedValue187 ) + ( rotatedValue189 * lerpResult208 ) ) + worldToObj361 ).y + ( ( WorldPosition.x * ( temp_output_146_0 * _Float1 ) ) + ( WorldPosition.z * temp_output_146_0 ) ) ));
				float clampResult143 = clamp( step( smoothstepResult138 , 0.04 ) , 0.0 , 1.0 );
				float Alpha256 = clampResult143;
				
				float3 BakedAlbedo = 0;
				float3 BakedEmission = 0;
				float3 Color = Colour258.rgb;
				float Alpha = Alpha256;
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
			#define _ALPHATEST_ON 1
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
			float4 _Diffuse_Colour_Top;
			float4 _Diffuse_Colour_Bottom;
			float4 _Diffuse_Colour_Backface;
			float4 _Spec_Colour;
			float2 _ColourGradientSmoothStep;
			float2 _RotateWater_X;
			float2 _RotateWater_Z;
			float _WaterLevel;
			float _RimDistance;
			float _Highlight_Strenght;
			float _Rim_Strenght;
			float _Spec_Smoothness;
			float _Float3;
			float _WaterWaveSpeed;
			float _Float0;
			float _Float1;
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			

			float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
			{
				original -= center;
				float C = cos( angle );
				float S = sin( angle );
				float t = 1 - C;
				float m00 = t * u.x * u.x + C;
				float m01 = t * u.x * u.y - S * u.z;
				float m02 = t * u.x * u.z + S * u.y;
				float m10 = t * u.x * u.y + S * u.z;
				float m11 = t * u.y * u.y + C;
				float m12 = t * u.y * u.z - S * u.x;
				float m20 = t * u.x * u.z - S * u.y;
				float m21 = t * u.y * u.z + S * u.x;
				float m22 = t * u.z * u.z + C;
				float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
				return mul( finalMatrix, original ) + center;
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

				float temp_output_206_0 = sin( ( _WaterWaveSpeed * _TimeParameters.x ) );
				float lerpResult199 = lerp( _RotateWater_X.x , _RotateWater_X.y , temp_output_206_0);
				float3 worldToObj136 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float3 temp_cast_0 = (worldToObj136.x).xxx;
				float3 rotatedValue187 = RotateAroundAxis( float3( 1,0,0 ), worldToObj136, temp_cast_0, 90.0 );
				float3 temp_cast_1 = (worldToObj136.z).xxx;
				float3 rotatedValue189 = RotateAroundAxis( float3( 0,0,1 ), worldToObj136, temp_cast_1, 90.0 );
				float lerpResult208 = lerp( _RotateWater_Z.x , _RotateWater_Z.y , temp_output_206_0);
				float3 worldToObj361 = mul( GetWorldToObjectMatrix(), float4( WorldPosition, 1 ) ).xyz;
				float temp_output_146_0 = ( sin( _TimeParameters.x * 0.5 ) * _Float0 );
				float smoothstepResult138 = smoothstep( _WaterLevel , _Float3 , ( ( ( ( lerpResult199 * rotatedValue187 ) + ( rotatedValue189 * lerpResult208 ) ) + worldToObj361 ).y + ( ( WorldPosition.x * ( temp_output_146_0 * _Float1 ) ) + ( WorldPosition.z * temp_output_146_0 ) ) ));
				float clampResult143 = clamp( step( smoothstepResult138 , 0.04 ) , 0.0 , 1.0 );
				float Alpha256 = clampResult143;
				
				float Alpha = Alpha256;
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
0;0;1920;1019;-4584.974;3203.763;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;265;3992.024,-3641.394;Inherit;False;3106.332;1680.092;Liquid;45;203;202;135;204;206;207;136;200;187;199;208;147;189;148;146;191;149;190;153;145;188;150;152;194;137;151;134;138;154;140;143;256;261;258;215;270;314;316;340;360;361;366;367;368;370;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;203;4042.024,-3467.601;Inherit;False;Property;_WaterWaveSpeed;WaterWaveSpeed;29;0;Create;True;0;0;0;False;0;False;0;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;202;4067.024,-3375.601;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;135;4079.41,-3163.517;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;204;4247.024,-3463.601;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;207;4410.461,-3003.295;Inherit;False;Property;_RotateWater_Z;RotateWater_Z;31;0;Create;True;0;0;0;False;0;False;0.003,-0.003;0.03,-0.03;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TransformPositionNode;136;4260.411,-3166.517;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.Vector2Node;200;4394.358,-3591.394;Inherit;False;Property;_RotateWater_X;RotateWater_X;30;0;Create;True;0;0;0;False;0;False;0.003,-0.003;0.03,-0.03;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SinOpNode;206;4454.075,-3462.307;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;199;4686.848,-3543.525;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;148;4738.672,-2371.02;Inherit;False;Property;_Float0;Float 0;20;0;Create;True;0;0;0;False;0;False;0.002;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;189;4599.236,-3135.589;Inherit;False;False;4;0;FLOAT3;0,0,1;False;1;FLOAT;90;False;2;FLOAT3;0,0,1;False;3;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;208;4700.849,-2972.718;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;187;4599.802,-3305.905;Inherit;False;False;4;0;FLOAT3;1,0,0;False;1;FLOAT;90;False;2;FLOAT3;1,0,0;False;3;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SinTimeNode;147;4742.926,-2513.978;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;191;4945.184,-3126.43;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;146;4929.362,-2391.87;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;360;4131.958,-2962.534;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;190;4937.688,-3377.5;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;149;4731.574,-2594.714;Inherit;False;Property;_Float1;Float 1;17;0;Create;True;0;0;0;False;0;False;0.002;0.001;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;145;4458.444,-2464.798;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TransformPositionNode;361;4369.333,-2880.402;Inherit;False;World;Object;False;Fast;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;153;4965.592,-2504.188;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;188;5220.868,-3212.739;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;194;5374.37,-2978.411;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;152;5060.948,-2215.302;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;150;5119.199,-2575.074;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;137;5518.433,-3013.85;Inherit;True;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleAddOpNode;151;5403.598,-2537.031;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;370;5771.974,-2655.763;Inherit;False;Property;_Float3;Float 3;34;0;Create;True;0;0;0;False;0;False;10;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;134;5155.722,-2768.487;Inherit;False;Property;_WaterLevel;WaterLevel;28;0;Create;True;0;0;0;False;0;False;0;2;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;154;5762.043,-2941.992;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;138;5981.109,-2901.768;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;140;6253.705,-2907.353;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.04;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;143;6506.706,-2900.353;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;322;7138.045,-3640.159;Inherit;False;1625.947;1790.751;Colours;13;279;312;318;315;267;272;313;317;308;179;266;269;271;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;324;1741.327,-3632.285;Inherit;False;2119.078;531.3668;Specular;15;339;338;337;336;335;334;333;332;331;330;329;328;327;326;325;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;256;6803.028,-2958.473;Inherit;False;Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;264;1064.814,-2990.814;Inherit;False;2861.628;1023.345;Cel Water;33;173;170;174;167;172;184;171;181;178;175;169;168;176;254;247;251;248;246;250;242;253;252;249;244;245;243;241;240;163;260;268;280;369;;1,1,1,1;0;0
Node;AmplifyShaderEditor.GetLocalVarNode;340;4211.342,-2248.933;Inherit;False;325;Specular;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-4087.272,-665.0515;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;326;1791.327,-3427.76;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ClampOpNode;88;-2919.003,-1049.935;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;215;4523.243,-2230.782;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;313;7188.045,-2699.314;Inherit;False;Property;_ColourGradientSmoothStep;ColourGradientSmoothStep;33;0;Create;True;0;0;0;False;0;False;0,1;0,1.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMinOpNode;172;1723.977,-2798.708;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Exp2OpNode;336;2988.033,-3273.676;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;-3729.243,-2544.567;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.Vector2Node;1;-5006.957,-2043.857;Inherit;False;Property;_Vector0;Vector 0;0;0;Create;True;0;0;0;False;0;False;10,5;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-3942.825,-1232.038;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.WorldNormalVector;332;2163.741,-3397.415;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;252;2732.264,-2242.925;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;16;-4106.777,-2176.703;Inherit;False;Property;_NoiseColour;NoiseColour;15;0;Create;True;0;0;0;False;0;False;0.2075472,0.01272696,0.01272696,0;0.2075468,0.01272696,0.01272696,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;51;-3613.037,-810.2122;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;312;7781.27,-2750.46;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;245;2286.55,-2238.534;Inherit;True;Step Antialiasing;-1;;19;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0.1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;317;7284.196,-2574.027;Inherit;False;316;Fill;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;314;4632.303,-2841.953;Inherit;False;FLOAT3;1;0;FLOAT3;0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.RangedFloatNode;92;-2925.979,-911.799;Inherit;False;Property;_MinNew;Min New;7;0;Create;True;0;0;0;False;0;False;-0.4;0.03;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;333;2433.337,-3415.775;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-4456.003,-1682.701;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.NegateNode;86;-3136.478,-1489.513;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;171;1531.744,-2862.531;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;243;2002.734,-2242.425;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;59;-4280.061,-872.8954;Inherit;False;Property;_MovementMulti;MovementMulti;16;0;Create;True;0;0;0;False;0;False;0.002;0.002;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;90;-2049.455,-1619.019;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;246;2117.874,-2403.958;Inherit;False;Property;_Highlight_Strenght;Highlight_Strenght;8;0;Create;True;0;0;0;False;0;False;1;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;166;4982.677,-1311.282;Inherit;False;Constant;_Float2;Float 2;22;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;24;-3711.652,-1230.455;Inherit;True;FLOAT4;1;0;FLOAT4;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.SimpleSubtractOpNode;315;7561.717,-2681.945;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;247;2368.789,-2515.286;Inherit;False;254;NormalDotLightStep;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;254;2241.63,-2893.824;Inherit;False;NormalDotLightStep;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;250;2564.691,-2309.829;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;271;7250.785,-2889.909;Inherit;True;270;Y_WorldPos;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;42;-4744.491,-748.9743;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;337;2839.087,-3273.676;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;93;-2927.979,-787.799;Inherit;False;Property;_MaxNew;Max New;23;0;Create;True;0;0;0;False;0;False;0.4;-1.56;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-4053.643,-830.6706;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;81;-2031.622,-1391.358;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;15;-4049.986,-1999.17;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.LightColorNode;248;1961.413,-2495.88;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;63;-4607.738,-2910.994;Inherit;False;Property;_FresnelPowerWhite;FresnelPowerWhite;24;0;Create;True;0;0;0;False;0;False;14;14;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-4172.822,-1517.939;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DotProductOpNode;169;1384.999,-2854.041;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;175;2227.345,-2612.126;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;1,1,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PannerNode;4;-4507.99,-2005.691;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;1,1;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-4698.986,-2361.249;Inherit;False;Property;_FresnelScale;FresnelScale;22;0;Create;True;0;0;0;False;0;False;1.3182;1.3182;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;12;-4341.642,-2429.317;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.IndirectDiffuseLighting;249;2490.358,-2159.28;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;325;3666.456,-3508.838;Inherit;False;Specular;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;241;1769.433,-2174.22;Inherit;False;Property;_RimDistance;RimDistance;6;0;Create;True;0;0;0;False;0;False;0.65;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;308;8012.664,-3116.911;Inherit;False;Property;_Diffuse_Colour_Backface;Diffuse_Colour_Backface;32;1;[HDR];Create;True;0;0;0;False;0;False;0.5176471,0.3215686,0.8588236,1;11.98431,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.IndirectDiffuseLighting;173;1900.791,-2577.588;Inherit;False;Tangent;1;0;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;34;-4813.735,-1709.326;Inherit;False;Property;_RotateX;RotateX;14;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-3560.38,-2183.563;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;257;4961.815,-1398.229;Inherit;False;256;Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SwitchByFaceNode;14;-3957.572,-2588.835;Inherit;False;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LightColorNode;369;2663.551,-2650.936;Inherit;False;0;3;COLOR;0;FLOAT3;1;FLOAT;2
Node;AmplifyShaderEditor.ColorNode;9;-4253.087,-2607.62;Inherit;False;Property;_BaseColour;BaseColour;9;0;Create;True;0;0;0;False;0;False;1,0,0,1;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;40;-4567.897,-1000.778;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RegisterLocalVarNode;279;8266.968,-3035.542;Inherit;False;Backface;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;60;-4366.668,-1874.276;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-4775.712,-1269.774;Inherit;False;Property;_RotateY;RotateY;13;0;Create;True;0;0;0;False;0;False;1;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;87;-2631.586,-1103.432;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-0.4;False;4;FLOAT;0.4;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;327;2431.625,-3285.661;Inherit;False;Property;_Spec_Smoothness;Spec_Smoothness;3;0;Create;True;0;0;0;False;0;False;0.954;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-4353.677,-2201.396;Inherit;False;Property;_EmissiveColour;EmissiveColour;12;0;Create;True;0;0;0;False;0;False;1,0,0,0;1,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-3901.335,-930.1553;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinTimeNode;43;-4273.709,-787.1596;Inherit;False;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;339;3492.363,-3385.2;Inherit;True;3;3;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;258;4779.014,-2206.088;Inherit;False;Colour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;82;-2902.429,-1507.869;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;10;-4256.287,-2778.581;Inherit;False;Property;_Color1;Color 1;10;0;Create;True;0;0;0;False;0;False;0.03773582,0,0,1;0.03773582,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ViewDirInputsCoordNode;335;1811.561,-3582.285;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;330;2087.509,-3508.224;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ColorNode;329;3257.85,-3272.762;Inherit;False;Property;_Spec_Colour;Spec_Colour;4;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;0.7490196,0.7490196,0.7490196,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;35;-4448.507,-1431.631;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-3961.842,-2404.557;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.PowerNode;328;3140.404,-3408.927;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldToObjectTransfNode;115;-5060.154,-1524.883;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;240;3266.802,-2385.293;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;91;-2321.148,-1495.08;Inherit;False;Property;_FineWhiteLine;FineWhiteLine;25;0;Create;True;0;0;0;False;0;False;3;3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ObjectToWorldTransfNode;367;4120.034,-2556.274;Inherit;False;1;0;FLOAT4;0,0,0,1;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;61;-4299.704,-3009.136;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;30;-4794.455,-1440.79;Inherit;False;False;4;0;FLOAT3;0,0,1;False;1;FLOAT;90;False;2;FLOAT3;0,0,1;False;3;FLOAT3;0,0,1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;253;2872.293,-2341.624;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RotatorNode;11;-4243.097,-1990.285;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;270;4891.177,-2905.779;Inherit;False;Y_WorldPos;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;259;4969.604,-1477.805;Inherit;False;258;Colour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;251;2668.229,-2488.556;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;260;3702.442,-2379.353;Inherit;False;Diffuse;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;266;8011.732,-2940.342;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;338;2707.261,-3277.101;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;85;-1942.827,-1917.257;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ClampOpNode;83;-2775.287,-1704.179;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.025;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;318;7565.196,-2592.027;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NormalizeNode;331;2245.015,-3485.968;Inherit;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LightAttenuation;170;1509.318,-2649.712;Inherit;False;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;89;-2393.372,-1739.908;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;3;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;22;-4566.513,-1155.145;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;54;-4276.963,-642.2011;Inherit;False;Property;_MovementWater;MovementWater;21;0;Create;True;0;0;0;False;0;False;0.002;0.002;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-4795.643,-2046.026;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;167;1114.814,-2767.266;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;272;7478.9,-2908.794;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;174;1954.923,-2817.463;Inherit;True;Step Antialiasing;-1;;17;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0.2;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;184;3055.894,-2744.436;Inherit;True;3;3;0;FLOAT3;0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;178;2461.79,-2677.543;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-3785.653,-2171.943;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;316;5445.819,-2677.958;Inherit;False;Fill;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;269;7443.933,-3090.665;Inherit;False;Property;_Diffuse_Colour_Bottom;Diffuse_Colour_Bottom;2;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;11.98431,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;334;3295.186,-3428.294;Inherit;False;Step Antialiasing;-1;;18;2a825e80dfb3290468194f83380797bd;0;2;1;FLOAT;0.1;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RotateAboutAxisNode;36;-4793.889,-1611.106;Inherit;False;False;4;0;FLOAT3;1,0,0;False;1;FLOAT;90;False;2;FLOAT3;1,0,0;False;3;FLOAT3;1,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;244;2516.026,-2083.469;Inherit;False;Property;_Rim_Strenght;Rim_Strenght;11;0;Create;True;0;0;0;False;0;False;1;50;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;368;4121.418,-2710.641;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RegisterLocalVarNode;267;8266.86,-2949.228;Inherit;False;DiffuseColour;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;23;-4256.215,-1094.599;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;47;-3918.329,-407.174;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;7;-4579.241,-1845.957;Inherit;False;1;0;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;64;-3340.432,-2596.511;Inherit;True;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;62;-4607.738,-2986.994;Inherit;False;Property;_FresnelScaleWhite;FresnelScaleWhite;19;0;Create;True;0;0;0;False;0;False;20;20;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;-3243.62,-989.4;Inherit;True;Property;_Fill;Fill;1;0;Create;True;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;176;2229.885,-2717.221;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;268;2826.433,-2555.307;Inherit;False;267;DiffuseColour;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;280;3219.534,-2286.004;Inherit;False;279;Backface;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;366;4926.626,-2778.677;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;181;2674.538,-2765.844;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwitchByFaceNode;163;3399.52,-2381.951;Inherit;True;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;52;-3382.959,-1240.716;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;2;-4788.541,-1900.401;Inherit;False;Property;_PannerSpeed;PannerSpeed;27;0;Create;True;0;0;0;False;0;False;0.05,0.025;0.05,0.025;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;94;-4484.948,-1755.476;Inherit;False;Property;_DivideValue;DivideValue;26;0;Create;True;0;0;0;False;0;False;6;97.77;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;84;-2574.37,-1757.39;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0.025;False;3;FLOAT;1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector;168;1114.814,-2940.814;Inherit;False;False;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ColorNode;179;7451.28,-3260.258;Inherit;False;Property;_Diffuse_Colour_Top;Diffuse_Colour_Top;5;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,0;11.98431,0,0,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;242;2345.731,-2319.007;Inherit;False;254;NormalDotLightStep;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;261;4208.155,-2333.474;Inherit;False;260;Diffuse;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-4698.986,-2285.249;Inherit;False;Property;_FresnelPower;FresnelPower;18;0;Create;True;0;0;0;False;0;False;3.138322;3.138322;0;20;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;235;4605.011,-1119.056;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;236;4605.011,-1119.056;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;234;4605.011,-1119.056;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;232;5272.011,-1629.056;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;New Amplify Shader;2992e84f91cbeb14eab234972e07ea9d;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;233;5293.365,-1467.454;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;3;Water;2992e84f91cbeb14eab234972e07ea9d;True;Forward;0;1;Forward;8;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;2;False;-1;False;False;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;17;d3d9;d3d11;glcore;gles;gles3;metal;vulkan;xbox360;xboxone;xboxseries;ps4;playstation;psp2;n3ds;wiiu;switch;nomrt;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;LightMode=UniversalForward;False;False;0;Hidden/InternalErrorShader;0;0;Standard;22;Surface;0;0;  Blend;0;0;Two Sided;0;638026329172051597;Cast Shadows;0;638026329335421153;  Use Shadow Threshold;0;0;Receive Shadows;0;638026451595828511;GPU Instancing;1;0;LOD CrossFade;0;0;Built-in Fog;0;0;DOTS Instancing;0;0;Meta Pass;0;0;Extra Pre Pass;0;638026364917304022;Tessellation;0;0;  Phong;0;0;  Strength;0.5,False,-1;0;  Type;0;0;  Tess;16,False,-1;0;  Min;10,False,-1;0;  Max;25,False,-1;0;  Edge Length;16,False,-1;0;  Max Displacement;25,False,-1;0;Vertex Position,InvertActionOnDeselection;1;0;0;5;False;True;False;True;False;False;;False;0
WireConnection;204;0;203;0
WireConnection;204;1;202;0
WireConnection;136;0;135;0
WireConnection;206;0;204;0
WireConnection;199;0;200;1
WireConnection;199;1;200;2
WireConnection;199;2;206;0
WireConnection;189;0;136;3
WireConnection;189;3;136;0
WireConnection;208;0;207;1
WireConnection;208;1;207;2
WireConnection;208;2;206;0
WireConnection;187;0;136;1
WireConnection;187;3;136;0
WireConnection;191;0;189;0
WireConnection;191;1;208;0
WireConnection;146;0;147;3
WireConnection;146;1;148;0
WireConnection;190;0;199;0
WireConnection;190;1;187;0
WireConnection;361;0;360;0
WireConnection;153;0;146;0
WireConnection;153;1;149;0
WireConnection;188;0;190;0
WireConnection;188;1;191;0
WireConnection;194;0;188;0
WireConnection;194;1;361;0
WireConnection;152;0;145;3
WireConnection;152;1;146;0
WireConnection;150;0;145;1
WireConnection;150;1;153;0
WireConnection;137;0;194;0
WireConnection;151;0;150;0
WireConnection;151;1;152;0
WireConnection;154;0;137;1
WireConnection;154;1;151;0
WireConnection;138;0;154;0
WireConnection;138;1;134;0
WireConnection;138;2;370;0
WireConnection;140;0;138;0
WireConnection;143;0;140;0
WireConnection;256;0;143;0
WireConnection;53;0;43;3
WireConnection;53;1;54;0
WireConnection;88;0;25;0
WireConnection;215;0;261;0
WireConnection;215;1;340;0
WireConnection;172;0;171;0
WireConnection;172;1;170;0
WireConnection;336;0;337;0
WireConnection;18;0;14;0
WireConnection;18;1;13;0
WireConnection;41;0;33;0
WireConnection;41;1;23;0
WireConnection;252;0;250;0
WireConnection;252;1;245;0
WireConnection;252;2;249;0
WireConnection;252;3;244;0
WireConnection;51;0;44;0
WireConnection;51;1;47;0
WireConnection;312;0;272;0
WireConnection;312;1;315;0
WireConnection;312;2;318;0
WireConnection;245;2;243;0
WireConnection;314;0;361;0
WireConnection;333;0;331;0
WireConnection;333;1;332;0
WireConnection;31;0;34;0
WireConnection;31;1;36;0
WireConnection;86;0;52;0
WireConnection;171;0;169;0
WireConnection;243;2;241;0
WireConnection;90;0;89;0
WireConnection;90;1;91;0
WireConnection;24;0;41;0
WireConnection;315;0;313;1
WireConnection;315;1;317;0
WireConnection;254;0;174;0
WireConnection;250;0;242;0
WireConnection;337;0;338;0
WireConnection;58;0;53;0
WireConnection;58;1;59;0
WireConnection;81;0;52;0
WireConnection;81;1;87;0
WireConnection;15;0;11;0
WireConnection;33;0;31;0
WireConnection;33;1;35;0
WireConnection;169;0;168;0
WireConnection;169;1;167;0
WireConnection;175;0;173;0
WireConnection;4;0;3;0
WireConnection;4;2;2;0
WireConnection;12;2;5;0
WireConnection;12;3;6;0
WireConnection;325;0;339;0
WireConnection;19;0;18;0
WireConnection;19;1;17;0
WireConnection;14;0;10;0
WireConnection;14;1;9;0
WireConnection;279;0;308;0
WireConnection;60;0;7;0
WireConnection;60;1;94;0
WireConnection;87;0;88;0
WireConnection;87;3;92;0
WireConnection;87;4;93;0
WireConnection;44;0;42;1
WireConnection;44;1;58;0
WireConnection;339;0;334;0
WireConnection;339;1;329;0
WireConnection;339;2;327;0
WireConnection;258;0;215;0
WireConnection;82;0;86;0
WireConnection;82;1;87;0
WireConnection;330;0;335;0
WireConnection;330;1;326;0
WireConnection;35;0;30;0
WireConnection;35;1;37;0
WireConnection;13;0;12;0
WireConnection;13;1;8;0
WireConnection;328;0;333;0
WireConnection;328;1;336;0
WireConnection;240;0;184;0
WireConnection;240;1;253;0
WireConnection;61;2;62;0
WireConnection;61;3;63;0
WireConnection;30;0;115;3
WireConnection;253;0;251;0
WireConnection;253;1;252;0
WireConnection;11;0;4;0
WireConnection;11;2;60;0
WireConnection;270;0;314;1
WireConnection;251;0;247;0
WireConnection;251;1;245;0
WireConnection;251;2;246;0
WireConnection;251;3;248;0
WireConnection;260;0;163;0
WireConnection;266;0;179;0
WireConnection;266;1;269;0
WireConnection;266;2;312;0
WireConnection;338;0;327;0
WireConnection;85;0;90;0
WireConnection;85;1;64;0
WireConnection;83;0;82;0
WireConnection;318;0;313;2
WireConnection;318;1;317;0
WireConnection;331;0;330;0
WireConnection;89;0;84;0
WireConnection;3;0;1;0
WireConnection;272;0;271;0
WireConnection;174;2;172;0
WireConnection;184;0;181;0
WireConnection;184;1;369;0
WireConnection;184;2;268;0
WireConnection;178;0;176;0
WireConnection;178;1;175;0
WireConnection;17;0;16;0
WireConnection;17;1;15;0
WireConnection;316;0;134;0
WireConnection;334;2;328;0
WireConnection;36;0;115;1
WireConnection;267;0;266;0
WireConnection;23;0;22;0
WireConnection;23;1;40;0
WireConnection;47;0;42;3
WireConnection;47;1;53;0
WireConnection;64;0;61;0
WireConnection;64;1;19;0
WireConnection;176;0;174;0
WireConnection;366;0;368;0
WireConnection;366;1;367;0
WireConnection;181;0;174;0
WireConnection;181;1;178;0
WireConnection;163;0;240;0
WireConnection;163;1;280;0
WireConnection;52;0;24;1
WireConnection;52;1;51;0
WireConnection;84;0;83;0
WireConnection;233;2;259;0
WireConnection;233;3;257;0
WireConnection;233;4;166;0
ASEEND*/
//CHKSM=1B7CF24238A935B5E5929848F5A8E03F7EDBB79A
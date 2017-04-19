#ifndef VACUUM_CURVEDWORLD_BASE_CGINC
#define VACUUM_CURVEDWORLD_BASE_CGINC 


#include "UnityCG.cginc"
////////////////////////////////////////////////////////////////////////////
//																		  //
//Variables 															  //
//																		  //
////////////////////////////////////////////////////////////////////////////

uniform float3 _V_CW_Bend;
uniform float3 _V_CW_Bias;	
uniform float4 _V_CW_PivotPoint_Position;

const float2 _zero2 = float2(0,0);
const float3 _zero3 = float3(0,0,0);
////////////////////////////////////////////////////////////////////////////
//																		  //
//Defines    															  //
//																		  //
////////////////////////////////////////////////////////////////////////////


#define SIGN(a) (float2(a.x < 0 ? -1.0f : 1.0f, a.y < 0 ? -1.0f : 1.0f))


#define PIVOT _V_CW_PivotPoint_Position.xyz

////////////////////////////////////////////////////////////////////////////
//																		  //
//Functions    															  //
//																		  //
////////////////////////////////////////////////////////////////////////////

inline void V_CW_TransformPoint(inout float4 vertex)
{	
	float4 worldPos = mul(unity_ObjectToWorld, vertex);
	worldPos.xyz -= PIVOT;

	float2 xzOff = max(_zero2, abs(worldPos.zx) - _V_CW_Bias.xz);
	xzOff *= step(_zero2, worldPos.zx) * 2 - 1;
	xzOff *= xzOff;
	worldPos = float4(0, (_V_CW_Bend.x * xzOff.x + _V_CW_Bend.z * xzOff.y) * 0.001, 0, 0);

	vertex += mul(unity_WorldToObject, worldPos);
} 

inline void V_CW_TransformPointAndNormal(inout float4 vertex, inout float3 normal, float3 worldPos, float3 worldTangent, float3 worldBinormal)
{

	float3 v0 = worldPos - PIVOT;
	float3 v1 = v0 + worldTangent;
	float3 v2 = v0 + worldBinormal;
	

	float2 xzOff = max(_zero2, abs(v0.zx) - _V_CW_Bias.xz);
	xzOff *= step(_zero2, v0.zx) * 2 - 1;
	xzOff *= xzOff;
	float3 transformedVertex = float3(0, (_V_CW_Bend.x * xzOff.x + _V_CW_Bend.z * xzOff.y) * 0.001, 0);
	v0 += transformedVertex;


	xzOff = max(_zero2, abs(v1.zx) - _V_CW_Bias.xz);
	xzOff *= step(_zero2, v1.zx) * 2 - 1;
	xzOff *= xzOff;
	v1.y += (_V_CW_Bend.x * xzOff.x + _V_CW_Bend.z * xzOff.y) * 0.001;


	xzOff = max(_zero2, abs(v2.zx) - _V_CW_Bias.xz);
	xzOff *= step(_zero2, v2.zx) * 2 - 1;
	xzOff *= xzOff;
	v2.y += (_V_CW_Bend.x * xzOff.x + _V_CW_Bend.z * xzOff.y) * 0.001;


	vertex.xyz += mul((float3x3)unity_WorldToObject, transformedVertex);
	normal = normalize(mul((float3x3)unity_WorldToObject, normalize(cross(v2 - v0, v1 - v0))));
}  

inline void V_CW_TransformPointAndNormal(inout float4 vertex, inout float3 normal, float4 tangent)
{	
	float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz; 
	float3 worldNormal = UnityObjectToWorldNormal(normal);
	float3 worldTangent = UnityObjectToWorldDir(tangent.xyz);
	float3 worldBinormal = cross(worldNormal, worldTangent) * -1;// * tangent.w;

	V_CW_TransformPointAndNormal(vertex, normal, worldPos, worldTangent, worldBinormal);
}

#endif 

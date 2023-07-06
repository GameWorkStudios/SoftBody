using UnityEngine;
using Unity.Jobs;
using Unity.Burst;
using Unity.Mathematics;
using Unity.Collections;

[BurstCompile]
public struct PressurableJob2 : IJob
{

    private float3 forceDirection;
    private float forcePower;
    
    private NativeArray<float3> currentVertexPositions; // Particles 01
    private NativeArray<float3> currentVertexPositionVelocities; // Particles 02
    //public Vector3[] currentMeshVertices;
    public NativeArray<float3> currentMeshVertices;

    private float _deltaTime;

    private int vertixid;

    public PressurableJob2(NativeArray<float3> currentVertexPositions, NativeArray<float3> currentVertexPositionVelocities,NativeArray<float3> currentMeshVertices, Vector3 forceDirection, float forcePower,int vertixid, float _deltaTime){
        this.currentVertexPositions = currentVertexPositions;
        this.currentVertexPositionVelocities = currentVertexPositionVelocities;
        this.currentMeshVertices = currentMeshVertices;
        this.forceDirection = forceDirection;
        this.forcePower = forcePower;
        this._deltaTime = _deltaTime;
        this.vertixid = vertixid;
    }

    public void Execute()
    {
        for(int i = 0; i < this.currentVertexPositions.Length; i++){
            if(vertixid != i){
                continue;
            }
            this.currentVertexPositionVelocities[i] = UpdateVelocity(forceDirection * forcePower, _deltaTime);            
            this.currentVertexPositions[i] += this.currentVertexPositionVelocities[i] * _deltaTime;                       
            this.currentMeshVertices[i] = this.currentVertexPositions[i];
        }
    }

    public float3 UpdateVelocity(float3 force, float _deltaTime){
        return force * _deltaTime;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Collections;

public class PressurableObject : MonoBehaviour
{

    [Header("Settings")]
    [SerializeField] float forcePower = 0.05f;
    [SerializeField] bool isDiving = false;

    private MeshFilter meshFilter;
    private Mesh mesh;

    NativeArray<float3> currentVertexPositions;
    NativeArray<float3> currentVertexPositionVelocities;
    NativeArray<float3> currentMeshVertices;


    private Vector3 forceDirection = Vector3.zero;
    float lastYValue;

    JobHandle handle;
    PressurableJob2 pressurable;
    
    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        GetVertices();
        lastYValue = transform.position.y;
    }

    
    private void Update()
    {
        if(isDiving)
        {            
            if(Mathf.Abs(transform.position.y - lastYValue) > 1f){
                for(int i = 0; i < this.currentVertexPositions.Length; i++){
                    Vector3 vertexPositionForForce = this.currentVertexPositions[i]; 
                    forceDirection =  (transform.position - transform.TransformPoint(vertexPositionForForce)).normalized;
                    pressurable = new PressurableJob2(
                        this.currentVertexPositions,
                        this.currentVertexPositionVelocities,
                        this.currentMeshVertices, 
                        forceDirection, 
                        forcePower, 
                        i, 
                        Time.deltaTime);                    
                    handle = pressurable.Schedule();
                    handle.Complete();
                    this.mesh.SetVertices(pressurable.currentMeshVertices);                
                }
                lastYValue = transform.position.y;                
            }
        }       
    }


    private void GetVertices(){
        this.currentMeshVertices = new NativeArray<float3>(mesh.vertices.Length, Allocator.Persistent);
        this.currentVertexPositions = new NativeArray<float3>(mesh.vertices.Length, Allocator.Persistent);
        this.currentVertexPositionVelocities = new NativeArray<float3>(mesh.vertices.Length, Allocator.Persistent);
        for(int i = 0; i<mesh.vertices.Length;i++){
            this.currentVertexPositions[i] = mesh.vertices[i];
            this.currentVertexPositionVelocities[i] = Vector3.zero;
            this.currentMeshVertices[i] = mesh.vertices[i];
        }
    }    
}

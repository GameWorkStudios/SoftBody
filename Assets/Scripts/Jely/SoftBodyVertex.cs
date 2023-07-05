using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftBodyVertex
{

    public int verticeIndex;
    public Vector3 initialVertexPosition;
    public Vector3 currentVertexPosition;

    public Vector3 currentVelocity;

    public SoftBodyVertex(int verticeIndex, Vector3 initialVertexPosition, Vector3 currentVertexPosition, Vector3 currentVelocity){
        this.verticeIndex = verticeIndex;
        this.initialVertexPosition = initialVertexPosition;
        this.currentVertexPosition = currentVertexPosition;
        this.currentVelocity = currentVelocity;        
    }

    public Vector3 GetCurrentDisplacement(){
        return currentVertexPosition - initialVertexPosition;
    }

    public void UpdateVelocity(float bounceSpeed){
        currentVelocity = currentVelocity - GetCurrentDisplacement() * bounceSpeed * Time.deltaTime;
    }

    public void Settle(float stiffness){
        currentVelocity *= 1f - stiffness * Time.deltaTime;
    }

    public void ApplyPressureToVertex(Transform tr, Vector3 position, float pressure){
        Vector3 distanceVerticePoint = currentVertexPosition - tr.InverseTransformPoint(position);
        float adaptedPressure = pressure / (1f + distanceVerticePoint.sqrMagnitude);
        float velocity = adaptedPressure * Time.deltaTime;
        currentVelocity += distanceVerticePoint.normalized * velocity;
    }

}

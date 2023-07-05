using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle
{

    public int verticeIndex;
    public Vector3 initialVertexPosition;
    public Vector3 currentVertexPosition;
    public Vector3 currentVelocity;

    public Particle(int verticeIndex, Vector3 initialVertexPosition, Vector3 currentVertexPosition){
        this.verticeIndex = verticeIndex;
        this.initialVertexPosition = initialVertexPosition;
        this.currentVertexPosition = currentVertexPosition;
        this.currentVelocity = Vector3.zero;   
    }

    public void UpdateVelocity(Vector3 force){
        this.currentVelocity = force * Time.deltaTime;
    }

}

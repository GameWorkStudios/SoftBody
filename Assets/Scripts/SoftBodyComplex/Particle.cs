using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is represent all vertices on the mesh.
/// TODO : Detail this text.
/// </summary>
namespace VertexGravity{

    public class Particle
    {
        public float mass;
        public Vector3 currentVelocity;
        public int verticeIndex;
        public Vector3 initialVertexPosition;
        public Vector3 currentVertexPosition;

        public Particle(int verticeIndex, Vector3 initialVertexPosition, Vector3 currentVertexPosition){
            this.verticeIndex = verticeIndex;
            this.initialVertexPosition = initialVertexPosition;
            this.currentVertexPosition = currentVertexPosition;
            this.currentVelocity = Vector3.zero;
        }

        public Vector3 GetCurrentDisplacement(){
            Vector3 currentDisp = currentVertexPosition - initialVertexPosition;
            return currentDisp;
        }

        
        public void UpdateVelocity(float bounceSpeed = 0){
            if(GetCurrentDisplacement() != Vector3.zero){
                currentVelocity = currentVelocity - GetCurrentDisplacement() * bounceSpeed * Time.deltaTime;
            }else{
                currentVelocity = bounceSpeed * Time.deltaTime * Vector3.down;
            }
        }

        public void Settle(float stiffness = 1){
            currentVelocity *= 1f - stiffness * Time.deltaTime;
        }
        

    }
}
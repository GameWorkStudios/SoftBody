using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JellyBehaviour{

    public class Soften : MonoBehaviour
    {

        [SerializeField] private float bounceSpeed;
        [SerializeField] private float fallForce;
        [SerializeField] private float stiffness;

        private MeshFilter meshFilter;
        private Mesh mesh;

        private SoftBodyVertex[] vertices;
        private Vector3[] currentMeshVertices;

        private void Start() {
            meshFilter = GetComponent<MeshFilter>();
            mesh = meshFilter.mesh;
            
            GetVertices();
        }

        private void GetVertices(){
            this.vertices = new SoftBodyVertex[mesh.vertices.Length];
            this.currentMeshVertices = new Vector3[mesh.vertices.Length];
            for(int i = 0; i<mesh.vertices.Length;i++){
                this.vertices[i] = new SoftBodyVertex(i, mesh.vertices[i], mesh.vertices[i], Vector3.zero);
                this.currentMeshVertices[i] = mesh.vertices[i];
            }
        }

        private void Update() {
            UpdateVertices();
        }

        private void UpdateVertices(){
            for(int i = 0; i < this.vertices.Length; i++){
                this.vertices[i].UpdateVelocity(bounceSpeed);
                this.vertices[i].Settle(stiffness);

                this.vertices[i].currentVertexPosition += this.vertices[i].currentVelocity * Time.deltaTime;
                this.currentMeshVertices[i] = this.vertices[i].currentVertexPosition;
            }

            this.mesh.vertices = currentMeshVertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();

        }

        private void OnCollisionEnter(Collision other) {
            ContactPoint[] collisionPoints = other.contacts;
            for(int i = 0; i < collisionPoints.Length; i++){
                Vector3 inputPoint = collisionPoints[i].point + (collisionPoints[i].point * 0.1f);
                ApplyPressureToPoint(inputPoint, fallForce);
            }
        }

        private void ApplyPressureToPoint(Vector3 inputPoint, float pressure){
            for(int i = 0; i < this.vertices.Length; i++){
                this.vertices[i].ApplyPressureToVertex(transform, inputPoint, pressure);
            }
        }

    }
}
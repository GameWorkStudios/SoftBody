using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VertexGravity{

    public class Graviton : MonoBehaviour
    {

        [SerializeField] private float massOfVertices = 10f;
        private MeshFilter meshFilter;
        private Mesh mesh;

        private Particle[] paritcles;
        private Vector3[] currentMeshVertices;

        private void Start() {
            meshFilter = GetComponent<MeshFilter>();
            mesh = meshFilter.mesh;
            GetParticles();
        }

        private void GetParticles(){
            this.paritcles = new Particle[mesh.vertices.Length];
            this.currentMeshVertices = new Vector3[mesh.vertices.Length];
            for(int i = 0; i<mesh.vertices.Length; i++){
                this.paritcles[i] = new Particle(i, mesh.vertices[i], mesh.vertices[i]);
                this.currentMeshVertices[i] = mesh.vertices[i];
            }
        }

        private void Update() {
            UpdateVertices();
        }

        private void UpdateVertices(){
            for (int i = 0; i < this.paritcles.Length; i++){
                this.paritcles[i].UpdateVelocity(massOfVertices * -9.8f);
                this.paritcles[i].Settle(15);
                this.paritcles[i].currentVertexPosition += this.paritcles[i].currentVelocity * Time.deltaTime;
                this.currentMeshVertices[i] = this.paritcles[i].currentVertexPosition;
            }
            this.mesh.vertices = currentMeshVertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
        }

        
    }
}
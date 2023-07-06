using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class Pressurable : MonoBehaviour
{

    private MeshFilter meshFilter;
    private Mesh mesh;

    private Particle[] particles;
    private Vector3[] currentMeshVertices;

    Rigidbody rb;
    int randomVertexID  = -1;

    [SerializeField] float forcePower = 0.05f;

    private Vector3 forceDirection = Vector3.zero;

    float lastYValue;

    [SerializeField] bool isDiving = false;

    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        meshFilter = GetComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        GetVertices();
        lastYValue = transform.position.y;
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F)){
            if(randomVertexID == -1){
                randomVertexID = Random.Range(0,this.particles.Length-1);
            }
            Vector3 vertexPositionForForce = this.particles[randomVertexID].currentVertexPosition; 
            forceDirection =  (transform.position - transform.TransformPoint(vertexPositionForForce)).normalized;
            UpdateVertices(randomVertexID);
            //rb.AddForceAtPosition( forceDirection * forcePower,vertexPositionForForce);
        }

        if(Input.GetKeyDown(KeyCode.A)){
            for(int i = 0; i < this.particles.Length; i++){
                Vector3 vertexPositionForForce = this.particles[i].currentVertexPosition; 
                forceDirection =  (transform.position - transform.TransformPoint(vertexPositionForForce)).normalized;
                UpdateVertices(i);
            }
        }

        /// <summary>
        /// if object previous location of y and current location of y difference bigger than 1f then apply all directions to pressure.
        /// </summary>
        if(isDiving)
        {            
            //rb.isKinematic = false;
            if(Mathf.Abs(transform.position.y - lastYValue) > 1f){
                for(int i = 0; i < this.particles.Length; i++){
                    Vector3 vertexPositionForForce = this.particles[i].currentVertexPosition; 
                    forceDirection =  (transform.position - transform.TransformPoint(vertexPositionForForce)).normalized;
                    UpdateVertices(i);
                }
                lastYValue = transform.position.y;
            }
        }        



        

    }

    private void UpdateVertices(int vertixid) {
            for(int i = 0; i < this.particles.Length; i++){
                if(vertixid != i){
                    continue;
                }
                this.particles[i].UpdateVelocity(forceDirection * forcePower);
            
                Debug.DrawRay(transform.TransformPoint(this.particles[i].currentVertexPosition), forceDirection, Color.red, 100f);
                
                this.particles[i].currentVertexPosition += this.particles[i].currentVelocity * Time.deltaTime;
                this.currentMeshVertices[i] = this.particles[i].currentVertexPosition;
            }
            this.mesh.vertices = currentMeshVertices;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
    }

    private void GetVertices(){
        Debug.Log("Count of vertices : " + mesh.vertices.Length);
        this.particles = new Particle[mesh.vertices.Length];
        this.currentMeshVertices = new Vector3[mesh.vertices.Length];
        for(int i = 0; i<mesh.vertices.Length;i++){
            this.particles[i] = new Particle(i, mesh.vertices[i], mesh.vertices[i]);
            this.currentMeshVertices[i] = mesh.vertices[i];
        }
    }
}

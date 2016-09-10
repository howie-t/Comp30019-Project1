using UnityEngine;
using System.Collections;

public class Water : MonoBehaviour {

    public Shader shader;
    public PointLight sun;

    public int size;
    private Color transparentBlue = new Color(0.529f, 0.807f, 0.922f, 0.5f);

    // Use this for initialization
    void Start()
    {
        // Add a MeshFilter component
        MeshFilter waterMesh = this.gameObject.AddComponent<MeshFilter>();
        waterMesh.mesh = this.CreateWaterMesh();

        // Add a MeshRenderer component
        MeshRenderer renderer = this.gameObject.AddComponent<MeshRenderer>();
        renderer.material.shader = shader;

        this.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer>();
        renderer.material.SetColor("_PointLightColor", sun.color);
        renderer.material.SetVector("_PointLightPosition", sun.GetWorldPosition());
    }

    // Method to create the semi-transparent water surface
    Mesh CreateWaterMesh()
    {
        Mesh m = new Mesh();
        m.name = "Water";

        m.vertices = new[]
        {
            new Vector3(0.0f,0.0f,0.0f),
            new Vector3(0.0f,0.0f,size-1),
            new Vector3(size-1,0.0f,0.0f),
            new Vector3(size-1,0.0f,0.0f),
            new Vector3(0.0f,0.0f,size-1),
            new Vector3(size-1,0.0f,size-1)
        };

        // Define the vertex colours
        m.colors = new[]
        {
            transparentBlue,
            transparentBlue,
            transparentBlue,
            transparentBlue,
            transparentBlue,
            transparentBlue
        };

        int[] triangles = new int[m.vertices.Length];
        for (int i = 0; i < m.vertices.Length; i++)
            triangles[i] = i;

        m.triangles = triangles;

        // Surface Normal and Vertex Normal is the same for the water surface
        m.normals = new[]
        {
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up,
            Vector3.up
        };
        return m;
    }
}

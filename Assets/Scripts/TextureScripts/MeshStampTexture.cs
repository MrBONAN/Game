using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshStampTexture : MonoBehaviour
{
    public Texture2D texture;

    private void Start()
    {
        CreateMesh();
        ApplyTexture();
    }

    void CreateMesh()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-1, -1, 0),
            new Vector3(1, -1, 0),
            new Vector3(1, 1, 0),
            new Vector3(-1, 1, 0)
        };

        int[] triangles = new int[]
        {
            0, 2, 1, // Первый треугольник
            2, 3, 1  // Второй треугольник
        };

        Vector2[] uv = new Vector2[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;

        mesh.RecalculateNormals();
    }

    void ApplyTexture()
    {
        if (texture == null)
        {
            Debug.LogError("Texture is not assigned.");
            return;
        }

        Material material = new Material(Shader.Find("Standard"));
        material.mainTexture = texture;
        GetComponent<MeshRenderer>().material = material;
    }
}

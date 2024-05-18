using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileController : MonoBehaviour
{
    public Vector2 tiling = new Vector2(1, 1);

    private Material material;

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        material = renderer.material;
        UpdateTiling();
    }

    void UpdateTiling()
    {
        if (material != null)
        {
            material.SetVector("_Tiling", tiling);
        }
    }

    void OnValidate()
    {
        UpdateTiling();
    }
}


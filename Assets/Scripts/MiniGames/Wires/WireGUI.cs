using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace MazeMiniGame
{
    public enum Rotation
    {
        Normal,
        Degree90,
        Degree180,
        Degree270
    }

    public class WireGUI : MonoBehaviour
    {
        public WireType type;
        public Vector2 position;
        public Rotation rotation;
        [NonSerialized] public GameObject gameObj;
        public GameObject defaultPrefab;
        public GameObject bridgePrefab;
        public GameObject longCornerPrefab;
        public GameObject cornerPrefab;
        public GameObject longPrefab;
        public Dictionary<WireType, GameObject> prefabs;
        public Dictionary<GameObject, Renderer> objectSizes;
        private new SpriteRenderer renderer;


        public void ChangeRotation(Rotation rotation)
        {
            this.rotation = rotation;
            gameObj.transform.Rotate(new Vector3(0,0, -90));
        }

        public void DrawWire(Transform parent)
        {
            prefabs = new Dictionary<WireType, GameObject>()
            {
                { WireType.Default, defaultPrefab },
                { WireType.Bridge, bridgePrefab },
                { WireType.LongCorner, longCornerPrefab },
                { WireType.Corner, cornerPrefab },
                { WireType.Long, longPrefab }
            };
            gameObj = Instantiate(prefabs[type], parent);
            gameObj.transform.localPosition = new Vector3(position.x - objectSizes[prefabs[type]].bounds.size.x / 2,
                position.y - objectSizes[prefabs[type]].bounds.size.y / 2, 0);
        }

        public void SetGUIPosition(Vector2 pos)
        {
            position = pos;
        }

        public void Visualize()
        {
            if (renderer == null)
                renderer = gameObj.GetComponent<SpriteRenderer>();
            renderer.color = new Color(1f, 1f, 1f, 1f); 
        }

        public void UnVisualize()
        {
            if (renderer != null)
                renderer.color = new Color(1f, 1f, 1f, 0.2f); 
        }
    }
}
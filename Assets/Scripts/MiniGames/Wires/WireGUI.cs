using System.Collections.Generic;
using System.Numerics;
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
        public Vector2Int position;
        public Rotation rotation;
        public GameObject gameObj;
        [SerializeField] private GameObject defaultPrefab;
        [SerializeField] private GameObject bridgePrefab;
        [SerializeField] private GameObject longCornerPrefab;
        [SerializeField] private GameObject cornerPrefab;
        [SerializeField] private GameObject longPrefab;
        private Dictionary<WireType, GameObject> prefabs;


        public void ChangeRotation(Rotation rotation)
        {
            this.rotation = rotation;
            gameObj.transform.Rotate(0, 90, 0);
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
            gameObj.transform.position = new Vector3(position.x, position.y, 0);
        }
    }
}
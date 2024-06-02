using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Threading;

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
        public GameObject defaultPrefab;
        public GameObject bridgePrefab;
        public GameObject longCornerPrefab;
        public GameObject cornerPrefab;
        public GameObject longPrefab;
        [NonSerialized] public GameObject gameObj;
        public Dictionary<WireType, GameObject> prefabs;
        public WireType type;
        public Dictionary<GameObject, Renderer> objectSizes;
        private new SpriteRenderer renderer;
        public bool rotating = false;
        
        public Vector2 position;


        public void ChangeRotation(float duration)
        {
            if (duration == 0)
                FastRotate();
            else
                StartCoroutine(Rotate(duration));
        }
        

        private IEnumerator Rotate(float durationSeconds)
        {
            if (rotating) yield break;
            rotating = true;
            Quaternion startRotation = gameObj.transform.rotation;
            Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, -90); // Установка желаемого поворота

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime / durationSeconds;
                gameObj.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t); // Плавное вращение к целевому повороту
                yield return null;        
            }

            rotating = false;
        }

        private void FastRotate()
        {
            gameObj.transform.Rotate(new Vector3(0, 0, -90));
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
            gameObj.transform.localPosition = new Vector3(position.x, position.y, 0);
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
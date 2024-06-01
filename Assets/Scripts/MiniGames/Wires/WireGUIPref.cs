using System;
using System.Collections.Generic;
using UnityEngine;

namespace MazeMiniGame
{
    public class WireGUIPref : MonoBehaviour
    {
        [SerializeField] public GameObject defaultPrefab;
        [SerializeField] public GameObject bridgePrefab;
        [SerializeField] public GameObject longCornerPrefab;
        [SerializeField] public GameObject cornerPrefab;
        [SerializeField] public GameObject longPrefab;

        [NonSerialized]
        public Dictionary<GameObject, Renderer> objectsRenderer = new Dictionary<GameObject, Renderer>();

        public void SetScales(float scale)
        {
            defaultPrefab.transform.localScale = new Vector3(1, 1, 1) * scale;
            bridgePrefab.transform.localScale = new Vector3(1, 1, 1) * scale;
            longPrefab.transform.localScale = new Vector3(1, 1, 1) * scale;
            longCornerPrefab.transform.localScale = new Vector3(1, 1, 1) * scale;
            cornerPrefab.transform.localScale = new Vector3(1, 1, 1) * scale;

            objectsRenderer.Add(defaultPrefab, defaultPrefab.GetComponent<Renderer>());
            objectsRenderer.Add(bridgePrefab, bridgePrefab.GetComponent<Renderer>());
            objectsRenderer.Add(longPrefab, longPrefab.GetComponent<Renderer>());
            objectsRenderer.Add(longCornerPrefab, longCornerPrefab.GetComponent<Renderer>());
            objectsRenderer.Add(cornerPrefab, cornerPrefab.GetComponent<Renderer>());
        }
    }
}
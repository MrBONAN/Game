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

        public void SetScales(float x1, float x2, float x3, float x4, float x5)
        {
            defaultPrefab.transform.localScale = new Vector3(1, 1, 1) * x1;
            bridgePrefab.transform.localScale = new Vector3(1, 1, 1) * x2;
            longPrefab.transform.localScale = new Vector3(1, 1, 1) * x3;
            longCornerPrefab.transform.localScale = new Vector3(1, 1, 1) * x4;
            cornerPrefab.transform.localScale = new Vector3(1, 1, 1) * x5;

            objectsRenderer.Add(defaultPrefab, defaultPrefab.GetComponent<Renderer>());
            objectsRenderer.Add(bridgePrefab, bridgePrefab.GetComponent<Renderer>());
            objectsRenderer.Add(longPrefab, longPrefab.GetComponent<Renderer>());
            objectsRenderer.Add(longCornerPrefab, longCornerPrefab.GetComponent<Renderer>());
            objectsRenderer.Add(cornerPrefab, cornerPrefab.GetComponent<Renderer>());
        }
    }
}
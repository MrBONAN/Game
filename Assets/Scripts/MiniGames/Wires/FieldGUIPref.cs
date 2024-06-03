using UnityEngine;

namespace MazeMiniGame
{
    public class FieldGUIPref : MonoBehaviour
    {
        [SerializeField] public GameObject fieldPrefab;
        [SerializeField] public Vector3 position;
        [SerializeField] public AudioClip winAnimationSound;
    }
}
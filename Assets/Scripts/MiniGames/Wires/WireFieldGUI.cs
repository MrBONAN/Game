using UnityEngine;

namespace MazeMiniGame
{
    public class WireFieldGUI : MonoBehaviour
    {
        [SerializeField] private GameObject fieldPrefab;
        [SerializeField] private Vector3 position;
        public void DrawField()
        {
            Instantiate(fieldPrefab, position, Quaternion.identity);
        }
    }
}
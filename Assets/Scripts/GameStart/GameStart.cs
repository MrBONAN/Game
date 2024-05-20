using UnityEngine;

public class GameStart : MonoBehaviour
{
    public void LoadLevel()
    {
        Debug.Log("Lobby entered");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Lobby");
    }
}

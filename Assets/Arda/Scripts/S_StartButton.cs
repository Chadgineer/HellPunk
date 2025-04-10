using UnityEngine;
using UnityEngine.SceneManagement;

public class S_StartButton : MonoBehaviour
{
    public void StartGame()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}
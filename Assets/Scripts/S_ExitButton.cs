using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public void ExitGame()
    {
        // This works in a built game (not in editor)
        Application.Quit();

    }
}
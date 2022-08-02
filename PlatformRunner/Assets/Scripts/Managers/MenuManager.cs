using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        public void SinglePlayerSceneLoad()
        {
            SceneManager.LoadScene(1);
        }

        public void WithAISceneLoad()
        {
            SceneManager.LoadScene(2);
        }

        public void ExitOnClick()
        {
            Application.Quit();
        }
    }
}
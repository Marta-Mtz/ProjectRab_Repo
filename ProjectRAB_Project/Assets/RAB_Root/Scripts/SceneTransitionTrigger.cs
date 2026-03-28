using UnityEngine;
using UnityEngine.SceneManagement; 

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Configuración")]
    public string nextSceneName; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}


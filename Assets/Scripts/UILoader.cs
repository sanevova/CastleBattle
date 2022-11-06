using UnityEngine;
using UnityEngine.SceneManagement;


public class UILoader : MonoBehaviour {
    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            if (SceneManager.GetSceneByName("UIScene").isLoaded) {
                SceneManager.UnloadSceneAsync("UIScene");
            } else {
                SceneManager.LoadSceneAsync("UIScene", LoadSceneMode.Additive);
            }
        }
    }
}

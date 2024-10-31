using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] [JapaneseLabel("プレイヤーのカメラ")] private GameObject MyPlayerCamera;
    [SerializeField] [JapaneseLabel("フリスビーの死亡時カメラ")] private GameObject DeathCamera;

    private GameObject activeCamera;
    
    private void Awake()
    {
        activeCamera = MyPlayerCamera;
        SwitchCamera(activeCamera);
    }
    public void MyDestroyed()
    {
        if (activeCamera != DeathCamera)
        {
            activeCamera = DeathCamera;
            SwitchCamera(activeCamera);
        }
    }

    public void RemoveCamera()
    {
        // MyPlayerCamera に切り替え
        if (activeCamera != MyPlayerCamera)
        {
            activeCamera = MyPlayerCamera;
            SwitchCamera(activeCamera);
        }
    }

    private void SwitchCamera(GameObject newCamera)
    {
        MyPlayerCamera.SetActive(false);
        DeathCamera.SetActive(false);
        newCamera.SetActive(true);
    }
}
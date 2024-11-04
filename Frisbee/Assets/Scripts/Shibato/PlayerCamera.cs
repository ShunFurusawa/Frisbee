using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject MyPlayerCamera;
    public GameObject DeathCamera;

    private GameObject activeCamera;

    // Start is called before the first frame update
    private void Start()
    {
        activeCamera = MyPlayerCamera;
        SwitchCamera(activeCamera);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void MyDestroyed()
    {
        if (activeCamera == MyPlayerCamera)
            activeCamera = DeathCamera;
        else
            activeCamera = MyPlayerCamera;

        SwitchCamera(activeCamera);
    }

    public void RemoveCamera()
    {
        if (activeCamera == DeathCamera)
            activeCamera = MyPlayerCamera;
        else
            activeCamera = DeathCamera;

        SwitchCamera(activeCamera);
    }

    private void SwitchCamera(GameObject newCamera)
    {
        // 全カメラを無効化し、選択したカメラを有効化
        MyPlayerCamera.SetActive(false);
        DeathCamera.SetActive(false);
        newCamera.SetActive(true);
    }
}
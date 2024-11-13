using UnityEngine;

public class WindStop : MonoBehaviour
{
    [JapaneseLabel("オンオフを切り替える風")] [SerializeField]
    private GameObject windObject;

    [JapaneseLabel("初期設定")] [SerializeField]
    private bool windSwitch;

    [JapaneseLabel("ONのマテリアル")][SerializeField] private Material ON;
    [JapaneseLabel("OFFのマテリアル")][SerializeField] private Material OFF;
    
    private Renderer windRenderer;
    private void Awake()
    {
        if (windObject == null)
        {
            Debug.LogError("WindObjectが設定されていません");
            enabled = false;
            return;
        }
        windRenderer = GetComponent<Renderer>();
        WindChange();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            windSwitch = !windSwitch;
            WindChange();
        }
    }

    private void WindChange()
    {
        windRenderer.material = windSwitch ? ON : OFF;
        windObject.SetActive(windSwitch);
    }
}
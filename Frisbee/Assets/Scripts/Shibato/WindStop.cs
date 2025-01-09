using UnityEngine;
namespace Shibato
{
    public class WindStop : MonoBehaviour
    {
        [JapaneseLabel("オンオフを切り替える風")] [SerializeField]
        private GameObject windObject;

        [JapaneseLabel("オンオフを切り替える風2個目")] [SerializeField]
        private GameObject windObject2;

        [JapaneseLabel("初期設定")] [SerializeField]
        private bool windSwitch;

        [JapaneseLabel("ONのマテリアル")] [SerializeField]
        private Material ON;

        [JapaneseLabel("OFFのマテリアル")] [SerializeField]
        private Material OFF;

        [SerializeField] private Renderer windRenderer;

        private void Awake()
        {
            if (windObject == null)
            {
                Debug.LogError("WindObjectが設定されていません");
                enabled = false;
                return;
            }

            WindChange();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Frisbee"))
            {
                windSwitch = !windSwitch;
                WindChange();
            }
        }

        private void WindChange()
        {
            if (ON != null || OFF != null)
            {
                windRenderer.material = windSwitch ? ON : OFF;
            }

            if (windObject2 != null)
            {
                windObject2.SetActive(!windSwitch);
            }

            windObject.SetActive(windSwitch);

        }
    }
}
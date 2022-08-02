using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Paint
{
    public class PaintTexture : MonoBehaviour
    {
        [SerializeField] private GameObject brushContainer;
        [SerializeField] private GameObject brushEntity;
        [SerializeField] private Camera sceneCamera, canvasCam;
        [SerializeField] private RenderTexture canvasTexture;
        [SerializeField] private Material baseMaterial;
        [SerializeField] private TextMeshProUGUI ratioText;
        [SerializeField] private Slider slider;
        [SerializeField] private Image fill;
        private Color _brushColor = Color.red;
        private float _brushSize = 1f;
        private int _brushCounter = 0;
        private const int MaxBrushCount = 500;
        private float _totalPixelCount;
        private float _redPixelCount = 0;

        private void Awake()
        {
            _totalPixelCount = canvasTexture.width * canvasTexture.height;
            slider.value = 0;
        }
        Texture2D ToTexture2D(RenderTexture rTex)
        {
            Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
            RenderTexture.active = rTex;
            tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tex.Apply();
            return tex;
        }
        void Update()
        {
            if (GameManager.Instance.CanPlayerPaint)
            {
                _redPixelCount = 0;
                var texture = ToTexture2D(canvasTexture);
                for (int i = canvasTexture.width-1; i >= 0; i--)
                {
                    for (int j = canvasTexture.height-1; j >= 0; j--)
                    {
                        if (texture.GetPixel(i, j).r>= .8f)
                        {
                            _redPixelCount++;
                        }
                    }
                }

                var ratio = (_redPixelCount / _totalPixelCount) * 100f;
                slider.value = Mathf.Round(ratio);
                ratioText.text = "%" + Mathf.Round(ratio);
                if (Mathf.Round(ratio)>90f)
                {
                    fill.color = Color.green;
                    ratioText.color = Color.green;
                    GameManager.Instance.OpenHalloweenVFX();
                }
                if (Input.GetMouseButton(0) || Input.touchCount > 0)
                {
                    DoAction();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    Invoke("SaveTexture", 0.1f);
                }
            }
            
            
        }

        void DoAction()
        {
            Vector3 uvWorldPosition = Vector3.zero;
            if (HitTestUVPosition(ref uvWorldPosition))
            {
                GameObject brushObj = Instantiate(brushEntity, brushContainer.transform, true);
                brushObj.GetComponent<SpriteRenderer>().color = _brushColor;
                _brushColor.a = _brushSize * 2.0f;
                brushObj.transform.localPosition = uvWorldPosition;
                brushObj.transform.localScale = Vector3.one * _brushSize;
            }
            _brushCounter++;
            if (_brushCounter >= MaxBrushCount)
            {
                Invoke("SaveTexture", 0.1f);
            }
        }
        bool HitTestUVPosition(ref Vector3 uvWorldPosition)
        {
            RaycastHit hit;
            Vector3 cursorPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.0f);
            Ray cursorRay = sceneCamera.ScreenPointToRay(cursorPos);
            if (Physics.Raycast(cursorRay, out hit, 200))
            {
                MeshCollider meshCollider = hit.collider as MeshCollider;
                if (meshCollider == null || meshCollider.sharedMesh == null)
                {
                    return false;
                }
                Vector2 pixelUV = new Vector2(hit.textureCoord.x, hit.textureCoord.y);
                var orthographicSize = canvasCam.orthographicSize;
                uvWorldPosition.x = pixelUV.x - orthographicSize; 
                uvWorldPosition.y = pixelUV.y - orthographicSize;
                uvWorldPosition.z = 0.0f;
                return true;
            }
            return false;
        }
        void SaveTexture()
        {
            _brushCounter = 0;
            RenderTexture.active = canvasTexture;
            Texture2D tex = new Texture2D(canvasTexture.width, canvasTexture.height, TextureFormat.RGB24, false);
            tex.ReadPixels(new Rect(0, 0, canvasTexture.width, canvasTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;
            baseMaterial.mainTexture = tex;
            foreach (Transform child in brushContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
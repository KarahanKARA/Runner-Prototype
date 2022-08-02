using System.Collections;
using TMPro;
using UnityEngine;

namespace UI
{
    public class RgbTMPColorChanger : MonoBehaviour
    {
        private TextMeshProUGUI _tmp;
        private int _colorR = 255, _colorG = 0, _colorB = 255;

        private void Start()
        {
            _tmp = GetComponent<TextMeshProUGUI>();
            _tmp.color = new Color(_colorR, _colorG, _colorB, 0.5f);
            StartCoroutine(ColorChanger());
        }

        private IEnumerator ColorChanger()
        {
            while (true)
            {
                if (_colorR == 255)
                {
                    for (_colorR = 255; _colorR > 0; _colorR -= 5)
                    {
                        _tmp.color = new Color32((byte)_colorR, (byte)_colorG, (byte)_colorB, 255);
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                else
                {
                    for (_colorR = 0; _colorR < 255; _colorR += 5)
                    {
                        _tmp.color = new Color32((byte)_colorR, (byte)_colorG, (byte)_colorB, 255);
                        yield return new WaitForSeconds(0.05f);
                    }
                }

                if (_colorG == 255)
                {
                    for (_colorG = 255; _colorG > 0; _colorG -= 5)
                    {
                        _tmp.color = new Color32((byte)_colorR, (byte)_colorG, (byte)_colorB, 255);
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                else
                {
                    for (_colorG = 0; _colorG < 255; _colorG += 5)
                    {
                        _tmp.color = new Color32((byte)_colorR, (byte)_colorG, (byte)_colorB, 255);
                        yield return new WaitForSeconds(0.05f);
                    }
                }

                if (_colorB == 255)
                {
                    for (_colorB = 255; _colorB > 0; _colorB -= 5)
                    {
                        _tmp.color = new Color32((byte)_colorR, (byte)_colorG, (byte)_colorB, 255);
                        yield return new WaitForSeconds(0.05f);
                    }
                }
                else
                {
                    for (_colorB = 0; _colorB < 255; _colorB += 5)
                    {
                        _tmp.color = new Color32((byte)_colorR, (byte)_colorG, (byte)_colorB, 255);
                        yield return new WaitForSeconds(0.05f);
                    }
                }
            }
        }
    }
}
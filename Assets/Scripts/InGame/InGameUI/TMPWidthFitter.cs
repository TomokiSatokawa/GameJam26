using TMPro;
using UnityEngine;

namespace Common.UI
{
    /// <summary>
    /// 文字サイズは変えず、最大幅を超えたら文字の横スケールだけを縮めて収める
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class TMPWidthScaler : MonoBehaviour
    {
        [SerializeField] private float _maxWidth = 200f;
        [SerializeField, Range(0.1f, 1f)] private float _minScaleX = 0.5f;

        private TMP_Text _text;
        private bool _isApplying;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Add(OnTextChanged);
            Apply();
        }

        private void OnDisable()
        {
            TMPro_EventManager.TEXT_CHANGED_EVENT.Remove(OnTextChanged);
        }

        private void OnTextChanged(Object obj)
        {
            if (obj != _text || _isApplying) return;
            Apply();
        }

        public void Apply()
        {
            if (_text == null) return;

            _isApplying = true;

            _text.ForceMeshUpdate();

            float textWidth = _text.textBounds.size.x;
            float scaleX = textWidth > _maxWidth ? Mathf.Max(_maxWidth / textWidth, _minScaleX) : 1f;

            ScaleVertices(scaleX);

            _isApplying = false;
        }

        private void ScaleVertices(float scaleX)
        {
            TMP_TextInfo textInfo = _text.textInfo;

            // テキスト全体の中心X(この点を基準に横へ縮める)
            float centerX = _text.textBounds.center.x;

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                Vector3[] vertices = textInfo.meshInfo[i].vertices;

                for (int v = 0; v < textInfo.meshInfo[i].vertexCount; v++)
                {
                    Vector3 vertex = vertices[v];
                    vertex.x = centerX + (vertex.x - centerX) * scaleX;
                    vertices[v] = vertex;
                }

                textInfo.meshInfo[i].mesh.vertices = vertices;
                _text.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }
        }
    }
}
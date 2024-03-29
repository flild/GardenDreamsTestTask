using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;


public class SimpleButtonAnim : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private float _scaleAmount = 0.8f; // Увеличение или уменьшение масштаба
    [SerializeField]
    private float _duration = 0.1f;
    [SerializeField]
    private float _delay = 0.1f;
    public void OnPointerClick(PointerEventData eventData)
    {
        DOTween.Sequence()
            .Append(transform.DOScale(_scaleAmount, _duration))
            .AppendInterval(_delay)
            .Append(transform.DOScale(1, _duration))
            .SetLink(gameObject);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] GameObject _cake;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySound("DM-CGS-21", true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       
        AudioManager.Instance.PlaySound("DM-CGS-32", true);

        if (_cake != null)
        {
            _cake.SetActive(true);
        }
          
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_cake != null)
        {
            _cake.SetActive(false);
        }
            
    }
}

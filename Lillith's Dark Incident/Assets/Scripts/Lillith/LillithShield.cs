using System.Collections;
using UnityEngine;

public class LillithShield : MonoBehaviour
{
    private void Update()
    {
        transform.position = transform.parent.position;
    }

    public void Break(float time, string param)
    {
        StartCoroutine(Pop(time, param));
    }

    private IEnumerator Pop(float time, string param)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Animator>().SetTrigger(param);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}

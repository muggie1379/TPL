using UnityEngine;
using System.Collections;

public class BootManager : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return null;
        yield return new WaitForSeconds(0.5f);
        SceneChanger.Instance.LoadScene(SceneEnum.TitleScene);
    }
}
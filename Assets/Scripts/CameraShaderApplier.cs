using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class FinalOverlayPostProcess : MonoBehaviour {
    [SerializeField] private Material postProcessMaterial;

    private Texture2D _screenCopy;
    private Camera _cam;

    private void Awake() {
        _cam = GetComponent<Camera>();
    }

    private void Start() {
        _screenCopy = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
    }

    private void OnEnable() => StartCoroutine(CaptureAndBlit());
    
    private IEnumerator CaptureAndBlit() {
        while (true) {
            yield return new WaitForEndOfFrame();
            _screenCopy.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            _screenCopy.Apply(false);
            var rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 0, RenderTextureFormat.Default);
            Graphics.Blit(_screenCopy, rt);
            Graphics.Blit(rt, null, postProcessMaterial);
            RenderTexture.ReleaseTemporary(rt);
        }
    }
}
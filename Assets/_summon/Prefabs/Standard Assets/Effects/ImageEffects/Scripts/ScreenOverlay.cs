using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityStandardAssets.ImageEffects
{
    [RequireComponent (typeof(Camera))]
    [AddComponentMenu ("Image Effects/Other/Screen Overlay")]
    public class ScreenOverlay : PostEffectsBase
	{
	    public enum OverlayBlendMode
		{
            Additive = 0,
            ScreenBlend = 1,
            Multiply = 2,
            Overlay = 3,
            AlphaBlend = 4,
        }

        public bool go;
        public OverlayBlendMode blendMode = OverlayBlendMode.Overlay;
        public float intensity = 1.0f;
        public Texture2D texture = null;

        public Shader overlayShader = null;
        public float scale = 1;
        private Material overlayMaterial = null;
        public float white;
        float timer;

        private void Start()
        {
            
        }


        public void SetGo(bool b)
        {
            go = b;
            scale = 1;
            intensity = 1;
        }

        private void Update()
        {
            if (go == true) {
                timer += Time.deltaTime;
                if (intensity>0)
                {
                    scale -= Time.deltaTime/3;
                    intensity -= Time.deltaTime;
                }

            }

            if (timer>8)
            {
                white += Time.deltaTime / 4;
            }
            if (timer>9)
            {
                white += Time.deltaTime / 4;

            }

            if (white>=1)
            {
                SceneManager.LoadScene("BeetleSummoned");
            }
        }

        public override bool CheckResources ()
		{
            CheckSupport (false);

            overlayMaterial = CheckShaderAndCreateMaterial (overlayShader, overlayMaterial);

            if	(!isSupported)
                ReportAutoDisable ();
            return isSupported;
        }

        void OnRenderImage (RenderTexture source, RenderTexture destination)
		{
            if (CheckResources() == false)
			{
                Graphics.Blit (source, destination);
                return;
            }

            Vector4 UV_Transform = new  Vector4(1, 0, 0, 1);

			#if UNITY_WP8
	    	// WP8 has no OS support for rotating screen with device orientation,
	    	// so we do those transformations ourselves.
			if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
				UV_Transform = new Vector4(0, -1, 1, 0);
			}
			if (Screen.orientation == ScreenOrientation.LandscapeRight) {
				UV_Transform = new Vector4(0, 1, -1, 0);
			}
			if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
				UV_Transform = new Vector4(-1, 0, 0, -1);
			}
			#endif

            overlayMaterial.SetVector("_UV_Transform", UV_Transform);
            overlayMaterial.SetFloat ("_Intensity", intensity);
            overlayMaterial.SetTexture ("_Overlay", source);
            overlayMaterial.SetFloat("_Scale", scale);
            overlayMaterial.SetFloat("_White", white);

            Graphics.Blit (source, destination, overlayMaterial, (int) blendMode);
        }
    }
}

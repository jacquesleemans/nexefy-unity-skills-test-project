using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JacquesLeemans
{
    /// <summary>
    /// ScreenCaptureUtil
    /// </summary>
    public static class ScreenCaptureUtil
    {
        private static Color m_transparent = new Color(0, 0, 0, 0);

        /// <summary>
        /// CaptureScreenshot
        /// </summary>
        /// <param name="camera"></param>
        /// <param name="isTransparent"></param>
        /// <param name="dirName"></param>
        /// <param name="fileName"></param>
        /// <param name="resWidth"></param>
        /// <param name="resHeight"></param>
        public static void CaptureScreenshot(Camera camera, bool isTransparent, string dirName, string fileName, int resWidth, int resHeight)
        {
            //Cache camera values
            var cachedClearFlags = camera.clearFlags;
            var cachedBgColor = camera.backgroundColor;

            Texture2D screenshotTexture2D = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
            RenderTexture screenshotTexture2DRt = new RenderTexture(screenshotTexture2D.width, screenshotTexture2D.height, 24);
            RenderTexture camRt = camera.targetTexture;

            //Transparent?
            if (isTransparent)
            {
                camera.clearFlags = CameraClearFlags.Color;
                camera.backgroundColor = m_transparent;    
            }

            camera.targetTexture = screenshotTexture2DRt;
            camera.Render();
            camera.targetTexture = camRt;

            RenderTexture.active = screenshotTexture2DRt;
            screenshotTexture2D.ReadPixels(new Rect(0, 0, screenshotTexture2D.width, screenshotTexture2D.height), 0, 0);
            screenshotTexture2D.Apply();

            IoUtils.SaveTexture2d(screenshotTexture2D, dirName, fileName);

            //Reset camera values
            camera.clearFlags = cachedClearFlags;
            camera.backgroundColor = cachedBgColor;
        }
    }
}
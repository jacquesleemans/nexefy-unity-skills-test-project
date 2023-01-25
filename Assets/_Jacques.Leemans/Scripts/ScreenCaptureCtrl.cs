using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JacquesLeemans
{
    /// <summary>
    /// ScreenCaptureCtrl
    /// </summary>
    public class ScreenCaptureCtrl : MonoBehaviour
    {
        [Tooltip("Which camera should be used for the render")]
        [SerializeField]
        private Camera m_camera;

        [Tooltip("Should the render be transparent?")]
        [SerializeField]
        private bool m_useTransparentBackground = false;

        [Tooltip("The number of intervals the obj should rotate by, eg: 2, would be 180 Deg rotation")]
        [Range(0, 360)]
        [SerializeField]
        private int m_intervals = 16;

        [SerializeField] private Vector2Int m_screenshoSize = new Vector2Int(512, 512);
        
        [Range(1,10)]
        [Tooltip("The multiplier value. eg, if 2, the render would be double the screenshotSize")]
        [SerializeField] private int m_screenshotMultiplier = 1;

        [Tooltip("The margin/edge/distance the model should be from the size of the screenshot")]
        [Range(0.01f,10f)]
        [SerializeField] private float m_screenshotMargin = 0.1f;
        
        private float m_angleInterval;
        
        //cache camera transform
        private Transform m_camTransform;
        private Coroutine m_takeScreenshotCoroutine;
        private const float m_360 = 360.0f;

        private void Awake()
        {
            if (this.m_camera != null)
            {
                this.m_camTransform = this.m_camera.transform;    
            }
        }

        private void Start()
        {
            this.CalculateIntervals();
        }

        /// <summary>
        /// CalculateIntervals
        /// </summary>
        private void CalculateIntervals()
        {
            //prevent div by zero
            if (this.m_intervals.Equals(0))
            {
                return;
            }
            
            this.m_angleInterval = m_360 / m_intervals;
        }

        /// <summary>
        /// ModelLoaded
        /// </summary>
        /// <param name="modeIdx"></param>
        /// <param name="modelName"></param>
        /// <param name="spawnedObj"></param>
        /// <param name="cb"></param>
        public void ModelLoaded(int modeIdx, string modelName, GameObject spawnedObj, System.Action<int> cb)
        {
            if (this.m_takeScreenshotCoroutine != null)
            {
                StopCoroutine(this.m_takeScreenshotCoroutine);
            }

            var bounds = GetObjectBounds(spawnedObj);
            
            Debug.Log($"{modelName} = {bounds}");

            Vector3 camPos = this.m_camTransform.position;

            camPos.y = bounds.center.y;

            this.m_camTransform.position = camPos;

            this.m_camera.orthographicSize = Mathf.Max(bounds.extents.x + m_screenshotMargin, bounds.extents.y + m_screenshotMargin, bounds.extents.z + m_screenshotMargin);

            this.m_takeScreenshotCoroutine = StartCoroutine(this.TakeScreenshotCoroutine(modelName, spawnedObj, modeIdx, cb));
        }

        /// <summary>
        /// GetObjectBounds 
        /// </summary>
        /// <param name="spawnedObj"></param>
        /// <returns></returns>
        private Bounds GetObjectBounds(GameObject spawnedObj)
        {
            var bounds = new Bounds (spawnedObj.transform.position, Vector3.one);
            Renderer[] renderers = spawnedObj.GetComponentsInChildren<Renderer> ();
            
            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate (renderer.bounds);
            }

            return bounds;
        }

        /// <summary>
        /// TestTakeScreenshot
        /// </summary>
        [ContextMenu("TestTakeScreenshot")]
        public void TestTakeScreenshot()
        {
            string fileName = $"TestScreenshot.png";

            this.TakeScreenshot(string.Empty, fileName);
        }

        /// <summary>
        /// TakeScreenshot
        /// </summary>
        /// <param name="dirName"></param>
        /// <param name="fileName"></param>
        private void TakeScreenshot(string dirName, string fileName)
        {
            int resWidth = this.m_screenshoSize.x * this.m_screenshotMultiplier;
            int resHeight = this.m_screenshoSize.y * this.m_screenshotMultiplier;
            
            ScreenCaptureUtil.CaptureScreenshot(this.m_camera, this.m_useTransparentBackground, dirName, fileName, resWidth, resHeight);
        }

        /// <summary>
        /// TakeScreenshotCoroutine
        /// </summary>
        /// <param name="modelName"></param>
        /// <param name="spawnedObj"></param>
        /// <param name="modeIdx"></param>
        /// <param name="cb"></param>
        /// <returns></returns>
        private IEnumerator TakeScreenshotCoroutine(string modelName, GameObject spawnedObj, int modeIdx,
            System.Action<int> cb)
        {
            yield return null;

            string dirName = modelName;

            //frame index counter.
            int currentFrameIdx = 0;
            float currentAngle = 0;

            while (currentAngle < m_360)
            {
                string fileName = $"frame{currentFrameIdx:D4}.png";

                //Make sure the frame is rendered.
                yield return new WaitForEndOfFrame();

                this.TakeScreenshot(dirName, fileName);

                currentAngle += this.m_angleInterval;

                currentFrameIdx++;

                //Apply rotation
                spawnedObj.transform.Rotate(Vector3.up, this.m_angleInterval);

                //slight delay, to allow frame to update
                yield return new WaitForEndOfFrame();
            }

            cb?.Invoke(modeIdx + 1);
        }
    }
}
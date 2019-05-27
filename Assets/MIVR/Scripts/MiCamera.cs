//-----------------------------------------------------------------------
// <copyright file="MiCamera.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// The VR camera.
    /// </summary>
    public class MiCamera : MonoBehaviour
    {
        /// <summary>
        /// The eye suffix
        /// </summary>
        private const string CEyeSuffix = "Eye";

        /// <summary>
        /// The reticle layer mask
        /// </summary>
        private static int reticleLayerMask = -1;

        /// <summary>
        /// Frame Counting
        /// </summary>
        private int frameCount = 0;

        /// <summary>
        /// Gets the left eye camera.
        /// </summary>
        /// <value>
        /// The left eye camera.
        /// </value>
        public Camera LeftEyeCamera { get; private set; }

        /// <summary>
        /// Gets the center eye camera.
        /// </summary>
        /// <value>
        /// The center eye camera.
        /// </value>
        public Camera CenterEyeCamera { get; private set; }

        /// <summary>
        /// Gets the right eye camera.
        /// </summary>
        /// <value>
        /// The right eye camera.
        /// </value>
        public Camera RightEyeCamera { get; private set; }

        /// <summary>
        /// Gets the left eye transform.
        /// </summary>
        /// <value>
        /// The left eye transform.
        /// </value>
        public Transform LeftEyeTransform { get; private set; }

        /// <summary>
        /// Gets the center eye transform.
        /// </summary>
        /// <value>
        /// The center eye transform.
        /// </value>
        public Transform CenterEyeTransform { get; private set; }

        /// <summary>
        /// Gets the right eye transform.
        /// </summary>
        /// <value>
        /// The right eye transform.
        /// </value>
        public Transform RightEyeTransform { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether camera rotation is locked.
        /// </summary>
        /// <value>
        /// Whether Camera Rotation is locked.
        /// </value>
        public bool LockRotation { get; set; }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            this.InitGameObjects();
            this.InitStereoRender();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
            // Debug.Log("MiCamera.Start");
            this.InitGameObjects();
            this.InitStereoRender();

            this.UpdateCameras();

            // this.UpdateTransforms();
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        private void Update()
        {
            // Debug.Log("MiCamera.Update: " + frameCount);
            this.InitGameObjects();

            this.UpdateCameras();
            this.UpdateTransforms();
            this.frameCount++;
        }

        /// <summary>
        /// Called when [enable].
        /// </summary>
        private void OnEnable()
        {
            this.StartCoroutine(this.EndOfFrame());
        }

        /// <summary>
        /// Called when [disable].
        /// </summary>
        private void OnDisable()
        {
            this.StopAllCoroutines();
        }

        /// <summary>
        /// This function is called when the Script will be destroyed.
        /// </summary>
        private void OnDestroy()
        {
            if (this.LeftEyeCamera != null)
            {
                this.LeftEyeCamera.targetTexture = null;
            }

            if (this.RightEyeCamera != null)
            {
                this.RightEyeCamera.targetTexture = null;
            }

#if UNITY_EDITOR
            MiEmulation.Instance.StereoRT = null;
#endif
        }

        /// <summary>
        /// Ends the of frame.
        /// </summary>
        /// <returns>the enumerator</returns>
        private IEnumerator EndOfFrame()
        {
            while (true)
            {
                if (this.CenterEyeCamera != null)
                {
                    this.CenterEyeCamera.enabled = true;
                }

                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Updates the transforms.
        /// </summary>
        private void UpdateTransforms()
        {
            // Debug.Log("MiCamera.UpdateTransforms");
#if UNITY_ANDROID && !UNITY_EDITOR
        MiTransform hmdLeftEye = VrManager.Instance.Hmd.GetEyeTransform(Eyes.Left);
        MiTransform hmdRightEye = VrManager.Instance.Hmd.GetEyeTransform(Eyes.Right);

        // this.CenterEyeTransform.localRotation = hmdLeftEye.Rotation;
        // this.LeftEyeTransform.localRotation = hmdLeftEye.Rotation;
        // this.RightEyeTransform.localRotation = hmdRightEye.Rotation;
        // this.ReticleTransform.localRotation = hmdLeftEye.Rotation;
        // Debug.Log(string.Format("hmdLeftEye rotation is {0}", hmdLeftEye.Rotation));
        if ( !LockRotation )
        {
            this.transform.localRotation = hmdLeftEye.Rotation;
        }

        this.CenterEyeTransform.localPosition = 0.5f * (hmdLeftEye.Position + hmdRightEye.Position);
        this.LeftEyeTransform.localPosition = hmdLeftEye.Position;
        this.RightEyeTransform.localPosition = hmdRightEye.Position;
// this.ReticleTransform.localPosition = hmdLeftEye.Position;
#elif UNITY_EDITOR
            MiEmulation emulation = MiEmulation.Instance;
            transform.rotation = Quaternion.Euler(emulation.headEulerAngles.y, emulation.headEulerAngles.x, emulation.headEulerAngles.z);
            this.CenterEyeTransform.localPosition = Vector3.zero;
            this.LeftEyeTransform.localPosition = new Vector3(-emulation.ipd * 0.5f, 0.0f, 0.0f);
            this.RightEyeTransform.localPosition = new Vector3(emulation.ipd * 0.5f, 0.0f, 0.0f);

            this.CenterEyeTransform.localRotation = Quaternion.identity;
            this.LeftEyeTransform.localRotation = Quaternion.identity;
            this.RightEyeTransform.localRotation = Quaternion.identity;

#endif
        }

        /// <summary>
        /// Updates the cameras.
        /// </summary>
        private void UpdateCameras()
        {
            this.LeftEyeCamera = this.ConfigureCamera(Eyes.Left);
            this.CenterEyeCamera = this.ConfigureCamera(Eyes.Center);
            this.RightEyeCamera = this.ConfigureCamera(Eyes.Right);
        }

        /// <summary>
        /// Ensures the game object integrity.
        /// </summary>
        private void InitGameObjects()
        {
            if (this.LeftEyeTransform == null)
            {
                this.LeftEyeTransform = this.ResetEyeTransform(Eyes.Left);
            }

            if (this.CenterEyeTransform == null)
            {
                this.CenterEyeTransform = this.ResetEyeTransform(Eyes.Center);
            }

            if (this.RightEyeTransform == null)
            {
                this.RightEyeTransform = this.ResetEyeTransform(Eyes.Right);
            }

            if (this.LeftEyeCamera == null)
            {
                this.LeftEyeCamera = this.LeftEyeTransform.GetComponent<Camera>();

                if (this.LeftEyeCamera.GetComponent<PostRender>() == null)
                {
                    this.LeftEyeCamera.gameObject.AddComponent<PostRender>();
                }
            }

            if (this.CenterEyeCamera == null)
            {
                this.CenterEyeCamera = this.CenterEyeTransform.GetComponent<Camera>();
            }

            if (this.RightEyeCamera == null)
            {
                this.RightEyeCamera = this.RightEyeTransform.GetComponent<Camera>();

                if (this.RightEyeCamera.GetComponent<PostRender>() == null)
                {
                    this.RightEyeCamera.gameObject.AddComponent<PostRender>();
                }
            }

            if (MiCamera.reticleLayerMask == -1)
            {
                MiCamera.reticleLayerMask = LayerMask.GetMask(new string[] { "CustomUI" });
            }
        }

        /// <summary>
        /// Resets the eye transform.
        /// </summary>
        /// <param name="eye">The eye.</param>
        /// <returns>the transform of the eye camera</returns>
        private Transform ResetEyeTransform(Eyes eye)
        {
            string eyeName = eye.ToString() + CEyeSuffix;
            Transform eyeTransform = this.transform.Find(eyeName);

            eyeTransform.name = eyeName;
            eyeTransform.parent = this.transform;
            eyeTransform.localScale = Vector3.one;
            eyeTransform.localPosition = Vector3.zero;
            eyeTransform.localRotation = Quaternion.identity;

            return eyeTransform;
        }

        /// <summary>
        /// Add stereo render in editor mode
        /// </summary>
        private void InitStereoRender()
        {
            if (!VrManager.Instance.TimeWarp)
            {
                ////    Find the parent object
                GameObject stereoRender = GameObject.Find("StereoRender");
                if (stereoRender == null)
                {
                    stereoRender = new GameObject("StereoRender");
                }

                ////    Add the pre pass - clear the screen black
                MiStereoPreRender preRender = UnityEngine.Object.FindObjectOfType<MiStereoPreRender>();
                if (preRender == null)
                {
                    GameObject go = new GameObject("StereoPreRender", typeof(MiStereoPreRender));
                    go.SendMessage("Reset");
                    go.transform.parent = stereoRender.transform;
                }

                ////    Add the post render - do the barrel distortion
                MiStereoPostRender postRender = UnityEngine.Object.FindObjectOfType<MiStereoPostRender>();
                if (postRender == null)
                {
                    GameObject go = new GameObject("StereoPostRender", typeof(MiStereoPostRender));
                    go.SendMessage("Reset");
                    go.transform.parent = stereoRender.transform;
                }
            }
        }

        /// <summary>
        /// Configures the camera.
        /// </summary>
        /// <param name="eye">The eye.</param>
        /// <returns>the camera relate to the eye</returns>
        private Camera ConfigureCamera(Eyes eye)
        {
            if (eye == Eyes.Center)
            {
                Camera centerEye = this.CenterEyeTransform.GetComponent<Camera>();

                // Clearing nothing to make sure the output image is not affected by this camera
                this.CenterEyeCamera.clearFlags = CameraClearFlags.Nothing;

                return centerEye;
            }

            Transform anchor = eye == Eyes.Left ? this.LeftEyeTransform : this.RightEyeTransform;
            Camera cam = anchor.GetComponent<Camera>();

#if UNITY_ANDROID && !UNITY_EDITOR
            if (VrManager.Instance.TimeWarp) {
                MiHMD.EyeParameter eyeDesc = VrManager.Instance.Hmd.GetEyeParameter(eye);

                cam.fieldOfView = eyeDesc.Fov.y;
                cam.aspect = eyeDesc.Resolution.x / eyeDesc.Resolution.y;
                cam.rect = new Rect(0f, 0f, 1.0f, 1.0f);
                cam.hdr = VrManager.Instance.IsHdrEnabled;
#if UNITY_5_6
                // current version is 5.6.*, not use shared depth buffer!
                cam.targetTexture = VrManager.Instance.Hmd.GetEyeSceneTexture(eye);
#else
                // current version is not 5.6.*, use shared depth buffer for each eye!
                cam.SetTargetBuffers(VrManager.Instance.Hmd.GetEyeSceneTexture(eye).colorBuffer,
                                     VrManager.Instance.Hmd.GetEyeSceneTextureDepthBuffer(eye));
#endif

                // Enforce camera render order
                cam.depth = (eye == Eyes.Left) ?
                        (int)PluginEvents.LeftEyeEndFrame :
                        (int)PluginEvents.RightEyeEndFrame;

                // AA is documented to have no effect in deferred, but it causes black screens.
                if (cam.actualRenderingPath == RenderingPath.DeferredLighting)
                {
                    QualitySettings.antiAliasing = 0;
                }
            }
#endif
            if (!VrManager.Instance.TimeWarp)
            {
                cam.fieldOfView = 97.5f;
                if (eye == Eyes.Left)
                {
                    cam.rect = new Rect(0.0f, 0.0f, 0.5f, 1.0f);
                }
                else
                {
                    cam.rect = new Rect(0.5f, 0.0f, 0.5f, 1.0f);
                }

                cam.targetTexture = MiEmulation.Instance.StereoRT;
                cam.hdr = VrManager.Instance.IsHdrEnabled;
                cam.depth = 0;
                if (cam.actualRenderingPath == RenderingPath.DeferredLighting ||
                    cam.actualRenderingPath == RenderingPath.DeferredShading)
                {
                    QualitySettings.antiAliasing = 0;
                }

                cam.enabled = true;
            }

            return cam;
        }
    }
}
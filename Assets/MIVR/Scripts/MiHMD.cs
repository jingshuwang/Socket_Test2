//-----------------------------------------------------------------------
// <copyright file="MiHMD.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Runtime.InteropServices;
    using UnityEngine;

    /// <summary>
    /// Selects a human eye.
    /// </summary>
    public enum Eyes
    {
        /// <summary>
        /// The center
        /// </summary>
        Center = -1,

        /// <summary>
        /// The left
        /// </summary>
        Left = 0,

        /// <summary>
        /// The right
        /// </summary>
        Right = 1,

        /// <summary>
        /// The count
        /// </summary>
        Count = 2
    }

    /// <summary>
    /// the head-mounted HMD
    /// </summary>
    public class MiHMD
    {
        /// <summary>
        /// The eye texture count.
        /// </summary>
        private const int EyeTextureCount = 3 * (int)Eyes.Count;

        /// <summary>
        /// The eye parameters
        /// </summary>
        private readonly EyeParameter[] eyeParameters = new EyeParameter[(int)Eyes.Count];

        /// <summary>
        /// The eye textures
        /// </summary>
        private readonly RenderTexture[] eyeTextures = new RenderTexture[EyeTextureCount];

        /// <summary>
        /// The eye texture ids
        /// </summary>
        private readonly int[] eyeTextureIds = new int[EyeTextureCount];

        /// <summary>
        /// The current eye texture index
        /// </summary>
        private int currentEyeTextureIdx = 0;

        /// <summary>
        /// The next eye texture index
        /// </summary>
        private int nextEyeTextureIdx = 0;

        /// <summary>
        /// The w axis
        /// </summary>
        private float w = 0.0f, x = 0.0f, y = 0.0f, z = 0.0f, fov = 90.0f;

        /// <summary>
        /// The w axis
        /// </summary>
        private float lx = 0.0f, ly = 0.0f, lz = 0.0f;

        /// <summary>
        /// Save for current head transform
        /// </summary>
        private MiTransform headTransform;

        /// <summary>
        /// Initializes a new instance of the <see cref="MiHMD"/> class.
        /// </summary>
        public MiHMD()
        {
            this.InitEyeParameters(Eyes.Left);
            this.InitEyeParameters(Eyes.Right);

            for (int i = 0; i < EyeTextureCount; i += 2)
            {
                this.InitEyeTexture(i, Eyes.Left);
                this.InitEyeTexture(i, Eyes.Right);
            }
        }

        /// <summary>
        /// Occurs when the head pose is reset.
        /// </summary>
        public event System.Action RecenteredHeadPosition;

        /// <summary>
        /// Gets a value indicating whether this instance is present.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is present; otherwise, <c>false</c>.
        /// </value>
        public bool IsPresent
        {
            get
            {
                return IsHMDPresent();
            }
        }

        /// <summary>
        /// Gets the current measured Latency values.
        /// </summary>
        public LatencyData Latency
        {
            get
            {
                return new LatencyData
                {
                    RenderDuration = 0.0f,
                    TimeWarpDuration = 0.0f,
                    PostDuration = 0.0f
                };
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void Update()
        {
            this.UpdateEyeTextureIndex();

            if (VrManager.Instance.TimeWarp) 
            {
                // Update HMD sensor data
                var index = 0;
                GetSensorData(
                  false,
                  ref this.w,
                  ref this.x,
                  ref this.y,
                  ref this.z,
                  ref this.lx,
                  ref this.ly,
                  ref this.lz,
                  ref this.fov,
                  ref index);
                VrManager.Instance.TimeWarpViewIndex = index;
                this.headTransform.Rotation = new Quaternion(-this.x, -this.y, this.z, this.w);
            }
        }

        /// <summary>
        /// Gets the head pose at the current time.
        /// </summary>
        /// <returns>the head transform</returns>
        public MiTransform GetHeadTransform()
        {
            return this.headTransform;
        }

        /// <summary>
        /// Gets the pose of the given eye, predicted for the time when the current frame will scan out.
        /// </summary>
        /// <param name="eye">The eye.</param>
        /// <returns>the eye transform</returns>
        public MiTransform GetEyeTransform(Eyes eye)
        {
            float eyeOffsetX = ((eye == Eyes.Left) ? -0.5f : 0.5f) * VrManager.Instance.Profile.Ipd;

            return new MiTransform
            {
                Position = new Vector3(eyeOffsetX, 0.0f, 0.0f) + this.headTransform.Position,
                Rotation = this.headTransform.Rotation,
            };
        }

        /// <summary>
        /// reset the header position to center.
        /// </summary>
        public void RecenterHeaderPosition()
        {
            ResetSensorOrientation();

            var handler = this.RecenteredHeadPosition;

            if (handler != null)
            {
                handler.Invoke();
            }
        }

        /// <summary>
        /// Gets the resolution and field of view for the given eye.
        /// </summary>
        /// <param name="eye">The eye.</param>
        /// <returns>the eye parameter</returns>
        public EyeParameter GetEyeParameter(Eyes eye)
        {
            return this.eyeParameters[(int)eye];
        }

        /// <summary>
        /// Gets the currently active render texture for the given eye.
        /// </summary>
        /// <param name="eye">The eye.</param>
        /// <returns>the eye's scene texture</returns>
        public RenderTexture GetEyeSceneTexture(Eyes eye)
        {
            return this.eyeTextures[this.currentEyeTextureIdx + ((int)eye)];
        }

        /// <summary>
        /// Gets the currently shared depth buffer for the given eye.
        /// </summary>
        /// <param name="eye">The eye.</param>
        /// <returns>the eye's depth texture</returns>
        public RenderBuffer GetEyeSceneTextureDepthBuffer(Eyes eye)
        {
            return this.eyeTextures[((int)eye)].depthBuffer;
        }

        /// <summary>
        /// Gets the currently active render texture's native ID for the given eye.
        /// </summary>
        /// <param name="eye">The eye.</param>
        /// <returns>the eye's scene texture id</returns>
        public int GetEyeSceneTextureId(Eyes eye)
        {
            return this.eyeTextureIds[this.currentEyeTextureIdx + ((int)eye)];
        }

        /// <summary>
        /// Resets the sensor orientation.
        /// </summary>
        /// <returns>true if success</returns>
        [DllImport(MiConfig.LibName)]
        private static extern bool ResetSensorOrientation();

        /// <summary>
        /// Determines whether [is HMD present].
        /// </summary>
        /// <returns>true if HMD is active</returns>
        [DllImport(MiConfig.LibName)]
        private static extern bool IsHMDPresent();

        /// <summary>
        /// Gets the camera position orientation.
        /// </summary>
        /// <param name="posX">The position X.</param>
        /// <param name="posY">The position Y.</param>
        /// <param name="posZ">The position Z.</param>
        /// <param name="rotationX">The rotationX.</param>
        /// <param name="rotationY">The rotationY.</param>
        /// <param name="rotationZ">The rotationZ.</param>
        /// <param name="rotationW">The rotationW.</param>
        /// <param name="curTime">At time.</param>
        /// <returns>true if success</returns>
        [DllImport(MiConfig.LibName)]
        private static extern bool GetCameraPositionOrientation(
            ref float posX,
            ref float posY,
            ref float posZ,
            ref float rotationX,
            ref float rotationY,
            ref float rotationZ,
            ref float rotationW,
            double curTime);

        /// <summary>
        /// Gets the sensor data.
        /// </summary>
        /// <param name="monoscopic">whether use single eye</param>
        /// <param name="w">The w.</param>
        /// <param name="rx">The rx.</param>
        /// <param name="ry">The y.</param>
        /// <param name="rz">The rz.</param>
        /// <param name="lx">The lx.</param>
        /// <param name="ly">The ly.</param>
        /// <param name="lz">The lz.</param>
        /// <param name="fov">The FOV.</param>
        /// <param name="viewNumber">The view number.</param>
        [DllImport(MiConfig.LibName)]
        private static extern void GetSensorData(
            bool monoscopic,
            ref float w,
            ref float rx,
            ref float ry,
            ref float rz,
            ref float lx,
            ref float ly,
            ref float lz,
            ref float fov,
            ref int viewNumber);

        /// <summary>
        /// Updates the index of the eye texture.
        /// </summary>
        private void UpdateEyeTextureIndex()
        {
            this.currentEyeTextureIdx = this.nextEyeTextureIdx;
            this.nextEyeTextureIdx = (this.nextEyeTextureIdx + 4) % EyeTextureCount;
        }

        /// <summary>
        /// Initializes the eye parameters.
        /// </summary>
        /// <param name="eye">The eye.</param>
        private void InitEyeParameters(Eyes eye)
        {
            Vector2 textureSize = new Vector2(Screen.width / 2, Screen.width / 2);

            // textureSize = new Vector2(1024, 1024);
            Vector2 fovSize = new Vector2(90, 90);

            this.eyeParameters[(int)eye] = new EyeParameter()
            {
                Resolution = textureSize,
                Fov = fovSize
            };

            this.headTransform.Position = new Vector3(
                0.0f,
                VrManager.Instance.Profile.EyeHeight - VrManager.Instance.Profile.NeckHeight,
                VrManager.Instance.Profile.EyeDepth);
        }

        /// <summary>
        /// Initializes the eye texture.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="eye">The eye.</param>
        private void InitEyeTexture(int index, Eyes eye)
        {
            EyeParameter eyeParameter = this.eyeParameters[(int)eye];

            // SceneTexture
            int eyeIndex = index + ((int)eye);

#if UNITY_5_6
        // it will occur native crash when using SetTargetBuffers function in 5.6.5
        int eyeDepth = (int)VrManager.Instance.EyeTextureDepth;
#else
            // use single depth buffer for each eye, create depth buffer only in first eye texture
            int eyeDepth = index == 0 ? (int)VrManager.Instance.EyeTextureDepth : 0;
#endif

            this.eyeTextures[eyeIndex] = new RenderTexture(
                (int)eyeParameter.Resolution.x,
                (int)eyeParameter.Resolution.y,
                eyeDepth,
                VrManager.Instance.EyeTextureFormat)
            {
                antiAliasing = (int)VrManager.Instance.EyeTextureAntiAliasing
            };
            this.eyeTextures[eyeIndex].Create();
            this.eyeTextureIds[eyeIndex] = this.eyeTextures[eyeIndex].GetNativeTexturePtr().ToInt32();
        }

        /// <summary>
        /// Specifies the size and field-of-view for one eye texture.
        /// </summary>
        public struct EyeParameter
        {
            /// <summary>
            /// The horizontal and vertical size of the texture.
            /// </summary>
            public Vector2 Resolution;

            /// <summary>
            /// The angle of the horizontal and vertical field of view in degrees.
            /// </summary>
            public Vector2 Fov;
        }

        /// <summary>
        /// Contains Latency measurements for a single frame of rendering.
        /// </summary>
        public struct LatencyData
        {
            /// <summary>
            /// Gets the time it took to render both eyes in seconds.
            /// </summary>
            public float RenderDuration;

            /// <summary>
            /// Gets the time it took to perform TimeWarpDuration in seconds.
            /// </summary>
            public float TimeWarpDuration;

            /// <summary>
            /// Gets the time between the end of TimeWarp and scan-out in seconds.
            /// </summary>
            public float PostDuration;
        }
    }
}

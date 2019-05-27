//-----------------------------------------------------------------------
// <copyright file="InputManager.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Collections;
    using System.Runtime.InteropServices;
    using UnityEngine;

    /// <summary>
    /// the input manager for get HMD and controller input
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// The controller button count
        /// </summary>
        private const int ControllerButtonCount = 3;

        /// <summary>
        /// The controller state interpolate frame
        /// </summary>
        private static int interpolateFrame = 4;

        /// <summary>
        /// The instance
        /// </summary>
        private static InputManager instance;

        /// <summary>
        /// Converted controller state
        /// </summary>
        private static ControllerState controllerState;

        /// <summary>
        /// Use oculus arm model
        /// </summary>
        private static bool useOCArmModel = true;

        /// <summary>
        /// The HMD state
        /// </summary>
        private HmdState hmdState;

//		/// <summary>
//		/// The native controller state
//		/// </summary>
//		private NativeControllerState nativeCtrlState;

        /// <summary>
        /// Frame Counting
        /// </summary>
        private int frameCount = 0;

        /// <summary>
        /// Gets a value indicating whether [HMD button up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [HMD button up]; otherwise, <c>false</c>.
        /// </value>
        public static bool HmdButtonUp
        {
            get { return instance != null && (instance.hmdState.ButtonUp == 0x01); }
        }

        /// <summary>
        /// Gets a value indicating whether [HMD button down].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [HMD button down]; otherwise, <c>false</c>.
        /// </value>
        public static bool HmdButtonDown
        {
            get { return instance != null && (instance.hmdState.ButtonDown == 0x01); }
        }

		//fixme
//		public static bool AppButtonDown
//		{
//			get { return instance != null && (instance.hmdState.AppButtonDown == 0x01); }
//		}

        /// <summary>
        /// Gets the state of the controller.
        /// </summary>
        /// <value>
        /// The state of the controller.
        /// </value>
        public static ControllerState ControllerState
        {
            get
            {
                return controllerState;
            }
        }

		/// <summary>
		/// Gets a value indicating whether [app button down].
		/// </summary>
		/// <value>
		///   <c>true</c> if [app button down]; otherwise, <c>false</c>.
		/// </value>
//		public static bool TriggerButtonDown
//		{
//			get { return instance != null && (instance.nativeCtrlState.AppButtonDown == 0x01); }
//		}

        /// <summary>
        /// Reads the state of the HMD
        /// </summary>
        /// <param name="state">The state.</param>
        [DllImport(MiConfig.LibName)]
        private static extern void ReadHMDState(ref HmdState state);

        /// <summary>
        /// Reads the state of the controller.
        /// </summary>
        /// <param name="state">The state.</param>
        [DllImport(MiConfig.LibName)]
        private static extern void ReadControllerStateForUnity(ref NativeControllerState state);

        /// <summary>
        /// Convert the native controller state.
        /// </summary>
        /// <param name="nativeState">State of the native.</param>
        private static void ConvertState(ref NativeControllerState nativeState)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            switch (nativeState.ConnectionState)
            {
                case 0:
                    controllerState.ConnectionState = ConnectionState.Disconnected;
                    break;
                case 1:
                    controllerState.ConnectionState = ConnectionState.Connected;
                    break;
                case 2:
                    controllerState.ConnectionState = ConnectionState.Connecting;
                    break;
                default:
                    controllerState.ConnectionState = ConnectionState.Disconnected;
                    break;
            }

            controllerState.IsTouching = nativeState.IsTouching == 0x01;
            controllerState.TouchDown = nativeState.TouchDown == 0x01;
            controllerState.TouchUp = nativeState.TouchUp == 0x01;
            controllerState.TouchPosition = new Vector2(nativeState.TouchPos.X, nativeState.TouchPos.Y);
            controllerState.Orientation = new Quaternion(nativeState.Orientation.X, nativeState.Orientation.Y, nativeState.Orientation.Z, nativeState.Orientation.W);
            
            if(useOCArmModel)
            {
                // use oculus' arm model
                controllerState.Position = new Vector3(nativeState.Position.X, nativeState.Position.Y, nativeState.Position.Z);
            }
            else
            {
                // use our arm model
                controllerState.Position = ArmModel.Instance.WristPosition;
            }

            controllerState.ClickButtonState = nativeState.ClickButtonState == 0x01;
            controllerState.ClickButtonDown = nativeState.ClickButtonDown == 0x01;
            controllerState.ClickButtonUp = nativeState.ClickButtonUp == 0x01;
            controllerState.AppButtonState = nativeState.AppButtonState == 0x01;
            controllerState.AppButtonDown = nativeState.AppButtonDown == 0x01;
            controllerState.AppButtonUp = nativeState.AppButtonUp == 0x01;
            controllerState.MenuButtonState = nativeState.MenuButtonState == 0x01;
            controllerState.MenuButtonDown = nativeState.MenuButtonDown == 0x01;
            controllerState.MenuButtonUp = nativeState.MenuButtonUp == 0x01;
            controllerState.Recentered = nativeState.Recentered == 0x01;
            controllerState.BatteryPercentRemaining = (int)nativeState.BatteryPercentRemaining;
#endif
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        private void Start()
        {
            controllerState = new ControllerState();
            if (VrManager.Instance.Model == VrManager.DeviceModel.MIVR)
            {
                useOCArmModel = false;
                ArmModel.Instance.OnStart();
            }
        }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("More than one InputMnager instance was found in your scene. ");
                this.enabled = false;
                return;
            }

            instance = this;
            this.hmdState.ButtonUp = 0x0;
        }

        /// <summary>
        /// Called when [destroy].
        /// </summary>
        private void OnDestroy()
        {
            instance = null;
        }

        /// <summary>
        /// Updates the controller.
        /// </summary>
        private void UpdateStates()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            ReadHMDState(ref this.hmdState);
            NativeControllerState nativeControllerState = new NativeControllerState();
            ReadControllerStateForUnity(ref nativeControllerState);
            ConvertState(ref nativeControllerState);

            if(!useOCArmModel && ArmModel.Instance != null)
            {
                ArmModel.Instance.OnControllerUpdate();
            }
#endif
        }

        /// <summary>
        /// Called when [enable].
        /// </summary>
        private void OnEnable()
        {
            this.UpdateStates();
        }

        /*
        /// <summary>
        /// Called when [disable].
        /// </summary>
        private void OnDisable()
        {
            this.StopCoroutine("UpdateStatesEndOfFrame");
        }

        /// <summary>
        /// Update states
        /// </summary>
        /// <returns>the enumerator</returns>
        private IEnumerator UpdateStatesEndOfFrame()
        {
            while (true)
            {
                this.UpdateStates();
                yield return new WaitForEndOfFrame();
                this.frameCount++;
            }
        }*/

        /// <summary>
        /// Update tick
        /// </summary>
        private void Update()
        {
            // Debug.Log("InputManager.Update: " + frameCount);
            this.UpdateStates();
            this.frameCount++;
        }

        /// <summary>
        /// The HMD state
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct HmdState
        {
            /// <summary>
            /// The button up
            /// </summary>
            public byte ButtonUp;

            /// <summary>
            /// The button down
            /// </summary>
            public byte ButtonDown;
        }

        /// <summary>
        /// the QUAT
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct Quatf
        {
            /// <summary>
            /// The X value
            /// </summary>
            internal float X;

            /// <summary>
            /// The Y Value
            /// </summary>
            internal float Y;

            /// <summary>
            /// The Z value
            /// </summary>
            internal float Z;

            /// <summary>
            /// The W value
            /// </summary>
            internal float W;
        }

        /// <summary>
        /// the vector 2f
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct Vector2f
        {
            /// <summary>
            /// the x
            /// </summary>
            internal float X;

            /// <summary>
            /// The y
            /// </summary>
            internal float Y;
        }

        /// <summary>
        /// the vector 2f
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct Vector3f
        {
            /// <summary>
            /// the x
            /// </summary>
            internal float X;

            /// <summary>
            /// The y
            /// </summary>
            internal float Y;

            /// <summary>
            /// The y
            /// </summary>
            internal float Z;
        }

        /// <summary>
        /// The controller state
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct NativeControllerState
        {
            /// <summary>
            /// The click button state
            /// </summary>
            public byte ClickButtonState;

            /// <summary>
            /// The app button state
            /// </summary>
            public byte AppButtonState;

            /// <summary>
            /// The menu button state
            /// </summary>
            public byte MenuButtonState;

            /// <summary>
            /// The click button down
            /// </summary>
            public byte ClickButtonDown;

            /// <summary>
            /// The app button down
            /// </summary>
            public byte AppButtonDown;

            /// <summary>
            /// The menu button down
            /// </summary>
            public byte MenuButtonDown;

            /// <summary>
            /// The click button up
            /// </summary>
            public byte ClickButtonUp;

            /// <summary>
            /// The app button up
            /// </summary>
            public byte AppButtonUp;

            /// <summary>
            /// The menu button up
            /// </summary>
            public byte MenuButtonUp;

            /// <summary>
            /// whether is touching
            /// </summary>
            public byte IsTouching;

            /// <summary>
            /// The touch down
            /// </summary>
            public byte TouchDown;

            /// <summary>
            /// The touch up
            /// </summary>
            public byte TouchUp;

            /// <summary>
            /// The touch position
            /// </summary>
            public Vector2f TouchPos;

            /// <summary>
            /// The orientation
            /// </summary>
            public Quatf Orientation;

            /// <summary>
            /// The Postion
            /// </summary>
            public Vector3f Position;

            /// <summary>
            /// The connection state
            /// </summary>
            public int ConnectionState;

            /// <summary>
            /// Whether the rotation is centered.
            /// </summary>
            public byte Recentered;

            /// <summary>
            /// The battery percent remaining
            /// </summary>
            public byte BatteryPercentRemaining;
        }
    }
}
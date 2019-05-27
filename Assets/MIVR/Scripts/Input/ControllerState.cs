//-----------------------------------------------------------------------
// <copyright file="ControllerState.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Runtime.InteropServices;
    using UnityEngine;
    using UnityEngineInternal;

    /// <summary>
    /// the controller connection state
    /// </summary>
    public enum ConnectionState
    {
        /// <summary>
        /// controller is disconnected.
        /// </summary>
        Disconnected,

        /// <summary>
        /// device is scanning the controller.
        /// </summary>
        Scanning,

        /// <summary>
        /// The device is connecting the controller.
        /// </summary>
        Connecting,

        /// <summary>
        /// The controller is connected.
        /// </summary>
        Connected,

        /// <summary>
        /// The error
        /// </summary>
        Error
    }

    /// <summary>
    /// the controller states
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ControllerState
    {
        /// <summary>
        /// Gets or sets the state of the connection.
        /// </summary>
        /// <value>
        /// The state of the connection.
        /// </value>
        public ConnectionState ConnectionState;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is touching.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is touching; otherwise, <c>false</c>.
        /// </value>
        public bool IsTouching;

        /// <summary>
        /// Gets or sets a value indicating whether [touch down].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [touch down]; otherwise, <c>false</c>.
        /// </value>
        public bool TouchDown;

        /// <summary>
        /// Gets or sets a value indicating whether [touch up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [touch up]; otherwise, <c>false</c>.
        /// </value>
        public bool TouchUp;

        /// <summary>
        /// Gets or sets the touch position.
        /// </summary>
        /// <value>
        /// The touch position.
        /// </value>
        public Vector2 TouchPosition;

        /// <summary>
        /// Gets or sets the orientation.
        /// </summary>
        /// <value>
        /// The orientation.
        /// </value>
        public Quaternion Orientation;

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>
        /// The position.
        /// </value>
        public Vector3 Position;

        /// <summary>
        /// Gets or sets a value indicating whether [click button state].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [click button state]; otherwise, <c>false</c>.
        /// </value>
        public bool ClickButtonState;

        /// <summary>
        /// Gets or sets a value indicating whether [click button down].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [click button down]; otherwise, <c>false</c>.
        /// </value>
        public bool ClickButtonDown;

        /// <summary>
        /// Gets or sets a value indicating whether [click button up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [click button up]; otherwise, <c>false</c>.
        /// </value>
        public bool ClickButtonUp;

        /// <summary>
        /// Gets or sets a value indicating whether [application button state].
        /// </summary>
        /// <value>
        /// <c>true</c> if [application button state]; otherwise, <c>false</c>.
        /// </value>
        public bool AppButtonState;

        /// <summary>
        /// Gets or sets a value indicating whether [application button down].
        /// </summary>
        /// <value>
        /// <c>true</c> if [application button down]; otherwise, <c>false</c>.
        /// </value>
        public bool AppButtonDown;

        /// <summary>
        /// Gets or sets a value indicating whether [application button up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [application button up]; otherwise, <c>false</c>.
        /// </value>
        public bool AppButtonUp;

        /// <summary>
        /// Gets or sets a value indicating whether [menu button state].
        /// </summary>
        /// <value>
        /// <c>true</c> if [menu button state]; otherwise, <c>false</c>.
        /// </value>
        public bool MenuButtonState;

        /// <summary>
        /// Gets or sets a value indicating whether [menu button down].
        /// </summary>
        /// <value>
        /// <c>true</c> if [menu button down]; otherwise, <c>false</c>.
        /// </value>
        public bool MenuButtonDown;

        /// <summary>
        /// Gets or sets a value indicating whether [menu button up].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [menu button up]; otherwise, <c>false</c>.
        /// </value>
        public bool MenuButtonUp;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ControllerState"/> is recentered.
        /// </summary>
        /// <value>
        ///   <c>true</c> if recentered; otherwise, <c>false</c>.
        /// </value>
        public bool Recentered;

        /// <summary>
        /// The battery percent remaining.
        /// 0-100
        /// </summary>
        /// <value>
        /// The battery percent remaining.
        /// </value>
        public int BatteryPercentRemaining;
    }
}
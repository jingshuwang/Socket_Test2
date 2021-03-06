﻿//-----------------------------------------------------------------------
// <copyright file="MiStereoPreRender.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Used in editor mode to realize the stereo rendering - pre render
    /// </summary>
    [RequireComponent(typeof(Camera))]
    [SuppressMessage("StyleCop.CSharp.NamingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.LayoutRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.SpacingRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "Reviewed. Suppression is OK here.")]
    public class MiStereoPreRender : MonoBehaviour
    {
        /// <summary>
        /// Gets the camera which is the component to the current GameObject.
        /// </summary>
        /// <value>
        /// The camera which is the component of the current GameObject.
        /// </value>
        public Camera cam { get; private set; }

        /// <summary>
        /// Awakes this instance.
        /// </summary>
        private void Awake()
        {
            cam = GetComponent<Camera>();
        }

        /// <summary>
        /// Reset the camera's status
        /// </summary>
        private void Reset()
        {
            //    The member variable 'cam' could not always be initialized when this method is called in edtior mode, so check it here. 
            if (cam == null)
            {
                cam = GetComponent<Camera>();
            }

            cam.backgroundColor = Color.black;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.cullingMask = 0;
            cam.useOcclusionCulling = false;
            //    -100 is a very small integer to make use this cam render before other cameras
            cam.depth = -100;
        }
    }
}
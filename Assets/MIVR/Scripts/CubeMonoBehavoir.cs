//-----------------------------------------------------------------------
// <copyright file="CubeMonoBehavoir.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.Rendering;

    /// <summary>
    /// A MonoBehaviour for testing CommandBuffer
    /// </summary>
    public class CubeMonoBehavoir : MonoBehaviour
    {
        /// <summary>
        /// CommandBuffer instance
        /// </summary>
        private CommandBuffer buf = null;

        /// <summary>
        /// before rendering this GO
        /// </summary>
        private void OnWillRenderObject()
        {
            VrManager vrManager = gameObject.transform.parent.gameObject.GetComponent<VrManager>();

            if (this.buf == null)
            {
                Debug.Log("cube OnWillRenderObject");
                this.buf = new CommandBuffer();
                this.buf.name = "VR command";
                vrManager.DoTimeWarpWithCommandBuffer(vrManager.TimeWarpViewIndex, this.buf);
                Camera.current.AddCommandBuffer(CameraEvent.AfterEverything, this.buf);
            }
        }
    }
}
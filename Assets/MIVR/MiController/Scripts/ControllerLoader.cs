//-----------------------------------------------------------------------
// <copyright file="ControllerLoader.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// ControllerLoader for v1 or v1o
    /// </summary>
    public class ControllerLoader : MonoBehaviour
    {
        /// <summary>
        /// Mi controller v1.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Unity need this.")]
        public GameObject ControllerV1;

        /// <summary>
        /// Mi controller v1o.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Unity need this.")]
        public GameObject ControllerV1o;

        /// <summary>
        /// controller gameobject.
        /// </summary>
        private GameObject controller = null;

        /// <summary>
        /// Awake function.
        /// </summary>
        private void Awake()
        {
            if (this.controller == null)
            {
                if (VrManager.Instance.Model == VrManager.DeviceModel.MIVRAllInOne ? true : false)
                {
                    this.controller = MonoBehaviour.Instantiate(this.ControllerV1o);
                }
                else
                {
                    this.controller = MonoBehaviour.Instantiate(this.ControllerV1);
                }

                this.controller.transform.parent = this.transform;
            }
        }
    }
}
//-----------------------------------------------------------------------
// <copyright file="BasePointerRaycaster.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;
    using UnityEngine.EventSystems;

    /// <summary>
    /// Base class of CustomRaycaster
    /// </summary>
    public abstract class BasePointerRaycaster : BaseRaycaster
    {
        /// <summary>
        /// The last ray used.
        /// </summary>
        private Ray lastRay;

        /// <summary>
        /// Initializes a new instance of the BasePointerRaycaster class.
        /// </summary>
        protected BasePointerRaycaster()
        {
        }

        /// <summary>
        /// Gets max distance for ray to cast
        /// </summary>
        public float MaxPointerDistance
        {
            get { return 20.0f; }
        }

        /// <summary>
        /// Gets radius of Pointer
        /// </summary>
        public float PointerRadius
        {
            get { return 0.0f; }
        }

        /// <summary>
        /// returns whether point is available
        /// </summary>
        /// <returns> IsPointerAvailable </returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed.")]
        public bool IsPointerAvailable()
        {
            return true;
        }

        /// <summary>
        /// Get the last ray
        /// </summary>
        /// <returns>last ray</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed.")]
        public Ray GetLastRay()
        {
            return this.lastRay;
        }

        /// <summary>
        /// get the ray
        /// </summary>
        /// <returns>the ray finded</returns>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1630:DocumentationTextMustContainWhitespace", Justification = "Reviewed.")]
        protected Ray GetRay()
        {
            if (LaserPointer.Instance == null)
            {
                return this.lastRay;
            }

            Transform pointerTransform = LaserPointer.Instance.transform;
            this.lastRay = new Ray(pointerTransform.position, pointerTransform.forward);

            return this.lastRay;
        }
    }
}
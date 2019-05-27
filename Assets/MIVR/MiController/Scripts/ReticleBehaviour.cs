//-----------------------------------------------------------------------
// <copyright file="ReticleBehaviour.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System.Diagnostics.CodeAnalysis;
    using UnityEngine;

    /// <summary>
    /// Reticle behaviour
    /// </summary>
    public class ReticleBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Reticle normal color
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed.")]
        [SerializeField]
        public Color NormalColor = new Color(1, 1, 1, 1);

        /// <summary>
        /// Reticle renderer
        /// </summary>
        private Renderer reticleRenderer;

        /// <summary>
        /// material Property Block
        /// </summary>
        private MaterialPropertyBlock materialPropertyBlock;

        /// <summary>
        /// color ID
        /// </summary>
        private int colorID;

        /// <summary>
        /// conormal Color
        /// </summary>
        private Color normalColor = new Color(1, 1, 1, 1);

        /// <summary>
        /// reticle Status ID
        /// </summary>
        private int reticleStatusID;

        /// <summary>
        /// reticle Status
        /// </summary>
        private Vector4 reticleStatus;

        /// <summary>
        /// pointer Enter
        /// </summary>
        private bool pointerEnter = false;

        /// <summary>
        /// Gets or sets a value indicating whether Pointer enter
        /// </summary>
        public bool PointerEnter
        {
            get
            {
                return this.pointerEnter;
            }

            set
            {
                this.pointerEnter = value;
            }
        }

        /// <summary>
        /// Initialize the reticle
        /// </summary>
        private void Initialize()
        {
            if (this.reticleRenderer == null)
            {
                this.reticleRenderer = this.GetComponent<Renderer>();
            }

            if (this.materialPropertyBlock == null)
            {
                this.materialPropertyBlock = new MaterialPropertyBlock();
            }

            this.colorID = Shader.PropertyToID("_Color");
            this.reticleStatusID = Shader.PropertyToID("_ReticleStatus");
            this.normalColor = this.NormalColor;
            this.reticleStatus = new Vector4(1.0f, 0.0f, 0.0f, 0.0f);
            this.materialPropertyBlock.SetColor(this.colorID, this.normalColor);
            this.materialPropertyBlock.SetVector(this.reticleStatusID, this.reticleStatus);
            this.reticleRenderer.SetPropertyBlock(this.materialPropertyBlock);
        }

        /// <summary>
        /// Use this for initialization
        /// </summary>
        private void Start()
        {
            this.Initialize();
        }

        /// <summary>
        /// Update is called once per frame
        /// </summary>
        private void Update()
        {
            // hovering else normal
            if (this.pointerEnter)
            {
                this.reticleStatus.z = 1.0f;
                this.materialPropertyBlock.SetVector(this.reticleStatusID, this.reticleStatus);
            }
            else
            {
                this.reticleStatus.z = 0.0f;
                this.materialPropertyBlock.SetVector(this.reticleStatusID, this.reticleStatus);
            }

            this.reticleRenderer.SetPropertyBlock(this.materialPropertyBlock);
        }
    }
}
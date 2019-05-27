//-----------------------------------------------------------------------
// <copyright file="OrderInfo.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using UnityEngine;

    /// <summary>
    /// The order info
    /// </summary>
    public class OrderInfo
    {
        /// <summary>
        /// Gets or sets the order identifier.
        /// </summary>
        /// <value>
        /// The order identifier.
        /// </value>
        public string OrderId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the application identifier.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public string AppId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the application key.
        /// </summary>
        /// <value>
        /// The application key.
        /// </value>
        public string AppKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the open identifier.
        /// </summary>
        /// <value>
        /// The open identifier.
        /// </value>
        public string OpenId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the local created time.
        /// </summary>
        /// <value>
        /// The local created time.
        /// </value>
        public long LocalCreatedTime
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public int Quantity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the currency.
        /// </summary>
        /// <value>
        /// The type of the currency.
        /// </value>
        public string CurrencyType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the name of the product.
        /// </summary>
        /// <value>
        /// The name of the product.
        /// </value>
        public string ProductName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the extra data.
        /// </summary>
        /// <value>
        /// The extra data.
        /// </value>
        public string ExtraData
        {
            get;
            set;
        }
    }
}

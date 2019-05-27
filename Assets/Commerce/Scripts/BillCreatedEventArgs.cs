//-----------------------------------------------------------------------
// <copyright file="BillCreatedEventArgs.cs" company="XiaoMi Corporation">
//     All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace MIVR
{
    using System;

    /// <summary>
    /// The bill created event args
    /// </summary>
    public class BillCreatedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// 102: Success.
        /// 100: Create bill failed.
        /// 101: Cancelled by user.
        /// 103: Pay failed.
        /// 104: Unknown error
        /// </value>
        public ErrorCode Code
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the order information.
        /// </summary>
        /// <value>
        /// The order information.
        /// </value>
        public OrderInfo OrderInfo
        {
            get;
            set;
        }
    }
}

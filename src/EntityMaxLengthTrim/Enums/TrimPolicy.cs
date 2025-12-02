// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2025-12-02 19:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-02 19:36
// ***********************************************************************
//  <copyright file="TrimPolicy.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace EntityMaxLengthTrim.Enums
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Values that represent trim policies.
    /// </summary>
    /// =================================================================================================
    public enum TrimPolicy
    {
        /// <summary>
        ///     Perform trimming silently. Default.
        /// </summary>
        Silent,
        
        /// <summary>
        ///     Invoke logger and trim.
        /// </summary>
        Warn,
        
        /// <summary>
        ///     Throw exception instead of trimming.
        /// </summary>
        Throw,
        
        /// <summary>
        ///     Report but do not modify.
        /// </summary>
        ReportOnly
    }
}
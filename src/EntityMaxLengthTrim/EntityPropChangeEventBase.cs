// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.EntityMaxLengthTrim
//  Author           : RzR
//  Created On       : 2023-12-15 14:32
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-12-16 15:12
// ***********************************************************************
//  <copyright file="EntityPropChangeEventBase.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.ComponentModel;
using EntityMaxLengthTrim.Interceptors;

#endregion

namespace EntityMaxLengthTrim
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>An entity property change event base.</summary>
    /// <remarks></remarks>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// =================================================================================================
    public class EntityPropChangeEventBase : INotifyPropertyChanged
    {
        /// <summary>
        ///     Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Property changed event
        /// </summary>
        /// <param name="callingEntity">Calling entity</param>
        /// <param name="propertyName">Changed property name</param>
        protected virtual void OnPropertyChanged<T>(T callingEntity, string propertyName)
        {
            StringInterceptor.ApplyStringMaxAllowedLength(callingEntity, propertyName, false);
            PropertyChanged?.Invoke(callingEntity, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        ///     Set content to property
        /// </summary>
        /// <param name="callingEntity">Calling entity</param>
        /// <param name="propertyName">Changed property name</param>
        /// <param name="getValue">Get property value</param>
        /// <param name="setValue">Set property value</param>
        /// <typeparam name="T">Calling entity type</typeparam>
        /// <returns></returns>
        protected virtual void SetContent<T>(T callingEntity, string propertyName, ref string getValue,
            ref string setValue)
        {
            if (getValue == setValue) return;

            getValue = setValue;
            StringInterceptor.ApplyStringMaxAllowedLength(callingEntity, propertyName, false);
            PropertyChanged?.Invoke(callingEntity, new PropertyChangedEventArgs(propertyName));
        }
    }
}
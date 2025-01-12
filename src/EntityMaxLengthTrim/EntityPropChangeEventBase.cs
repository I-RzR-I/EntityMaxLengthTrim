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
using EntityMaxLengthTrim.Extensions;
using EntityMaxLengthTrim.Interceptors;

#endregion

namespace EntityMaxLengthTrim
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>An entity property change event base.</summary>
    /// <remarks></remarks>
    /// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
    /// =================================================================================================
    public abstract class EntityPropChangeEventBase : INotifyPropertyChanged
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
        /// <typeparam name="TEntity">Calling entity type</typeparam>
        protected virtual void OnPropertyChanged<TEntity>(TEntity callingEntity, string propertyName)
            where TEntity : class
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
        /// <typeparam name="TEntity">Calling entity type</typeparam>
        /// <returns></returns>
        protected virtual void SetContent<TEntity>(TEntity callingEntity, string propertyName, ref string getValue,
            ref string setValue)
            where TEntity : class
        {
            if (getValue == setValue) return;

            getValue = setValue;
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
        /// <param name="length">Property maximum allowed length.</param>
        /// <typeparam name="TEntity">Calling entity type</typeparam>
        /// <returns></returns>
        protected virtual void SetContent<TEntity>(TEntity callingEntity, string propertyName, ref string getValue,
            ref string setValue, int length)
            where TEntity : class
        {
            if (getValue == setValue) return;

            getValue = setValue.Truncate(length);
            PropertyChanged?.Invoke(callingEntity, new PropertyChangedEventArgs(propertyName));
        }
    }
}
// ***********************************************************************
//  Assembly         : RzR.Shared.Entity
//  Author           : RzR
//  Created On       : 2022-08-01 13:55
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-03 18:53
// ***********************************************************************
//  <copyright file="GeneralAssemblyInfo.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Reflection;
using System.Resources;

#endregion

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("RzR ®")]
[assembly: AssemblyProduct("")]
[assembly: AssemblyCopyright("Copyright © 2022 RzR All rights reserved.")]
[assembly: AssemblyTrademark("® RzR™")]
[assembly: AssemblyDescription("One important thing about this repository, you have the possibility to avoid database exceptions related to exceeding the limit of the maximum allowed length of the string type columns. To specify the maximum allowed string length you can use data annotation attributes predefined in `System.ComponentModel.DataAnnotations` or a new custom attribute.")]

[assembly: AssemblyMetadata("TermsOfService", "")]

[assembly: AssemblyMetadata("ContactUrl", "")]
[assembly: AssemblyMetadata("ContactName", "RzR")]
[assembly: AssemblyMetadata("ContactEmail", "ddpRzR@hotmail.com")]

[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]

[assembly: AssemblyVersion("1.0.1.0644")]
[assembly: AssemblyFileVersion("1.0.1.0644")]
[assembly: AssemblyInformationalVersion("1.0.1.0644")]
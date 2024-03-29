﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Entity.ConsoleFramework45Test
//  Author           : RzR
//  Created On       : 2023-01-05 09:51
// 
//  Last Modified By : RzR
//  Last Modified On : 2023-01-05 09:59
// ***********************************************************************
//  <copyright file="Program.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using ConsoleFramework45Test.Models;
using EntityMaxLengthTrim.Interceptors;
using EntityMaxLengthTrim.Options;

#endregion

namespace ConsoleFramework45Test
{
    internal class Program
    {
        private static void Main()
        {
            var newParsedData = StringInterceptor.ApplyStringMaxAllowedLength(new Foo
            {
                Name = "Here should be the new name of FOO",
                CreatedOn = DateTime.MaxValue,
                Id = 1,
                FullName = string.Empty,
                Description = null
            });

            var newParsedDataWithDots = StringInterceptor.ApplyStringMaxAllowedLength(new Foo
            {
                Name = "Here should be the new name of FOO",
                CreatedOn = DateTime.MaxValue,
                Id = 1,
                FullName = string.Empty,
                Description = null
            }, true);

            var newParsedDataByList = StringInterceptor.ApplyStringMaxAllowedLength(new Foo
            {
                Name = "Here should be the new name of FOO",
                CreatedOn = DateTime.MaxValue,
                Id = 1,
                FullName = string.Empty,
                Description = null
            }, new List<string> {nameof(Foo.Name)});

            var newParsedDataByListWithFilter1 = StringInterceptor.ApplyStringMaxAllowedLength(new Foo
            {
                Name = "Here should be the new name of FOO",
                CreatedOn = DateTime.MaxValue,
                Id = 1,
                FullName = string.Empty,
                Description =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
            }, new List<string> {nameof(Foo.Name)}, true);

            var newParsedDataByListWithFilter2 = StringInterceptor.ApplyStringMaxAllowedLength(new Foo
            {
                Name = "Here should be the new name of FOO",
                CreatedOn = DateTime.MaxValue,
                Id = 1,
                FullName =
                    "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                Description =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
            }, new List<PropertyOption>
            {
                new PropertyOption {Name = nameof(Foo.Name), UseDots = false},
                new PropertyOption {Name = nameof(Foo.Description), UseDots = true}
            });

            var newParsedDataByListWithFilter3 = StringInterceptor.ApplyStringMaxAllowedLength(new Foo
            {
                Name = "Here should be the new name of FOO",
                CreatedOn = DateTime.MaxValue,
                Id = 1,
                FullName =
                    "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                Description =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
            }, new List<PropertyOption>
            {
                new PropertyOption {Name = nameof(Foo.Name), UseDots = false},
                new PropertyOption {Name = nameof(Foo.Description), UseDots = true}
            }, true);

            var newParsedDataByPropertyName = StringInterceptor.ApplyStringMaxAllowedLength(new Foo
            {
                Name = "Here should be the new name of FOO",
                CreatedOn = DateTime.MaxValue,
                Id = 1,
                FullName =
                    "It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).",
                Description =
                    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."
            }, "FullName");

            Console.ReadKey();
        }
    }
}
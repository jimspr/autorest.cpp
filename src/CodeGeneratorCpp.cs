// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoRest.Core;
using AutoRest.Core.Model;
using AutoRest.Core.Logging;
using AutoRest.Core.Utilities;
using AutoRest.Cpp.Model;
using AutoRest.Cpp.Templates.Rest.Client;
using AutoRest.Cpp.Templates.Rest.Common;
using static AutoRest.Core.Utilities.DependencyInjection;
using AutoRest.Extensions;

namespace AutoRest.Cpp
{
    public class CodeGeneratorCpp : CodeGenerator
    {
        private const string ClientRuntimePackage = "Microsoft.Rest.ClientRuntime.2.2.0";

        public override string UsageInstructions => string.Format(CultureInfo.InvariantCulture,
            Properties.Resources.UsageInformation, ClientRuntimePackage);

        public override string ImplementationFileExtension => ".h";

        private async Task GenerateServerSideCode(CodeModelCpp codeModel)
        {
            foreach (string methodGrp in codeModel.MethodGroupNames)
            {
                using (NewContext)
                {
                    codeModel.Name = methodGrp;
                    // Service server
                    var serviceControllerTemplate = new AutoRest.Cpp.Templates.Rest.Server.ServiceControllerTemplate { Model = codeModel };
                    await Write(serviceControllerTemplate, $"{codeModel.Name}{ImplementationFileExtension}");
                }
            }
            // Models
            foreach (CompositeTypeCpp model in codeModel.ModelTypes.Union(codeModel.HeaderTypes))
            {
                var modelTemplate = new ModelTemplate { Model = model };
                await Write(modelTemplate, Path.Combine(Settings.Instance.ModelsName, $"{model.Name}{ImplementationFileExtension}"));
            }
        }

        private async Task GenerateClientSideCode(CodeModelCpp codeModel)
        {
            // Service client
            var serviceClientTemplate = new ServiceClientTemplate { Model = codeModel };
            await Write(serviceClientTemplate, $"{codeModel.Name}{ImplementationFileExtension}");

            // Service client interface
            var serviceClientInterfaceTemplate = new ServiceClientImplTemplate { Model = codeModel };
            await Write(serviceClientInterfaceTemplate, $"{codeModel.Name}.cpp");

            // operations
            foreach (MethodGroupCpp methodGroup in codeModel.Operations)
            {
                if (!methodGroup.Name.IsNullOrEmpty())
                {
                    // Operation
                    var operationsTemplate = new MethodGroupTemplate { Model = methodGroup };
                    await Write(operationsTemplate, $"{operationsTemplate.Model.TypeName}{ImplementationFileExtension}");

                    // Operation interface
                    var operationsInterfaceTemplate = new MethodGroupImplTemplate { Model = methodGroup };
                    await Write(operationsInterfaceTemplate, $"{operationsInterfaceTemplate.Model.TypeName}.cpp");
                }

                var operationExtensionsTemplate = new ExtensionsTemplate { Model = methodGroup };
                await Write(operationExtensionsTemplate, $"{methodGroup.ExtensionTypeName}Extensions{ImplementationFileExtension}");
            }

            // Models
            foreach (CompositeTypeCpp model in codeModel.ModelTypes.Union(codeModel.HeaderTypes))
            {
                if (model.Extensions.ContainsKey(SwaggerExtensions.ExternalExtension) &&
                    (bool)model.Extensions[SwaggerExtensions.ExternalExtension])
                {
                    continue;
                }

                var modelTemplate = new ModelTemplate { Model = model };
                await Write(modelTemplate, Path.Combine(Settings.Instance.ModelsName, $"{model.Name}{ImplementationFileExtension}"));

                var modelImplTemplate = new ModelImplTemplate { Model = model };
                await Write(modelImplTemplate, Path.Combine(Settings.Instance.ModelsName, $"{model.Name}.cpp"));
            }

            // Enums
            foreach (EnumTypeCpp enumType in codeModel.EnumTypes)
            {
                var enumTemplate = new EnumTemplate { Model = enumType };
                await Write(enumTemplate, Path.Combine(Settings.Instance.ModelsName, $"{enumTemplate.Model.Name}{ImplementationFileExtension}"));
            }

            // Exceptions
            foreach (CompositeTypeCpp exceptionType in codeModel.ErrorTypes)
            {
                var exceptionTemplate = new ExceptionTemplate { Model = exceptionType, };
                await Write(exceptionTemplate, Path.Combine(Settings.Instance.ModelsName, $"{exceptionTemplate.Model.ExceptionTypeDefinitionName}{ImplementationFileExtension}"));
            }

            // Xml Serialization
            if (codeModel.ShouldGenerateXmlSerialization)
            {
                var xmlSerializationTemplate = new XmlSerializationTemplate();
                await Write(xmlSerializationTemplate, Path.Combine(Settings.Instance.ModelsName, $"{XmlSerialization.XmlDeserializationClass}{ImplementationFileExtension}"));
            }

            // write all headers
            var allTemplate = new AllHeadersTemplate {Model = codeModel};
            await Write(allTemplate, Path.Combine(Settings.Instance.ModelsName, $"all_{codeModel.Name}.h"));
        }

        private async Task GenerateRestCode(CodeModelCpp codeModel)
        {
            if (Settings.Instance.CodeGenerationMode.IsNullOrEmpty() || Settings.Instance.CodeGenerationMode.EqualsIgnoreCase("rest-client"))
            {
                Logger.Instance.Log(Category.Info, "Defaulting to generate client side Code");
                await GenerateClientSideCode(codeModel);
            }
            else if (Settings.Instance.CodeGenerationMode.EqualsIgnoreCase("rest"))
            {
                Logger.Instance.Log(Category.Info, "Generating client side Code");
                await GenerateClientSideCode(codeModel);
                Logger.Instance.Log(Category.Info, "Generating server side Code");
                await GenerateServerSideCode(codeModel);
            }
            else if (Settings.Instance.CodeGenerationMode.EqualsIgnoreCase("rest-server"))
            {
                Logger.Instance.Log(Category.Info, "Generating server side Code");
                await GenerateServerSideCode(codeModel);
            }

        }

        /// <summary>
        /// Generates C++ code for service client.
        /// </summary>
        /// <param name="cm"></param>
        /// <returns></returns>
        public override async Task Generate(CodeModel cm)
        {
            // get c++ specific codeModel
            var codeModel = cm as CodeModelCpp;
            if (codeModel == null)
            {
                throw new InvalidCastException("CodeModel is not a c++ CodeModel");
            }
            if (Settings.Instance.CodeGenerationMode.IsNullOrEmpty() || Settings.Instance.CodeGenerationMode.ToLower().StartsWith("rest"))
            {
                Logger.Instance.Log(Category.Info, "Generating Rest Code");
                await GenerateRestCode(codeModel);
            }
            else
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture,
                        string.Format(AutoRest.Core.Properties.Resources.ParameterValueIsNotValid, Settings.Instance.CodeGenerationMode, "server/client"), "CodeGenerator"));
            }
        }
    }
}

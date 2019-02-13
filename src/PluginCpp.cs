// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using AutoRest.Core;
using AutoRest.Core.Extensibility;
using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.Cpp.Model;

namespace AutoRest.Cpp
{
    public sealed class PluginCpp :Plugin<GeneratorSettingsCpp, TransformerCpp, CodeGeneratorCpp, CodeNamerCpp, CodeModelCpp>
    {
        public PluginCpp()
        {
            Context = new DependencyInjection.Context
            {
                // inherit base settings
                Context,

                // set code model implementations our own implementations 
                new Factory<CodeModel, CodeModelCpp>(),
                new Factory<Method, MethodCpp>(),
                new Factory<CompositeType, CompositeTypeCpp>(),
                new Factory<Property, PropertyCpp>(),
                new Factory<Parameter, ParameterCpp>(),
                new Factory<DictionaryType, DictionaryTypeCpp>(),
                new Factory<SequenceType, SequenceTypeCpp>(),
                new Factory<MethodGroup, MethodGroupCpp>(),
                new Factory<EnumType, EnumTypeCpp>(),
                new Factory<PrimaryType, PrimaryTypeCpp>()
            };
        }
    }
}
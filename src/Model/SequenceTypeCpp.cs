// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using AutoRest.Core.Model;
using AutoRest.Core.Utilities;

namespace AutoRest.Cpp.Model
{
    public class SequenceTypeCpp : SequenceType
    {
        public SequenceTypeCpp()
        {
//            Name.OnGet += v => $"std::vector<{ElementType.AsNullableType(!ElementType.IsValueType() || IsNullable)}>";
            Name.OnGet += v => $"std::vector<{ElementType.AsNullableType(IsNullable)}>";
        }

//        public virtual bool IsNullable => Extensions.Get<bool>("x-nullable") ?? true;
        public virtual bool IsNullable => Extensions.Get<bool>("x-nullable") ?? false;
    }

    public class DictionaryTypeCpp : DictionaryType
    {
        public DictionaryTypeCpp()
        {
//            Name.OnGet += v => $"std::unordered_map<std::string, {ValueType.AsNullableType(!ValueType.IsValueType() || IsNullable)}>";
            Name.OnGet += v => $"std::unordered_map<std::string, {ValueType.AsNullableType(IsNullable)}>";
        }

//        public virtual bool IsNullable => Extensions.Get<bool>("x-nullable") ?? true;
        public virtual bool IsNullable => Extensions.Get<bool>("x-nullable") ?? false;
    }
}
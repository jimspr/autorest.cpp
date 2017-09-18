// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using AutoRest.Core;
using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.Core.Utilities.Collections;

namespace AutoRest.Cpp
{
    public class CodeNamerCpp : CodeNamer
    {
        /// <summary>
        ///     Initializes a new instance of CodeNamerCpp.
        /// </summary>
        public CodeNamerCpp()
        {
            ReservedWords.AddRange(
                new[]
                {
                    "alignas", "alignof", "and", "auto",
                    "bool", "break", "byte", 
                    "case", "catch", "char", "char16_t", "char32_t", "class", "compl", "const", "constexpr", "continue",
                    "decimal", "decltype", "default", "delete", "do", "double", "dynamic",
                    "else", "enum", "explicit", "export", "extern", 
                    "false", "finally", "fixed", "float", "for", "foreach", "friend", "from", 
                    "goto",
                    "if", "import", "inline", "int", "interface",
                    "long",
                    "mutable",
                    "namespace","new", "noexcept", "nullptr", 
                    "operator", "override", 
                    "private", "protected", "public", 
                    "register", "ref", "return",
                    "short", "sizeof", "static", "static_assert", "string", "struct", "switch",
                    "template", "this", "thread_local", "throw", "true", "try", "typedef", "typeid", "typename", "typeof", 
                    "union", "unsigned", "using",
                    "virtual", "void", "volatile",
                    "wchar_t", "while"
                });
        }

        protected override string RemoveInvalidCharactersNamespace(string name)
        {
            return GetValidName(name, '_');
        }

        /// <summary>
        /// Returns true when the name comparison is a special case and should not 
        /// be used to determine name conflicts.
        ///  </summary>
        /// <param name="whoIsAsking">the identifier that is checking to see if there is a conflict</param>
        /// <param name="reservedName">the identifier that would normally be reserved.</param>
        /// <returns></returns>
        public override bool IsSpecialCase(IIdentifier whoIsAsking, IIdentifier reservedName)
        {
            if (whoIsAsking is Property && reservedName is CompositeType)
            {
                var parent = (whoIsAsking as IChild)?.Parent as IIdentifier;
                if (ReferenceEquals(parent, reservedName))
                {
                    return false;
                }
                // special case: properties can have the same name as a compositetype
                // unless it is the same name as a parent.
                return true;
            }
            return false;
        }

        public override string GetPropertyName(string name)
        {
            return GetVariableName(name);
        }

        public override string GetEnumMemberName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            return CamelCase(RemoveInvalidCharacters(GetEscapedReservedName(name, "Enum")));
        }

        public override string GetNamespaceName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            return RemoveInvalidCharactersNamespace(name);
        }

        public override string EscapeDefaultValue(string defaultValue, IModelType type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            var primaryType = type as PrimaryType;
            if (defaultValue != null)
            {
                if (type is CompositeType)
                {
                    return "new " + type.Name + "()";
                }
                if (type is EnumType && (type as EnumType).ModelAsString)
                {
                    return Instance.QuoteValue(defaultValue);
                }
                if (primaryType != null)
                {
                    if (primaryType.KnownPrimaryType == KnownPrimaryType.String)
                    {
                        return Instance.QuoteValue(defaultValue);
                    }
                    if (primaryType.KnownPrimaryType == KnownPrimaryType.Boolean)
                    {
                        return defaultValue.ToLowerInvariant();
                    }
                    if ((primaryType.KnownPrimaryType == KnownPrimaryType.Date) ||
                        (primaryType.KnownPrimaryType == KnownPrimaryType.DateTime) ||
                        (primaryType.KnownPrimaryType == KnownPrimaryType.DateTimeRfc1123) ||
                        (primaryType.KnownPrimaryType == KnownPrimaryType.TimeSpan) ||
                        (primaryType.KnownPrimaryType == KnownPrimaryType.ByteArray) ||
                        (primaryType.KnownPrimaryType == KnownPrimaryType.Base64Url) ||
                        (primaryType.KnownPrimaryType == KnownPrimaryType.UnixTime))
                    {
                        return
                            $"Microsoft.Rest.Serialization.SafeJsonConvert.DeserializeObject<{primaryType.Name}>" +
                            $"({Instance.QuoteValue($"\"{defaultValue}\"")}, this.Client.SerializationSettings)";
                    }
                }
            }
            return defaultValue;
        }
    }
}

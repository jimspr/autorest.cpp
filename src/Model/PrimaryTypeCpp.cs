// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.
// 

using System;
using AutoRest.Core.Model;
using static AutoRest.Core.Utilities.DependencyInjection;

namespace AutoRest.Cpp.Model
{
    public interface IExtendedModelType
    {
        bool IsValueType { get; }
    }

    public class PrimaryTypeCpp : PrimaryType, IExtendedModelType
    {
        public PrimaryTypeCpp(KnownPrimaryType primaryType) : base(primaryType)
        {
            Name.OnGet += v =>
            {
                return ImplementationName;
            };
        }

        protected PrimaryTypeCpp() : base()
        {
            Name.OnGet += v =>
            {
                return ImplementationName;
            };
        }

        public virtual string ImplementationName
        {
            get
            {
                switch (KnownPrimaryType)
                {
                    case KnownPrimaryType.Base64Url:
                        return "base64_url_t";
                        
                    case KnownPrimaryType.ByteArray:
                        return "byte_array_t";

                    case KnownPrimaryType.Boolean:
                        return "bool";

                    case KnownPrimaryType.Date:
                        return "std::string";
//                        return "std::tm";

                    case KnownPrimaryType.DateTime:
                        return "std::string";
//                        return "std::tm";

                    case KnownPrimaryType.DateTimeRfc1123:
                        return "std::string";
//                        return "std::tm";

                    case KnownPrimaryType.Double:
                        return "double";

                    case KnownPrimaryType.Decimal:
                        return "decimal";

                    case KnownPrimaryType.Int:
                        return "int";

                    case KnownPrimaryType.Long:
                        return "int64_t";

                    case KnownPrimaryType.Stream:
//                        return "uv_stream_t*";
//                        return "awaituv::future_t<std::string>";
                        return "std::string";

                    case KnownPrimaryType.String:
                        switch (KnownFormat)
                        {
                            case KnownFormat.@char:
                                return "char";

                            default:
                                return "std::string";
                        }

                    case KnownPrimaryType.TimeSpan:
                        return "std::string";
//                        return "std::chrono::duration<float>";

                    case KnownPrimaryType.Object:
                        return "std::string"; // json file specified "object"
//                        return "std::variant<>"; // multiple objects

                    case KnownPrimaryType.Credentials:
                        return "service_client_credentials_t";

                    case KnownPrimaryType.UnixTime:
                        return "std::string";
//                        return "std::tm";

                    case KnownPrimaryType.Uuid:
                        return "guid_t";

                }
                throw new NotImplementedException($"Primary type {KnownPrimaryType} is not implemented in {GetType().Name}");
            }
        }

        public virtual bool IsValueType
        {
            get
            {
                switch (KnownPrimaryType)
                {
                    case KnownPrimaryType.Boolean:
                    case KnownPrimaryType.DateTime:
                    case KnownPrimaryType.Date:
                    case KnownPrimaryType.Decimal:
                    case KnownPrimaryType.Double:
                    case KnownPrimaryType.Int:
                    case KnownPrimaryType.Long:
                    case KnownPrimaryType.TimeSpan:
                    case KnownPrimaryType.DateTimeRfc1123:
                    case KnownPrimaryType.UnixTime:
                    case KnownPrimaryType.Uuid:
                        return true;

                    case KnownPrimaryType.String:
                        return KnownFormat == KnownFormat.@char;

                    default:
                        return false;
                }
            }
        }
    }
}
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AutoRest.Core;
using AutoRest.Core.Model;
using AutoRest.Core.Utilities;
using AutoRest.Core.Utilities.Collections;
using AutoRest.Extensions;
using Newtonsoft.Json;

namespace AutoRest.Cpp.Model
{
    public class CompositeTypeCpp : CompositeType, IExtendedModelType
    {
        private readonly ConstructorModel _constructorModel = null;

        public CompositeTypeCpp()
        {
            _constructorModel = new ConstructorModel(this);
        }

        public CompositeTypeCpp(string name) : base(name)
        {
            _constructorModel = new ConstructorModel(this);
        }

        [JsonIgnore]
        public string MethodQualifier => (BaseModelType.ShouldValidateChain()) ? "override" : "virtual";

        [JsonIgnore]
        public bool NeedsPolymorphicConverter => BaseIsPolymorphic && Name != SerializedName;

        [JsonIgnore]
        public bool NeedsTransformationConverter => Properties.Any(p => p.WasFlattened());

        [JsonIgnore]
        public bool NeedsDefaultCtor => _constructorModel.NeedsDefaultCtor();

        [JsonIgnore]
        public string ConstructorParametersH => _constructorModel.Signature(true);

        [JsonIgnore]
        public string ConstructorParametersCpp => _constructorModel.Signature(false);

        [JsonIgnore]
        public IEnumerable<string> ConstructorParametersDocumentation => _constructorModel.SignatureDocumentation;

        [JsonIgnore]
        public string BaseConstructorCall => _constructorModel.BaseCall;

        [JsonIgnore]
        public virtual string ExceptionTypeDefinitionName
        {
            get
            {
                if (Extensions.ContainsKey(SwaggerExtensions.NameOverrideExtension))
                {
                    var ext = Extensions[SwaggerExtensions.NameOverrideExtension] as Newtonsoft.Json.Linq.JContainer;
                    if (ext != null && ext["name"] != null)
                    {
                        return ext["name"].ToString();
                    }
                }
                return Name + "Exception";
            }
        }

        public virtual IEnumerable<string> Usings => Enumerable.Empty<string>();

        /// <summary>
        /// Returns properties for this type and all ancestor types, including information on which level of ancestry
        /// the property comes from (0 for top-level base class that has properties, 1 for a class derived from that
        /// top-level class, etc.).
        /// </summary>
        private IEnumerable<InheritedPropertyInfo> AllPropertyTemplateModels
        {
            get
            {
                var compositeCpp = BaseModelType as CompositeTypeCpp;
                IEnumerable<InheritedPropertyInfo> baseProperties;
                if (compositeCpp == null)
                {
                    baseProperties = Enumerable.Empty<InheritedPropertyInfo>();
                }
                else
                {
                    baseProperties = compositeCpp.AllPropertyTemplateModels.ToList();
                }

                int depth = 0;
                foreach (var property in baseProperties)
                {
                    depth = Math.Max(depth, property.Depth);
                }

                return baseProperties.Concat(Properties.Select(p => new InheritedPropertyInfo(p, depth)));
            }
        }

        private class InheritedPropertyInfo
        {
            public InheritedPropertyInfo(Property property, int depth)
            {
                Property = property;
                Depth = depth;
            }

            public Property Property { get; private set; }

            public int Depth { get; private set; }
        }

        private class ConstructorParameterModel
        {
            public ConstructorParameterModel(Property underlyingProperty)
            {
                UnderlyingProperty = underlyingProperty;
            }

            public Property UnderlyingProperty { get; private set; }

            public string Name => CodeNamer.Instance.CamelCase(UnderlyingProperty.Name);
        }

        private class ConstructorModel
        {
            private readonly CompositeTypeCpp _model;
            public ConstructorModel(CompositeTypeCpp model)
            {
                _model = model;
            }

            // TODO: this could just be the "required" parameters instead of required and all the optional ones
            // with defaults if we wanted a bit cleaner constructors
            public IEnumerable<ConstructorParameterModel> Parameters => _model.AllPropertyTemplateModels.OrderBy(p => !p.Property.IsRequired).ThenBy(p => p.Depth).Select(p => new ConstructorParameterModel(p.Property));

            public IEnumerable<string> SignatureDocumentation
            {
                get
                {
                    IEnumerable<Property> parametersWithDocumentation =
                    Parameters.Where(p => !(string.IsNullOrEmpty(p.UnderlyingProperty.Summary) &&
                    string.IsNullOrEmpty(p.UnderlyingProperty.Documentation)) &&
                    !p.UnderlyingProperty.IsConstant).Select(p => p.UnderlyingProperty);

                    foreach (var property in parametersWithDocumentation)
                    {
                        var documentationInnerText = string.IsNullOrEmpty(property.Summary) ? property.Documentation.EscapeXmlComment() : property.Summary.EscapeXmlComment();

                        yield return string.Format(
                            CultureInfo.InvariantCulture,
                            "<param name=\"{0}\">{1}</param>",
                            char.ToLower(property.Name.CharAt(0)) + property.Name.Substring(1),
                            documentationInnerText);
                    }
                }
            }

            // Used to determine if need a default ctor or not
            public bool NeedsDefaultCtor()
            {
                // If there are any params that aren't defaulted then we need a true default ctor
                foreach (var property in Parameters.Where(p => !p.UnderlyingProperty.IsConstant).Select(p => p.UnderlyingProperty))
                {
                    if (property.IsRequired)
                        return true;
                }
                return false;
            }
            public string Signature(bool addDefaults)
            {
                var declarations = new List<string>();
                foreach (var property in Parameters.Where(p => !p.UnderlyingProperty.IsConstant).Select(p => p.UnderlyingProperty))
                {
                    if (addDefaults & !property.IsRequired)
                    {
                        string format = "const {0}& {1} = {{}}";
                        declarations.Add(string.Format(CultureInfo.InvariantCulture,
                            format, property.ModelTypeName, CodeNamer.Instance.CamelCase(property.Name)));
                    }
                    else
                    {
                        string format = "const {0}& {1}";
                        declarations.Add(string.Format(CultureInfo.InvariantCulture,
                            format, property.ModelTypeName, CodeNamer.Instance.CamelCase(property.Name)));
                    }
                }

                return string.Join(", ", declarations);
            }

            public string BaseCall => CreateBaseCall(_model);

            private static string CreateBaseCall(CompositeTypeCpp model)
            {
                if (model.BaseModelType != null)
                {
                    IEnumerable<ConstructorParameterModel> parameters = (model.BaseModelType as CompositeTypeCpp)._constructorModel.Parameters;
                    if (parameters.Any())
                    {
                        return $": {model.BaseModelType.ClassName}({string.Join(", ", parameters.Select(p => p.Name))})";
                    }
                }

                return string.Empty;
            }
        }

        public bool IsValueType => false;

        private static void AddIfCompositeOrEnum(HashSet<string> set, IModelType type)
        {
            if (type != null)
            {
                // Get underlying fundamental type if this is a sequence or dictionary
                if (type is SequenceType)
                {
                    type = (type as SequenceType).ElementType;
                }
                else if (type is DictionaryType)
                {
                    type = (type as DictionaryType).ValueType;
                }

                if (type is CompositeType)
                {
                    set.Add(type.Name);
                }
                else if (type is EnumType)
                {
                    if (!(type as EnumType).ModelAsString)
                        set.Add(type.Name); // Don't use DeclarationName as that will be string for an enum
                }
            }
        }

        /// <summary>
        /// Get the set of referenced composite types
        /// </summary>
        public void AddReferencedCompositeTypes(HashSet<string> set)
        {
            foreach (var property in this.Properties)            
            {
                AddIfCompositeOrEnum(set, property.ModelType);
            }
        }
    }
}
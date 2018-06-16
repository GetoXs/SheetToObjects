﻿using System;
using System.Linq;
using System.Reflection;
using CSharpFunctionalExtensions;
using SheetToObjects.Core;
using SheetToObjects.Lib.AttributesConfiguration;
using SheetToObjects.Lib.AttributesConfiguration.MappingType;
using SheetToObjects.Lib.AttributesConfiguration.RuleAttributes;

namespace SheetToObjects.Lib.FluentConfiguration
{
    internal class MappingConfigByAttributeCreator<T>
    {
        public Result<MappingConfig> CreateMappingConfig()
        {
            var type = typeof(T);

            var sheetToConfigAttribute = type.GetCustomAttributes().OfType<SheetToObjectConfig>().FirstOrDefault();
            if (sheetToConfigAttribute.IsNotNull())
            {
                return Result.Ok(CreateMappingConfigForType(type, sheetToConfigAttribute));
            }

            return Result.Fail<MappingConfig>($"No SheetToObjectConfig attribute found on model of type {type}");
        }

        private MappingConfig CreateMappingConfigForType(Type type, SheetToObjectConfig sheetToConfigAttribute)
        {
            var mappingConfig = new MappingConfig
            {
                HasHeaders = sheetToConfigAttribute.SheetHasHeaders,
                AutoMapProperties = sheetToConfigAttribute.AutoMapProperties
            };

            foreach (var property in type.GetProperties())
            {
                var columnIsMappedByAttribute = false;
                var mappingConfigBuilder = new ColumnMappingBuilder<T>();
                var attributes = property.GetCustomAttributes().ToList();

                if (attributes.OfType<IgnorePropertyMapping>().Any())
                    continue;

                foreach (var mappingAttribute in attributes.OfType<IMappingAttribute>())
                {
                    mappingAttribute.SetColumnMapping(mappingConfigBuilder);
                    columnIsMappedByAttribute = true;
                }

                foreach (var attribute in attributes)
                {
                    if (attribute is IParsingRuleAttribute ruleAttribute)
                    {
                        mappingConfigBuilder.AddParsingRule(ruleAttribute.GetRule());
                    }

                    if (attribute is Format formatAttribute)
                    {
                        mappingConfigBuilder.UsingFormat(formatAttribute.FormatString);
                    }
                }

                if (columnIsMappedByAttribute || mappingConfig.AutoMapProperties)
                {
                    mappingConfig.ColumnMappings.Add(mappingConfigBuilder.MapTo(property));
                }
            }

            return mappingConfig;
        }
    }
}
﻿using Bonsai.Dag;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Expressions
{
    class PropertyMappingNameConverter : StringConverter
    {
        static Node<ExpressionBuilder, ExpressionBuilderArgument> GetBuilderNode(
            PropertyMapping mapping,
            ExpressionBuilderGraph nodeBuilderGraph)
        {
            foreach (var node in nodeBuilderGraph)
            {
                var builder = ExpressionBuilder.Unwrap(node.Value);
                var mappingBuilder = builder as PropertyMappingBuilder;
                if (mappingBuilder != null && mappingBuilder.PropertyMappings.Contains(mapping))
                {
                    return node;
                }

                var workflowBuilder = builder as IWorkflowExpressionBuilder;
                if (workflowBuilder != null)
                {
                    var builderNode = GetBuilderNode(mapping, workflowBuilder.Workflow);
                    if (builderNode != null) return builderNode;
                }
            }

            return null;
        }

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            var nodeBuilderGraph = (ExpressionBuilderGraph)context.GetService(typeof(ExpressionBuilderGraph));
            if (nodeBuilderGraph != null)
            {
                var mapping = (PropertyMapping)context.Instance;
                var builderNode = GetBuilderNode(mapping, nodeBuilderGraph);
                return builderNode != null && builderNode.Successors.Count > 0;
            }

            return false;
        }

        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var nodeBuilderGraph = (ExpressionBuilderGraph)context.GetService(typeof(ExpressionBuilderGraph));
            if (nodeBuilderGraph != null)
            {
                var mapping = (PropertyMapping)context.Instance;
                var builderNode = GetBuilderNode(mapping, nodeBuilderGraph);
                if (builderNode != null)
                {
                    var properties = from successor in builderNode.Successors
                                     let element = ExpressionBuilder.GetWorkflowElement(successor.Target.Value)
                                     where element != null
                                     from descriptor in TypeDescriptor.GetProperties(element).Cast<PropertyDescriptor>()
                                     where descriptor.IsBrowsable && !descriptor.IsReadOnly
                                     select descriptor.Name;
                    return new StandardValuesCollection(properties.Distinct().ToArray());
                }
            }

            return base.GetStandardValues(context);
        }
    }
}
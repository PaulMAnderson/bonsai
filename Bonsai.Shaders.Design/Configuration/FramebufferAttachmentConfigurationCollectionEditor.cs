﻿using Bonsai.Resources.Design;
using System;

namespace Bonsai.Shaders.Configuration.Design
{
    public class FramebufferAttachmentConfigurationCollectionEditor : CollectionEditor
    {
        const string BaseText = "Attach";

        public FramebufferAttachmentConfigurationCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override string GetDisplayText(object value)
        {
            var configuration = (FramebufferAttachmentConfiguration)value;
            var attachment = configuration.Attachment;
            var textureName = configuration.TextureName;
            if (string.IsNullOrEmpty(textureName))
            {
                return string.Format("{0}({1})", BaseText, attachment);
            }
            else return string.Format("{0}({1} : {2})", BaseText, attachment, textureName);
        }
    }
}

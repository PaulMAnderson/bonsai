﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Bonsai.Shaders.Configuration
{
    [XmlInclude(typeof(Texture2D))]
    [XmlInclude(typeof(ImageTexture))]
    public abstract class TextureConfiguration : ResourceConfiguration<Texture>
    {
    }
}
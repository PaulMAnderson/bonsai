﻿using OpenTK;
using OpenTK.Input;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace Bonsai.Shaders.Input
{
    [DefaultProperty(nameof(Button))]
    [Description("Produces a sequence of events whenever a mouse button is pressed over the shader window.")]
    [Editor("Bonsai.Shaders.Configuration.Design.ShaderConfigurationComponentEditor, Bonsai.Shaders.Design", typeof(ComponentEditor))]
    public class MouseDown : Source<EventPattern<INativeWindow, MouseButtonEventArgs>>
    {
        [TypeConverter(typeof(NullableEnumConverter))]
        [Description("The optional mouse button to use as a filter.")]
        public MouseButton? Button { get; set; }

        public override IObservable<EventPattern<INativeWindow, MouseButtonEventArgs>> Generate()
        {
            return ShaderManager.WindowSource.SelectMany(window => window.EventPattern<MouseButtonEventArgs>(
                handler => window.MouseDown += handler,
                handler => window.MouseDown -= handler))
                .Where(evt =>
                {
                    var args = evt.EventArgs;
                    var button = Button.GetValueOrDefault(args.Button);
                    return args.Button == button;
                });
        }
    }
}

﻿using OpenCV.Net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bonsai.Dsp
{
    [Description("Subtracts a reference channel from all the channels in the input array.")]
    public class ReferenceChannels : Transform<Mat, Mat>
    {
        [TypeConverter(typeof(UnidimensionalArrayConverter))]
        [Editor("Bonsai.Dsp.Design.SelectChannelEditor, Bonsai.Dsp.Design", typeof(UITypeEditor))]
        [Description("The channels to use as reference. If empty, the average of all the array channels is used.")]
        public int[] Channels { get; set; }

        public override IObservable<Mat> Process(IObservable<Mat> source)
        {
            return source.Select(input =>
            {
                var channels = Channels;
                var output = new Mat(input.Size, input.Depth, input.Channels);
                var reference = new Mat(1, input.Cols, input.Depth, input.Channels);
                if (channels == null || channels.Length == 0)
                {
                    CV.Reduce(input, reference, 0, ReduceOperation.Avg);
                }
                else if (channels.Length == 1)
                {
                    CV.Copy(input.GetRow(channels[0]), reference);
                }
                else
                {
                    reference.SetZero();
                    for (int i = 0; i < channels.Length; i++)
                    {
                        using (var referenceChannel = input.GetRow(channels[i]))
                        {
                            CV.Add(reference, referenceChannel, reference);
                        }
                    }

                    CV.ConvertScale(reference, reference, 1f / channels.Length);
                }

                CV.Repeat(reference, output);
                CV.Sub(input, output, output);
                return output;
            });
        }
    }
}

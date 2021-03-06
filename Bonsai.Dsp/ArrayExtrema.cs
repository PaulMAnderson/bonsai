﻿using OpenCV.Net;

namespace Bonsai.Dsp
{
    public struct ArrayExtrema
    {
        public double MinValue;
        public double MaxValue;
        public Point MinLocation;
        public Point MaxLocation;

        public override string ToString()
        {
            return string.Format("{{MinValue={0}, MaxValue={1}, MinLocation={2}, MaxLocation={3}}}",
                                 MinValue, MaxValue,
                                 MinLocation, MaxLocation);
        }
    }
}

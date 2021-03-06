﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Globalization;

namespace Bonsai.Design
{
    public partial class Slider : UserControl
    {
        const int TrackBarPadding = 16;
        int? decimalPlaces;
        double minimum;
        double maximum;
        double value;

        public Slider()
        {
            maximum = 100;
            InitializeComponent();
            trackBar.Top = -10;
        }

        public TypeConverter Converter { get; set; }

        public double Minimum
        {
            get { return minimum; }
            set { minimum = value; }
        }

        public double Maximum
        {
            get { return maximum; }
            set { maximum = value; }
        }

        public int? DecimalPlaces
        {
            get { return decimalPlaces; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("value");
                }

                decimalPlaces = value;
            }
        }

        public double Value
        {
            get { return value; }
            set
            {
                if (minimum >= maximum)
                {
                    throw new InvalidOperationException("The slider range is invalid. Minimum value is greater than or equal to maximum.");
                }

                UpdateValue(value);
                trackBar.Value = (int)(trackBar.Maximum * (this.value - minimum) / (maximum - minimum));
            }
        }

        public event EventHandler ValueChanged
        {
            add { trackBar.ValueChanged += value; }
            remove { trackBar.ValueChanged -= value; }
        }

        private void UpdateTrackBarRange()
        {
            trackBar.Minimum = 0;
            trackBar.Maximum = Width - TrackBarPadding;
            Value = value;
        }

        private void UpdateValue(double value)
        {
            if (decimalPlaces.HasValue) value = Math.Round(value, decimalPlaces.Value);
            value = Math.Max(minimum, Math.Min(value, maximum));
            if (Converter != null) valueLabel.Text = Converter.ConvertToInvariantString(value);
            else valueLabel.Text = value.ToString(CultureInfo.InvariantCulture);
            this.value = value;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            UpdateTrackBarRange();
            base.OnSizeChanged(e);
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            UpdateValue(minimum + (maximum - minimum) * trackBar.Value / (double)trackBar.Maximum);
        }
    }
}

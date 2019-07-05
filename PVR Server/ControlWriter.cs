using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PVR_God
{
    public class ControlWriter : TextWriter
    {
        private TextBox textbox;
        public ControlWriter(TextBox textbox)
        {
            this.textbox = textbox;
        }

        public override void Write(char value)
        {
            textbox.Text += value;
        }

        public override void Write(string value)
        {
            textbox.Text += value;
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }

        public override void WriteLine(string text)
        {
            base.WriteLine(DateTime.Now.ToString(@"H\:mm\:ss ") + text);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class GraphForm : Form
    {
        public GraphForm(string title)
        {
            InitializeComponent();
            this.Text = title;
        }

        public PictureBox Picture { get { return this.pictureBox1; } set { this.pictureBox1 = value; } }

        private void GraphForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Picture.Image = null;
            this.Picture.Dispose();
        }
    }
}

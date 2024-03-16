using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ImageDownsizer
{
    public partial class DownsizerForm : Form
    {
        Bitmap originalImage;

        public DownsizerForm()
        {
            InitializeComponent();
        }

        private void downsizebtn_Click(object sender, EventArgs e)
        {
            if (originalImage == null)
            {
                MessageBox.Show("Please select an image first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double scaleFactor;

            if (!double.TryParse(scalingFactorTextBox.Text, out scaleFactor) || scaleFactor <= 0 || scaleFactor > 100)
            {
                MessageBox.Show("Please enter a valid downscaling factor (0 < factor <= 100).", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Threader.SingleThread(originalImage, scaleFactor);

            consequentialTB.Text = sw.Elapsed.ToString();
            sw.Restart();

            Threader.MulthiThread(originalImage, scaleFactor);
            parallelTB.Text = sw.Elapsed.ToString();

            GC.Collect();
        }

        private void addImageBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();

            if (fd.ShowDialog() == DialogResult.OK)
            {
                string imagePath = fd.FileName;

                originalImage = new Bitmap(imagePath);

                imageBox.Image = originalImage;
            }
        }
    }
}

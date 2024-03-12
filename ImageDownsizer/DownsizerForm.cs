using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

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
            string workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

            string imagePath = Path.Combine(projectDirectory, "images\\original.jpg");

            originalImage = new Bitmap(imagePath);

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

            Bitmap resizedImage = Downsizer.Bilinear(originalImage, scaleFactor);

            string outputPath = Path.Combine(projectDirectory, "images\\downsized_sequential.jpg");

            resizedImage.Save(outputPath,ImageFormat.Jpeg);
        }
    }
}

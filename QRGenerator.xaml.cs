using SkiaSharp;
using ZXing.Net.Maui;

namespace QRGenerator;

public partial class QRGenerator : ContentPage
{
	public QRGenerator()
	{
		InitializeComponent();
	}

    private void OnGenerateQrCodeClicked(object sender, EventArgs e)
    {
        var qrText = InputText.Text;
        if (!string.IsNullOrWhiteSpace(qrText))
        {
            // Set the QR code value
            QrCodeImageView.Value = qrText;
        }
        btnDownload.IsVisible = true;
    }

    private async void OnDownloadQrCodeClicked(object sender, EventArgs e)
    {
        if (QrCodeImageView.Value != null)
        {
            var qrText = QrCodeImageView.Value;

            // Create the BarcodeWriter instance to generate the QR code
            var barcodeWriter = new BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300, // Set the width of the QR code
                    Height = 300 // Set the height of the QR code
                }
            };

            // Generate the QR code as a 2D array of pixels (bool array)
            var matrix = barcodeWriter.Encode(qrText);

            // Add 10px padding
            int padding = 10;
            int newWidth = matrix.Width + 2 * padding;
            int newHeight = matrix.Height + 2 * padding;

            // Create a new SKBitmap with added padding
            using var bitmapWithPadding = new SKBitmap(newWidth, newHeight);

            // Create a canvas to draw the QR code with padding
            using (var canvas = new SKCanvas(bitmapWithPadding))
            {
                // Set the background color to white
                canvas.DrawColor(SKColors.White);

                // Create an SKPaint to define the drawing color (black for QR code)
                var paint = new SKPaint
                {
                    Color = SKColors.Black
                };

                // Draw the QR code in the center of the new bitmap (with padding)
                for (int x = 0; x < matrix.Width; x++)
                {
                    for (int y = 0; y < matrix.Height; y++)
                    {
                        if (matrix[x, y])
                        {
                            // Draw black pixels (QR code) at the correct position with padding
                            canvas.DrawPoint(x + padding, y + padding, paint);
                        }
                    }
                }
            }

            // Save the bitmap with padding to a file
            string fileName = "QRCode_with_padding.png";
            string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                // Encode and save the bitmap with padding as a PNG file
                bitmapWithPadding.Encode(stream, SKEncodedImageFormat.Png, 100);
            }

            // Trigger the download (platform-dependent)
            await ShareFileAsync(filePath, fileName);
        }
    }

    private async Task ShareFileAsync(string filePath, string fileName)
    {
        // Create ShareFile for sharing
        var shareFile = new ShareFile(filePath);

        // Share the file using ShareFileRequest
        await Share.RequestAsync(new ShareFileRequest
        {
            Title = "Download QR Code",
            File = shareFile
        });
    }
}
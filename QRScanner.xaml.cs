using ZXing.Net.Maui;

namespace QRGenerator;

public partial class QRScanner : ContentPage
{
	public QRScanner()
	{
		InitializeComponent();
	}

    protected void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        Dispatcher.Dispatch(async () =>
        {
            if (e.Results.Count() > 0)
            {
                //await DisplayAlert("QR Code Detected", e.Results[0].Value, "OK");
                textLabel.Text = e.Results[0].Value;
            }
        });
    }

    protected void OnSwitchCameraClicked(object sender, EventArgs e)
    {
        cameraBarcodeReaderView.CameraLocation = cameraBarcodeReaderView.CameraLocation == CameraLocation.Rear ? CameraLocation.Front : CameraLocation.Rear;
    }
}
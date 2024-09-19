using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.IO;
using ZXing;
using ZXing.Common;

namespace YourNamespace.Controllers
{
    public class QRCodeController : Controller
    {

        public IActionResult GenerateQRCode()
        {
            return View();
        }

        /// <summary>
        /// Action to generate the QR code based on the provided link
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GenerateQRCode(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return Content("Link cannot be empty.");
            }

            // Create a barcode writer instance
            var barcodeWriter = new ZXing.BarcodeWriter
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Width = 300,  
                    Height = 300, 
                    Margin = 1    
                }
            };

            // Generate the QR code
            using (Bitmap bitmap = barcodeWriter.Write(link))
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    var qrCodeUrl = "data:image/png;base64," + Convert.ToBase64String(stream.ToArray());
                    TempData["QRCodeUrl"] = qrCodeUrl; 
                    return RedirectToAction("GenerateQRCode"); 
                }
            }
        }
    }
}

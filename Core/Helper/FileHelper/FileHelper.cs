using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Html2pdf;



namespace Core.Helper.FileHelper
{
    public static class FileHelper
    {
        public static async Task<string> SaveFileAsync(this IFormFile file, string WebRootPath)
        {
            string filePath = Path.Combine(wwwrootGetPath.GetwwwrootPath, "uploads");
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            var path = "/uploads/" + Guid.NewGuid().ToString() + file.FileName;
            using FileStream fileStream = new(wwwrootGetPath.GetwwwrootPath + path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            return path;
        }
        public static async Task<List<string>> SaveFileRangeAsync(this List<IFormFile> file, string WebRootPath)
        {
            List<string> pictures = new();
            for (int i = 0; i < file.Count; i++)
            {
                pictures.Add(await file[i].SaveFileAsync(WebRootPath));
            }
            return pictures;
        }
        public static bool RemoveFileRange(this List<string> PhotoPaths)
        {
            foreach (var path in PhotoPaths)
            {
                string filePath = Path.Combine(wwwrootGetPath.GetwwwrootPath+path);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);

                }
                else
                {
                    continue;
                }

            }
            return true;
        }

        //public static string SaveOrderPdf(List<GeneratePdfOrderProductDTO> items,ShippingMethodInOrderPdfDTO shippingMethod, PaymentMethodInOrderPdfDTO paymentMethod)
        //{
        //    decimal totalPrice = 0;
        //    string tableBody = "";
        //    Guid guid = Guid.NewGuid();
        //    foreach (var item in items)
        //    {
        //        totalPrice += item.Price;
        //     tableBody += "  <tr>\r\n               " +
        //        "     <td style=\"border: 1px solid #ebebeb; padding: 10px;\">\r\n           " +
        //       $"           {item.ProductCode}\r\n     " +
        //        "               </td>\r\n      " +
        //        "              <td style=\"border: 1px solid #ebebeb; padding: 10px;\">\r\n         " +
        //        $"              {item.ProductName}\r\n            " +
        //        "        </td>\r\n       " +
        //        "             <td style=\"border: 1px solid #ebebeb; padding: 10px;\">\r\n         " +
        //        $"               {item.Size}\r\n             " +
        //        "       </td>\r\n       " +
        //        "             <td style=\"border: 1px solid #ebebeb; padding: 10px;\">\r\n    " +
        //        $"                    {item.Quantity}\r\n     " +
        //        "               </td>\r\n  " +
        //        "                  <td style=\"border: 1px solid #ebebeb; padding: 10px;\">\r\n  " +
        //        $"                     {item.Price} &#x20BC;\r\n      " +
        //        "              </td>\r\n       " +
        //        "         </tr>\r\n    ";
        //    }
        //    totalPrice += shippingMethod.Price;
        //    string htmlContent = "<!DOCTYPE html>\r\n" +
        //        "<html lang=\"en\">\r\n" +
        //        "<head>\r\n   " +
        //        " <meta charset=\"UTF-8\">\r\n   " +
        //        " <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n\r\n  " +
        //        "  <title>Document</title>\r\n" +
        //        "</head>\r\n" +
        //        "<body>\r\n  " +
        //        "  <div style=\"width: 90%; border: 2px solid #ebebeb; padding: 40px; height: auto;margin: auto;\">\r\n\r\n " +
        //        "       <div style=\"text-align: center; margin-bottom: 20px;\">\r\n  " +
        //        "          <h5>Elektron Çek Məzmunu</h5>\r\n   " +
        //      $"   <span>Çek №{guid.ToString().Substring(0,6)}</span>" +
        //        "        \r\n        </div>\r\n    \r\n    " +
        //        "    <table style=\"width: 100%; border-collapse: collapse;\">\r\n  " +
        //        "          <thead>\r\n   " +
        //        "             <tr>\r\n  " +
        //        "                  <th style=\"font-weight: bold; border: 1px solid #ebebeb; padding: 10px;\">Məhsul Kodu</th>\r\n   " +
        //        "                 <th style=\"font-weight: bold; border: 1px solid #ebebeb; padding: 10px;\">Məhsul Adı</th>\r\n    " +
        //        "                <th style=\"font-weight: bold; border: 1px solid #ebebeb; padding: 10px;\">Ölçü</th>\r\n           " +
        //        "         <th style=\"font-weight: bold; border: 1px solid #ebebeb; padding: 10px;\">Say</th>\r\n          " +
        //        "          <th style=\"font-weight: bold; border: 1px solid #ebebeb; padding: 10px;\">Qiyməti</th>\r\n   " +
        //        "             </tr>\r\n     " +
        //        "       </thead>\r\n     " +
        //        "       <tbody style=\"text-align: center;\">\r\n  " +
              

        //     $"  {tableBody}" +
        //        " <tr style=\"text-align: end;\">\r\n                  " +
        //        "  <td></td>\r\n                 " +
        //        "   <td></td>\r\n                  " +
        //        "  <td></td>\r\n                 " +
        //        "   <td style=\"border: 1px solid #ebebeb; padding: 10px;\">Çatdirilma Haqqı</td>\r\n                 " +
        //        $"   <td style=\"border: 1px solid #ebebeb; padding: 10px;\">{shippingMethod.Price} &#x20BC;</td>\r\n               " +
        //        " </tr>\r\n              " +
        //        "  <tr style=\"text-align: end;\">\r\n                 " +
        //        "   <td></td>\r\n               " +
        //        "     <td></td>\r\n                 " +
        //        "   <td></td>\r\n                  " +
        //        "  <td style=\"border: 1px solid #ebebeb; padding: 10px;\">Cəmi</td>\r\n                 " +
        //       $"   <td style=\"border: 1px solid #ebebeb; padding: 10px;\">{totalPrice} &#x20BC;</td>\r\n                </tr>\r\n              " +
        //        "  <tr style=\"text-align: end;\">\r\n                  " +
        //        "  <td></td>\r\n                   " +
        //        " <td></td>\r\n                    " +
        //        "<td>" +
        //        "</td>\r\n                  " +
        //        "  <td style=\"border: 1px solid #ebebeb; padding: 10px;\">Ödəniş Üsulu</td>\r\n               " +
        //        $"     <td style=\"border: 1px solid #ebebeb; padding: 10px;\">{paymentMethod.Content}</td>\r\n              " +
        //        "  </tr>\r\n         " +
        //        "   </tbody>\r\n       " +
        //        " </table>\r\n        \r\n       \r\n    \r\n     \r\n    \r\n   " +
        //        " </div>\r\n   " +
        //        " \r\n</body>\r\n" +
        //        "</html>";

        //    string htmlPath = System.IO.Path.Combine(wwwrootGetPath.GetwwwrootPath+ $"uploads\\OrderPDFs\\{guid.ToString().Substring(0, 6)}.html");
        //    string pdfPath = System.IO.Path.Combine(wwwrootGetPath.GetwwwrootPath + $"uploads\\OrderPDFs\\{guid.ToString().Substring(0, 6)}.pdf");

        //    using (FileStream htmlStream = File.Create(System.IO.Path.Combine(htmlPath)))
        //    {
        //        htmlStream.Close();
        //        File.WriteAllText(htmlPath, htmlContent);

        //    using (FileStream pdfDest = File.Create(System.IO.Path.Combine(pdfPath)))

        //    {
        //        ConverterProperties converterProperties = new ConverterProperties();
        //        converterProperties.SetCharset("UTF-8");


        //        converterProperties.IsContinuousContainerEnabled();
        //        HtmlConverter.ConvertToPdf(htmlStream, pdfDest, converterProperties);

        //    }
        //    File.Delete(htmlPath);
        //    }

        //    return guid.ToString().Substring(0, 6);
        //}
    }
}

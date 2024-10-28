using Cancellation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Cancellation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _webhostingEnvironment;
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment webhostingEnvironment)
        {
            _logger = logger;
            _webhostingEnvironment = webhostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadPdf(CancellationToken cancellationToken)
        {
            // 模擬 PDF 轉檔延遲
            try
            {
                await Task.Delay(5000, cancellationToken);  // 假設轉檔時間為 5 秒
                string fileName = "momoga.pdf";
                
                var pdfFilePath = Path.Combine(_webhostingEnvironment.WebRootPath, "files", fileName);
                
                if(!System.IO.File.Exists(pdfFilePath))
                {
                    return NotFound();
                }
                    
                var fileBytes = await System.IO.File.ReadAllBytesAsync(pdfFilePath, cancellationToken);

                // 下載 PDF 檔案
                return File(fileBytes, "application/pdf", fileName);
            }
            catch (OperationCanceledException)
            {
                return StatusCode(499, "Download cancelled by user.");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

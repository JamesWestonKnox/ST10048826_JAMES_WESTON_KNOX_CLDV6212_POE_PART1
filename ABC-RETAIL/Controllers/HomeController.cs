using ABC_RETAIL.Models;
using ABC_RETAIL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ABC_RETAIL.Controllers
{
    public class HomeController : Controller
    {
        private readonly BlobService _blobService;
        private readonly TableService _tableService;
        private readonly QueueService _queueService;
        private readonly FileService _fileService;

        //initializing storage services for use in program
        public HomeController(BlobService blobService, TableService tableService, QueueService queueService, FileService fileService)
        {
            _blobService = blobService;
            _tableService = tableService;
            _queueService = queueService;
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Customers()
        {
            return View();
        }

        public IActionResult Products()
        {
            return View();
        }
        public IActionResult Orders()
        {
            return View();
        }
        public IActionResult Contracts()
        {
            return View();
        }

        /// <summary>
        /// Method that checks if the file is not null then uses azure blob service and queue service to upload to the storage services
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _blobService.UploadBlobAsync("product-images", file.FileName, stream);
                await _queueService.SendMessageAsync("product-processing", $"Uploading product {file.FileName}");
            }
            return RedirectToAction("Products");
        }

        /// <summary>
        /// Method that checks if the customer profile model state is valid then adds the customer profile entity to the table storage as welll as sends a message to the queue service
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="Email"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddCustomerProfile(CustomerProfile profile, string Email)
        {
            if (ModelState.IsValid)
            {
                await _tableService.AddEntityAsync(profile);
                await _queueService.SendMessageAsync("customer-processing", $"Adding customer {Email}");
            }
            return RedirectToAction("Customers");
        }

        /// <summary>
        /// Method that checks if orderId is greater then zero then sends a message to the order processing queue service
        /// </summary>
        /// <param name="orderID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ProcessOrder(int orderID)
        {
            if (orderID > 0)
            {
                await _queueService.SendMessageAsync("order-processing", $"Processing order {orderID}");
            }
            return RedirectToAction("Orders");
        }

        /// <summary>
        /// Method that checks if the contract file is not null then Upload the chosen file to the file share storage service as well as a message to the queue service
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadContract(IFormFile file)
        {
            if (file != null)
            {
                using var stream = file.OpenReadStream();
                await _fileService.UploadFileAsync("contracts", file.FileName, stream);
                await _queueService.SendMessageAsync("contract-processing", $"Uploading contract {file.FileName}");
            }
            return RedirectToAction("Contracts");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
//McCall, B., 2024. CLDV_SemesterTwo_Byron. [online] GitHub. Available at: https://github.com/ByronMcCallLecturer/CLDV_SemesterTwo_Byron [Accessed 29 August 2024].
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using System.Drawing;
using System.Xml.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Quality;
using PdfSharp.Fonts;

namespace ProjectOnsMagasin.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        public OrderController(IOrderRepository orderRepository,
            IUserRepository userRepository, IOrderProductRepository orderProductRepository)
        {

            _orderRepository = orderRepository;
            _orderProductRepository = orderProductRepository;
            _userRepository = userRepository;
        }
        // GET: OrderController
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetMostLoyalUsers();
            return View(users);
        }

        // GET: OrderController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var orders = await _orderRepository.ListOrdersOfUser(id);
            return View(orders);
        }
        public async Task<IActionResult> GetOrderProductDetails(int id)
        {
            var orderProducts = await _orderProductRepository.GetOrderProductsByOrderId(id);
            return View(orderProducts);
        }
        public async Task<IActionResult> GetUnPaidOrder(int userId)
        {
            Order? cart = await _orderRepository.GetUserCartWithProducts(userId);
            return View(cart);
        }

        // GET: OrderController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<IActionResult> GetPdf(int orderId)
        {
            Order? order = await _orderRepository.GetByIdWithProducts(orderId);


            if (order != null)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (iTextSharp.text.Document document = new iTextSharp.text.Document())
                    {
                        iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, memoryStream);
                        document.Open();

                        iTextSharp.text.Font font = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.TIMES_ROMAN, 12);

                        
                        document.Add(new iTextSharp.text.Paragraph("Order Id: " + order.Id, font));
                        document.Add(new iTextSharp.text.Paragraph("Total Price: " + order.TotalPrice.ToString("C"), font));
                        document.Add(new iTextSharp.text.Paragraph("Created At: " + order.CreatedAt.ToString(), font));
                        document.Add(new iTextSharp.text.Paragraph(" ", font));

                        iTextSharp.text.pdf.PdfPTable table = new iTextSharp.text.pdf.PdfPTable(3);
                        table.WidthPercentage = 100;
                        table.AddCell(new iTextSharp.text.Phrase("Product Id", font));
                        table.AddCell(new iTextSharp.text.Phrase("Quantity", font));
                        table.AddCell(new iTextSharp.text.Phrase("Price", font));

                        foreach (var orderProduct in order.OrdersProducts)
                        {
                            table.AddCell(new iTextSharp.text.Phrase(orderProduct.Product.Name));
                            table.AddCell(new iTextSharp.text.Phrase(orderProduct.Quantity.ToString()));
                            table.AddCell(new iTextSharp.text.Phrase(orderProduct.Price.ToString("C")));
                        }

                        document.Add(table);
                        document.Close();
                    }

                    byte[] pdfBytes = memoryStream.ToArray();
                    return File(pdfBytes, "application/pdf", "Invoice.pdf");
                }
            }
            return File(new byte[0], "application/pdf", "Invoice.pdf");

        }

    }
}

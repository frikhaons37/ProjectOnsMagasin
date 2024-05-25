using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace ProjectOnsMagasin.Controllers
{
    public class CartController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderProductRepository _orderProductRepository;
        public CartController(IOrderRepository orderRepository,
            IProductRepository productRepository,
            IOrderProductRepository orderProductRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _orderProductRepository = orderProductRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            int userId = 0;

            int.TryParse(User.FindFirst("Id")?.Value, out userId);

            Order? cart = await _orderRepository.GetUserCartWithProducts(userId);

            return View(cart);
        }

        // POST: OrderController/AddToCart/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            int userId = 0;
            int.TryParse(User.FindFirst("Id")?.Value, out userId);

            Product? product = await _productRepository.GetById(productId);

            if (product == null)
                return BadRequest("Wrong Product Id");

            if (product.Qunatity == 0)
                return BadRequest("Out Of Stock");

            Order? cart = await _orderRepository.GetUserCartWithoutProducts(userId);

            if (cart == null)
            {
                cart = new()
                {
                    OrderType = OrderTypeEnum.Cart,
                    UserId = userId,
                    TotalPrice = 0
                };
            }

            cart.TotalPrice += product.Price;
            product.Qunatity -= 1;

            List<OrderProduct>? ordersProducts = null;

            if (cart.Id > 0)
            {
                ordersProducts = cart.OrdersProducts.ToList();

                OrderProduct? orderProduct = ordersProducts.FirstOrDefault(o => o.ProductId == productId && o.OrderId == cart.Id);

                if (orderProduct != null)
                {
                    orderProduct.Quantity += 1;
                    orderProduct.Price = product.Price;
                    await _orderProductRepository.Edit(orderProduct);
                }
                else
                {
                    orderProduct = new()
                    {
                        OrderId = cart.Id,
                        ProductId = productId,
                        Quantity = 1,
                        Price = product.Price
                    };
                    await _orderProductRepository.Add(orderProduct);
                }
                await _orderRepository.Edit(cart);

            }
            else
            {
                cart.OrdersProducts = new List<OrderProduct>()
                        {
                            new()
                            {
                                ProductId = productId,
                                Price = product.Price,
                                Quantity = 1
                            }
                        };

                await _orderRepository.Add(cart);
            }
            await _productRepository.Edit(product);

            return RedirectToAction("ListProduct", "Product");
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckOutCart()
        {
            int userId = 0;
            int.TryParse(User.FindFirst("Id")?.Value, out userId);

            Order? cart = await _orderRepository.GetUserCartWithoutProducts(userId);

            if (cart == null)
                return NotFound("Here is no active cart");

            cart.OrderType = OrderTypeEnum.Invoice;

            await _orderRepository.Edit(cart);

            return RedirectToAction("ListProduct", "Product");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int productId, int orderId)
        {
            try
            {
                await _orderProductRepository.Remove(productId, orderId);
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            return RedirectToAction("Index", "Cart");
        }

        /* // GET: OrderController/Edit/5
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
         }*/
    }
}



using System.Diagnostics;
using GonzaShoes.Model;
using GonzaShoes.Model.DTOs;
using GonzaShoes.Model.DTOs.Order;
using GonzaShoes.Model.DTOs.Product;
using GonzaShoes.Models;
using GonzaShoes.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GonzaShoes.Controllers
{
    public class OrderController : BackendController
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly IUserService userService;

        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService,
                               IProductService productService,
                               IUserService userService,
                               ILogger<OrderController> logger)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.userService = userService;

            _logger = logger;
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            SetUser();
            this.orderService.SetCurrentUser(userId);
            this.productService.SetCurrentUser(userId);
            return base.OnActionExecutionAsync(context, next);
        }

        private async Task GetPorductChooserAsync()
        {
            var products = await this.productService.GetProductsAsync(new ProductSearchDTO { ActivationState = ActivationStateEnum.Active, OnlyWithStock = true });

            var groupedProducts = products
            .GroupBy(p => new { p.ModelProductId, p.BrandId })
            .Select(g => new ProductGrouping
            {
                ModelProductId = g.Key.ModelProductId,
                BrandId = g.Key.BrandId,
                Id = g.First().Id,
                ModelProductName = g.First().ModelProductName,
                BrandName = g.First().BrandName,
                Price = g.First().Price,
                Stock = g.First().Stock,
                Colors = g.GroupBy(p => new { p.ColorId, p.ColorName, p.ColorHexCode })
                .Select(c => new ColorGrouping
                {
                    ColorId = c.Key.ColorId,
                    ColorName = c.Key.ColorName,
                    ColorHexCode = c.Key.ColorHexCode,
                    Sizes = c.GroupBy(p => new { p.SizeId, p.SizeNumber, p.Price, p.Stock })
                    .Select(s => new SizeGrouping
                    {
                        SizeId = s.Key.SizeId,
                        SizeNumber = s.Key.SizeNumber
                    }).ToList()
                }).ToList(),
            }).ToList();

            ViewBag.Products = groupedProducts;
        }

        private async Task GetFiltersAsync()
        {
            ViewBag.Users = await userService.GetNameIdDTOsAsync();
        }


        public async Task<IActionResult> HistoryAsync([FromQuery] OrderSearchDTO searchDTO)
        {
            if (!searchDTO.DateFrom.HasValue)
                searchDTO.DateFrom = DateTime.Now;
            if (!searchDTO.DateTo.HasValue)
                searchDTO.DateTo = DateTime.Now;

            List<OrderDTO> orders = await orderService.GetOrdersAsync(searchDTO);
            await GetFiltersAsync();

            return View(orders);
        }

        public async Task<IActionResult> EditAsync(int id)
        {
            await GetPorductChooserAsync();

            if (id > 0)
            {
                var user = await this.orderService.GetOrderByIdAsync(id);
                if (user == null)
                    return NotFound();

                return View("Edit", user);
            }
            else
                return View("Edit", new OrderDTO());
        }

        public async Task<IActionResult> DuplicateAsync(int id)
        {
            await GetPorductChooserAsync();

            if (id > 0)
            {
                var modelProduct = await this.orderService.GetOrderByIdAsync(id);
                if (modelProduct == null)
                    return NotFound();

                OrderDTO newModel = (OrderDTO)modelProduct.Clone();
                newModel.Id = 0;
                newModel.Status = Model.OrderStatusEnum.Pending;
                newModel.CreatedUserId = userId;
                newModel.DateCreated = DateTime.Now;
                newModel.DateUpdated = null;
                newModel.UpdatedUserId = null;

                newModel.OrderPayment.Id = 0;
                newModel.OrderPayment.OrderId = 0;
                newModel.OrderPayment.CreatedUserId = userId;
                newModel.OrderPayment.DateCreated = DateTime.Now;
                newModel.OrderPayment.DateUpdated = null;
                newModel.OrderPayment.UpdatedUserId = null;

                newModel.OrderItems = newModel.OrderItems.Select(x =>
                {
                    x.Id = 0; x.OrderId = 0; x.CreatedUserId = userId; x.DateCreated = DateTime.Now; x.DateUpdated = null; x.UpdatedUserId = null; return x;
                }).ToList();

                return View("Edit", newModel);
            }

            return RedirectToAction("History");
        }

        public async Task<IActionResult> UpdateStatusAsync(int id, bool isActive)
        {
            if (id > 0)
            {
                ValidationResultDTO validationResultDTO = await this.orderService.UpdateStatusAsync(id, isActive);
                if (validationResultDTO.HasErrors)
                    TempData["ErrorMessage"] = validationResultDTO.GetErrorMessages();
            }
            return RedirectToAction("History");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] OrderDTO dto)
        {
            if (ModelState.IsValid)
            {
                ValidationResultDTO validationResultDTO = await this.orderService.SaveOrderAsync(dto);
                if (validationResultDTO.HasErrors)
                {
                    ModelState.AddModelError(string.Empty, validationResultDTO.GetErrorMessages());
                    await GetPorductChooserAsync();
                    return Json(new { success = false, errors = validationResultDTO.GetErrorMessages() });
                }
                return Json(new { success = true });
            }

            await GetPorductChooserAsync();
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

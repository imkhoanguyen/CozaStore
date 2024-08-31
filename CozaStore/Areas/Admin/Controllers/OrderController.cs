using CozaStore.Data.Enum;
using CozaStore.Helpers;
using CozaStore.Interfaces;
using CozaStore.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CozaStore.Areas.Admin.Controllers
{
    [Area("admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(OrderParams orderParams)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync(orderParams);

            var vm = new OrderVM
            {
                OrderList = orders,
                CurrentSort = orderParams.SortOrder, // sort
                //sort params
                IdSortParm = orderParams.SortOrder == "id" ? "id_desc" : "id",
                TotalSortParm = orderParams.SortOrder == "total" ? "total_desc" : "total",
                CurrentFilter = orderParams.SearchString, // search query
                // query params
                SelectedShipping = orderParams.SelectedShipping,
                SelectedPayment = orderParams.SelectedPayment,
                SelectedStatus = orderParams.SelectedStatus,
                DateStart = orderParams.DateStart,
                DateEnd = orderParams.DateEnd,
                PriceMin = orderParams.PriceMin,
                PriceMax = orderParams.PriceMax,
                //combobox
                ShippingList = await _unitOfWork.ShippingRepository.GetAllContainDeleteAsync()
            };

            return View(vm);
        }

        public async Task<IActionResult> Detail(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(orderId);
            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(orderId);
            if (order == null)
                return Json(new { success = false, message = "Order not found!!!" });

            _unitOfWork.OrderRepository.UpdateStatus(order.Id, (int)OrderStatus.Confirmed);

            if (await _unitOfWork.Complete())
            {
                if(order.PaymentStatus == (int)PaymentStatus.Paid) return Json(new { success = true, message = "Order confirmation successfully" });
                
                foreach (var item in order.OrderItemList)
                {
                    if((item.Size == null && item.Color == null) || (item.Size.IsNullOrEmpty() && item.Color.IsNullOrEmpty())) // no variant
                    {
                        var productNoVariant = await _unitOfWork.ProductRepository.GetProductAsync(item.Name);
                        _unitOfWork.ProductRepository.UpdateQuantity(productNoVariant.Id, item.Count);
                    } else // have variant
                    {
                        var product = await _unitOfWork.ProductRepository.GetProductAsync(item.Name, item.Size, item.Color);
                        if (product == null) return Json(new { success = false, message = "Update quantity product fail (product not found)!!!" });

                        var variant = await _unitOfWork.ProductRepository.GetVariantOfProductAsync(product.Id, product.Variants.FirstOrDefault().ColorId,
                             product.Variants.FirstOrDefault().SizeId);

                        if (variant == null) return Json(new { success = false, message = "Update quantity product fail (variant not found)!!!" });

                        _unitOfWork.VariantRepository.UpdateQuantity(variant.Id, item.Count);
                    }

                }

                if (await _unitOfWork.Complete())
                    return Json(new { success = true, message = "Order confirmation successfully" });

                return Json(new { success = false, message = "Problem update quantity product!!!" });

            }
                

            return Json(new { success = false, message = "Problem comfirm order!!!" });
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(orderId);
            if (order == null)
                return Json(new { success = false, message = "Order not found!!!" });

            _unitOfWork.OrderRepository.UpdatePaymentStatus(order.Id, (int)PaymentStatus.Paid);

            if (await _unitOfWork.Complete())
                return Json(new { success = true, message = "Confirm payment successfully" });

            return Json(new { success = false, message = "Problem comfirm payment!!!" });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int orderId)
        {
            var order = await _unitOfWork.OrderRepository.GetAsync(orderId);

            if (order == null) return Json(new { success = false, message = "Order not found!!!" });

            _unitOfWork.OrderRepository.Delete(order);

            if (await _unitOfWork.Complete())
                return Json(new { success = true, message = "Delete order successfully" });

            return Json(new { success = false, message = "Problem delete order!!!" });
        }
    }
}

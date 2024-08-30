using CozaStore.Data;
using CozaStore.DTOs;
using CozaStore.Helpers;
using CozaStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace CozaStore.ViewModels
{
    public class OrderVM
    {
        [ValidateNever]
        public required PagedList<Order> OrderList { get; set; }
        //params
        [ValidateNever]
        public int SelectedShipping { get; set; }
        [ValidateNever]
        public int SelectedPayment { get; set; }
        [ValidateNever]
        public int SelectedStatus { get; set; }
        [ValidateNever]
        public DateTime? DateStart { get; set; } = null;
        [ValidateNever]
        public DateTime? DateEnd { get; set; } = null;
        public decimal PriceMin { get; set; }
        public decimal PriceMax { get; set; }
        //search
        [ValidateNever]
        public string? CurrentFilter { get; set; }
        [ValidateNever]
        public IEnumerable<ShippingMethod>? ShippingList { get; set; }
        [ValidateNever]
        public IEnumerable<StatusDto> PaymentStatusList { get; set; } = SD.GetPaymentStatusList();
        [ValidateNever]
        public IEnumerable<StatusDto> StatusList { get; set; } = SD.GetOrderStatusList();
    }
}

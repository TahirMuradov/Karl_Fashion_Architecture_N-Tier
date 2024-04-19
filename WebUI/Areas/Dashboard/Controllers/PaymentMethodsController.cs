using Bussines.Abstract;
using Core.Utilities.Results.Abstract;
using Entities.DTOs.PaymentMethodDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Dashboard.Controllers
{
        [Area("Dashboard")]
        [Authorize(Roles = "Admin")]
    public class PaymentMethodsController : Controller
    {
        private readonly IPaymentMethodServices _paymentMethodServices;

        public PaymentMethodsController(IPaymentMethodServices paymentMethodServices)
        {
            _paymentMethodServices = paymentMethodServices;
        }

        public IActionResult Index()
        {
            var currentCulture=Thread.CurrentThread.CurrentCulture.Name;
            IDataResult<List<GetPaymentMethodDTO>> result = _paymentMethodServices.GetAllPaymentMethod(currentCulture);
           
            return View(result.Data);
        }
        public IActionResult Delete(string id)
        {
            if (id is  null )
            {
             return   RedirectToAction("Index");

            }
            var result= _paymentMethodServices.DeletePaymentMethod(id);
            return  RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(AddPaymentMethodsDTO addPaymentMethodsDTO)
        {
            if (addPaymentMethodsDTO.LangCode.Count!=addPaymentMethodsDTO.Content.Count ||addPaymentMethodsDTO.LangCode.Count<=0
                ||addPaymentMethodsDTO.Content.Contains(null)
                || addPaymentMethodsDTO.LangCode.Contains(null)
                )
            {
                ModelState.AddModelError("Error", "Data Is Empty");
                return View();
            }
            var result = _paymentMethodServices.Add(addPaymentMethodsDTO);
            if (result.IsSuccess)
            {
                return RedirectToAction("Index");
                
            }
            ModelState.AddModelError("Error", result.Message);
            return View();
        }
    }
}

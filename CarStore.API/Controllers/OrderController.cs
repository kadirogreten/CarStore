using CarStore.API.Common;
using CarStore.Business.UOW;
using CarStore.Domain;
using CarStore.Domain.Dto_s;
using CarStore.Domain.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CarStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private UserManager<Customer> _userManager;

        private SignInManager<Customer> _signInManager;
        private ILogger<AccountController> _logger;
        private readonly IUnitOfWork _uow;


        public OrderController(ILogger<AccountController> logger, SignInManager<Customer> signInManager, UserManager<Customer> userManager, IUnitOfWork uow)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _uow = uow;
        }


        /// <summary>
        /// username veya email ile geçerli
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("BuyCar")]
        public async Task<IActionResult> BuyCar([FromBody] OrderViewModel model)
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;

            ResponseModel response;

            Customer customer;
            List<string> errors = new List<string>();
            bool hasError = false;

            if (model.CarId == 0)
            {
                errors.Add("Araç boş geçilemez!");
                hasError = true;
            }
            if (string.IsNullOrEmpty(model.CustomerId))
            {
                errors.Add("Müşteri boş geçilemez!");
                hasError = true;
            }
            if (string.IsNullOrEmpty(model.SalerId))
            {
                errors.Add("Satıcı boş geçilemez!");
                hasError = true;
            }
            if (model.TotalPrice == 0)
            {
                errors.Add("Satış fiyatı boş geçilemez!");
                hasError = true;
            }
            if (model.Installment <= 0)
            {
                errors.Add("Taksit sayısını belirtiniz!");
                hasError = true;
            }

            if (hasError)
            {
                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;
                response = new ResponseModel
                {
                    Code = System.Net.HttpStatusCode.NotAcceptable,
                    Success = false,
                    Errors = errors.ToArray(),
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };
                return Ok(response);
            }

            customer = await this._userManager.FindByIdAsync(model.CustomerId);


            if (customer == null)
            {
                errors.Add("Müşteri boş olamaz!");

                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;
                response = new ResponseModel
                {
                    Code = System.Net.HttpStatusCode.NotAcceptable,
                    Success = false,
                    Errors = errors.ToArray(),
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };
                return Ok(response);
            }


            var saler = await _userManager.FindByIdAsync(model.SalerId);

            if (saler == null)
            {
                errors.Add("Satıcı boş olamaz!");

                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;
                response = new ResponseModel
                {
                    Code = System.Net.HttpStatusCode.NotAcceptable,
                    Success = false,
                    Errors = errors.ToArray(),
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };
                return Ok(response);
            }

            if (model.Installment == 1)
            {

                var car = _uow.Car.FindOne(a => a.Id == model.CarId);

                if (car == null)
                {
                    errors.Add("Araba boş olamaz!");

                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;
                    response = new ResponseModel
                    {
                        Code = System.Net.HttpStatusCode.NotAcceptable,
                        Success = false,
                        Errors = errors.ToArray(),
                        ExecTime = exec_time,
                        Data = new
                        {

                        }
                    };
                    return Ok(response);
                }



                


                Order order = new Order
                {
                    CarId = model.CarId,
                    SalerId = saler.Id,
                    Created = DateTime.Now,
                    CreatedBy = User.Identity.Name,
                    CustomerId = customer.Id,
                    Installment = model.Installment,
                    TotalPrice = model.TotalPrice.Value,
                    CompletedAt = DateTime.Now,
                    OrderStatus = OrderStatus.Completed,
                    PayedInstallment = 1
                };


                car.BuyerId = customer.Id;
                car.SalesDate = DateTime.Now;


                _uow.Order.Insert(order);
                _uow.Car.Update(car);

                await _uow.SaveChanges();

                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                response = new ResponseModel
                {
                    Code = System.Net.HttpStatusCode.OK,
                    Success = true,
                    Message = "Başarılı!",
                    ExecTime = exec_time,
                    Data = order
                };

            }
            else
            {

                var car = _uow.Car.FindOne(a => a.Id == model.CarId);

                if (car == null)
                {
                    errors.Add("Araba boş olamaz!");

                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;
                    response = new ResponseModel
                    {
                        Code = System.Net.HttpStatusCode.NotAcceptable,
                        Success = false,
                        Errors = errors.ToArray(),
                        ExecTime = exec_time,
                        Data = new
                        {

                        }
                    };
                    return Ok(response);
                }

                Order order = _uow.Order.FindOne(a => a.CarId == model.CarId && a.SalerId == saler.Id && a.CustomerId == customer.Id);

                if (order == null)
                {
                    order = new Order
                    {
                        CarId = model.CarId,
                        SalerId = saler.Id,
                        Created = DateTime.Now,
                        CreatedBy = User.Identity.Name,
                        CustomerId = customer.Id,
                        Installment = model.Installment,
                        TotalPrice = model.TotalPrice.Value,
                        OrderStatus = OrderStatus.Pending,
                        PayedInstallment = 1
                    };


                    car.BuyerId = customer.Id;
                    


                    _uow.Order.Insert(order);
                    _uow.Car.Update(car);
                    await _uow.SaveChanges();

                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    response = new ResponseModel
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Message = "Başarılı!",
                        ExecTime = exec_time,
                        Data = order
                    };

                }
                else
                {


                    order.PayedInstallment++;
                    order.OrderStatus = order.PayedInstallment == order.Installment ? OrderStatus.Completed : OrderStatus.Pending;
                    order.Modified = DateTime.Now;
                    order.ModifiedBy = User.Identity.Name;

                    _uow.Order.Update(order);

                    if (order.OrderStatus == OrderStatus.Completed)
                    {
                        car.BuyerId = customer.Id;
                        car.SalesDate = DateTime.Now;

                        _uow.Car.Update(car);
                    }

                    await _uow.SaveChanges();

                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    response = new ResponseModel
                    {
                        Code = System.Net.HttpStatusCode.OK,
                        Success = true,
                        Message = "Başarılı!",
                        ExecTime = exec_time,
                        Data = order
                    };
                }


            }

            return Ok(response);
        }
    }
}

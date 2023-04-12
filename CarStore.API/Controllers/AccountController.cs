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
    public class AccountController : ControllerBase
    {
        private UserManager<Customer> _userManager;

        private SignInManager<Customer> _signInManager;
        private ILogger<AccountController> _logger;
        private readonly IUnitOfWork _uow;


        public AccountController(ILogger<AccountController> logger, SignInManager<Customer> signInManager, UserManager<Customer> userManager, IUnitOfWork uow)
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
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;

            ResponseModel response;



            Customer user;
            List<string> errors = new List<string>();
            bool hasError = false;

            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                errors.Add("Kullanıcı Adı veya Şifre boş geçilemez.");
                hasError = true;

                //throw new Exception("Kullanıcı Adı veya Şifre boş geçilemez.");
            }

            if (model.Username.Length > 150)
            {
                errors.Add("E-posta en fazla 150 karakter olmalı.");
                hasError = true;
            }

            if (model.Username.Contains("@") && model.Username.Length < 8)
            {
                errors.Add("E-posta en az 8 karakter olmalı.");
                hasError = true;
            }

            if (model.Username.Length < 5)
            {
                errors.Add("E-posta en az 5 karakter olmalı.");
                hasError = true;
            }

            if (model.Password.Length > 75)
            {
                errors.Add("Kullanıcı Adı en fazla 75 karakter olmalı.");
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


            user = await this._userManager.FindByNameAsync(model.Username);



            if (user != null)
            {
                //var userRole = await _userManager.GetRolesAsync(user);

                if (await IsUsernameAndPassword(user, model.Password))
                {
                    var data = await GenerateToken(user);


                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        exp.Stop();

                        exec_time = exp.ElapsedMilliseconds;

                        response = new ResponseModel
                        {
                            Code = System.Net.HttpStatusCode.OK,
                            Success = true,
                            Message = "Giriş başarılı!",
                            ExecTime = exec_time,
                            Data = data
                        };
                    }
                    else
                    {
                        var firstError = result.Errors.FirstOrDefault()?.Description;
                        exp.Stop();

                        exec_time = exp.ElapsedMilliseconds;
                        response = new ResponseModel
                        {
                            Code = System.Net.HttpStatusCode.NotAcceptable,
                            Success = false,
                            Errors = result.Errors.Select(a => a.Description).ToArray(),
                            ExecTime = exec_time,
                            Data = new
                            {

                            }
                        };
                    }




                    return Ok(response);
                }
                else
                {
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;

                    response = new ResponseModel
                    {
                        Code = System.Net.HttpStatusCode.Unauthorized,
                        Success = false,
                        Errors = new string[] { $"Kullanıcı adı veya şifre hatalı!" },
                        ExecTime = exec_time,
                        Data = new
                        {

                        }
                    };


                    return Ok(response);
                }
            }
            else
            {
                exp.Stop();

                exec_time = exp.ElapsedMilliseconds;

                response = new ResponseModel
                {
                    Code = System.Net.HttpStatusCode.Unauthorized,
                    Success = false,
                    Errors = new string[] { $"Kullanıcı girişi başarısız." },
                    ExecTime = exec_time,
                    Data = new
                    {

                    }
                };


                return Ok(response);
            }




        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {

            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            ResponseModel response;
            List<string> errors = new List<string>();
            bool hasError = false;



            if (model.Email.Length < 8)
            {
                errors.Add("E-posta en az 8 karakter olmalı.");
                hasError = true;

            }

            if (model.Email.Length > 150)
            {
                errors.Add("E-posta en fazla 150 karakter olmalı.");
                hasError = true;
            }


            if (!EmailValidation.IsValidEmail(model.Email))
            {
                errors.Add("Lütfen geçerli bir e-posta adresi giriniz.");
                hasError = true;
            }


            if (model.Password.Length < 5)
            {
                errors.Add("Şifreniz en az 5 karakter olmalı.");
                hasError = true;

            }

            if (model.Password.Length > 75)
            {
                errors.Add("Şifreniz en fazla 75 karakter olmalı.");
                hasError = true;

            }

            if (model.ConfirmPassword.Length < 5)
            {
                errors.Add("Şifreniz en az 5 karakter olmalı.");
                hasError = true;

            }

            if (model.ConfirmPassword.Length > 75)
            {
                errors.Add("Şifreniz en fazla 75 karakter olmalı.");
                hasError = true;

            }
            if (string.IsNullOrEmpty(model.Name))
            {
                errors.Add("İsim boş geçilemez.");
                hasError = true;

            }

            if (model.Name != null && model.Name.Length < 2)
            {
                errors.Add("İsim en az 2 karakter olmalı.");
                hasError = true;

            }



            if (string.IsNullOrEmpty(model.Surname))
            {
                errors.Add("Soyisim boş geçilemez.");
                hasError = true;

            }

            if (model.Surname != null && model.Surname.Length < 2)
            {
                errors.Add("Soyisim en az 2 karakter olmalı.");
                hasError = true;

            }


            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.ConfirmPassword))
            {
                errors.Add("Lütfen zorunlu alanları giriniz.");
                hasError = true;
            }



            if (model.Password != model.ConfirmPassword)
            {
                errors.Add("Girdiğiniz şifreler uyuşmuyor.");
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


            var user = new Customer
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname,
                Created = DateTime.Now,
                CreatedBy = $"{model.Name} {model.Surname}"


            };

            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    var firstError = result.Errors.FirstOrDefault()?.Description;
                    exp.Stop();

                    exec_time = exp.ElapsedMilliseconds;
                    response = new ResponseModel
                    {
                        Code = System.Net.HttpStatusCode.NotAcceptable,
                        Success = false,
                        Errors = result.Errors.Select(a => a.Description).ToArray(),
                        ExecTime = exec_time,
                        Data = new
                        {

                        }
                    };
                    return Ok(response);
                }




            }
            catch (Exception ex)
            {



                exp.Stop();
                exec_time = exp.ElapsedMilliseconds;

                response = new ResponseModel
                {
                    Code = System.Net.HttpStatusCode.BadRequest,
                    Success = false,
                    ExecTime = exec_time,
                    Errors = new string[] { $"Kod oluşturma başarısız! {ex.Message.ToString()}" },
                    Data = { }
                };

                _logger.LogError("Register ------ {0} ------ {1}", $"message = {ex.ToString()} ---\n ! exec_time={exec_time}", DateTime.Now);


                return Ok(response);
            }


            exp.Stop();

            exec_time = exp.ElapsedMilliseconds;
            response = new ResponseModel
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = "Kayıt işlemi başarılı!",
                ExecTime = exec_time,
                Data = new
                {

                }
            };

            return Ok(response);

        }




        /// <summary>
        /// Aracımı sisteme kaydetme
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("SaveMyCar")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveMyCar([FromBody] CarViewModel model)
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;
            ResponseModel response;
            List<string> errors = new List<string>();
            bool hasError = false;

            if (string.IsNullOrEmpty(model.CarName))
            {
                errors.Add("İsim boş geçilemez!");
                hasError = true;
            }
            if (model.Year == 0 && model.Year > 2024)
            {
                errors.Add("Model yılı boş geçilemez!");
                hasError = true;
            }
            if (model.FuelType == null)
            {
                errors.Add("Yakıt tipi boş geçilemez!");
                hasError = true;
            }
            if (model.BodyType == null)
            {
                errors.Add("Kasa tipi boş geçilemez!");
                hasError = true;
            }
           
            if (string.IsNullOrEmpty(model.Color))
            {
                errors.Add("Araç rengi boş geçilemez!");
                hasError = true;
            }
            if (model.BrandId == 0)
            {
                errors.Add("Araç Markası boş geçilemez!");
                hasError = true;
            }
            if (model.GearType == null)
            {
                errors.Add("Vites tipi boş geçilemez!");
                hasError = true;
            }
            if (model.PriceWithTax == null && model.PriceWithTax == 0)
            {
                errors.Add("Satış fiyatı boş geçilemez!");
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

            var saler = await _userManager.FindByNameAsync(User.Identity.Name);

            Car car = new Car();
            car.CarName = model.CarName;
            car.Year = model.Year;
            car.FuelType = model.FuelType.Value;
            car.BodyType = model.BodyType.Value;
            car.Color = model.Color;
            car.BrandId = model.BrandId;
            car.Created = DateTime.Now;
            car.CreatedBy = User.Identity.Name;
            car.GearType = model.GearType.Value;
            car.PriceWithTax = model.PriceWithTax.Value;
            car.SalerId = saler.Id;

            _uow.Car.Insert(car);

            await _uow.SaveChanges();


            

            exp.Stop();

            exec_time = exp.ElapsedMilliseconds;
            response = new ResponseModel
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = "Kayıt işlemi başarılı!",
                ExecTime = exec_time,
                Data = new
                {
                    //_uow.Car.Where(a=>a.Deleted == null).ToList()
                }
            };

            return Ok(response);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetCarsForSale")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCarsForSale()
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;

            ResponseModel response;

            Customer customer;
            List<string> errors = new List<string>();

            var brandList = _uow.Brand.Where(a => a.Deleted == null).ToList();

            var carsForSale = _uow.Car.Where(a => a.SalesDate == null && a.BuyerId == null).Select( a=> new CarListResponseModel
            {
                CarId = a.Id,
                CarName = a.CarName,
                BrandId = a.BrandId,
                BrandName = brandList.FirstOrDefault(b=>b.Id == a.BrandId).Name,
                Saler = new SalerResponseModel
                {
                    SalerId = a.SalerId,
                    SalerName = _userManager.FindByIdAsync(a.SalerId).GetAwaiter().GetResult().Name,
                    SalerSurname = _userManager.FindByIdAsync(a.SalerId).GetAwaiter().GetResult().Surname

                },
                BodyType = a.BodyType,
                Color = a.Color,
                FuelType= a.FuelType,
                GearType = a.GearType,
                PriceWithTax = a.PriceWithTax,
                Year= a.Year,
                
            }).ToList();
            exp.Stop();

            exec_time = exp.ElapsedMilliseconds;

            response = new ResponseModel
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = "Başarılı!",
                ExecTime = exec_time,
                Data = carsForSale
            };
            return Ok(response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetSoldCars")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetSoldCars()
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;

            ResponseModel response;

            Customer customer;
            List<string> errors = new List<string>();

            var brandList = _uow.Brand.Where(a => a.Deleted == null).ToList();

            var carsForSale = _uow.Car.Where(a => a.Deleted == null).Select(a => new CarListResponseModel
            {
                CarId = a.Id,
                CarName = a.CarName,
                BrandId = a.BrandId,
                BrandName = brandList.FirstOrDefault(b => b.Id == a.BrandId).Name,
                Saler = new SalerResponseModel
                {
                    SalerId = a.SalerId,
                    SalerName = _userManager.FindByIdAsync(a.SalerId).GetAwaiter().GetResult().Name,
                    SalerSurname = _userManager.FindByIdAsync(a.SalerId).GetAwaiter().GetResult().Surname

                },
                Buyer = new BuyerResponseModel
                {
                    BuyerId = a.BuyerId,
                    BuyerName = _userManager.FindByIdAsync(a.BuyerId).GetAwaiter().GetResult().Name,
                    BuyerSurname = _userManager.FindByIdAsync(a.BuyerId).GetAwaiter().GetResult().Name,
                },
                BodyType = a.BodyType,
                Color = a.Color,
                FuelType = a.FuelType,
                GearType = a.GearType,
                PriceWithTax = a.PriceWithTax,
                Year = a.Year,
                SalesDate = a.SalesDate,

            }).ToList();
            exp.Stop();

            exec_time = exp.ElapsedMilliseconds;

            response = new ResponseModel
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = "Başarılı!",
                ExecTime = exec_time,
                Data = carsForSale
            };
            return Ok(response);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("GetMyBoughtCars")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMyBoughtCars()
        {
            Stopwatch exp = new Stopwatch();
            exp.Start();
            long exec_time = 0;

            ResponseModel response;

            Customer customer;
            List<string> errors = new List<string>();

            var brandList = _uow.Brand.Where(a => a.Deleted == null).ToList();


            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var carsForSale = _uow.Car.Where(a => a.Deleted == null && a.BuyerId == user.Id && a.SalesDate != null).Select(a => new CarListResponseModel
            {
                CarId = a.Id,
                CarName = a.CarName,
                BrandId = a.BrandId,
                BrandName = brandList.FirstOrDefault(b => b.Id == a.BrandId).Name,
                Saler = new SalerResponseModel
                {
                    SalerId = a.SalerId,
                    SalerName = _userManager.FindByIdAsync(a.SalerId).GetAwaiter().GetResult().Name,
                    SalerSurname = _userManager.FindByIdAsync(a.SalerId).GetAwaiter().GetResult().Surname

                },
                Buyer = new BuyerResponseModel
                {
                    BuyerId = a.BuyerId,
                    BuyerName = _userManager.FindByIdAsync(a.BuyerId).GetAwaiter().GetResult().Name,
                    BuyerSurname = _userManager.FindByIdAsync(a.BuyerId).GetAwaiter().GetResult().Surname,
                },
                BodyType = a.BodyType,
                Color = a.Color,
                FuelType = a.FuelType,
                GearType = a.GearType,
                PriceWithTax = a.PriceWithTax,
                Year = a.Year,
                SalesDate = a.SalesDate,

            }).ToList();
            exp.Stop();

            exec_time = exp.ElapsedMilliseconds;

            response = new ResponseModel
            {
                Code = System.Net.HttpStatusCode.OK,
                Success = true,
                Message = "Başarılı!",
                ExecTime = exec_time,
                Data = carsForSale
            };
            return Ok(response);
        }

        #region Login Check

        private async Task<bool> IsUsernameAndPassword(Customer user, string password)
        {
            return await this._userManager.CheckPasswordAsync(user, password);
        }


        private async Task<dynamic> GenerateToken(Customer user)
        {



            var userRole = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddHours(5)).ToUnixTimeSeconds().ToString())
                };



            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("da758005-8edf-43e0-976b-55880eb27e71"));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                expires: DateTime.Now.AddHours(5),
                claims: claims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );


            var response = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo,
                userName = user.UserName,
                userId = user.Id,
                email = user.Email
            };

            return response;

        }

        #endregion
    }
}

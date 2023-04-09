using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarStore.Domain.Dto_s
{
    public class RegisterViewModel
    {
        /// <summary>
        /// 
        /// </summary>
        public string? Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Surname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string? Password { get; set; }


        /// <summary>
        /// 
        /// </summary>

        public string? ConfirmPassword { get; set; }
    }
}

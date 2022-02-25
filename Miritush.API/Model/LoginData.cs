using System.ComponentModel.DataAnnotations;
using Miritush.DTO.Enums;

namespace Miritush.API.Model
{
    public class LoginData
    {
        /// <summary>
        /// Grant type
        /// </summary>
        /// <value></value>
        public GrantType grant_type { get; set; } = GrantType.authorization_code;

        /// <summary>
        /// Username
        /// - Left empty for refresh-token or authorization_code grant types
        /// </summary>
        /// <value></value>
        [MinLength(4)]
        public string Username { get; set; }

        /// <summary>
        /// The user's password
        /// - Left empty for refresh-token or authorization_code grant types
        /// </summary>
        /// <value></value>
        [MinLength(4)]
        public string Password { get; set; }

        /// <summary>
        /// - If realm=sms, send the user's phone number
        /// - Left empty for refresh-token or authorization_code grant types
        /// </summary>
        /// <value></value>
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The user's one time verification code.
        /// - Left empty for refresh-token or authorization_code grant types
        /// </summary>
        /// <value></value>
        public int OtpCode { get; set; }

        /// <summary>
        /// - If realm=email, send the user's email address
        /// - Left empty for refresh-token or authorization_code grant types
        /// </summary>
        /// <value></value>
        [EmailAddress]
        public string Email { get; set; }
    }
}

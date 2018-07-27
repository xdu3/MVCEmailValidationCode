using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using MVCEmailValidationCode.Data;
using MVCEmailValidationCode.Models;
using MVCEmailValidationCode.Models.AccountViewModels;
using MVCEmailValidationCode.Services;

namespace MVCEmailValidationCode.Controllers
{
    public class SendMailController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SendMailController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult SendEmail(string email)
        {
            bool success = false;
            if (!string.IsNullOrEmpty(email))
            {
                var result = _context.ApplicationUser.Any(e => e.Email== email);
                if (result)
                {
                    try
                    {
                        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);


                        client.EnableSsl = true;


                        MailAddress from = new MailAddress("dxtestemail@gmail.com", "du xin");


                        MailAddress to = new MailAddress(email, "Your recepient name");


                        MailMessage message = new MailMessage(from, to);
                        string checkCode = new Random().Next(100000, 1000000).ToString();
                        if (string.IsNullOrEmpty(HttpContext.Session.GetString(checkCode)))
                        {
                            HttpContext.Session.SetString(email, checkCode);
                        }

                        message.Body = "Check Code:" + checkCode;


                        message.Subject = "Gmail test email with SSL and Credentials";


                        NetworkCredential myCreds = new NetworkCredential("dxtestemail@gmail.com", "21568328", "");


                        client.Credentials = myCreds;

                        client.Send(message);

                        success = true;
                        return Json("Email have been already send");
                    }
                    catch (Exception)
                    {
                        return Json("there is something wrong please register again");
                        
                    }

                }
                else
                {
                    return Json("This email have been already taken");
                }
                
            }
            else
            {
                return Json("please enter a email to register");
                
            }
        }
    }
}
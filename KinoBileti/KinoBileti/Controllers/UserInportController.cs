using ExcelDataReader;
using KinoBileti.Data;
using KinoBileti.Models;
using KinoBileti.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KinoBileti.Controllers
{
    public class UserInportController : Controller
    {
        private readonly UserManager<KinoBiletUser> _userManager;
        public UserInportController(UserManager<KinoBiletUser> userManager)
        {
            _userManager = userManager;
           
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Import(IFormFile file)
        {
            string path = $"{Directory.GetCurrentDirectory()}\\Imports\\{file.FileName}";


            using (FileStream fileStream = System.IO.File.Create(path))
            {
                file.CopyTo(fileStream);

                fileStream.Flush();
            }
            addUsers(file.FileName);
            return null;

        }
        private void addUsers(string fileName)
        {

            string pathToFile = $"{Directory.GetCurrentDirectory()}\\Imports\\{fileName}";

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            List<UserInport> userList = new List<UserInport>();

            using (var stream = System.IO.File.Open(pathToFile, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    while (reader.Read())
                    {
                        userList.Add(new Models.UserInport
                        {
                            Email = reader.GetValue(0).ToString(),
                            Password= reader.GetValue(1).ToString(),
                            uloga = reader.GetValue(2).ToString(),
                        });
                    }
                }
            }
            foreach(var item in userList)
            {
                var user = new KinoBiletUser
                {
                    UserName = item.Email,
                    Email = item.Email,
                    EmailConfirmed=true,
                    uloga=item.uloga,
                    userCart = new ShoppingCart()
                };
               var rez= _userManager.CreateAsync(user, item.Password).Result;
            }
           

            

        }
    }
}

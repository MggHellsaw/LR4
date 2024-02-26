using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;

namespace LR4.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly string _profileFilePath = "Config/profile.json";
        private readonly string _booksFilePath = "Config/books.xml";

        [HttpGet("")]
        public ActionResult<string> SayHello()
        {
            return "Welcome to the Library!";
        }

        [HttpGet("Books")]
        public ActionResult<string[]> GetBooks()
        {
            string[] books = System.IO.File.ReadAllLines(_booksFilePath);
            return books;
        }

        [HttpGet("Profile/{id?}")]
        public ActionResult<string> GetUserProfile(int? id)
        {
            if (id.HasValue && id >= 0 && id <= 5)
            {
                var profilesJson = System.IO.File.ReadAllText(_profileFilePath);
                var profiles = JsonConvert.DeserializeObject<dynamic>(profilesJson);

                if (profiles.ContainsKey(id.ToString()))
                {
                    return $"User profile for ID {id}: Name - {profiles[id.ToString()].name}, Age - {profiles[id.ToString()].age}";
                }
                else
                {
                    return "User not found!";
                }
            }
            else
            {
                return "Invalid user ID!";
            }
        }
    }
}

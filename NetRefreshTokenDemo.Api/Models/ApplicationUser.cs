using Microsoft.AspNetCore.Identity;

namespace NetRefreshTokenDemo.Api.Models
{
    //انا كان بامكاني استخدم الايدنيتي يوزر العادي ولكن عملت inheritance لانه بدي اضيف خاصيات جديدة
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
    }
}
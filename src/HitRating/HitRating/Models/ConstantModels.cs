using System;
using System.Collections.Generic;

namespace HitRating.Models
{
    public static class ApplicationConfig
    {
        public static string Domain = "/";
        public static string DefaultPassword = "666666";
        public static string ImagePath = AppDomain.CurrentDomain.BaseDirectory + "Uploads\\Images\\";
        public static string DefaultAccountPhotoUrl = Domain + "Images/default_photo.jpg";
    }
}
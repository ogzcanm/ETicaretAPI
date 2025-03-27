using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exeptions
{
    public class NotFoundUserExeption : Exception
    {
        public NotFoundUserExeption() : base("Kullanıcı Adı veya Şifre Hatalı..!")
        {
        }

        public NotFoundUserExeption(string? message) : base(message)
        {
        }

        public NotFoundUserExeption(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}

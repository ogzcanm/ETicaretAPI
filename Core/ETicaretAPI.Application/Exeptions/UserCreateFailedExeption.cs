using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Exeptions
{
    public class UserCreateFailedExeption : Exception
    {
        public UserCreateFailedExeption() : base("Kullanıcı Oluşturulurken Beklenmeyen Bir Hata İle Karşılaşıldı!")
        {
        }

        public UserCreateFailedExeption(string? message) : base(message)
        {
        }

        public UserCreateFailedExeption(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}

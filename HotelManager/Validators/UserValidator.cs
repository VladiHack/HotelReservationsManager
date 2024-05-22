using HotelManager.Models;

namespace HotelManager.Validators
{
    public static class UserValidator
    {
        public static string ReturnErrorsCreate(List<User> users,User user)
        {
            string msg = "";
            if(string.IsNullOrWhiteSpace(user.Username))
            {
                msg += "Въведете потребителско име!";
            }
            if (String.IsNullOrWhiteSpace(user.Email))
            {
                msg += "Въведете имейл адрес!\n";
            }
            if (String.IsNullOrWhiteSpace(user.PhoneNumber) || user.PhoneNumber.Length < 10)
            {
                msg += "Въведете телефонен номер с дължина 10 цифри!\n";
            }
            if (users.FirstOrDefault(p => p.FirstName == user.FirstName && p.Surname == user.Surname && p.LastName == user.LastName) != null)
            {
                msg += "Това име вече присъства в базата със служители!\n";
            }
            if (users.FirstOrDefault(p => p.Username == user.Username) != null)
            {
                msg += "Това потребителско име вече присъства в базата със служители!";
            }


            return msg;
        }
        public static string ReturnErrorsEdit(List<User> users, User user,int id)
        {
            string msg = "";
            if (String.IsNullOrWhiteSpace(user.Email))
            {
                msg += "Въведете имейл адрес!\n";
            }
            if (String.IsNullOrWhiteSpace(user.PhoneNumber) || user.PhoneNumber.Length < 10)
            {
                msg += "Въведете телефонен номер с дължина 10 цифри!\n";
            }
            if ( user.EGN.Length != 10)
            {
                msg += "Въведете ЕГН с дължина 10 цифри!\n";
            }
            if (users.FirstOrDefault(p => p.FirstName == user.FirstName && p.Surname == user.Surname && p.LastName == user.LastName && user.ID!=id) != null)
            {
                msg += "Това име вече присъства в базата със служители!\n";
            }
            if (users.FirstOrDefault(p => p.Username == user.Username && user.ID!=user.ID) != null)
            {
                msg += "Това потребителско име вече присъства в базата със служители!";
            }



            return msg;
        }
    }
}

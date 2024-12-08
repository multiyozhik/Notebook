using System.ComponentModel.DataAnnotations;

namespace _21_NotebookDb.Models
{
    //прикручиваем атрибуты валидации модели,
    //чтоб отработка ошибок была на стороне клиента сразу в представлении,
    //для этого в представлении подключаем таг-хелперы и <span asp-validation-for ="с-во"/>

    public record Contact(
        Guid Id,

        [Required(ErrorMessage ="Не указана фамилия")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 20 символов")]
        string FamilyName,

        [Required(ErrorMessage = "Не указано имя")]
        string Name,

        string? MiddleName,

        [Required(ErrorMessage = "Не указан номер телефона")]
        [RegularExpression(@"\+7\(\d{3}\)\d{3}-\d{4}",
        ErrorMessage = "Некорректный номер телефона. Необходимо использовать шаблон +7(ххх)ххх-хххх")]
        string PhoneNumber,

        [StringLength(100, MinimumLength = 15,
        ErrorMessage = "Длина строки должна быть от 10 символов")]
        string? Adress)
    {
        public string Description => $"{FamilyName}: {PhoneNumber}";
    }    
}

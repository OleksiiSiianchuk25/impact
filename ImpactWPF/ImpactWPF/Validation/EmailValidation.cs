using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ImpactWPF.Validation
{
    public class EmailValidation : ValidationRule
    {
        public override System.Windows.Controls.ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value as string;
            if (string.IsNullOrEmpty(email))
            {
                return new System.Windows.Controls.ValidationResult(false, "Електронна адреса не може бути порожньою.");
            }
            else
            {
                // Перевірка на відповідність електронної адреси стандартному формату.
                Regex regex = new Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
                if (!regex.IsMatch(email))
                {
                    return new System.Windows.Controls.ValidationResult(false, "Неправильний формат електронної адреси.");
                }
            }

            return System.Windows.Controls.ValidationResult.ValidResult;
        }
    }
}

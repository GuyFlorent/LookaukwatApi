using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace LookaukwatApi.Models
{
    // Modèles utilisés comme paramètres des actions AccountController.

    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "Jeton d’accès externe")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe actuel")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel
    {
        [Required]
        [Display(Name = "Prénom")]
        public string FirstName { get; set; }
        [Display(Name = "Prénom du parrain")]
        public string ParrainName { get; set; }
        [Required]
        [Display(Name = "Numéro de téléphone")]
        [System.Web.Mvc.Remote("Checked_IfNumber_AlreadyExist", "User", ErrorMessage = "Ce numéro de téléphone existe déjà !")]
        //[RegularExpression(@"^(?:\d{10}|00\d{11}|\+\d{2}\d{9})$", ErrorMessage = "Numéro incorect ! ex:06...,01..., +33... , + 49..., 0033...")]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mot de passe")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le mot de passe ")]
        [Compare("Password", ErrorMessage = "Le mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "E-mail")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Fournisseur de connexion")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Clé du fournisseur")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "La chaîne {0} doit comporter au moins {2} caractères.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nouveau mot de passe")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmer le nouveau mot de passe")]
        [Compare("NewPassword", ErrorMessage = "Le nouveau mot de passe et le mot de passe de confirmation ne correspondent pas.")]
        public string ConfirmPassword { get; set; }
    }
}

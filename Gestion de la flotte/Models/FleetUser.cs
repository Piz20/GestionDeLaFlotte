using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Gestion_de_la_flotte.Models
{


    public class FleetUser
    {
       
        public String? Id { get; set; }


        [Required(ErrorMessage = "Le matricule est requis.")]
        public string Matricule { get; set; }

        [Required(ErrorMessage = "L'email est requis.")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le nom est requis.")]
        [DisplayName("Nom")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Le prénom est requis.")]
        [DisplayName("Prénom")]
        public string SurName { get; set; }


        [Required(ErrorMessage = "Le numéro CNI est requis.")]
        [DisplayName("Numéro de CNI")]
        public string CNINumber { get; set; }

        public DateTime CreationDate { get; set; }
        public int Status { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}\n" +
                   $"Matricule: {Matricule}\n" +
                   $"Email: {Email}\n" +
                   $"Nom: {Name}\n" +
                   $"Prénom: {SurName}\n" +
                   $"Numéro CNI: {CNINumber}\n" +
                   $"Date de Création: {CreationDate}\n" +
                   $"Statut: {Status}\n";
        }
    }



}


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebProjectAzure.Models
{
    /// <summary>
    /// Classe de l'abonnement 
    /// </summary>
    public class AbonnementModel
    {
        /// <summary>
        /// Id de l'abonnement
        /// </summary>
        [Required]
        [Key]
        public int Id { get; set; }


        /// <summary>
        /// Date de début de l'abonnement
        /// </summary>
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DateDebut { get; set; }

        /// <summary>
        /// Duree de l'abonnement
        /// </summary>
        [Required]
        [Range(1, 12)]
        [DisplayName("Duree")]
        public int Duree { get; set; }

        /// <summary>
        /// Tarif mensuel de l'abonnement
        /// </summary>
        [Required]
        [DisplayName("Tarif Mensuel")]
        [DataType(DataType.Currency)]
        public double TarifMensuel { get; set; }

        /// <summary>
        /// Mail de l'utilisateur
        /// </summary>
        [Required]
        [DisplayName("Mail")]
        public string Mail { get; set; }

        /// <summary>
        /// Identifiant Azure de la VM
        /// </summary>
        public string? IdVm { get; set; }
    }
}

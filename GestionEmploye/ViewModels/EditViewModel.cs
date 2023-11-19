using System.ComponentModel.DataAnnotations;

namespace GestionEmploye.ViewModels
{
    public class EditViewModel :CreateViewModel
    {
        public int Id { get; set; }
        
        //[Range(300, 5000, ErrorMessage = "Doit être entre 300 et 5000")]
       // public int Salary { get; set; }
        public string ExistingPhotoPath { get; set; }
    }
}

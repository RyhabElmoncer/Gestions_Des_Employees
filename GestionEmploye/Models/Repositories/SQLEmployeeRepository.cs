using System.Xml.Linq; // Importe la directive pour utiliser System.Xml.Linq

namespace GestionEmploye.Models.Repositories
{
    // Définition de la classe SQLEmployeeRepository qui implémente l'interface IEmployeeRepository
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext context; // Déclaration d'un champ privé pour stocker le contexte de la base de données

        // Constructeur de la classe, prenant un objet de contexte de base de données en tant que paramètre
        public SQLEmployeeRepository(AppDbContext context)
        {
            this.context = context;
        }

        // Méthode pour ajouter un employé à la base de données
        public Employe Add(Employe employe)
        {
            context.Employees.Add(employe); // Ajoute l'employé au contexte de la base de données
            context.SaveChanges(); // Enregistre les modifications dans la base de données
            return employe; // Retourne l'employé ajouté (éventuellement mis à jour avec une clé primaire)
        }

        // Méthode pour supprimer un employé de la base de données en fonction de son identifiant (Id)
        public Employe Delete(int Id)
        {
            Employe employee = context.Employees.Find(Id); // Recherche l'employé par son identifiant
            if (employee != null)
            {
                context.Employees.Remove(employee); // Supprime l'employé du contexte de la base de données
                context.SaveChanges(); // Enregistre les modifications dans la base de données
            }
            return employee; // Retourne l'employé supprimé (ou null si l'employé n'a pas été trouvé)
        }

        // Méthode pour récupérer tous les employés de la base de données
        public IEnumerable<Employe> GetAllEmployee()
        {
            return context.Employees; // Retourne une liste de tous les employés de la base de données
        }

        // Méthode pour récupérer un employé en fonction de son identifiant (Id)
        public Employe GetEmployee(int Id)
        {
            return context.Employees.Find(Id); // Recherche et retourne un employé par son identifiant
        }

        // Méthode pour mettre à jour les informations d'un employé (non implémentée dans ce code)
        public Employe Update(Employe employeeChanges)
        {
            // Recherche l'employé existant par son identifiant
            var employee = context.Employees.Find(employeeChanges.Id);

            if (employee != null)
            {
                // Met à jour les propriétés de l'employé existant avec les nouvelles valeurs
                employee.Name = employeeChanges.Name;
                employee.Departement = employeeChanges.Departement;
                employee.Salary = employeeChanges.Salary;

                context.SaveChanges(); // Enregistre les modifications dans la base de données
            }

            return employee; // Retourne l'employé mis à jour (ou null si l'employé n'a pas été trouvé)
        }

    }
}

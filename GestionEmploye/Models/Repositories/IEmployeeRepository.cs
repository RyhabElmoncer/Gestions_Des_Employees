namespace GestionEmploye.Models.Repositories
{
    public interface IEmployeeRepository
    {
        Employe GetEmployee(int Id);
        IEnumerable<Employe> GetAllEmployee();
        Employe Add(Employe employee);
        Employe Update(Employe employeeChanges);
        Employe Delete(int Id);

    }
}

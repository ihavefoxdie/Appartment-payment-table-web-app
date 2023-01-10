namespace Lab1DBwithASP.Models
{
    public interface IApartmentDataService
    {
        List<ApartmentModel> GetApartments(int year);

        List<ApartmentModel> SearchApartments(string query);

        List<ApartmentModel> GetApartmentsMonth(int year, int month, int apartStart, int apartEnd);

        List<ApartmentModel> GetApartmentYear(int year, int id);

        ApartmentModel GetApartmentById(int id, int year, int month);

        bool Insert(ApartmentModel model);
        int Update(ApartmentModel model);
        int Delete(int id);

        int DeleteEntry(int id);
    }
}

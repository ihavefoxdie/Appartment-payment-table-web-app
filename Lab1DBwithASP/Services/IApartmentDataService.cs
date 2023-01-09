namespace Lab1DBwithASP.Models
{
    public interface IApartmentDataService
    {
        List<ApartmentModel> GetApartments(int year);

        List<ApartmentModel> SearchApartments(string query);

        ApartmentModel GetApartmentById(int id, int year, int month);

        bool Insert(ApartmentModel model);
        int Update(ApartmentModel model);
        int Delete(int id);
    }
}

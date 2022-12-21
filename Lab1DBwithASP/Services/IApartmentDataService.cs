namespace Lab1DBwithASP.Models
{
    public interface IApartmentDataService
    {
        List<ApartmentModel> GetApartments();

        List<ApartmentModel> SearchApartments(string query);

        ApartmentModel GetApartmentById(int id);

        bool Insert(ApartmentModel model);
        int Update(ApartmentModel model);
        int Delete(int id);
    }
}

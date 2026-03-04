namespace MVC_Buddies.Dtos
{
    
        //public class MainViewModel
        //{
        //    public string? Location { get; set; }
        //    public List<SitterCardDto>? Sitters { get; set; } = new();
        //}




    public record HomeSitterCardDto(
      int Id,
      string Name,
      string Location,
      string AvatarUrl,
      decimal PricePerDay,
      double Rating,
      int ReviewsCount,
      List<string> Services
  );

    public record HomeMainDto(string Location, List<HomeSitterCardDto> Sitters);

}

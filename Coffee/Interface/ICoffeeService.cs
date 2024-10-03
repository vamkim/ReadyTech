using Coffee.Model;

namespace Coffee.Interface
{
    public interface ICoffeeService
    {
        CoffeeResult BrewCoffee(DateTime currentDate);
    }
}

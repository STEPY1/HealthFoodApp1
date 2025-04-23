using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HealthFoodApp.Models;

public class CartItem : INotifyPropertyChanged
{
    public int Id { get; set; }
    public string Image { get; set; }
    public string Title { get; set; }
    public string Restaurant { get; set; }
    public string Price { get; set; }

    private int _quantity;
    public int Quantity
    {
        get => _quantity;
        set
        {
            if (_quantity != value)
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

using PropertyChanged;
using System.ComponentModel;

namespace DoosanTest
{
    [AddINotifyPropertyChangedInterface]
    public class BaseViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };
    }
}

using SnapShotUndo.Model;
using System.Collections.ObjectModel;

namespace SnapShotUndo.Strategy
{
    internal class CompositeSnapshot : IEquatable<CompositeSnapshot>
    {
        public string TextBoxValue { get; set; } = string.Empty;
        public string RadioNames { get; set; } = string.Empty;
        public IEnumerable<bool> CheckBoxValue { get; set; } = new List<bool>();
        public IEnumerable<double> SliderValue { get; set; } = new List<double>();
        public ObservableCollection<Person> PersonValue { get; set; } = new();

        public bool Equals(CompositeSnapshot? other)
        {
            if (other is null)
                return false;

            return TextBoxValue == other.TextBoxValue
                && RadioNames == other.RadioNames
                && CheckBoxValue.SequenceEqual(other.CheckBoxValue)
                && SliderValue.SequenceEqual(other.SliderValue)
                && PersonValue.SequenceEqual(other.PersonValue);
        }


    }
}

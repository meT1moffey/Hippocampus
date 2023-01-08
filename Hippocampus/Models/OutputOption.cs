using System;

namespace Hippocampus.Models
{
    public enum OutputOptionEnum
    {
        Text = 0,
        Image = 1,
        File = 2,
    }

    public class OutputOption
    {
        public static implicit operator OutputOption(OutputOptionEnum option) => new(option);
        public static implicit operator OutputOption(string option)
            => Enum.Parse<OutputOptionEnum>(option);
        public static explicit operator OutputOptionEnum(OutputOption option)
            => option.option;
        public static implicit operator string(OutputOption option)
            => option.option.ToString();
        public override string ToString() => (string)this;
        public static string[] GetAllOptions() => Enum.GetNames<OutputOptionEnum>();

        public OutputOptionEnum option;
        public OutputOption(OutputOptionEnum _option = OutputOptionEnum.Text) => option = _option;
    }
}

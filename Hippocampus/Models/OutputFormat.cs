using Hippocampus.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hippocampus.Models
{
    public enum FormatEnum
    {
        Text = 0,
        Image = 1,
        File = 2,
    }

    public class OutputFormat
    {
        public static implicit operator OutputFormat(FormatEnum format) => new(format);
        public static implicit operator OutputFormat(string format)
            => Enum.Parse<FormatEnum>(format);
        public static explicit operator FormatEnum(OutputFormat format) => format.format;
        public static string[] AllFormats() => Enum.GetNames<FormatEnum>();

        public FormatEnum format;
        public OutputFormat(FormatEnum _format = FormatEnum.Text) => format = _format;
    }
}

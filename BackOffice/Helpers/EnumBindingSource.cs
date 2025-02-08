using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace BackOffice.Helpers
{
    public class EnumBindingSource : MarkupExtension
    {
        private Type _enumType;

        public Type EnumType
        {
            get => _enumType;
            set
            {
                if (value != _enumType)
                {
                    if (value != null)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (!enumType.IsEnum)
                        {
                            throw new ArgumentException("Type must be an Enum.");
                        }
                    }

                    _enumType = value;
                }
            }
        }

        public EnumBindingSource() { }

        public EnumBindingSource(Type enumType)
        {
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (_enumType == null)
            {
                throw new InvalidOperationException("EnumType must be specified.");
            }

            Type actualEnumType = Nullable.GetUnderlyingType(_enumType) ?? _enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == _enumType)
            {
                return enumValues;
            }

            // If the enum is nullable, add a null value at the beginning
            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}

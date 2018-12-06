using System;

namespace Hymperia.Facade.Extensions
{
  public static class EnumExtension
  {
    public static TEnum Parse<TEnum>(string value) where TEnum : struct
    {
      if (!Enum.TryParse(value, out TEnum result))
        throw new InvalidOperationException();

      return result;
    }
  }
}

using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Desktop.ViewModel
{
    class PictureConverter : IValueConverter
    {
        public object Convert(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            if (!(value is byte[]))
                return Binding.DoNothing;
try
            {
                using (MemoryStream stream = new MemoryStream(value as byte[])) // a képet a memóriába egy adatfolyamba helyezzük
                {
                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad; // a betöltött tartalom a képbe kerül
                    image.StreamSource = stream; // átalakítjuk bitképpé
                    image.EndInit();
                    return image; // visszaadjuk a létrehozott bitképet
                }
            }
            catch
            {
                return Binding.DoNothing;
            }
        }

        public object ConvertBack(Object value, Type targetType, Object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
